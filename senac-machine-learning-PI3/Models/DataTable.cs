using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace senac_machine_learning_PI3.Models
{
    public class DataTable
    {
        public Dictionary<int,string> ColumnsToKeep { get; private set; }
        public List<int> LinesToKeep { get; private set; }

        public TableSchema Schema { get; private set; }

        private string[] linesOfFile;

        public List<Line> Data { get; set; }
        public DataTable(string filePath, TableSchema schema)
        {
            ColumnsToKeep = new Dictionary<int, string>();
            LinesToKeep = new List<int>();
            
            //Configuring Schema of the Table
            Data = new List<Line>(schema.TotalOfRecords);
            for (int i = 0; i < schema.TotalOfRecords; i++)
            { 
                LinesToKeep.Add(i);
                Data.Add(new Line() { Columns = new string[schema.Columns.Count], Id = i });
            }

            foreach (var column in schema.Columns)
                ColumnsToKeep.Add(column.Key, column.Value.Name);

            this.Schema = schema;

            // Loading File

            linesOfFile = File.ReadAllLines(filePath);

            ParseInternal();
        }

        public void RemoveColumn(int column) => ColumnsToKeep.Remove(column);

        public void RemoveLine(int line) => LinesToKeep.Remove(line);

        public void ConvertEnums()
        {
            foreach (var column in Schema.Columns.Where(c => c.Value.Type == Column.ColumnType.Nominal))
            {
                for (int i = 0; i < Schema.TotalOfRecords; i++)
                {
                    var newVal = Schema.ConvertStringToEnum(Schema.Columns[column.Key].Enum, Data[i].Columns[column.Key]);
                    if (newVal == "0")
                        Console.WriteLine("deu ruim");
                    Data[i].Columns[column.Key] = newVal;
                }
            }
        }

        private void ParseInternal()
        {
            int lineNumber = 0;
            foreach (var line in linesOfFile)
            {
                foreach (var column in Schema.Columns)
                {
                    Data[lineNumber].Columns[column.Key] = line.Split(',')[column.Key];
                }
                lineNumber++;
            }
        }
    }
}
