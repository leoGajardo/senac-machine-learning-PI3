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


        public void PrintResult()
        {
            var ks = SimpleErrors.Select(se => se.K).Distinct();

            foreach (var k in ks)
            {
                var type = ReferenceTable.Schema.Type;
                PrintHeader(k);
                if (type == "Multi")
                    PrintMultiType(k);

                else
                    PrintBinaryType(k);
            }
            PrintBestK();
        }

        private void PrintBestK()
        {
            File.AppendAllText("result/" + ReferenceTable.fileName + "Result", $"The best K for this situation is {GetBestK()} with error Rate of {GetCrossError(GetBestK())}");
        }

        private void PrintHeader(int k)
        {
            if (k==1)
            File.AppendAllText("result/" + ReferenceTable.fileName + "Result", $"Running K = 1-NN for file {ReferenceTable.fileName}");

            else if (k == 3)
                File.AppendAllText("result/" + ReferenceTable.fileName + "Result", $"Running K = (M+2)-NN for file {ReferenceTable.fileName}");

            else if (k==11)
                File.AppendAllText("result/" + ReferenceTable.fileName + "Result", $"Running K = (M*10+1)-NN for file {ReferenceTable.fileName}");

            else
                File.AppendAllText("result/" + ReferenceTable.fileName + "Result", $"Running K = (Q/2+1)-NN or (Q/2)-NN for file {ReferenceTable.fileName}");

        }

        private void PrintBinaryType(int k)
        {
            SetInternalStatistics(k);

            Sensibility = TP / (TP + FN);
            Specifity = TN / (TN + FP);
            Precision = TP / (TP + FP);
            Recall = TP / (TP + FN);

            var print = System.String.Format("Sensibility: {0}  ; Specifity: {1} ; Precision: {2} ; Recall: {3}", Sensibility, Specifity, Precision, Recall);
            File.AppendAllText("result/" + ReferenceTable.fileName + "Result", print);
        }

        private void SetInternalStatistics(int k)
        {
            var predictions = SimpleErrors.Where(se => se.K == k);
            TP = predictions.Sum(se => se.Predictions.Count(p => p.ExpectedClassNumber == p.PreviewedClassNumber && p.PreviewedClassNumber == 1));
            FP = predictions.Sum(se => se.Predictions.Count(p => p.PreviewedClassNumber == 1 && p.ExpectedClassNumber == 0));
            TN = predictions.Sum(se => se.Predictions.Count(p => p.ExpectedClassNumber == p.PreviewedClassNumber && p.PreviewedClassNumber == 0));
            FN = predictions.Sum(se => se.Predictions.Count(p => p.PreviewedClassNumber == 0 && p.ExpectedClassNumber == 1));
        }

        private void PrintMultiType(int k)
        {
            var enumValues = ReferenceTable.Schema.Columns.First(c => c.Value.Type == Column.ColumnType.Class).Value.Enum.GetEnumValues();
            var values = SimpleErrors.Where(se => se.K == k).SelectMany(s => s.Predictions);
            StringBuilder header = new StringBuilder();
            var lines = new List<string>();
            foreach (var enumNamePredicted in enumValues)
                header.Append(enumNamePredicted.ToString() + "\t\t");

            foreach (var enumNameExpected in enumValues)
            {
                StringBuilder line = new StringBuilder();
                foreach (var enumNamePreviewed in enumValues)
                {
                    var val = values.Count(v => v.ExpectedClass == enumNameExpected.ToString() && v.PreviewedClass == enumNamePreviewed.ToString());
                    line.Append(val + "\t\t");
                }
                lines.Add(line.ToString());
            }

            File.AppendAllText("result/" + ReferenceTable.fileName + "Result", header.ToString());
            File.AppendAllLines("result/" + ReferenceTable.fileName + "Result", lines);
        }

    }
}
