
namespace DESChipherConsoleTool
{
    public static class BitArrayHelper
    {
        /// <summary>
        /// Преобразует массив байтов в массив битов, корректируя порядок битов в каждом байте так, чтобы они были представлены в правильном порядке.
        /// </summary>
        /// <param name="bytes">Массив байтов для преобразования в массив битов.</param>
        /// <returns>Массив битов, представляющий переданные байты с корректным порядком битов.</returns>
        public static BitArray ConvertCorrectByteToBitArray(byte[] bytes)
        {
            List<bool> bools = new List<bool>();

            for (int i = 0; i < bytes.Length; i++)
            {
                BitArray bitArray = new BitArray(new byte[] { bytes[i] });
                bools.AddRange(bitArray.ToCorrectBitArray().ToBoolArray());
            }

            return new BitArray(bools.ToArray());
        }

        /// <summary>
        /// Объединяет несколько битовых массивов в один.
        /// </summary>
        /// <param name="array">Массивы битов, которые необходимо объединить.</param>
        /// <returns>Результат объединения всех переданных массивов.</returns>
        public static BitArray MergeArrays(params BitArray[] bitArrays)
        {
            List<bool> bools = new List<bool>();

            foreach (var bitArray in bitArrays)
            {
                for (int i = 0; i < bitArray.Length; i++)
                {
                    bools.Add(bitArray[i]);
                }
            }

            return new BitArray(bools.ToArray());
        }

        /// <summary>
        /// Производит операцию над элементами двух битовых массивов с помощью указанной функции.
        /// </summary>
        /// <param name="array">Первая массив.</param>
        /// <param name="otherArray">Вторая массив.</param>
        /// <param name="func">Функция, определяющая операцию над элементами массивов.</param>
        /// <returns>Результат операции над элементами двух битовых массивов.</returns>
        public static BitArray ApplyOperation(BitArray array, BitArray otherArray, Func<bool, bool, bool> func)
        {
            if (array.Length != otherArray.Length)
                throw new ArgumentException("Exception: The array sizes do not match.");

            BitArray bitArray = new BitArray(new bool[array.Length]);

            for (int i = 0; i < array.Length; i++)
            {
                bitArray[i] = func.Invoke(array[i], otherArray[i]);
            }

            return bitArray;
        }

        /// <summary>
        /// Генерирует битовый массив заданной длинный
        /// </summary>
        /// <param name="size">Размер генерируемого массива</param>
        /// <returns>Новый битовый массив заданной длинны.</returns>
        public static BitArray GenerateRandomBitArray(int size = 64)
        {
            Random random = new Random();

            bool[] array = new bool[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(2) == 1;
            }

            return new BitArray(array);
        }

        /// <summary>
        /// Генерирует массив битов из строки
        /// </summary>
        /// <param name="input">строка на вход</param>
        /// <returns>массив бит</returns>
        public static BitArray FromString(string input)
        {
            List<bool> bools = new List<bool>();

            for (int i = 0; i < input.Length; i++)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(input[i].ToString());
                bools.AddRange(ConvertCorrectByteToBitArray(bytes).ToBoolArray());
            }

            return new BitArray(bools.ToArray());
        }

        /// <summary>
        /// Генерирует массив бит из массива чисел
        /// </summary>
        /// <param name="inputArray">массив чисел на вход</param>
        /// <returns>массив бит</returns>
        public static BitArray FromIntArray(int[] inputArray)
        {
            List<bool> bools = new List<bool>();

            for (int i = 0; i < inputArray.Length; i++)
            {
                bools.AddRange(ConvertToBinary(inputArray[i]));
            }

            return new BitArray(bools.ToArray());
        }

        /// <summary>
        /// Метод который возвращает 4 бита из числа 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool[] ConvertToBinary(int input)
        {
            string binaryString = Convert.ToString(input, 2);
            bool[] boolArray = new bool[4];

            for (int i = 0; i < binaryString.Length; i++)
            {
                char bitChar = binaryString[i];
                boolArray[i] = bitChar == '1';
            }
            
            if (binaryString.Length < 4)
            {
                int value = binaryString.Length - 4;
                boolArray = PadLeft(boolArray, value);
            }

            return boolArray;
        }

        private static bool[] PadLeft(bool[] boolArray, int count)
        {
            List<bool> bools = new List<bool>();

            for (int i = 0; i < count; i++)
            {
                bools.Add(false);
            }

            bools.AddRange(boolArray);
            return bools.ToArray();
        }
    }
}
