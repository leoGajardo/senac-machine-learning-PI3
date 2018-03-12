using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public static class Knn
    {

        public static void Run(List<Line> trainData, List<Line> testData, int[] columns, int k, int classColumn, ref FinalResultData results)
        {
            var rnd = new Random();
            var randomTrainData = trainData.OrderBy(o => rnd.Next()).ToList();
            rnd = new Random();
            var randomTestData = testData.OrderBy(o => rnd.Next()).ToList();
            var simpleError = new SimpleError(k);

            var EnumValues = results.ReferenceTable.Schema.Columns[classColumn].Enum.GetEnumValues();
            
            foreach (var data in randomTestData)
            {
                var result = CalculateLine(randomTrainData, data, columns, k, classColumn);
                simpleError.Predictions.Add(
                    new Prediction()
                    {
                        ExpectedClass = EnumValues.GetValue(Int32.Parse(data.Columns[classColumn])).ToString(),
                        PreviewedClass = EnumValues.GetValue((int)result).ToString(),
                        ExpectedClassNumber = Int32.Parse(data.Columns[classColumn]),
                        PreviewedClassNumber = (int)result
                    });
            }

            results.SimpleErrors.Add(simpleError);
        }

        public static int GetK(int ImplementOfK, DataTable table)
        {
            switch(ImplementOfK)
            {
                case 1: return 1;

                case 2: return 3;

                case 3: return 11;

                case 4:
                    if (table.Data.Count % 2 == 0)
                        return (table.Data.Count / 2) + 1;
                    else
                        return (table.Data.Count / 2);
            }

            return -1;
        }


        private static double CalculateLine(List<Line> trainData, Line testData, int[] columns, int k, int classColumn)
        {
            var distances = new Dictionary<int, double>();
            foreach (var baseData in trainData)
            {
                var distance = GetDistance(columns, testData.getColumnsAsDouble(), baseData.getColumnsAsDouble());
                distances.Add(baseData.Id, distance);
            }
            var ordenedDistances = distances.OrderByDescending(o => o.Value);
            var neighbours = ordenedDistances.Take(k).Select(n => n.Key);

            var neighboursLines = trainData.Where(t => neighbours.Contains(t.Id));
            var neighboursGrouped = neighboursLines.GroupBy(g => g.Columns[classColumn]);
            var maxVal = neighboursGrouped.Max(m => m.Count());
            var matches = neighboursGrouped.Count(c => c.Count() == maxVal);
            double calculatedClass = 0;
            if (matches == 1)
            {
                calculatedClass = neighboursGrouped.First(f => f.Count() == maxVal).First().getColumnsAsDouble()[classColumn];
            }
            else if(matches > 1)
            {
                calculatedClass = neighboursGrouped.OrderBy(m => m.Sum(s => distances[s.Id])).First().First().getColumnsAsDouble()[classColumn];
            }

            return calculatedClass;
        }
         

        private static double GetDistance(int[] columns, double[] a, double[] b)
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
