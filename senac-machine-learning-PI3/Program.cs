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
            var columnsAbalone = new List<Column> {
                new Column(){ Name= "Sex", Type = Column.ColumnType.Nominal } ,
                new Column(){ Name= "Length", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name= "Diameter", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name= "Height", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name= "Whole weight", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name= "Shucked weight", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name= "Viscera weight", Type = Column.ColumnType.Continuous } ,
                new Column() { Name = "Shell weight", Type = Column.ColumnType.Continuous } ,
                new Column() { Name = "Rings", Type = Column.ColumnType.Integer } };
            var tsAbalone = new TableSchema(columnsAbalone);
            tsAbalone.EnumsByColumn.Add(0, typeof(Enums.Abalone.Sex));
            tsAbalone.TotalOfRecords = 4177;

            var dtAbalone = new Models.DataTable("Data/Abalone.csv", tsAbalone);

            var columnsAdult = new List<Column>            {
                new Column(){Name="Age", Type=Column.ColumnType.Continuous} ,
                new Column(){Name="Workclass", Type=Column.ColumnType.Nominal} ,
                new Column(){Name="Final_Weight", Type=Column.ColumnType.Continuous} ,
                new Column(){Name="Education", Type=Column.ColumnType.Nominal} ,
                new Column(){Name="Education_Num", Type=Column.ColumnType.Continuous} ,
                new Column(){Name="Marital_Status", Type=Column.ColumnType.Nominal} ,
                new Column(){Name="Occupation", Type=Column.ColumnType.Nominal} ,
                new Column(){Name="Relationship", Type=Column.ColumnType.Nominal} ,
                new Column(){Name="Race", Type=Column.ColumnType.Nominal} ,
                new Column(){Name="Sex", Type=Column.ColumnType.Nominal} ,
                new Column(){Name="Capital_Gain", Type=Column.ColumnType.Continuous} ,
                new Column(){Name="Capital_Loss", Type=Column.ColumnType.Continuous} ,
                new Column(){Name="Hours_per_Work", Type=Column.ColumnType.Continuous} ,
                new Column(){Name="Native_Country", Type=Column.ColumnType.Nominal},
            };
            var tsAdult = new TableSchema(columnsAdult);
            tsAdult.EnumsByColumn.Add(1, typeof(Enums.Adult.Workclass));
            tsAdult.EnumsByColumn.Add(3, typeof(Enums.Adult.Education));
            tsAdult.EnumsByColumn.Add(5, typeof(Enums.Adult.Marital_Status));
            tsAdult.EnumsByColumn.Add(6, typeof(Enums.Adult.Occupation));
            tsAdult.EnumsByColumn.Add(7, typeof(Enums.Adult.Relationship));
            tsAdult.EnumsByColumn.Add(8, typeof(Enums.Adult.Race));
            tsAdult.EnumsByColumn.Add(9, typeof(Enums.Adult.Sex));
            tsAdult.EnumsByColumn.Add(13, typeof(Enums.Adult.Native_Country));
            tsAdult.TotalOfRecords = 32561;

            var dtAdult = new Models.DataTable("Data/Adult.csv", tsAdult);
        }
    }
}
