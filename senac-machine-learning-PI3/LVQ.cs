using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public static class LVQ
    {
        public static NeuronManager neuronManager;
        //Fazer estrutura para neuronios
        //calcular neuronios
        //intanciar neuronios
        //Treinar dados
	       // 3 formulas
        //Fazer previsões

        public static void Run(List<Line> trainData, List<Line> testData, int[] columns, int nR, int classColumn, ref FinalResultData results)
        {
            var numberClass = results.ReferenceTable.Schema.Columns[classColumn]?.Enum?.GetEnumValues()?.Length ?? results.ReferenceTable.Schema.TotalOfClassValues;

            if (neuronManager == null)
                neuronManager = new NeuronManager();
            var neurons = neuronManager.GetOrCreateNeurons(nR, numberClass, true);

            var r = GetR(nR, neurons);

            TrainNeurons(trainData, columns, classColumn, ref neurons);

            foreach (var data in testData)
            {

            }
        }

        private static void TrainNeurons(List<Line> trainData, int[] columns, int classColumn, ref Neuron[] neurons)
        {
            foreach (var data in trainData)
            {
                var distances = new Dictionary<int, LighweightData>();
                foreach (var neuron in neurons)
                {
                    var distance = GetDistance(columns, data.getColumnsAsDouble(), neuron.Weights);
                    distances.Add(neuron.Id, new LighweightData(distance, (int)data.getColumnsAsDouble()[classColumn]));
                }
                var nearNeuronId = distances.OrderBy(d => d.Value.distance).First().Key;
                var nearNeuron = neurons.First(n => n.Id == nearNeuronId);
                UpdateNearNeuron(nearNeuron, data);
                //UpdateNearNeurons(neurons, data)
            }
        }

        private static void UpdateNearNeuron(Neuron neuron, Line Data)
        {

        }

        private static void UpdateNearNeurons(Neuron[] neurons, Line Data, int line, int column)
        {

        }

        private static int GetR(int ImplementOfR, Neuron[] neurons)
        {
            switch (ImplementOfR)
            {
                case 1: return 1;

                case 2: return 3;

                case 3: return neurons.Length / 2;

                case 4: return neurons.Length;
            }

            return -1;
        }

        private static int CalculateLine(List<Line> trainData, Line testData, int[] columns, int k, int classColumn)
        {
            return 0;
        }

        private static double GetDistance(int[] columns, double[] a, double[] b)
        {
            double distancia = 0;
            foreach (var column in columns)
                distancia += Math.Pow((a[column] - b[column]), 2);

            return Math.Sqrt(distancia);
        }
    }
}
