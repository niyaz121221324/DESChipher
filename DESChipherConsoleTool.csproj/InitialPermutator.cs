
namespace DESChipherConsoleTool
{
    public class InitialPermutator 
    {
        private readonly int[] initialPermutationTable =
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        /// <summary>
        /// Метод выполняет начальную перестановку в алгоритме DES.
        /// </summary>
        /// <param name="input">Входной блок данных для перестановки.</param>
        /// <returns>Битовый массив, представляющий результат начальной перестановки.</returns>
        public BitArray InitialPermutate(BitArray input)
        {
            if (input.Length != 64)
                throw new ArgumentException("Размер входного массива должен быть 64 бита.");

            BitArray output = new BitArray(64);

            for (int i = 0; i < 64; i++)
            {
                int newIndex = initialPermutationTable[i] - 1; // Индекс для новой позиции бита
                output[i] = input[newIndex];
            }

            return output;
        }
    }
}
