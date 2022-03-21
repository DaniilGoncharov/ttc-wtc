using System;

namespace ttc_wtc
{
    static class HelpFunctions
    {
        public static char[,] MT(char[,] inputMatrix)
        {
            char[,] outputMatrix = new char[inputMatrix.GetLength(1), inputMatrix.GetLength(0)];
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < inputMatrix.GetLength(1); j++)
                {
                    outputMatrix[j, i] = inputMatrix[i, j];
                }
            }
            return outputMatrix;
        }

        public static string[] MST(string[] inputArray)
        {
            char[,] charArray = new char[inputArray.Length - 2, inputArray[0].Length];
            for (int i = 0; i < inputArray.Length - 2; i++)
            {
                char[] s = inputArray[i].ToCharArray();
                for (int j = 0; j < inputArray[0].Length; j++)
                {
                    charArray[i, j] = s[j];
                }
            }
            charArray = MT(charArray);
            string[] outputArray = new string[inputArray[0].Length + 2];
            for (int i = 0; i < outputArray.Length - 2; i++)
            {
                char[] s = new char[inputArray.Length - 2];
                for (int j = 0; j < inputArray.Length - 2; j++)
                {
                    s[j] = charArray[i, j];
                }
                outputArray[i] = new string(s);
            }
            outputArray[^2] = inputArray[^2];
            outputArray[^1] = inputArray[^1];
            return outputArray;
        }

        public static void Haise()
        {
            string text = System.IO.File.ReadAllText("../../../Haise.txt");
            string[] textArray = text.Split('\n');
            for (int j = 0; j < textArray.Length - 1; j++)
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < 35; i++)
                {
                    Console.WriteLine(textArray[j].Substring(i * 90, 90));
                }
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
