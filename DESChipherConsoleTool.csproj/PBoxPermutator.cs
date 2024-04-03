

namespace DESChipherConsoleTool
{
    class PBoxPermutator : IPFinalPermutator
    {
        private readonly int[] PTable =
        {
            14,  6, 19, 20,
            28, 11, 27, 16,
             0, 14, 22, 25,
             4, 17, 30,  9,
             1,  7, 23, 13,
            31, 26,  2,  8,
            18, 12, 29,  5,
            21, 10,  3, 24
        };

        public BitArray Permutate(BitArray input)
        {
            bool[] bools = new bool[PTable.Length];

            for (int i = 0; i < bools.Length; i++)
            {
                bools[i] = input[PTable[i]];
            }

            return new BitArray(bools);
        }
    }
}
