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
                table.RemoveInconsistentLines();
                foreach (var column in table.Schema.Columns.Where(c => c.Value.Type != Column.ColumnType.Nominal && c.Value.Type != Column.ColumnType.Integer))
                    table.RemoveOutliers(column.Key);

                table.NomalizeData();
                table.PrintOutputTable();
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
                new Column(){ Name = "age", Type = Column.ColumnType.Integer } ,
                new Column(){ Name = "workclass", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Workclass) } ,
                new Column(){ Name = "Final_Weight", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "education", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Education) } ,
                new Column(){ Name = "education-num", Type = Column.ColumnType.Integer } ,
                new Column(){ Name = "marital-status", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Marital_Status) } ,
                new Column(){ Name = "occupation", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Occupation) } ,
                new Column(){ Name = "relationship", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Relationship) } ,
                new Column(){ Name = "race", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Race) } ,
                new Column(){ Name = "sex", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Sex) } ,
                new Column(){ Name = "hours-per-week", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "native-country", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Native_Country) }
                 };
            var AdultSchema = new TableSchema(AdultColumns);
            AdultSchema.TotalOfRecords = 48842;
            tables.Add(new Models.DataTable("Data/adult.csv", AdultSchema));

            var BreastCancerColumns = new List<Column>
            {
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

            var WineColumns = new List<Column>
            {
                new Column(){ Name ="Alcohol", Type=Column.ColumnType.Integer } ,
                new Column(){ Name ="Malic-Acid", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Ash", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Alcalinity-of-Ash", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Magnesium", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Total-Phenols", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Flavanoids", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="NonFlavanoids-Phenols", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Proanthocyanins", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Color-Intensity", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Hue", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="OD280-OD315-of-diluted-wines", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Proline", Type=Column.ColumnType.Continuous } ,
            };
            var WineSchema = new TableSchema(WineColumns);
            WineSchema.TotalOfRecords = 178;
            tables.Add(new Models.DataTable("Data/wine.csv", WineSchema));

            var RedWineQualityColumns = new List<Column>
            {
                new Column(){ Name ="Fixed-Acidity", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Violatile-Acidity", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Citric-Acid", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Residual-Sugar", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Chlorides", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Free-Sufor-Dioxide", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Total-Sufor-Dioxide", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Density", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="pH", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Sulphates", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Alcohol", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Quality", Type=Column.ColumnType.Integer } ,
            };
            var RedWineQualitySchema = new TableSchema(RedWineQualityColumns);
            RedWineQualitySchema.TotalOfRecords = 1599;
            tables.Add(new Models.DataTable("Data/winequality-red.csv", RedWineQualitySchema));


            var WhiteWineQualityColumns = new List<Column>
            {
                new Column(){ Name ="Fixed-Acidity", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Violatile-Acidity", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Citric-Acid", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Residual-Sugar", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Chlorides", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Free-Sufor-Dioxide", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Total-Sufor-Dioxide", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Density", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="pH", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Sulphates", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Alcohol", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Quality", Type=Column.ColumnType.Integer } ,
            };
            var WhiteWineQualitySchema = new TableSchema(WhiteWineQualityColumns);
            WhiteWineQualitySchema.TotalOfRecords = 4898;
            tables.Add(new Models.DataTable("Data/winequality-white.csv", WhiteWineQualitySchema));
        }
    }
}
