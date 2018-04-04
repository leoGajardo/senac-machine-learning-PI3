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
        private static double learningRate;
        private static double stdDeviation;
        public static void Run(List<Line> trainData, List<Line> testData, int[] columnsToCompare, int nColumns, int nR, int classColumn, ref FinalResultData results)
        {
            learningRate = 0.1;

            var numberClass = results.ReferenceTable.Schema.Columns[classColumn]?.Enum?.GetEnumValues()?.Length ?? results.ReferenceTable.Schema.TotalOfClassValues;

            if (neuronManager == null)
                neuronManager = new NeuronManager();
            var neurons = neuronManager.GetOrCreateNeurons(nR, numberClass, columnsToCompare, nColumns, true, true);

            var r = GetR(nR, neurons);
            stdDeviation = r;
            TrainNeurons(trainData, columnsToCompare, classColumn, ref neurons);

            foreach (var data in testData)
            {

            }
        }

        private static void TrainNeurons(List<Line> trainData, int[] columns, int classColumn, ref Neuron[] neurons)
        {
            for (int iteration = 0; iteration < 500; iteration++)
            {
                //Deste modo esta rodando 500 iteraçoes e em cada uma ele checa todas instancias
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
                    UpdateBestMatchNeuron(nearNeuron, data, iteration, classColumn, columns);
                    //UpdateNeighboursOfBestMatchNeurons(neurons, data)
                }
                UpdateLearningRate(iteration);
                UpdateStdDeviation(iteration);
            }
        }

        private static void UpdateBestMatchNeuron(Neuron neuron, Line Data, int iteration, int classColumn, int[] columns)
        {
            if (neuron.Class == 0 || neuron.Class == Int32.Parse(Data.Columns[classColumn]))
            {
                for (int i = 0; i < neuron.Weights.Count(); i++)
                    neuron.Weights[i] = neuron.Weights[i] + learningRate * GetH(columns, Data.getColumnsAsDouble(), neuron.Weights, iteration) * (neuron.Weights[i] - Data.getColumnsAsDouble()[i]);
            }
            else
            {
                for (int i = 0; i < neuron.Weights.Count(); i++)
                    neuron.Weights[i] = neuron.Weights[i] - learningRate * GetH(columns, Data.getColumnsAsDouble(), neuron.Weights, iteration) * (neuron.Weights[i] - Data.getColumnsAsDouble()[i]);
            }
        }

        private static void UpdateNeighboursOfBestMatchNeurons(Neuron[] neurons, Line Data, int line, int column)
        {
            // fazer lista de neurons proximos e depois fazer update neles
        }

        private static double GetH(int[] columns, double[] x, double[] neuron, int n)
        {
            double power = 0 ;
            try
            {
                power = Math.Pow(GetDistance(columns, x, neuron), 2) / (2 * Math.Pow(stdDeviation, 2)); 
            }
            catch (DivideByZeroException)
            {
                power = 0;
            }
            return Math.Exp(-power);
        }

        private static void UpdateStdDeviation(int n)
        {
            stdDeviation = stdDeviation * Math.Exp((-n * Math.Log(stdDeviation, 10)) / 1000);
        }
        private static void UpdateLearningRate(int n)
        {
            if (learningRate == 0.1)
                return;
            var newVal = learningRate * Math.Exp(-n / 1000);
            if (newVal >= 0.1)
                learningRate = newVal;
            else
                learningRate = 0.1;
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
