using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace senac_machine_learning_PI3.Models
{
    public class DataTable
    {
        public Dictionary<int, string> ColumnsToKeep { get; private set; }

        public TableSchema Schema { get; private set; }

        private string[] linesOfFile; // total de linhas do arquivo csv
        public string fileName; //nome do arquivo

        public List<Line> Data { get; set; }
        public DataTable(string filePath, TableSchema schema)
        {
            //constrói um dicionario para podermos guardar todas as colunas
            ColumnsToKeep = new Dictionary<int, string>();

            //Configura o esquema da tabela 
            Data = new List<Line>(schema.TotalOfRecords); //Cria o Data, que é uma lista de linhas de acordo com o total de instâncias do esquema
            for (int i = 0; i < schema.TotalOfRecords; i++)
                Data.Add(new Line() { Columns = new string[schema.Columns.Count], Id = i });//adiona um ID para aquela linha do Data

            foreach (var column in schema.Columns)
                ColumnsToKeep.Add(column.Key, column.Value.Name); // adiciona o número de qual é aquela coluna e o seu nome

            this.Schema = schema;

            // Carrega o arquivo csv
            var fileInfo = new FileInfo(filePath);
            fileName = fileInfo.Name;

            //lê todas as linhas do arquivo e chama a função que irá colocar os valores nas colunas
            linesOfFile = File.ReadAllLines(filePath);
            ParseInternal();

            CheckIfResultDirectoryExists(); // Função que cria o diretório  para guardar os resultados finais de todo o processo do projeto
        }

        public void RemoveColumn(int column) => ColumnsToKeep.Remove(column);

        //Remove uma unica linha da tabela dada o seu ID
        public void RemoveLine(int line) => Data.Remove(Data.FirstOrDefault(d => d.Id == line));

        //Remove várias linhas da tabela dado um vetor com valores dos IDs a serem removidos
        public void RemoveLines(int[] lines) => Data.RemoveAll(d => lines.Contains(d.Id));

        public void ConvertEnums()
        {   
            //Todas as colunas daquela tabela são varridas, e caso elas sejam do tipo Nominal ou Classe e o seu enum não seja nulo, os valores das strings serão trocados para seus respectivos valores numéricos
            foreach (var column in Schema.Columns.Where(c => c.Value.Type == Column.ColumnType.Nominal || (c.Value.Type == Column.ColumnType.Class && c.Value.Enum != null)))
            {
                //essa mudança é feita para todas as linhas de dados, por isso o total de instâncias é o limite
                for (int i = 0; i < Schema.TotalOfRecords; i++)
                {   
                    //primeiramente é checado qual é o enum e qual é a coluna que precisará ser trocada
                    var newVal = Schema.ConvertStringToEnum(Schema.Columns[column.Key].Enum, Data[i].Columns[column.Key]);
                    if (newVal == "0") //simples verificação criada durante o processo do trabalho apra saber se aconteceu algum problema na troca dos enums
                        Console.WriteLine("Aconteceu um problema na troca de enums");
                    Data[i].Columns[column.Key] = newVal; // troca o valor da string pelo valor numérico
                }
            }

        }

        public void PrintOutputTable()
        {
            //imprime todos os dados normalizados da tabela em um arquivo com seu nome na pasta output
            CheckIfOutputDirectoryExists();
            File.WriteAllLines("output/" + fileName, Data.Select(d => d.ToString()));
        }


        private void CheckIfResultDirectoryExists()
        {
            //checa se o diretório  de resultados já existe e caso não exista cria o diretório
            if (!Directory.Exists("result"))
                Directory.CreateDirectory("result");

        }

        public void PrintStatisticData()
        {
            //Primeiramente verifica se o Diretório para salvar os dados existe e caso não exista é então criado
            if (!Directory.Exists("statistics"))
                Directory.CreateDirectory("statistics");

            //Verificado se já  existe um arquivo de estatisticas para aquela tabela e caso exista ele é deletado
            if (Directory.Exists("statistics"))
                File.Delete("statistics\\statistics" + fileName);

            //Padrão de linhas que será construido
            var lines = new StringBuilder();
            lines.Append("Atributo \t\t Menor Valor \t\t Maior valor \t\t Média \t\t Desvio Padrão \n\r");

            //passa por todas as colunas do esquema da tabela que não sejam do tipo classe ou nominal, ou seja as classes que não contém valor de strings
            foreach (var column in Schema.Columns.Where(c => c.Value.Type != Column.ColumnType.Nominal && c.Value.Type != Column.ColumnType.Class)) 
            {
                var average = Data.Average(d => d.getColumnsAsDouble()[column.Key]);// valor médio da coluna
                var min = Data.Min(d => d.getColumnsAsDouble()[column.Key]); // menor valor da coluna
                var max = Data.Max(d => d.getColumnsAsDouble()[column.Key]); // maior valor da coluna
                var stdDeviation = CalculateStdDev(Data.Select(d => d.getColumnsAsDouble()[column.Key])); // Desvio padrão da coluna

                //escreve no arquivo as estatisticas calculadas
                lines.Append(column.Value.Name + "\t\t" + min + "\t\t" + max + "\t\t" + average + "\t\t" + stdDeviation + " \n\r");

            }
            File.WriteAllText("statistics\\statistics" + fileName, lines.ToString());
        }

        //Função para calcular o desvio padrão
        private double CalculateStdDev(IEnumerable<double> values) 
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                //Calcula a média     
                double avg = values.Average();
                //Faz a soma de (valor-média)^2                    
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //retira a raiz quadrada da soma divido pelo total de valores  
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }

        private void CheckIfOutputDirectoryExists()
        {
            //checa se o diretório para output, os dados normalizados, já existe, caso nao exista cria o diretório
            if (!Directory.Exists("output"))
                Directory.CreateDirectory("output");
        }

        private void ParseInternal()
        {
            int lineNumber = 0;
            //executa o código para o total de linhas do arquivo
            foreach (var line in linesOfFile)
            {
                foreach (var column in Schema.Columns)//executa de acordo com cada coluna do esquema da tabela
                {
                    //adiciona os dados do arquivo a linha, separando as colunas através do identificador '/' 
                    Data[lineNumber].Columns[column.Key] = line.Split('/')[column.Key];
                }
                lineNumber++;
            }
        }
    }
}
