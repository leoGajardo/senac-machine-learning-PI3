using senac_machine_learning_PI3.Enums;
using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    class Program
    {
        static void Main(string[] args)
        {
            var columns = new string[] { "Sex", "Length", "Diameter", "Height", "Whole weight", "Shucked weight", "Viscera weight", "Shell weight", "Rings" };
            var ts = new TableSchema(columns);
            ts.EnumsByColumn.Add(0, typeof(Enums.Abalone.Sex));
            ts.TotalOfRecords = 4177;

            var dt = new Models.DataTable("Data/Abalone.csv", ts);

        }
    }
}
