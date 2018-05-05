using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public static class Knn
    {

        public static void Run(List<Line> trainData, List<Line> testData, int[] columns, int k, int classColumn, ref FinalResultData results)
        {
            //cria uma instância do modelo de SimpleError que irá guardar as predições número de erros, total de predições e o valor de erro para poder fazer os calculos posteriores de erro
            var simpleError = new SimpleError(k);

            //recebe os valores dos enums
            var EnumValues = results.ReferenceTable.Schema.Columns[classColumn].Enum?.GetEnumValues();

            var task = Parallel.ForEach(testData, (data) =>
            {
                //Calcula qual é a classe predizida dados os vizinhos
                var result = CalculateLine(trainData, data, columns, k, classColumn);

                //Verifica qual é a classe esperada para aquela linha e atribui para a classe esperada.
                var expectedClass = EnumValues != null ? EnumValues.GetValue(Int32.Parse(data.Columns[classColumn]) - 1).ToString() : data.Columns[classColumn];

                //Atribui a classe predizida
                var previewedClass = EnumValues != null ? EnumValues?.GetValue((int)result - 1).ToString() : result.ToString();

                //Cria um objeto Predição do nosso modelo de Prection, no qual contém a classe real e a classe predizida com seus valores em string e inteiro
                simpleError.Predictions.Add(
                    new Prediction()
                    {
                        ExpectedClass = expectedClass,
                        PreviewedClass = previewedClass,
                        ExpectedClassNumber = Int32.Parse(data.Columns[classColumn]),
                        PreviewedClassNumber = (int)result
                    });
            });

            while (!task.IsCompleted)
            { }

            //Adiciona o resultado desta tabela a lista de resultados de tabelas.
            results.SimpleErrors.Add(simpleError);
        }

        //Pega o valor do K que deverá ser implementado
        public static int GetK(int ImplementOfK, DataTable table)
        {
            switch (ImplementOfK)
            {   
                //No caso do projeto, todos os conjuntos de dados tem apenas 1 coluna de classe, assim o M se tornou valor 1 em todos os casos, sendo assim preferivel colocar seu valor final para agilizar o código.
                case 1: return 1;// 1-NN

                case 2: return 3;// (M+2)-NN

                case 3: return 11; //(M*10 + 1)-NN

                case 4: // (Q/2)-NN
                    if (table.Data.Count % 2 == 0) // caso de total de instancias par
                        return (table.Data.Count / 2) + 1;
                    else
                        return (table.Data.Count / 2); // caso de total de instancias impar
            }
            //retorna -1 caso alguma coisa tenha acontecido errado e tenha ignorado todos os casos de Ks anteriores
            return -1;
        }


        private static double CalculateLine(List<Line> trainData, Line testData, int[] columns, int k, int classColumn)
        {
            //cria um dicionario para guardar as distancias
            var distances = new Dictionary<int, LighweightData>();


            int[] maxValArray = new int[20];

            foreach (var baseData in trainData)
            {
                //calcula as distancias e guarda cada uma delas
                var distance = GetDistance(columns, testData.getColumnsAsDouble(), baseData.getColumnsAsDouble());
                distances.Add(baseData.Id, new LighweightData(distance, Int32.Parse(baseData.Columns[classColumn])));
            }
            //calcula os vizinhos feito uma ordenação pelas distancias
            var neighbours = distances.OrderBy(o => o.Value.distance).Take(k);

            //executa o algoritimo para cada um dos vizinhos, somando um para cada uma das classes apresentadas dentre os vizinhos
            foreach (var neighbour in neighbours)
                maxValArray[neighbour.Value.classVal] += 1;

            
            var maxVal = maxValArray.Max();//calcula a classe com maior ocorrencia dentre os vizinhos
            var matches = maxValArray.Count(c => c == maxVal); // calcula qual o número de maiores ocorrencias, para caso duas classes tenham tido o mesmo valor;
            double calculatedClass = 0;
            if (matches == 1)//executa caso apenas uma classe tenha se sobresaido sobre as outras, assim é atribuido o valor dequela classe
            {
                calculatedClass = Array.IndexOf(maxValArray, maxVal);
            }
            else if (matches > 1) //criétio de desempate caso duas classes tenham tido o mesmo número de ocorrencias dentre os vizinhos
            {
                calculatedClass = neighbours.GroupBy(g => g.Value.classVal).Where(d => d.Count() == maxVal).OrderBy(m => m.Sum(s => s.Value.distance)).First().Key;
            }
            //por fim retorna a classe calculada
            return calculatedClass;
        }


        //Calcula a distância através da distancia eucliadiana 
        private static double GetDistance(int[] columns, double[] a, double[] b)
        {
            double distancia = 0;
            foreach (var column in columns)
            {
                distancia += Math.Pow((a[column] - b[column]), 2);
            }

            return Math.Sqrt(distancia);
        }
    }

    //clase interna criada para melhorar a performance do projeto, nela são guardadas algumas informaçõs como a distancia e o valor da classe.
    internal class LighweightData{

        public LighweightData(double distance, int classVal)
        {
            this.distance = distance;
            this.classVal = classVal;
        }

        public double distance { get; set; }
        public int classVal { get; set; }

    }
}
