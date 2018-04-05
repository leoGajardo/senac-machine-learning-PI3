using senac_machine_learning_PI3.Enums;
using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
            //Função que criará a ordem e esquema do projeto, como são as tabelas, nome das colunas e etc.
            BuildSchemaTables();

            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("");
            Console.WriteLine("Normalizando Tabelas");

            //agora para cada uma das tabelas criadas na função anterior será feita a normalização dos dados
            foreach (var table in tables)
            {
                Console.WriteLine("");
                Console.WriteLine("============================================================================");
                Console.WriteLine("");
                Console.WriteLine("Convertendo Atributos da Tabela: {0}",table.fileName);

                //Primeiramente são convertidos os dados de String para os seus respequitivos enums
                table.ConvertEnums();

                Console.WriteLine("Atributos da Tabela: {0}, Convertidos com Sucesso!", table.fileName);
                Console.WriteLine("");
                Console.WriteLine("============================================================================");
                Console.WriteLine("");
                Console.WriteLine("Removendo Linhas Inconsistentes da Tabela: {0}", table.fileName);

                //Depois é criada uma lista de inteiros para guardar os IDs das instâncias que contém algum nulo nos seus dados
                var idsToBeRemovedFirst = new List<int>();
                table.RemoveInconsistentLines(ref idsToBeRemovedFirst); //função que verifica as instâncias que contém dados nulos

                Console.WriteLine("");
                Console.WriteLine("Linhas Inconsistentes da Tabela: {0}, Removidas com Sucesso!", table.fileName);
                Console.WriteLine("");
                Console.WriteLine("============================================================================");
                Console.WriteLine("============================================================================");
                Console.WriteLine("");
                Console.WriteLine("Removendo Outliers da Tabela: {0}", table.fileName);

                //Com todos os IDs das instâncias com nulos é então chamada a função que irá remove-los de acordo com seus respetivos IDs
                table.RemoveLines(idsToBeRemovedFirst.ToArray());


                //Imprime todos os dados estatisticos da tabela
                table.PrintStatisticData();

                //Cria uma lista de inteiros que irá guardar os IDs dos outliers que precisam ser removidos dos dados
                var idsToBeRemoved = new List<int>();
                //Para cada uma das colunas da tabela irá chamar a função que verifica os outliers que precisam ser removidos, ignora as colunas de classe e do tipo nominal, ou seja, as colunas do tipo string que já foram trocadas para valores inteiros dos enums
                foreach (var column in table.Schema.Columns.Where(c => c.Value.Type != Column.ColumnType.Nominal && c.Value.Type != Column.ColumnType.Class))
                    table.RemoveOutliers(column.Key, ref idsToBeRemoved);

                //imprime todos os outliers 
                Outliers.PrintOutliers(table, idsToBeRemoved);
                
                //remove toda as linhas que tiveram seus IDs identificados como outliers pela função anterior
                table.RemoveLines(idsToBeRemoved.ToArray());

                Console.WriteLine("");
                Console.WriteLine("Outliers da Tabela: {0}, Removidas com Sucesso!", table.fileName);
                Console.WriteLine("");
                Console.WriteLine("============================================================================");
                Console.WriteLine("============================================================================");
                Console.WriteLine("");
                Console.WriteLine("Normalizando Dados da Tabela: {0}", table.fileName);

                //Chama a função que normaliza os dados da tabela
                table.NomalizeData();

                Console.WriteLine("");
                Console.WriteLine("Dados da Tabela: {0}, Normalizados com Sucesso!", table.fileName);
                Console.WriteLine("");
                Console.WriteLine("============================================================================");

                //imprime os dados normalizados em um arquivo de output
                table.PrintOutputTable();

            }

            Console.WriteLine("");
            Console.WriteLine("Tabelas Normalizadas com Sucesso!");
            Console.WriteLine("**********************************************************************************************");

            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("");
            Console.WriteLine("Aplicando Algoritimos de KNN e Cross-Validation");

            //cria uma lista do modelo de Resultados
            var results = new List<FinalResultData>();
            foreach (var table in tables)
            {
                var TenFold = table.Data.Count / 10; // calcula o valor de um dos TenFolds
                var result = new FinalResultData(table); // cria uma variavel do modelo de resultado
                //var task2 = Parallel.For(1, 5, (ImpleementOf) =>  // executa de forma paralela cada implementação do Algoritimo utilizando de threads

                var timer = new Stopwatch();

                for (int ImpleementOf = 1; ImpleementOf < 5; ImpleementOf++)
                {
                    //executa todas as 10 folds do algoritimo do cross-validation e tem uma folga (a décima primeira iteração) caso ao dividir em 10 folds diferentes ainda tenha sobrado algumas instâncias
                    //var task = Parallel.For(0, 11, (i) => 
                    for (int i = 0; i < 12; i++)
                    {
                        timer.Start();
                        //cria os valores do cross-validation
                        var TestData = table.Data.Skip(i * TenFold).Take(TenFold).ToList(); // seleciona o grupo de teste
                        var TrainData = table.Data.Except(TestData).ToList(); //seleciona o grupo de treino

                        if (TestData.Count == 0)
                            continue;

                        var classColumn = table.Schema.Columns.First(c => c.Value.Type == Column.ColumnType.Class).Key; // recebe qual é a classe de coluna daquela tabela

                        var columnsToCompare = table.ColumnsToKeep.Keys.Where(k => k != classColumn).ToArray(); //recebe todas as colunas daquela tabela que não sejam as colunas de classe

                        var nColumns = table.ColumnsToKeep.Keys.Count();

                        var rnd = new Random();//cria uma variavel do tipo random para randomizar os dados do cross-validation
                        var randomTrainData = TrainData.OrderBy(o => rnd.Next()).ToList();//randomiza os dados de treino
                        rnd = new Random();
                        var randomTestData = TestData.OrderBy(o => rnd.Next()).ToList(); // randomiza os dados de teste


                        ////roda o algoritimo de KNN
                        //Knn.Run(randomTrainData, randomTestData, columns, Knn.GetK(ImpleementOf, table), classColumn, ref result);
                        Console.WriteLine($"Running ");
                        //roda o algoritimo de LVQ
                        LVQ.Run(TrainData, TestData, columnsToCompare, nColumns, ImpleementOf,  classColumn, ref result);
                        Console.WriteLine($"Executed LVQ R = {ImpleementOf} TenFold Validation Fold = {i} in {timer.Elapsed.ToString()}");
                        timer.Reset();
                    }
                    //});
                }
                //Serve de trava para que todas as threads dessa task terminem antes de seguir para a proxima task
                //    while (!task.IsCompleted)
                //    { }
                //});
                //while (!task2.IsCompleted)
                //{ }
                results.Add(result); // adiona aquela variavel de resultado criada anteriormente a lista de resultados, onde contém o resultado de todas as tabelas
                //GenerateHeatMap
            }

            //imprime os resultados de todas as tabelas
            foreach (var result in results)
                result.PrintResult();

            Console.WriteLine("");
            Console.WriteLine("Algoritimos Aplicados com Sucesso!");
            Console.WriteLine("**********************************************************************************************");
        }

        private static void BuildSchemaTables()
        {
            Console.WriteLine("");
            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("Construindo Schema das Tabelas");

            //Cria uma lista do modelo DataTable
            tables = new List<Models.DataTable>();

            //Criamos uma lista do nosso modelo de Colunas para armazenar as colunas na ordem do arquivo de leitura

            //var AbaloneColumns = new List<Column> {
            //    new Column(){ Name = "Sex", Type = Column.ColumnType.Class, Enum = typeof(Enums.Abalone.Sex) } , //para as colunas de classes ou nominais é passado um enum que contém um valor númerico para as strings contidas nessa coluna

            //    new Column(){ Name = "Length", Type = Column.ColumnType.Continuous } ,
            //    new Column(){ Name = "Diameter", Type = Column.ColumnType.Continuous } ,
            //    new Column(){ Name = "Height", Type = Column.ColumnType.Continuous } ,
            //    new Column(){ Name = "Whole weight", Type = Column.ColumnType.Continuous } ,
            //    new Column(){ Name = "Shucked weight", Type = Column.ColumnType.Continuous } ,
            //    new Column(){ Name = "Viscera weight", Type = Column.ColumnType.Continuous } ,
            //    new Column(){ Name = "Shell weight", Type = Column.ColumnType.Continuous } ,
            //    new Column(){ Name = "Rings", Type = Column.ColumnType.Integer }
            //};
            //var AbaloneSchema = new TableSchema(AbaloneColumns); // É criado o esquema das tabelas dada a lista de colunas
            //AbaloneSchema.TotalOfRecords = 4177; // o total de instâncias do arquivo original
            //AbaloneSchema.Type = "Multi"; // o Tipo da Classe dos dados, ou seja Multi-Type ou Binary-Type
            //Por fim é criado uma tabela com o respectivo caminho para os dados, o esquema da tabela e então adicionado a lista de tabelas
            //tables.Add(new Models.DataTable("Data/abalone.csv", AbaloneSchema));

            //Console.WriteLine("Abalone OK");


            //var AdultColumns = new List<Column> {
            //    new Column(){ Name = "Age", Type = Column.ColumnType.Integer } ,
            //    new Column(){ Name = "Workclass", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Workclass) } ,
            //    new Column(){ Name = "Final_Weight", Type = Column.ColumnType.Continuous } ,
            //    new Column(){ Name = "Education", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Education) } ,
            //    new Column(){ Name = "Education-Num", Type = Column.ColumnType.Integer } ,
            //    new Column(){ Name = "Marital_Status", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Marital_Status) } ,
            //    new Column(){ Name = "Occupation", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Occupation) } ,
            //    new Column(){ Name = "Relationship", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Relationship) } ,
            //    new Column(){ Name = "Race", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Race) } ,
            //    new Column(){ Name = "Sex", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Sex) } ,
            //    new Column(){ Name = "Hours_per_Week", Type = Column.ColumnType.Continuous } ,
            //    new Column(){ Name = "Native_Country", Type = Column.ColumnType.Nominal, Enum = typeof(Enums.Adult.Native_Country) },
            //    new Column(){ Name = "Class", Type = Column.ColumnType.Class, Enum = typeof(Enums.Adult.Class) } ,
            //     };
            //var AdultSchema = new TableSchema(AdultColumns);
            //AdultSchema.TotalOfRecords = 48842;
            //AdultSchema.Type = "Binary";
            //tables.Add(new Models.DataTable("Data/adult.csv", AdultSchema));

            //Console.WriteLine("Adult OK");



            //var BreastCancerColumns = new List<Column>
            //{
            //    new Column(){ Name ="Diagonis", Type=Column.ColumnType.Class, Enum = typeof(Enums.BreastCancer.Diagnosis) } ,
            //    new Column(){ Name ="Radius_Cell-1",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Texture_Cell-1",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Perimeter_Cell-1",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Area_Cell-1",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Smoothness_Cell-1",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Compactness_Cell-1",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Concavity_Cell-1",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Concave-Points_Cell-1",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Symmetry_Cell-1",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Fractal-Dimension_Cell-1",Type=Column.ColumnType.Continuous} ,

            //    new Column(){ Name ="Radius_Cell-2",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Texture_Cell-2",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Perimeter_Cell-2",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Area_Cell-2",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Smoothness_Cell-2",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Compactness_Cell-2",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Concavity_Cell-2",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Concave-Points_Cell-2",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Symmetry_Cell-2",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Fractal-Dimension_Cell-2",Type=Column.ColumnType.Continuous} ,

            //    new Column(){ Name ="Radius_Cell-3",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Texture_Cell-3",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Perimeter_Cell-3",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Area_Cell-3",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Smoothness_Cell-3",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Compactness_Cell-3",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Concavity_Cell-3",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Concave-Points_Cell-3",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Symmetry_Cell-3",Type=Column.ColumnType.Continuous} ,
            //    new Column(){ Name ="Fractal-Dimension_Cell-3",Type=Column.ColumnType.Continuous} ,
            //};
            //var BreastCancerSchema = new TableSchema(BreastCancerColumns);
            //BreastCancerSchema.TotalOfRecords = 569;
            //BreastCancerSchema.Type = "Binary";
            //tables.Add(new Models.DataTable("Data/BreastCancer.csv", BreastCancerSchema));

            //Console.WriteLine("Breast Cancer OK");


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

            //Console.WriteLine("Iris OK");

            //var WineColumns = new List<Column>
            //{
            //    new Column(){ Name ="Alcohol", Type=Column.ColumnType.Class } ,
            //    new Column(){ Name ="Malic-Acid", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Ash", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Alcalinity-of-Ash", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Magnesium", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Total-Phenols", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Flavanoids", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="NonFlavanoids-Phenols", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Proanthocyanins", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Color-Intensity", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Hue", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="OD280-OD315-of-diluted-wines", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Proline", Type=Column.ColumnType.Continuous } ,
            //};
            //var WineSchema = new TableSchema(WineColumns);
            //WineSchema.TotalOfRecords = 178;
            //WineSchema.Type = "Multi";
            //tables.Add(new Models.DataTable("Data/wine.csv", WineSchema));

            //Console.WriteLine("Wine OK");

            //var RedWineQualityColumns = new List<Column>
            //{
            //    new Column(){ Name ="Fixed-Acidity", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Violatile-Acidity", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Citric-Acid", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Residual-Sugar", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Chlorides", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Free-Sufor-Dioxide", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Total-Sufor-Dioxide", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Density", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="pH", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Sulphates", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Alcohol", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Quality", Type=Column.ColumnType.Class } ,
            //};
            //var RedWineQualitySchema = new TableSchema(RedWineQualityColumns);
            //RedWineQualitySchema.TotalOfRecords = 1599;
            //RedWineQualitySchema.Type = "Multi";
            //tables.Add(new Models.DataTable("Data/winequality-red.csv", RedWineQualitySchema));

            //Console.WriteLine("Red Wine Quality OK");


            //var WhiteWineQualityColumns = new List<Column>
            //{
            //    new Column(){ Name ="Fixed-Acidity", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Violatile-Acidity", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Citric-Acid", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Residual-Sugar", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Chlorides", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Free-Sufor-Dioxide", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Total-Sufor-Dioxide", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Density", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="pH", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Sulphates", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Alcohol", Type=Column.ColumnType.Continuous } ,
            //    new Column(){ Name ="Quality", Type=Column.ColumnType.Class } ,
            //};
            //var WhiteWineQualitySchema = new TableSchema(WhiteWineQualityColumns);
            //WhiteWineQualitySchema.TotalOfRecords = 4898;
            //WhiteWineQualitySchema.Type = "Multi";
            //tables.Add(new Models.DataTable("Data/winequality-white.csv", WhiteWineQualitySchema));

            //Console.WriteLine("White Wine Quality OK");

            Console.WriteLine("Schema das Tabelas Construido com Sucesso!");
            Console.WriteLine("**********************************************************************************************");
            Console.WriteLine("");

        }
    }
}
