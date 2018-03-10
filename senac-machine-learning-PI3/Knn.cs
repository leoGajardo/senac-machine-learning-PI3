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

        public static void Run(List<Line> trainData, List<Line> testData, int[] columns, int k)
        {

        }

        public static void CalculateLine(List<Line> trainData, Line testData, int[] columns, int k, int classColumn)
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
            var calculatedClass = "";
            if (matches == 1)
            {
                calculatedClass = neighboursGrouped.First(f => f.Count() == maxVal).First().Columns[classColumn];
            }
            else if(matches > 1)
            {
                // Regra para desempate
            }


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
