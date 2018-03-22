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
            var simpleError = new SimpleError(k);

            var EnumValues = results.ReferenceTable.Schema.Columns[classColumn].Enum?.GetEnumValues();

            var task = Parallel.ForEach(testData, (data) =>
            {
                var result = CalculateLine(trainData, data, columns, k, classColumn);
                var expectedClass = EnumValues != null ? EnumValues.GetValue(Int32.Parse(data.Columns[classColumn]) - 1).ToString() : data.Columns[classColumn];
                var previewedClass = EnumValues != null ? EnumValues?.GetValue((int)result - 1).ToString() : result.ToString();
                simpleError.Predictions.Add(
                    new Prediction()
                    {
                        ExpectedClass = expectedClass,
                        PreviewedClass = previewedClass,
                        ExpectedClassNumber = Int32.Parse(data.Columns[classColumn]),
                        PreviewedClassNumber = (int)result
                    });
            });

            while (!task.IsCompleted)
            { }

            results.SimpleErrors.Add(simpleError);
        }

        public static int GetK(int ImplementOfK, DataTable table)
        {
            switch (ImplementOfK)
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
            var distances = new Dictionary<int, LighweightData>();

            int[] maxValArray = new int[20];

            foreach (var baseData in trainData)
            {
                var distance = GetDistance(columns, testData.getColumnsAsDouble(), baseData.getColumnsAsDouble());
                distances.Add(baseData.Id, new LighweightData(distance, Int32.Parse(baseData.Columns[classColumn])));
            }

            var neighbours = distances.OrderBy(o => o.Value.distance).Take(k);
            foreach (var neighbour in neighbours)
                maxValArray[neighbour.Value.classVal] += 1;
            //var neighbours = ordenedDistances.Take(k).Select(n => n.Key);
            
            //var neighboursLines = trainData.Where(t => neighbours.Contains(t.Id));
            //var neighboursGrouped = neighboursLines.GroupBy(g => g.Columns[classColumn]).OrderByDescending(o => o.Count());
            //var maxVal = neighboursGrouped.First().Count();
            var maxVal = maxValArray.Max();
            var matches = maxValArray.Count(c => c == maxVal);
            double calculatedClass = 0;
            if (matches == 1)
            {
                calculatedClass = Array.IndexOf(maxValArray, maxVal);
            }
            else if (matches > 1)
            {
                calculatedClass = neighbours.GroupBy(g => g.Value.classVal).Where(d => d.Count() == maxVal).OrderBy(m => m.Sum(s => s.Value.distance)).First().Key;
                //calculatedClass = neighboursGrouped.Where(n => n.Count() == maxVal).OrderBy(m => m.Sum(s => distances[s.Id])).First().First().getColumnsAsDouble()[classColumn];
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

    internal class LighweightData{

        public LighweightData(double distance, int classVal)
        {
            this.distance = distance;
            this.classVal = classVal;
        }

        public double distance { get; set; }
        public int classVal { get; set; }

    }
}
