
namespace DESChipherConsoleTool
{
    public class KeyCompressionPermutator
    {
        // Определите таблицу сжатия, указав, какие биты следует сохранить, а какие пропустить.
        private readonly int[] compressionTable =
        {
            1, 2, 3, 4, 5, 6, 7,
            9, 10, 11, 12, 13, 14, 15,
            17, 18, 19, 20, 21, 22, 23,
            25, 26, 27, 28, 29, 30, 31,
            33, 34, 35, 36, 37, 38, 39,
            41, 42, 43, 44, 45, 46, 47,
            49, 50, 51, 52, 53, 54, 55,
            57, 58, 59, 60, 61, 62, 63
        };

        /// <summary>
        /// Производит перестановку битов входного блока данных согласно определенной таблице сжатия ключа.
        /// </summary>
        /// <param name="input">Входной блок данных, который нужно переставить.</param>
        /// <returns>Битовый массив, представляющий результат перестановки.</returns>
        public BitArray Permutate(BitArray input)
        {
            if (input.Length != 64)
                throw new ArgumentException("Размер ключа должен быть ровно 64 бит");

            BitArray output = new BitArray(compressionTable.Length);

            for (int i = 0; i < compressionTable.Length; i++)
            {
                output[i] = input[compressionTable[i] - 1]; // Скорректировать индекс, отсчитываемый от 0
            }

            return output;
        }
    }
}
