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

            //recebe os valores dos enums
            var EnumValues = results.ReferenceTable.Schema.Columns[classColumn].Enum?.GetEnumValues();

            var numberClass = EnumValues?.Length ?? results.ReferenceTable.Schema.TotalOfClassValues;

            if (neuronManager == null)
                neuronManager = new NeuronManager();
            var neurons = neuronManager.GetOrCreateNeurons(nR, numberClass, columnsToCompare, nColumns, true, true);

            var r = GetR(nR, neurons);
            stdDeviation = r;

            //cria uma instância do modelo de SimpleError que irá guardar as predições número de erros, total de predições e o valor de erro para poder fazer os calculos posteriores de erro
            var simpleError = new SimpleError(r);

            TrainNeurons(trainData, columnsToCompare, classColumn, ref neurons, r);

            foreach (var data in testData)
            {
                var result = CalculateLine(neurons.ToList(), data, columnsToCompare, classColumn);

                //Verifica qual é a classe esperada para aquela linha e atribui para a classe esperada.
                var expectedClass = EnumValues != null ? EnumValues.GetValue(Int32.Parse(data.Columns[classColumn]) - 1).ToString() : data.Columns[classColumn];

                //Atribui a classe predizida
                var previewedClass = EnumValues != null ? EnumValues?.GetValue((int)result - 1).ToString() : result.ToString();

                //Cria um objeto Predição do nosso modelo de Prection, no qual contém a classe real e a classe predizida com seus valores em string e inteiro
                simpleError.Predictions.Add(
                    new Prediction()
                    {
                        ExpectedClass = expectedClass,
                        PreviewedClass = previewedClass,
                        ExpectedClassNumber = Int32.Parse(data.Columns[classColumn]),
                        PreviewedClassNumber = (int)result
                    });
            }

            results.SimpleErrors.Add(simpleError);
        }

        private static void TrainNeurons(List<Line> trainData, int[] columns, int classColumn, ref Neuron[] neurons, int r)
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
                    UpdateNeighboursOfBestMatchNeurons(neurons, nearNeuron, data, r, columns, iteration, classColumn);
                }
                UpdateLearningRate(iteration);
                UpdateStdDeviation(iteration);
            }
        }

        private static void UpdateBestMatchNeuron(Neuron neuron, Line Data, int iteration, int classColumn, int[] columns)
        {
            if (neuron.Class == 0)
                neuron.Class = Int32.Parse(Data.Columns[classColumn]);

            if (neuron.Class == Int32.Parse(Data.Columns[classColumn]))
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

        private static void UpdateNeighboursOfBestMatchNeurons(Neuron[] neurons, Neuron bestMatch, Line Data, int r, int[] columns, int iteration, int classColumn)
        {
            var size = (int)Math.Sqrt(neurons.Count());
            var index = neurons.ToList().IndexOf(bestMatch);

            int bestMatchLine = (int)Math.Floor((double)(index / size));
            int bestMatchColumn = index % size;

            var neighbours = new List<Neuron>();
            for (int col = 0; col < size; col++)
            {
                for (int line = 0; line < size; line++)
                {
                    var distance = Math.Sqrt(Math.Pow((bestMatchLine - line), 2) + Math.Pow((bestMatchColumn - col), 2));
                    if (distance <= r)
                        neighbours.Add(neurons.GetNeuron(line, col));
                }
            }

            foreach (var neighbour in neighbours)
            {
                if (bestMatch.Class == Int32.Parse(Data.Columns[classColumn]))
                {
                    for (int i = 0; i < neighbour.Weights.Count(); i++)
                        neighbour.Weights[i] = neighbour.Weights[i] + learningRate * GetH(columns, Data.getColumnsAsDouble(), neighbour.Weights, iteration) * (neighbour.Weights[i] - Data.getColumnsAsDouble()[i]);
                }
                else
                {
                    for (int i = 0; i < neighbour.Weights.Count(); i++)
                        neighbour.Weights[i] = neighbour.Weights[i] - learningRate * GetH(columns, Data.getColumnsAsDouble(), neighbour.Weights, iteration) * (neighbour.Weights[i] - Data.getColumnsAsDouble()[i]);
                }
            }
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
            if (learningRate == 0.01)
                return;
            var newVal = learningRate * Math.Exp(-n / 1000);
            if (newVal >= 0.01)
                learningRate = newVal;
            else
                learningRate = 0.01;
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

        private static int CalculateLine(List<Neuron> neurons, Line testData, int[] columns, int classColumn)
        {
            //cria um dicionario para guardar as distancias
            var distances = new Dictionary<int, LighweightData>();

            int[] maxValArray = new int[20];

            foreach (var neuron in neurons)
            {
                //calcula as distancias e guarda cada uma delas
                var distance = GetDistance(columns, testData.getColumnsAsDouble(), neuron.Weights);
                distances.Add(neuron.Id, new LighweightData(distance, neuron.Class));
            }
            //calcula os vizinhos feito uma ordenação pelas distancias
            var prediction = distances.OrderBy(o => o.Value.distance).Take(1).First().Value.classVal;
            return prediction;
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
