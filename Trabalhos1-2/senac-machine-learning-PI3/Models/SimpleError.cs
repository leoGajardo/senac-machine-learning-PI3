using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3.Models
{
    public class SimpleError
    {
        //cria uma lista com as predições que serão feitas
        public SimpleError(int k)
        {
            this.K = k;
            Predictions = new List<Prediction>();
        }
        //guarda qual foi o valor do K que está sendo rodado
        public int K { get; private set; }
        //Calcula o total de erros feitos nas predições
        public int NumOfErrors
        {
            get
            {
                var i = 0;
                foreach (var prediction in Predictions)
                    if (prediction.ExpectedClass != prediction.PreviewedClass) //compara a clase predizida com a classe real
                        i++;
                return i;
            }
        }

        //Calcula o total de predições
        public int NumOfRecords {
            get
            {
                return Predictions.Count();
            }
        }

        public List<Prediction> Predictions { get; set; }

        public double ErrorValue()
        {
            return (double)NumOfErrors / (double)NumOfRecords;
        }


    }
}
