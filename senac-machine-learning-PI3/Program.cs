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

        private static List<Models.DataTable> tables;

        static void Main(string[] args)
        {
            BuildSchemaTables();
            foreach (var table in tables)
            {
                table.ConvertEnums();

                var teste = table.Data.OrderBy(o => o.Columns[0]);
            }
        }

        private static void BuildSchemaTables()
        {
            tables = new List<Models.DataTable>();
            var AdultColumns = new List<Column> {
                new Column(){ Name = "age", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "workclass", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Workclass) } ,
                new Column(){ Name = "fnlwgt", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "education", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Education) } ,
                new Column(){ Name = "education-num", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "marital-status", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Marital_Status) } ,
                new Column(){ Name = "occupation", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Occupation) } ,
                new Column(){ Name = "relationship", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Relationship) } ,
                new Column(){ Name = "race", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Race) } ,
                new Column(){ Name = "sex", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Sex) } ,
                new Column(){ Name = "capital-gain", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "capital-loss", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "hours-per-week", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "native-country", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Native_Country) }
                 };
            var AdultSchema = new TableSchema(AdultColumns);
            AdultSchema.TotalOfRecords = 32561;

            tables.Add(new Models.DataTable("Data/adult.csv", AdultSchema));
        }
    }
}
