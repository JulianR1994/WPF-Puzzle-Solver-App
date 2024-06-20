using System.IO;

namespace TestWPFApp
{
    public class FileConverter
    {
        /// <summary>
        /// Convert the texts from the text-file into objects used to solve the puzzle
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Inputs GetInputsFromFile(string filePath)
        {
            string[] rawInputs = File.ReadAllLines(filePath);

            if (rawInputs.Length != 3)
                throw new ArgumentException($"The file {filePath} must contain 3 lines exactly!");

            Inputs inputs = new() 
            { 
                Depth = Convert.ToInt32(rawInputs[0]),
                Board = ConvertStringToBoard(rawInputs[1]),
                Pieces = ConvertStringToPieces(rawInputs[2])
            };

            return inputs;
        }

        private int[,] ConvertStringToBoard(string stringLine)
        {
            //First, convert stringLine to array of strings
            string[] boardRows = stringLine.Split(",");

            //We assume the board always has equals sides
            int rowCount = boardRows.Length;
            int columnCount = boardRows[0].Length;

            // Initialize the 2D array
            int[,] array2D = new int[rowCount, columnCount];

            // Fill the 2D array
            for (int i = 0; i < rowCount; i++)
            {
                string row = boardRows[i];
                for (int j = 0; j < columnCount; j++)
                {
                    array2D[i, j] = int.Parse(row[j].ToString());
                }
            }

            return array2D;
        }

        private List<string> ConvertStringToPieces(string stringLine)
        {
            return stringLine.Split(' ').ToList();
        }
    }
}
