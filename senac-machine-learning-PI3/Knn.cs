using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public static class Knn
    {


        public static double Distancia(int[] columns, double[] a, double[] b)
        {
            double distancia = 0;
            foreach (var column in columns)
            {                   
                distancia += Math.Pow((a[column] - b[column]), 2);
            }

            return Math.Sqrt(distancia);
        }
    }
}
