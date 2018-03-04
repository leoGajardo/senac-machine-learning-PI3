using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public static class NormalizeData
    {
        public static DataTable NomalizeData(this DataTable data)
        {
            foreach (var column in data.Schema.Columns.Where(c => c.Value.Type != Column.ColumnType.Nominal))
            {
                var maxVal = data.Data.Max(d => double.Parse(d.Columns[column.Key]));
                var minVal = data.Data.Min(d => double.Parse(d.Columns[column.Key]));
                foreach (var line in data.Data)
                {
                    var val = double.Parse(line.Columns[column.Key]);
                    var newVal = (val - minVal) / (maxVal - minVal);
                    line.Columns[column.Key] = newVal.ToString();
                }
            }
            return data;
        }
    }
}
