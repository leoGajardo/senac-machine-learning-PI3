using System;
using System.Collections.Generic;

namespace senac_machine_learning_PI3.Models
{
    public class TableSchema
    {
        public Dictionary<int, Column> Columns { get; private set; }
        public Dictionary<int, Type> EnumsByColumn { get; set; }
        public int TotalOfRecords { get; set; }

        public TableSchema(List<Column> columns)
        {
            int a = 0;
            Columns = new Dictionary<int, Column>();
            EnumsByColumn = new Dictionary<int, Type>();
            foreach (var column in columns)
                Columns.Add(a++, column);
        }

        public string ConvertStringToEnum(Type Enum, string value)
        {
            var EnumValues = Enum.GetEnumValues();
            
            foreach (var item in EnumValues)
            {
                if (value == Enum.GetEnumName(item)) 
                    return ((int)item).ToString();
            }
            return "0";
        }

    }

}