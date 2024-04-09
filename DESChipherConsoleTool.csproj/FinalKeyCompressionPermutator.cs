﻿
namespace DESChipherConsoleTool
{
    public class FinalKeyCompressionPermutator
    {
        private readonly int[] keyPC2Table =
        {
            14, 17, 11, 24, 1,  5,  3,  28, 15, 6,  21, 10, 23, 19, 12, 4,
            26, 8,  16, 7,  27, 20, 13, 2,  41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
        };

        /// <summary>
        /// Переставляет биты входного массива с использованием определенного алгоритма сжатия ключа.
        /// </summary>
        /// <param name="input">Входной битовый массив для перестановки.</param>
        /// <returns>Переставленный битовый массив.</returns>
        public BitArray Permutate(BitArray input)
        {
            bool[] bools = new bool[keyPC2Table.Length];

            for (int i = 0; i < bools.Length; i++)
            {
                bools[i] = input[keyPC2Table[i] - 1];
            }

            return new BitArray(bools);
        }
    }
}
