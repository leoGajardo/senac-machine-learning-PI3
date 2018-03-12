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

            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("");
            Console.WriteLine("Normalizando Tabelas");

            foreach (var table in tables)
            {
                Console.WriteLine("");
                Console.WriteLine("============================================================================");
                Console.WriteLine("");
                Console.WriteLine("Convertendo Atributos da Tabela: {0}",table.fileName);

                table.ConvertEnums();

                Console.WriteLine("Atributos da Tabela: {0}, Convertidos com Sucesso!", table.fileName);
                Console.WriteLine("");
                Console.WriteLine("============================================================================");
                Console.WriteLine("");
                Console.WriteLine("Removendo Linhas Inconsistentes da Tabela: {0}", table.fileName);

                table.RemoveInconsistentLines();

                Console.WriteLine("");
                Console.WriteLine("Linhas Inconsistentes da Tabela: {0}, Removidas com Sucesso!", table.fileName);
                Console.WriteLine("");
                Console.WriteLine("============================================================================");
                Console.WriteLine("============================================================================");
                Console.WriteLine("");
                Console.WriteLine("Removendo Outliers da Tabela: {0}", table.fileName);

                foreach (var column in table.Schema.Columns.Where(c => c.Value.Type != Column.ColumnType.Nominal && c.Value.Type != Column.ColumnType.Class))
                    table.RemoveOutliers(column.Key);

                Console.WriteLine("");
                Console.WriteLine("Outliers da Tabela: {0}, Removidas com Sucesso!", table.fileName);
                Console.WriteLine("");
                Console.WriteLine("============================================================================");
                Console.WriteLine("============================================================================");
                Console.WriteLine("");
                Console.WriteLine("Normalizando Dados da Tabela: {0}", table.fileName);

                table.NomalizeData();

                Console.WriteLine("");
                Console.WriteLine("Dados da Tabela: {0}, Normalizados com Sucesso!", table.fileName);
                Console.WriteLine("");
                Console.WriteLine("============================================================================");

                table.PrintOutputTable();

            }

            Console.WriteLine("");
            Console.WriteLine("Tabelas Normalizadas com Sucesso!");
            Console.WriteLine("**********************************************************************************************");

            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("");
            Console.WriteLine("Aplicando Algoritimos de KNN e Cross-Validation");
            List<FinalResultData> results = new List<FinalResultData>();
            foreach (var table in tables)
            {
                var TenFold = table.Data.Count / 10;
                var result = new FinalResultData(table);
                for (int ImpleementOfk = 1; ImpleementOfk < 5; ImpleementOfk++)
                {

                    for (int i = 0; i < 11; i++)
                    {
                        var TestData = table.Data.Skip(i * TenFold).Take(TenFold).ToList();
                        var TrainData = table.Data.Except(TestData).ToList();
                        var classColumn = table.Schema.Columns.First(c => c.Value.Type == Column.ColumnType.Class).Key;
                        var columns = table.ColumnsToKeep.Keys.Where(k => k != classColumn).ToArray();
                        Knn.Run(TrainData, TestData, columns, Knn.GetK(ImpleementOfk, table), classColumn, ref result);

                    }
                }
                result.PrintResult();
            }

            Console.WriteLine("");
            Console.WriteLine("Algoritimos Aplicados com Sucesso!");
            Console.WriteLine("**********************************************************************************************");
        }

        private static void BuildSchemaTables()
        {
            Console.WriteLine("");
            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("Construindo Schema das Tabelas");
            tables = new List<Models.DataTable>();

            var AbaloneColumns = new List<Column> {
                new Column(){ Name = "Sex", Type = Column.ColumnType.Class, Enum = typeof(Enums.Abalone.Sex) } ,
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
            AbaloneSchema.Type = "Multi";
            tables.Add(new Models.DataTable("Data/abalone.csv", AbaloneSchema));

            Console.WriteLine("Abalone OK");


            var AdultColumns = new List<Column> {
                new Column(){ Name = "Age", Type = Column.ColumnType.Integer } ,
                new Column(){ Name = "Workclass", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Workclass) } ,
                new Column(){ Name = "Final_Weight", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "Education", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Education) } ,
                new Column(){ Name = "Education-Num", Type = Column.ColumnType.Integer } ,
                new Column(){ Name = "Marital_Status", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Marital_Status) } ,
                new Column(){ Name = "Occupation", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Occupation) } ,
                new Column(){ Name = "Relationship", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Relationship) } ,
                new Column(){ Name = "Race", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Race) } ,
                new Column(){ Name = "Sex", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Sex) } ,
                new Column(){ Name = "Hours_per_Week", Type = Column.ColumnType.Continuous } ,
                new Column(){ Name = "Native_Country", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Native_Country) },
                new Column(){ Name = "Class", Type = Column.ColumnType.Class, Enum = typeof(Enums.Adult.Class) } ,
                 };
            var AdultSchema = new TableSchema(AdultColumns);
            AdultSchema.TotalOfRecords = 48842;
            AdultSchema.Type = "Binary";
            tables.Add(new Models.DataTable("Data/adult.csv", AdultSchema));

            Console.WriteLine("Adult OK");



            var BreastCancerColumns = new List<Column>
            {
                new Column(){ Name ="Diagonis", Type=Column.ColumnType.Class, Enum = typeof(Enums.BreastCancer.Diagnosis) } ,
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
            BreastCancerSchema.Type = "Binary";
            tables.Add(new Models.DataTable("Data/BreastCancer.csv", BreastCancerSchema));

            Console.WriteLine("Breast Cancer OK");


            var IrisColumns = new List<Column>
            {
                new Column(){ Name ="Sepal-Lengh", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Sepal-Width", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Petal-Lengh", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Petal-Width", Type=Column.ColumnType.Continuous } ,
                new Column(){ Name ="Class", Type=Column.ColumnType.Class, Enum = typeof(Enums.Iris.IrisType) } ,
            };
            var IrisSchema = new TableSchema(IrisColumns);
            IrisSchema.TotalOfRecords = 150;
            IrisSchema.Type = "Multi";
            tables.Add(new Models.DataTable("Data/Iris.csv", IrisSchema));

            Console.WriteLine("Iris OK");

            var WineColumns = new List<Column>
            {
                new Column(){ Name ="Alcohol", Type=Column.ColumnType.Class } ,
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
            WineSchema.Type = "Multi";
            tables.Add(new Models.DataTable("Data/wine.csv", WineSchema));

            Console.WriteLine("Wine OK");

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
                new Column(){ Name ="Quality", Type=Column.ColumnType.Class } ,
            };
            var RedWineQualitySchema = new TableSchema(RedWineQualityColumns);
            RedWineQualitySchema.TotalOfRecords = 1599;
            RedWineQualitySchema.Type = "Multi";
            tables.Add(new Models.DataTable("Data/winequality-red.csv", RedWineQualitySchema));

            Console.WriteLine("Red Wine Quality OK");


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
            WhiteWineQualitySchema.Type = "Multi";
            tables.Add(new Models.DataTable("Data/winequality-white.csv", WhiteWineQualitySchema));

            Console.WriteLine("White Wine Quality OK");

            Console.WriteLine("Schema das Tabelas Construido com Sucesso!");
            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("");

        }
    }
}
