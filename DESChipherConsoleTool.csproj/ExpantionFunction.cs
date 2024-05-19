
namespace DESChipherConsoleTool
{
    public static class ExpantionFunction
    {
        private static readonly int[] E = {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        /// <summary>
        /// Метод реализуй функцию расширения
        /// </summary>
        /// <param name="input">Массив бит на вход</param>
        /// <returns>Расширенный блок бит</returns>
        public static BitArray ExpansionPermutation(BitArray input)
        {
            BitArray output = new BitArray(48);
            for (int i = 0; i < 48; i++)
            {
                output[i] = input[E[i] - 1];
            }
            return output;
        }
    }
}
