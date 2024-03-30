﻿
namespace DESChipherConsoleTool
{
    public class FinalKeyCompressionPermutator : IFinalKeyCompressionPermutator
    {
        private readonly int[] keyPC2Table = 
        {
            13, 16, 10, 23,  0,  4,
             2, 27, 14,  5, 20,  9,
            22, 18, 11,  3, 25,  7,
            15,  6, 26, 19, 12,  1,
            40, 51, 30, 36, 46, 54,
            29, 39, 50, 44, 32, 47,
            43, 48, 38, 55, 33, 52,
            45, 41, 49, 35, 28, 31
        };


        public BitArray Permutate(BitArray input)
        {
            bool[] bools = new bool[keyPC2Table.Length];

            for (int i = 0; i < bools.Length; i++)
            {
                bools[i] = input[keyPC2Table[i]];
            }

            return new BitArray(bools);
        }
    }
}
