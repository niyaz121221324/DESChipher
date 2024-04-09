
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
                throw new ArgumentException("Не совпадают длинны массивов");

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
        /// Преобразует массив байтов в массив битов, корректируя порядок битов в каждом байте так, чтобы они были представлены в правильном порядке.
        /// </summary>
        /// <param name="bytes">Массив байтов для преобразования в массив битов.</param>
        /// <returns>Массив битов, представляющий переданные байты с корректным порядком битов.</returns>
        public static BitArray ConvertCorrectByteToBitArray(byte[] bytes)
        {
            List<bool> bools = new List<bool>();

            for (int i = 0; i < bytes.Length; i++)
            {
                // Преобразуем текущий байт в массив битов
                BitArray bitArray = new BitArray(new byte[] { bytes[i] });

                // Переворачиваем порядок битов в массиве битов
                bool[] reversedBits = new bool[bitArray.Length];
                for (int j = 0; j < bitArray.Length; j++)
                {
                    reversedBits[j] = bitArray[bitArray.Length - 1 - j];
                }

                // Добавляем скорректированные биты в список
                bools.AddRange(reversedBits);
            }

            // Создаем и возвращаем новый экземпляр BitArray на основе списка битов
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
        /// Преобразует целое число в его двоичное представление из 4 битов.
        /// </summary>
        /// <param name="input">Целое число для преобразования.</param>
        /// <returns>Массив из 4 булевых значений, представляющих двоичное представление числа.</returns>
        public static bool[] ConvertToBinary(int input)
        {
            // Представление числа в двоичном формате
            string binaryString = Convert.ToString(input, 2);
            bool[] boolArray = new bool[4];

            // Проход по строке двоичного представления числа и преобразование каждого символа в булево значение
            for (int i = 0; i < binaryString.Length; i++)
            {
                char bitChar = binaryString[i];
                boolArray[i] = bitChar == '1';
            }

            // Если длина двоичной строки меньше 4 бит, то заполняем ее слева нулями
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
