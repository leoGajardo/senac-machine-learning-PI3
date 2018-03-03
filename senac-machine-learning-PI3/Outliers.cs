using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    class Outliers
    {
        public double[] coluna;
        public double IQR;
        public double Q3;
        public double Q1;
        public double LimiteInferior;
        public double LimiteSuperior;

        public Outliers(double[] coluna)
        {
            var result = coluna.OrderBy(x => x);
            this.coluna = result.ToArray<double>();
            this.Q1 = GetQuartil(this.coluna, 1);
            this.Q3 = GetQuartil(this.coluna, 3);
            this.IQR = GetIQR(Q3, Q1);
            this.LimiteInferior = GetLimiteInferior(this.coluna, IQR);
            this.LimiteSuperior = GetLimiteSuperior(this.coluna, IQR);


        }

        public double GetLimiteSuperior(double[] coluna, double IQR)
        {
            //Calcula o Limite Superior da coluna, utilizando da média e da Amplitude do Interquartil
            double temp = coluna.Average() + 1.5 * IQR;
            return temp;
        }

        public double GetLimiteInferior(double[] coluna, double IQR)
        {
            //Calcula o Limite Inferior da coluna, utilizando da média e da Amplitude do Interquartil
            double temp = coluna.Average() - 1.5 * IQR;
            return temp;
        }

        public double GetIQR(double Q3, double Q1)
        {
            //Calcula a Amplitude Interquertil
            return IQR = Q3 - Q1;
        }

        public double GetQuartil(double[] coluna, int NumeroQuartil)
        {
            double temp;
            if (NumeroQuartil == 1)
            {   
                temp = ((double)coluna.Length + 1) / 4; //temporário que acha a Interpolação do Quartil 1        
                int k = (int)temp; //Parte inteira da Interpolação, para as posições do array
                double fk = temp - k; // parte fracionaria da interpolação para multiplicar para o valor do Quartil
                Q1 = coluna[k - 1] + fk * (coluna[k] - coluna[k - 1]);
                return Q1;
            }
            
            if (NumeroQuartil == 3)
            {
                temp =  (3 * ((double)coluna.Length + 1) / 4);//temporário que acha a Interpolação do Quartil 2
                int k = (int)temp; //Parte inteira da Interpolação, para as posições do array
                double fk = temp - k; // parte fracionaria da interpolação para multiplicar para o valor do Quartil
                Q3 = coluna[k - 1] + fk * (coluna[k] - coluna[k - 1]);
                return Q3;
            }

            return -1;         
        }

    }
}
