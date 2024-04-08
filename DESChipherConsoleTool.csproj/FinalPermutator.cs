
namespace DESChipherConsoleTool
{
    class FinalPermutator : IFinalPermutator
    {
        private readonly int[] finalPermutationTable = 
        {
            39,  7, 47, 15, 55, 23, 63, 31,
            38,  6, 46, 14, 54, 22, 62, 30,
            37,  5, 45, 13, 53, 21, 61, 29,
            36,  4, 44, 12, 52, 20, 60, 28,
            35,  3, 43, 11, 51, 19, 59, 27,
            34,  2, 42, 10, 50, 18, 58, 26,
            33,  1, 41,  9, 49, 17, 57, 25,
            32,  0, 40,  8, 48, 16, 56, 24
        };

        public BitArray Permutate(BitArray input)
        {
            if (input.Length != 64)
                throw new ArgumentException("Размер входного массива должен быть 64 бита.");

            BitArray output = new BitArray(64);

            for (int i = 0; i < 64; i++)
            {
                int newIndex = finalPermutationTable[i]; // Индекс для новой позиции бита
                output[i] = input[newIndex];
            }

            return output;
        }
    }
}
