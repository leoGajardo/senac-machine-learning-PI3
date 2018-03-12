﻿using senac_machine_learning_PI3.Models;
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
            File.AppendAllText("result/" + ReferenceTable.fileName + "Result", $"Running K = {k} for file {ReferenceTable.fileName}");
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



        }

    }
}
