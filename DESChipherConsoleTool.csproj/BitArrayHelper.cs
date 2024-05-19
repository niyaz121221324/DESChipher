
namespace DESChipherConsoleTool
{
    public static class BitArrayHelper
    {
        /// <summary>
        /// Объединяет несколько битовых массивов в один.
        /// </summary>
        /// <param name="array">Массивы битов, которые необходимо объединить.</param>
        /// <returns>Результат объединения всех переданных массивов.</returns>
        public static BitArray MergeArrays(params BitArray[] bitArrays)
        {
            int totalLength = bitArrays.Sum(array => array.Length);

            BitArray mergedArray = new BitArray(totalLength);

            int currentIndex = 0;
            foreach (var bitArray in bitArrays)
            {
                for (int i = 0; i < bitArray.Length; i++)
                {
                    mergedArray[currentIndex++] = bitArray[i];
                }
            }

            return mergedArray;
        }
    }
}
