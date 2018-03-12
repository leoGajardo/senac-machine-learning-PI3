using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public class FinalResultData
    {
        public DataTable ReferenceTable { get; private set; }
        public double Sensibility, Specifity, Precision, Recall;
        public double TP, TN, FN, FP;



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


        public int GetBestK()
        {
            return SimpleErrors.GroupBy(se => se.K).OrderBy(g => GetCrossError(g.Key)).First().Key;
        }


        public void PrintResult(FinalResultData finalResult)
        {
            var type = ReferenceTable.Schema.Type;
            if (type == "Multi")
                PrintMultiType(finalResult);

            else
                PrintBinaryType(finalResult);
        }

        private void PrintBinaryType(FinalResultData binaryResult)
        {
            Sensibility = TP / (TP + FN);
            Specifity = TN / (TN + FP);
            Precision = TP / (TP + FP);
            Recall = TP / (TP + FN);

            var print = System.String.Format("Sensibility: {0}  ; Specifity: {1} ; Precision: {2} ; Recall: {3}", Sensibility, Specifity, Precision, Recall);
            File.WriteAllText("result/" + ReferenceTable.fileName + "Result", print);
        }


        private void PrintMultiType(FinalResultData multiTypeResult)
        {



        }

    }
}
