using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace senac_machine_learning_PI3
{
    public static class PrintFinalResult
    {
        public static string type;
        public static double Sensibility;
        public static double Specifity;
        public static double Precision;
        public static double Recall;

        public static void PrintResult(string type, FinalResultData finalResult, string fileName)
        {
            if (type == "Multi")
                PrintMultiType(finalResult);

            else
                PrintBinaryType(finalResult, fileName);
        }

        private static void PrintBinaryType(FinalResultData binaryResult, string fileName)
        {
            Sensibility = TP / (TP + FN);
            Specifity = TN / (TN + FP);
            Precision = TP / (TP + FP);
            Recall = TP / (TP + FN);

            var print = System.String.Format("Sensibility: {0}  ; Specifity: {1} ; Precision: {2} ; Recall: {3}", Sensibility, Specifity, Precision, Recall);
            File.WriteAllText("result/" + fileName + "Result", print );
        }

        private static void PrintMultiType(FinalResultData multiTypeResult)
        {



        }


    }
}
