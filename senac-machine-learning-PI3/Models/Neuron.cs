using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3.Models
{
    public class Neuron
    {
        public int Id { get; set; }
        public int Class { get; set; }
        public double[] Weights { get; set; }
        public string Color { get; set; }
    }

    public static class Extension {
        public static Neuron GetNeuron(this Neuron[] neurons, int line, int col)
        {
            return neurons[(int)Math.Sqrt(neurons.Count()) * line + col];
        }
    }

}
