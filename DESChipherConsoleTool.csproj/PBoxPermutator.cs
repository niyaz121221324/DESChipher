
namespace DESChipherConsoleTool
{
    public static class PBoxPermutator
    {
        private static readonly int[] P = 
        {
            16, 7, 20, 21,
            29, 12, 28, 17,
            1, 15, 23, 26,
            5, 18, 31, 10,
            2, 8, 24, 14,
            32, 27, 3, 9,
            19, 13, 30, 6,
            22, 11, 4, 25
        };

        /// <summary>
        /// Метод реализующий последнюю перестановку в функции Фейстеля
        /// </summary>
        /// <param name="input">Массив бит на вход</param>
        /// <returns>Массив бит после перестановки</returns>
        public static BitArray Permutation(BitArray input)
        {
            BitArray output = new BitArray(32);
            for (int i = 0; i < 32; i++)
            {
                output[i] = input[P[i] - 1];
            }
            return output;
        }
    }
}
