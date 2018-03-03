using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Outliers
{
    class RemoverOutliers
    {        
        public double[] coluna;
        public double media;
        public double total;
        public double IQR;
        public double Q3;
        public double Q1;
        public double LimiteInferior;
        public double LimiteSuperior;
        RemoverOutliers(double[] coluna){
            var result = coluna.OrderBy(x => x);
            this.coluna = result.ToArray<double>();

            this.Q1 = GetQuartil(coluna, 1);
            this.Q3 = GetQuartil(coluna, 3);
            this.IQR = GetIQR(Q3, Q1);
            this.LimiteInferior = GetLimiteInferior(coluna, IQR);
            this.LimiteSuperior = GetLimiteSuperior(coluna, IQR);


        }

        public double GetLimiteSuperior(double[] coluna, double IQR){
            
            double temp = coluna.Average() - 1.5 * IQR;

            return temp;
        }

        public double GetLimiteInferior(double[] coluna, double IQR)
        {
            
            double temp = coluna.Average() - 1.5 * IQR;

            return temp;
        }

        public double GetIQR(double Q3, double Q1){

            return IQR = Q3 - Q1;
        }

        public double GetQuartil(double[] coluna, int NumeroQuartil){

            if (NumeroQuartil == 1)
            {
                double temp = (coluna.Length + 1) / 4;

                Q1 = coluna[Convert.ToInt32(temp)] + temp * (coluna[Convert.ToInt32(temp) + 1] - coluna[Convert.ToInt32(temp)]);
                return Q1;
            }
                

            if (NumeroQuartil== 3)
            {
                double temp = 3 * ((coluna.Length + 1) / 2);

                Q3 = coluna[Convert.ToInt32(temp)] + temp * (coluna[Convert.ToInt32(temp) + 1] - coluna[Convert.ToInt32(temp)]);
                return Q3;
            }
            return -1;
        }

    }
}
