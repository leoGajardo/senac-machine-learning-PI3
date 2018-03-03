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
            var columns = new List<Column> {
                new Column(){ Name= "Sex", Type = Column.ColumnType.Nominal } ,
                new Column(){ Name= "Length", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name= "Diameter", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name= "Height", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name= "Whole weight", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name= "Shucked weight", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name= "Viscera weight", Type = Column.ColumnType.Continuous } ,
                new Column() { Name = "Shell weight", Type = Column.ColumnType.Continuous } ,
                new Column() { Name = "Rings", Type = Column.ColumnType.Integer } };
            var ts = new TableSchema(columns);
            ts.EnumsByColumn.Add(0, typeof(Enums.Abalone.Sex));
            ts.TotalOfRecords = 4177;

            var dt = new Models.DataTable("Data/Abalone.csv", ts);

        }
    }
}
