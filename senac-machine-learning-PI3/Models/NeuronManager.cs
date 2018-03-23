using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3.Models
{
    public class NeuronManager
    {

        public Dictionary<int,Neuron[]> Neurons { get; private set; }

        public Neuron[] GetOrCreateNeurons(int nR, int n, bool shouldClear)
        {
            Neuron[] neurons;
            if (Neurons.ContainsKey(nR))
                neurons = Neurons[nR];
            else
            {
                int size = n * 10 % n * 10 == 0 ? (int)(n * 10) : (int)(Math.Floor(Math.Sqrt(n * 10)) * Math.Floor(Math.Sqrt(n * 10)));
                neurons = new Neuron[size];
                var id = 0;
                foreach (var neuron in neurons)
                    neuron.Id = id++;
            }

            if (!shouldClear)
                return neurons;

            neurons = ClearNeurons(neurons);

            return neurons;
        }

        private Neuron[] ClearNeurons(Neuron[] neurons)
        {
            var rnd = new Random();
            foreach (var neuron in neurons)
                for (int i = 0; i < neuron.Weights.Length; i++)
                    neuron.Weights[i] = rnd.NextDouble();
            return neurons;
        }
    }
}
