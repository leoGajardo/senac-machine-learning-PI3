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
            var shouldBeRemoved = new List<int>();
            foreach (var line in data.Data)
                foreach (var value in line.Columns)
                    if (value.Trim().ToLower() == "NULL".ToLower())
                        shouldBeRemoved.Add(line.Id);

            //Necessário fazer separado, pois mexer no IEnumerable durante o foreach quebra o iterador
            foreach (var id in shouldBeRemoved)
                data.RemoveLine(id);

            return data;
        }
    }
}
