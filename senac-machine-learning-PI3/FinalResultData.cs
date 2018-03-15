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
        public double Sensibility, Specifity, Precision, Recall, Accuracy;
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
            if (Directory.Exists("result/"))
                File.Delete("result/" + ReferenceTable.fileName);
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
            File.AppendAllText("result/" + ReferenceTable.fileName, $"The best K for this situation is {GetBestK()} with error Rate of {GetCrossError(GetBestK())} ");
        }

        private void PrintHeader(int k)
        {
            if (k == 1)
                File.AppendAllText("result/" + ReferenceTable.fileName, $"Running K = 1-NN for file {ReferenceTable.fileName} \n\r");

            else if (k == 3)
                File.AppendAllText("result/" + ReferenceTable.fileName, $"Running K = (M+2)-NN for file {ReferenceTable.fileName} \n\r");

            else if (k == 11)
                File.AppendAllText("result/" + ReferenceTable.fileName, $"Running K = (M*10+1)-NN for file {ReferenceTable.fileName} \n\r");

            else
                File.AppendAllText("result/" + ReferenceTable.fileName, $"Running K = (Q/2+1)-NN or (Q/2)-NN for file {ReferenceTable.fileName} \n\r");

        }

        private void PrintBinaryType(int k)
        {
            SetInternalStatistics(k);

            Sensibility = (double)TP / ((double)TP + (double)FN);
            Specifity = (double)TN / ((double)TN + (double)FP);
            Precision = (double)TP / ((double)TP + (double)FP);
            Recall = (double)TP / ((double)TP + (double)FN);
            Accuracy = ((double)TP + (double)TN) / ((double)TP + (double)TN + (double)FN + (double)FP);

            var print = System.String.Format("Sensibility: {0}  ; Specifity: {1} ; Precision: {2} ; Recall: {3} ; Accuracy: {4}\n\r", Sensibility, Specifity, Precision, Recall, Accuracy);
            File.AppendAllText("result/" + ReferenceTable.fileName, print);
        }

        private void SetInternalStatistics(int k)
        {
            var predictions = SimpleErrors.Where(se => se.K == k);
            TP = predictions.Sum(se => se.Predictions.Count(p => p.ExpectedClassNumber == p.PreviewedClassNumber && p.PreviewedClassNumber == 1));
            FP = predictions.Sum(se => se.Predictions.Count(p => p.PreviewedClassNumber == 1 && p.ExpectedClassNumber == 2));
            TN = predictions.Sum(se => se.Predictions.Count(p => p.ExpectedClassNumber == p.PreviewedClassNumber && p.PreviewedClassNumber == 2));
            FN = predictions.Sum(se => se.Predictions.Count(p => p.PreviewedClassNumber == 2 && p.ExpectedClassNumber == 1));
        }

        private void PrintMultiType(int k)
        {
            var classColumn = ReferenceTable.Schema.Columns.First(c => c.Value.Type == Column.ColumnType.Class);
            var enumValues = classColumn.Value.Enum?.GetEnumValues();
            if (enumValues == null)
                enumValues = ReferenceTable.Data.SelectMany(s => s.Columns[classColumn.Key]).Distinct().OrderBy(o => o).ToArray();
            var values = SimpleErrors.Where(se => se.K == k).SelectMany(s => s.Predictions);
            StringBuilder header = new StringBuilder($"\t\t");
            var lines = new List<string>();
            foreach (var enumNamePredicted in enumValues)
                header.Append(enumNamePredicted.ToString() + "\t\t");

            foreach (var enumNameExpected in enumValues)
            {
                StringBuilder line = new StringBuilder(enumNameExpected.ToString() + "\t\t");
                foreach (var enumNamePreviewed in enumValues)
                {
                    var val = values.Count(v => v.ExpectedClass == enumNameExpected.ToString() && v.PreviewedClass == enumNamePreviewed.ToString());
                    line.Append(val + "\t\t");
                }
                lines.Add(line.ToString() + $"\n\r");
            }

            File.AppendAllText("result/" + ReferenceTable.fileName, header.ToString() + $"\n\r");
            File.AppendAllLines("result/" + ReferenceTable.fileName, lines);
            var accuracy = ((double)SimpleErrors.Where(se => se.K == k).Sum(s => s.NumOfRecords) - (double)SimpleErrors.Where(se => se.K == k).Sum(s => s.NumOfErrors)) / (double)SimpleErrors.Where(se => se.K == k).Sum(s => s.NumOfRecords);
            File.AppendAllText("result/" + ReferenceTable.fileName, $"Accuracy of {accuracy} \n\r");
        }

    }
}
