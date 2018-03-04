using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3.Models
{
    public class Column
    {
        public ColumnType Type { get; set; }
        public string Name { get; set; }
        public Type Enum { get; set; }
        public enum ColumnType
        {
            Continuous,
            Nominal,
            Integer
        }
    }
}
