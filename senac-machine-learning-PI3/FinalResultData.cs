using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public class FinalResultData
    {
        public DataTable ReferenceTable { get; private set; }
        
        public FinalResultData(DataTable referenceTable)
        {
            this.ReferenceTable = referenceTable;
            SimpleErrors = new List<SimpleError>();
        }

        public List<SimpleError> SimpleErrors { get; set; }

        public double GetCrossError(int k)
        {
            return SimpleErrors.Where(se => se.K == k).Average(a => a.ErrorValue());
        }

        public void PrintConfusionMatrix()
        {
            
        }

        public int GetBestK()
        {
            return SimpleErrors.GroupBy(se => se.K).OrderBy(g => GetCrossError(g.Key)).First().Key;
        }
    }
}
