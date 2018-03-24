using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public static class Outliers
    {

        public static DataTable RemoveOutliers(this DataTable table, int column, ref List<int> shouldBeRemoved)
        {
            var coluna = table.Data.Select(d => Double.Parse(d.Columns[column])).OrderBy(x => x).ToArray<double>();

            var Q1 = GetQuartil(coluna, 1); //calcula o primeiro quartil
            var Q3 = GetQuartil(coluna, 3); // calcula o terceiro quartil
            var IQR = GetIQR(Q3, Q1); // calcula o IQR
            var LimiteInferior = GetLimiteInferior(coluna, IQR); // calcula o limite inferior da coluna apartir do IQR
            var LimiteSuperior = GetLimiteSuperior(coluna, IQR);//  calcula o superior inferior da coluna apartir do IQR

            //faz a verificação para todas as linhas dos dados da tabela se eles possuem valores menores do que o limite inferior ou maior do que o limite superior e caso possua é adicionado o id daquela linha a lista dos valores que serão removidos
            foreach (var line in table.Data)
                if (Double.Parse(line.Columns[column]) > LimiteSuperior
                    || Double.Parse(line.Columns[column]) < LimiteInferior)
                    shouldBeRemoved.Add(line.Id);
            return table;
        }

        public static void PrintOutliers(DataTable table, List<int> shouldBeRemoved)
        {
            //checa se o diretório de outliers já existe e caso não exista cria o diretório
            if (!Directory.Exists("outliers"))
                Directory.CreateDirectory("outliers");

            //checa se já existe um arquivo de outliers para aquela tabela e caso já existe dela o existnte para poder criar o novo
            if (Directory.Exists("outliers"))
                File.Delete("outliers\\outliers_" + table.fileName);
            //imprime todos os outliers em seu respectivo arquivo
            File.WriteAllLines("outliers\\outliers_" + table.fileName, table.Data.Where(d => shouldBeRemoved.Contains(d.Id)).Select(s => s.ToString()));
        }

        public static double GetLimiteSuperior(double[] coluna, double IQR)
        {
            //Calcula o Limite Superior da coluna, utilizando da média e da Amplitude do Interquartil
            double temp = coluna.Average() + 1.5 * IQR;
            return temp;
        }

        public static double GetLimiteInferior(double[] coluna, double IQR)
        {
            //Calcula o Limite Inferior da coluna, utilizando da média e da Amplitude do Interquartil
            double temp = coluna.Average() - 1.5 * IQR;
            return temp;
        }

        public static double GetIQR(double Q3, double Q1)
        {
            //Calcula a Amplitude Interquertil
            return Q3 - Q1;
        }

        public static double GetQuartil(double[] coluna, int QuartilNumber)
        {
            double temp;
            if (QuartilNumber == 1)
            {   
                temp = ((double)coluna.Length + 1) / 4; //temporário que acha a Interpolação do Quartil 1        
                int k = (int)temp; //Parte inteira da Interpolação, para as posições do array
                double fk = temp - k; // parte fracionaria da interpolação para multiplicar para o valor do Quartil
                if (k == 0)
                {
                    return coluna[k];
                }
                return coluna[k - 1] + fk * (coluna[k] - coluna[k - 1]);
                //Considerando Array iniciado na posição 0, precisa-se retirar 1 de K e o k+1 se torna somente k
            }

            if (QuartilNumber == 3)
            {
                temp =  ( (3 * (double)coluna.Length + 1) / 4);//temporário que acha a Interpolação do Quartil 3
                int k = (int)temp; //Parte inteira da Interpolação, para as posições do array
                double fk = temp - k; // parte fracionaria da interpolação para multiplicar para o valor do Quartil
                return coluna[k - 1] + fk * (coluna[k] - coluna[k - 1]);
                //Considerando Array iniciado na posição 0, precisa-se retirar 1 de K e o k+1 se torna somente k
            }

            return -1;         
        }

    }
}
