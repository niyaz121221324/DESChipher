
namespace DESChipherConsoleTool
{
    public class KeyCompressionPermutator
    {
        // Определите таблицу сжатия, указав, какие биты следует сохранить, а какие пропустить.
        private readonly int[] compressionTable =
        {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        public BitArray Permutate(BitArray input)
        {
            if (input.Length != 64)
                throw new ArgumentException("Размер ключа должен быть ровно 64 бит");

            BitArray output = new BitArray(compressionTable.Length);

            for (int i = 0; i < compressionTable.Length; i++)
            {
                output[i] = input[compressionTable[i] - 1]; // Коррекция индекса для соответствия алгоритму DES
            }

            return output;
        }

    }
}
