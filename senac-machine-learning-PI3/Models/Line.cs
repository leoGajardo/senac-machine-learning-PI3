using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3.Models
{
    public class Line
    {
        public int Id { get; set; }
        public string[] Columns { get; set; }

        public override string ToString() => string.Join("/", Columns);

    }
}
