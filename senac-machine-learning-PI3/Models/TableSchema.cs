using System;
using System.Collections.Generic;

namespace senac_machine_learning_PI3.Models
{
    public class TableSchema
    {
        public Dictionary<int, Column> Columns { get; private set; }
        public int TotalOfRecords { get; set; }

        public TableSchema(List<Column> columns)
        {
            int a = 0;
            Columns = new Dictionary<int, Column>();
            foreach (var column in columns)
                Columns.Add(a++, column);
        }

        public string ConvertStringToEnum(Type Enum, string value)
        {
            var EnumValues = Enum.GetEnumValues();
            
            foreach (var item in EnumValues)
            {
                if (value.Trim().ToLower() == Enum.GetEnumName(item).ToLower()) 
                    return ((int)item).ToString();
            }
            return "0";
        }

    }

}