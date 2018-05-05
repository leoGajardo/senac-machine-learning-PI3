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
            //Normaliza cada uma das colunas da tabela que não sejam do tipo da classe
            foreach (var column in data.Schema.Columns.Where(c => c.Value.Type != Column.ColumnType.Class))
            {
                var maxVal = data.Data.Max(d => double.Parse(d.Columns[column.Key])); // recebe o maior valor daquela coluna
                var minVal = data.Data.Min(d => double.Parse(d.Columns[column.Key]));// recebe o menor valor daquela coluna

                //exectua o código  para todas as linhas da massa de dados
                foreach (var line in data.Data)
                {
                    var val = double.Parse(line.Columns[column.Key]); // recebe o valor daquela linha
                    var newVal = (val - minVal) / (maxVal - minVal); // calcula o novo valor normalizado daquela linha, considerando o valor subtraido pelo valor minimo sendo divido pelo valor maximo subtraido do minimo
                    line.Columns[column.Key] = newVal.ToString();//salva o novo valor normalizado no lugar do antigo valor naquela coluna naquela linha
                }
            }
            return data;
        }
    }
}
