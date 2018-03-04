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

            var AbaloneColumns = new List<Column> {
                new Column(){ Name = "Sex", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Abalone.Sex) } ,
                new Column(){ Name = "Length", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "Diameter", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "Height", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "Whole weight", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "Shucked weight", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "Viscera weight", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "Shell weight", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "Rings", Type = Column.ColumnType.Integer }
            };
            var AbaloneSchema = new TableSchema(AbaloneColumns);
            AbaloneSchema.TotalOfRecords = 4177;
            tables.Add(new Models.DataTable("Data/abalone.csv", AbaloneSchema));


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

            var BreastCancerColumns = new List<Column>
            {
                new Column(){ Name ="ID", Type=Column.ColumnType.Integer } ,
                new Column(){ Name ="Diagonis", Type=Column.ColumnType.Nominal, Enum = typeof(Enums.BreastCancer.Diagnosis) } ,
                new Column(){ Name ="Radius_Cell-1",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Texture_Cell-1",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Perimeter_Cell-1",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Area_Cell-1",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Smoothness_Cell-1",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Compactness_Cell-1",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Concavity_Cell-1",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Concave-Points_Cell-1",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Symmetry_Cell-1",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Fractal-Dimension_Cell-1",Type=Column.ColumnType.Continuous} ,

                new Column(){ Name ="Radius_Cell-2",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Texture_Cell-2",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Perimeter_Cell-2",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Area_Cell-2",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Smoothness_Cell-2",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Compactness_Cell-2",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Concavity_Cell-2",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Concave-Points_Cell-2",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Symmetry_Cell-2",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Fractal-Dimension_Cell-2",Type=Column.ColumnType.Continuous} ,

                new Column(){ Name ="Radius_Cell-3",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Texture_Cell-3",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Perimeter_Cell-3",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Area_Cell-3",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Smoothness_Cell-3",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Compactness_Cell-3",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Concavity_Cell-3",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Concave-Points_Cell-3",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Symmetry_Cell-3",Type=Column.ColumnType.Continuous} ,
                new Column(){ Name ="Fractal-Dimension_Cell-3",Type=Column.ColumnType.Continuous} ,
            };
            var BreastCancerSchema = new TableSchema(BreastCancerColumns);
            BreastCancerSchema.TotalOfRecords = 569;
            tables.Add(new Models.DataTable("Data/BreastCancer.csv", BreastCancerSchema));

            var IrisColumns = new List<Column>
            {
                new Column(){ Name ="Sepal-Lengh", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Sepal-Width", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Petal-Lengh", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Petal-Width", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Class", Type=Column.ColumnType.Nominal, Enum = typeof(Enums.Iris.IrisType) } ,
            };
            var IrisSchema = new TableSchema(IrisColumns);
            IrisSchema.TotalOfRecords = 150;
            tables.Add(new Models.DataTable("Data/Iris.csv", IrisSchema));

            var Wine = new List<Column>
            {
                new Column(){ Name ="Alcohol", Type=Column.ColumnType.Integer } ,
                new Column(){ Name ="Malic-Acid", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Ash", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Alcalinity-of-Ash", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Magnesium", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Total-Phenols", Type=Column.ColumnType.Integer } ,
                new Column(){ Name ="Flavanoids", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="NonFlavanoids-Phenols", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Proanthocyanins", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Color-Intensity", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Hue", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="OD280-OD315-of-diluted-wines", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Proline", Type=Column.ColumnType.Integer } ,
            };
        }
    }
}
