
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
        /// Возвращает массив матриц из строки для алгоритма DES
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <returns>Массив битовых матриц</returns>
        public static BitMatrix[] GetBlocksFromString(string input)
        {
            BitArray inputBitArray = BitArrayHelper.FromString(input);
            BitArray[] blocks = inputBitArray.SplitArrayIntoEqualParts(64).ToArray();

            BitMatrix[] bitMatrices = new BitMatrix[blocks.Length];

            for (int i = 0; i < bitMatrices.Length; i++)
            {
                bitMatrices[i] = FromBitArray(blocks[i], 8, 8);
            }

            return bitMatrices;
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
                throw new ArgumentException("BitArray length does not match matrix dimensions");

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

        /// <summary>
        /// Преобразует бинарную матрицу в строковое представление.
        /// </summary>
        /// <returns>Строка, сформированная из текущей матрицы.</returns>
        /// <exception cref="ArgumentException">Вызывается, когда количество столбцов не кратно 8.</exception>
        public string GetString()
        {
            if (Columns % 8 != 0)
                throw new ArgumentException("Columns count must be a multiple of 8.");

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Rows; i++)
            {
                int symbolValue = 0;

                for (int j = 0; j < Columns; j++)
                {
                    symbolValue <<= 1;
                    if (matrix[i, j])
                        symbolValue |= 1;
                }

                sb.Append((char)symbolValue);
            }

            return sb.ToString();
        }
    }
}
