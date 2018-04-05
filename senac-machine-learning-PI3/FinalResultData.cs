using senac_machine_learning_PI3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace senac_machine_learning_PI3
{
    public class FinalResultData
    {
        //a Tabela que este modelo de resultados está representando
        public DataTable ReferenceTable { get; private set; }
        //as informações para o calculo das classes binarias
        public double Sensibility, Specifity, Precision, Recall, Accuracy; 
        public double TP, TN, FN, FP;


        //construtor da classe
        public FinalResultData(DataTable referenceTable)
        {
            this.ReferenceTable = referenceTable;
            SimpleErrors = new List<SimpleError>();
        }

        //cria uma lista do modelo de simple errors que guarda os simple errors dos 4 diferentes
        public List<SimpleError> SimpleErrors { get; set; }

        //Retorna o cross error
        public double GetCrossError(int k)
        {
            return SimpleErrors.Where(se => se.K == k).Average(a => a.ErrorValue());
        }

        //Retorna qual foi o melhor K 
        public int GetBestK()
        {
            return SimpleErrors.GroupBy(se => se.K).OrderBy(g => GetCrossError(g.Key)).First().Key;
        }

        //imprime os resultados
        public void PrintResult()
        {
            //primeiramente separa os ks diferentes e verifica se existe algum arquivo na tabela de resultados, se existir é deletado
            var ks = SimpleErrors.Select(se => se.K).Distinct();
            if (Directory.Exists("resultLVQ/"))
                File.Delete("resultLVQ/" + ReferenceTable.fileName);

            //para cada um dos Ks irá ser feito a impressão dos dados
            foreach (var k in ks)
            {
                //feito a verificação de qual é o tipo da tabela, se ele é Multi-Type ou Binary-Type
                var type = ReferenceTable.Schema.Type;
                PrintHeader(k);
                if (type == "Multi")
                    PrintMultiType(k);

                else
                    PrintBinaryType(k);
            }
            PrintBestK();
        }

        //Imprime qual foi o melhor K calculado pelo código.
        private void PrintBestK()
        {
            File.AppendAllText("resultLVQ/" + ReferenceTable.fileName, $"The best K for this situation is {GetBestK()} with error Rate of {GetCrossError(GetBestK())} ");
        }

        //imprime um cabeçalho para diferenciar os ks 
        private void PrintHeader(int k)
        {
            if (k == 1)
                File.AppendAllText("resultLVQ/" + ReferenceTable.fileName, $"Running K = 1-NN for file {ReferenceTable.fileName} \n\r");

            else if (k == 3)
                File.AppendAllText("resultLVQ/" + ReferenceTable.fileName, $"Running K = (M+2)-NN for file {ReferenceTable.fileName} \n\r");

            else if (k == 11)
                File.AppendAllText("resultLVQ/" + ReferenceTable.fileName, $"Running K = (M*10+1)-NN for file {ReferenceTable.fileName} \n\r");

            else
                File.AppendAllText("resultLVQ/" + ReferenceTable.fileName, $"Running K = (Q/2+1)-NN or (Q/2)-NN for file {ReferenceTable.fileName} \n\r");

        }

        //Função que calcula e imprime os resultados da tabelas de classe do tipo Binário
        private void PrintBinaryType(int k)
        {
            SetInternalStatistics(k);

            //Calcula as estatisticas baseados nos erros e acertos de cada K
            Sensibility = (double)TP / ((double)TP + (double)FN);
            Specifity = (double)TN / ((double)TN + (double)FP);
            Precision = (double)TP / ((double)TP + (double)FP);
            Recall = (double)TP / ((double)TP + (double)FN);
            Accuracy = ((double)TP + (double)TN) / ((double)TP + (double)TN + (double)FN + (double)FP);

            //Imprime no arquivo todas as estatisticas
            var print = System.String.Format("Sensibility: {0}  ; Specifity: {1} ; Precision: {2} ; Recall: {3} ; Accuracy: {4}\n\r", Sensibility, Specifity, Precision, Recall, Accuracy);
            File.AppendAllText("resultLVQ/" + ReferenceTable.fileName, print);
        }

        //Calcula as estatisticas de acertos e erros daquele K 
        private void SetInternalStatistics(int k)
        {
            var predictions = SimpleErrors.Where(se => se.K == k);
            TP = predictions.Sum(se => se.Predictions.Count(p => p.ExpectedClassNumber == p.PreviewedClassNumber && p.PreviewedClassNumber == 1));
            FP = predictions.Sum(se => se.Predictions.Count(p => p.PreviewedClassNumber == 1 && p.ExpectedClassNumber == 2));
            TN = predictions.Sum(se => se.Predictions.Count(p => p.ExpectedClassNumber == p.PreviewedClassNumber && p.PreviewedClassNumber == 2));
            FN = predictions.Sum(se => se.Predictions.Count(p => p.PreviewedClassNumber == 2 && p.ExpectedClassNumber == 1));
        }


        //Calcula e Imprime a Matriz de confusão para as classes de Multi-type
        private void PrintMultiType(int k)
        {
            //Acha qual é a coluna de classe naquela tabela
            var classColumn = ReferenceTable.Schema.Columns.First(c => c.Value.Type == Column.ColumnType.Class);
            //Busca quais são os valores de String daquela classe no enum e caso os valores não sejam do tipo string, ou seja não tenha enum, os valores são transformados em string
            var enumValues = classColumn.Value.Enum?.GetEnumValues();
            if (enumValues == null)
                enumValues = ReferenceTable.Data.SelectMany(s => s.Columns[classColumn.Key]).Distinct().OrderBy(o => o).ToArray();
            
            //Constroi um cabeçalho para a matriz de confusão, colocando o nome das classes
            var values = SimpleErrors.Where(se => se.K == k).SelectMany(s => s.Predictions);
            StringBuilder header = new StringBuilder($"\t\t");
            var lines = new List<string>();
            foreach (var enumNamePredicted in enumValues)
                header.Append(enumNamePredicted.ToString() + "\t\t");

            //Constroi o corpo da matriz de confusão, lendo onde que uma classe foi predizida e qual era a sua real classe, fazendo assim que seja possível saber quais foram predizidas de forma correta ou não na matriz de confusão
            foreach (var enumNameExpected in enumValues)
            {
                StringBuilder line = new StringBuilder(enumNameExpected.ToString() + "\t\t");
                foreach (var enumNamePreviewed in enumValues)
                {
                    var val = values.Count(v => v.ExpectedClass == enumNameExpected.ToString() && v.PreviewedClass == enumNamePreviewed.ToString());
                    line.Append(val + "\t\t");
                }
                lines.Add(line.ToString() + $"\n\r");
            }

            //Imprime um cabeçalho contendo os dados dos resultados daquele K e imprime tudo que foi construido logo acima
            File.AppendAllText("resultLVQ/" + ReferenceTable.fileName, header.ToString() + $"\n\r");
            File.AppendAllLines("resultLVQ/" + ReferenceTable.fileName, lines);
            var accuracy = ((double)SimpleErrors.Where(se => se.K == k).Sum(s => s.NumOfRecords) - (double)SimpleErrors.Where(se => se.K == k).Sum(s => s.NumOfErrors)) / (double)SimpleErrors.Where(se => se.K == k).Sum(s => s.NumOfRecords);
            File.AppendAllText("resultLVQ/" + ReferenceTable.fileName, $"Accuracy of {accuracy} \n\r");
        }

    }
}
