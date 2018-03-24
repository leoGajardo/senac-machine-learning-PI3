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

        //Pega o valor do K que deverá ser implementado
        public static int GetK(int ImplementOfK, DataTable table)
        {
            switch (ImplementOfK)
            {   
                //No caso do projeto, todos os conjuntos de dados tem apenas 1 coluna de classe, assim o M se tornou valor 1 em todos os casos, sendo assim preferivel colocar seu valor final para agilizar o código.
                case 1: return 1;// 1-NN

                case 2: return 3;// (M+2)-NN

                case 3: return 11; //(M*10 + 1)-NN

                case 4: // (Q/2)-NN
                    if (table.Data.Count % 2 == 0) // caso de total de instancias par
                        return (table.Data.Count / 2) + 1;
                    else
                        return (table.Data.Count / 2); // caso de total de instancias impar
            }
            //retorna -1 caso alguma coisa tenha acontecido errado e tenha ignorado todos os casos de Ks anteriores
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
            }

            return calculatedClass;
        }


        //Calcula a distância através da distancia eucliadiana 
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
