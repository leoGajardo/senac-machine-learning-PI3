using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace senac_machine_learning_PI3.Models
{
    class DataTable
    {
        public Dictionary<int,string> ColumnsToKeep { get; private set; }
        public List<int> LinesToKeep { get; private set; }

        public TableSchema Schema { get; private set; }

        private string[] linesOfFile;

        public string[,] Data { get; set; }
        public DataTable(string filePath, TableSchema schema)
        {
            //Configuring Schema of the Table
            for (int i = 0; i < schema.TotalOfRecords; i++)
                LinesToKeep.Add(i);

            Data = new string[schema.TotalOfRecords, schema.Columns.Count];

            foreach (var column in schema.Columns)
                ColumnsToKeep.Add(column.Key, column.Value);

            this.Schema = schema;

            // Loading File

            linesOfFile = File.ReadAllLines(filePath);

            ParseInternal();
        }

        public void RemoveColumn(int column) => ColumnsToKeep.Remove(column);

        public void RemoveLine(int line) => LinesToKeep.Remove(line);

        private void ParseInternal()
        {
            int lineNumber = 0;
            foreach (var line in linesOfFile)
            {
                foreach (var column in Schema.Columns)
                {
                    Data[column.Key, lineNumber++] = line.Split(',')[column.Key];
                }
            }
        }


    }
}
