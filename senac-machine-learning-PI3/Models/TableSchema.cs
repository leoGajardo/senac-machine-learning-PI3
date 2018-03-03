﻿using System;
using System.Collections.Generic;

namespace senac_machine_learning_PI3.Models
{
    public class TableSchema
    {
        public Dictionary<int, string> Columns { get; private set; }
        public Dictionary<int, Type> EnumsByColumn { get; set; }
        public int TotalOfRecords { get; set; }

        public TableSchema(List<string> columns)
        {
            int a = 0;
            foreach (var column in columns)
                Columns.Add(a++, column);
        }

        public int ConvertStringToEnum(Type Enum, string value)
        {
            var EnumValues = Enum.GetEnumValues();

            foreach (var item in EnumValues)
            {
                if (value == Enum.GetEnumName(item)) 
                    return (int)item;
            }
            return 0;
        }

    }

}