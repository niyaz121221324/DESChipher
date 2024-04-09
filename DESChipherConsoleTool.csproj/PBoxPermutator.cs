
namespace DESChipherConsoleTool
{ 
    /// <summary>
    /// P-Перестановка
    /// </summary>
    class PBoxPermutator 
    {
        private readonly int[] PTable = new int[]
        {
            16, 7, 20, 21, 29, 12, 28, 17,
            1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9,
            19, 13, 30, 6, 22, 11, 4, 25
        };

        /// <summary>
        /// Метод реализующий перестановку P в алгоритме DES 
        /// </summary>
        /// <param name="input">битовый массив на вход</param>
        /// <returns>массив после перестановки</returns>
        public BitArray Permutate(BitArray input)
        {
            bool[] bools = new bool[PTable.Length];

            for (int i = 0; i < bools.Length; i++)
            {
                bools[i] = input[PTable[i] - 1];
            }

            return new BitArray(bools);
        }
    }
}
