
namespace DESChipherConsoleTool
{
    class KeysGenerator
    {
        // Таблица для изначального сжатия ключа
        private static readonly int[] PC1 = {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        // Таблица для перестановки PC2
        private static readonly int[] PC2 = {
            14, 17, 11, 24, 1, 5, 3, 28,
            15, 6, 21, 10, 23, 19, 12, 4,
            26, 8, 16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56,
            34, 53, 46, 42, 50, 36, 29, 32
        };

        // Таблица для определения значения сдвига влево для каждого раунда
        private static readonly int[] Shifts = {
            1, 1, 2, 2, 2, 2, 2, 2,
            1, 2, 2, 2, 2, 2, 2, 1
        };

        private BitArray Permute(BitArray input, int[] table)
        {
            BitArray output = new BitArray(table.Length);
            for (int i = 0; i < table.Length; i++)
            {
                output[i] = input[table[i] - 1];
            }
            return output;
        }

        private BitArray LeftShift(BitArray input, int count)
        {
            BitArray output = new BitArray(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[(i + count) % input.Length];
            }
            return output;
        }

        public BitArray[] GenerateKeys(BitArray key, BitArray[] subkeys)
        {
            BitArray permutedKey = Permute(key, PC1);
            BitArray C = new BitArray(28);
            BitArray D = new BitArray(28);

            for (int i = 0; i < 28; i++)
            {
                C[i] = permutedKey[i];
                D[i] = permutedKey[i + 28];
            }

            for (int i = 0; i < 16; i++)
            {
                C = LeftShift(C, Shifts[i]);
                D = LeftShift(D, Shifts[i]);

                BitArray combined = new BitArray(56);
                for (int j = 0; j < 28; j++)
                {
                    combined[j] = C[j];
                    combined[j + 28] = D[j];
                }

                subkeys[i] = Permute(combined, PC2);
            }

            return subkeys;
        }
    }
}
