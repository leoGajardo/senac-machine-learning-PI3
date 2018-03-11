using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3.Models
{
    public class SimpleError
    {
        public SimpleError(int k)
        {
            this.K = k;
            Predictions = new List<Prediction>();
        }
        public int K { get; private set; }
        public int NumOfErrors
        {
            get
            {
                var i = 0;
                foreach (var prediction in Predictions)
                    if (prediction.ExpectedClass != prediction.PreviewedClass)
                        i++;
                return i;
            }
        }

        public int NumOfRecords {
            get
            {
                return Predictions.Count();
            }
        }

        public List<Prediction> Predictions { get; set; }

        public double ErrorValue()
        {
            return NumOfErrors / NumOfRecords;
        }


    }
}
