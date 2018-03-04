using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public static class Outliers
    {
        
        public static DataTable RemoveOutliers(this DataTable table, int column) 
        {
            var coluna = table.Data.Select(d => Double.Parse(d.Columns[column])).OrderBy(x => x).ToArray<double>();
            var Q1 = GetQuartil(coluna, 1);
            var Q3 = GetQuartil(coluna, 3); 
            var IQR = GetIQR(Q3, Q1); 
            var LimiteInferior = GetLimiteInferior(coluna, IQR); 
            var LimiteSuperior = GetLimiteSuperior(coluna, IQR);

            //var shouldBeRemoved = new List<int>();
            //foreach (var line in table.Data)
            //    foreach (var value in line.Columns)
            //        if (Double.Parse(value) > LimiteSuperior
            //            || Double.Parse(value) < LimiteInferior)
            //            shouldBeRemoved.Add(line.Id);

            ////var shouldBeRemoved = table.Data.Where(d => 
            ////   Double.Parse(d.Columns[column]) > LimiteSuperior 
            ////|| Double.Parse(d.Columns[column]) < LimiteInferior).Select(d => d.Id);

            ////print shouldBeeRemoved

            return table;
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
                return coluna[k - 1] + fk * (coluna[k] - coluna[k - 1]);
            }
            
            if (QuartilNumber == 3)
            {
                temp =  (3 * ((double)coluna.Length + 1) / 4);//temporário que acha a Interpolação do Quartil 2
                int k = (int)temp; //Parte inteira da Interpolação, para as posições do array
                double fk = temp - k; // parte fracionaria da interpolação para multiplicar para o valor do Quartil
                return coluna[k - 1] + fk * (coluna[k] - coluna[k - 1]); 
            }

            return -1;         
        }

    }
}
