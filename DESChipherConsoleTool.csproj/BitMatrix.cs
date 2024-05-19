
namespace DESChipherConsoleTool
{
    public class BitMatrix
    {
        private readonly bool[,] matrix;

        public int Rows { get; }
        public int Columns { get; }

        public int Length => Rows * Columns;

        public BitMatrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            matrix = new bool[rows, columns];
        }

        public bool this[int row, int column]
        {
            get => matrix[row, column];
            set => matrix[row, column] = value;
        }

        /// <summary>
        /// Преобразует массив битов в матрицу битов.
        /// </summary>
        /// <param name="bitArray">Массив битов для преобразования.</param>
        /// <param name="rows">Количество строк в матрице.</param>
        /// <param name="columns">Количество столбцов в матрице.</param>
        /// <returns>Матрицу битов с заданным количеством строк и столбцов.</returns>
        /// <exception cref="ArgumentException">Генерируется, если длина массива битов не соответствует размерам матрицы.</exception>
        public static BitMatrix FromBitArray(BitArray bitArray, int rows, int columns)
        {
            if (bitArray.Length != rows * columns)
                throw new ArgumentException("Длинна массива не совпадает с произведением строк и столбцов");

            BitMatrix result = new BitMatrix(rows, columns);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int index = i * columns + j;
                    result[i, j] = bitArray[index];
                }
            }

            return result;
        }

        public void Print()
        {
            for (int row = 0;  row < Rows; row++)
            {
                for(int col = 0; col < Columns; col++)
                {
                    Console.Write(matrix[row, col] ? "1 " : "0 ");
                }
                Console.WriteLine();
            }
        }
    }
}
