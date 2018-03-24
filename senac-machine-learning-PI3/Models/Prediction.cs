using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3.Models
{
    public class Prediction
    {
        //Guarda as predições de uma instância, guardando qual é a classe esperada e qual a classe predizida, guardando os valores em string e em int, dados os valores do enum,
        public string ExpectedClass { get; set; }
        public string PreviewedClass { get; set; }

        public int ExpectedClassNumber { get; set; }

        public int PreviewedClassNumber { get; set; }

    }
}
