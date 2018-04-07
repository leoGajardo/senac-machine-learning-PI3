using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3.Models
{
    public class NeuronManager
    {

        public Dictionary<int, Neuron[]> Neurons { get; private set; }
        public NeuronManager()
        {
            Neurons = new Dictionary<int, Neuron[]>();
        }

        public Neuron[] GetOrCreateNeurons(int nR, int n, int[] columns, int nWeights, bool shouldClearWeights, bool shouldClearClass)
        {
            Neuron[] neurons;
            if (Neurons.ContainsKey(nR))
                neurons = Neurons[nR];
            else
            {
                int size = (double)(n * 10) % Math.Sqrt(n * 10) == 0 ? (int)(n * 10) : (int)((Math.Floor(Math.Sqrt(n * 10)) + 1) * (Math.Floor(Math.Sqrt(n * 10)) + 1));
                neurons = new Neuron[size];
                var id = 0;
                for (int i = 0; i < size; i++)
                {
                    neurons[i] = new Neuron();
                    neurons[i].Weights = new double[nWeights];
                    neurons[i].Id = id++;
                }

            }

            if (shouldClearWeights)
                neurons = ClearNeuronsWeights(neurons, columns);
            if (shouldClearClass)
                neurons = ClearNeuronClass(neurons);
            Neurons[nR] = neurons;
            return neurons;
        }

        private Neuron[] ClearNeuronsWeights(Neuron[] neurons, int[] columns)
        {
            var rnd = new Random();
            foreach (var neuron in neurons)
                foreach (var col in columns)
                    neuron.Weights[col] = -1 + (rnd.NextDouble() * 2);
            return neurons;
        }

        private Neuron[] ClearNeuronClass(Neuron[] neurons)
        {
            foreach (var neuron in neurons)
                neuron.Class = 0;
            return neurons;
        }
    }
}
