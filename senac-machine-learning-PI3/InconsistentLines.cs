using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public static class InconsistentLines
    {

        public static DataTable RemoveInconsistentLines(this DataTable data)
        {

            foreach (var line in data.Data)
                foreach (var value in line.Columns)
                    if (value.Trim().ToLower() == "NULL".ToLower())
                        data.RemoveLine(line.Id);

            return data;
        }

    }
}
