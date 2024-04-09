
namespace DESChipherConsoleTool
{
    public static class BitArrayExtentions
    {
        /// <summary>
        /// Создаёт bool[] из массива битов
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns>bool[]</returns>
        /// <exception cref="ArgumentNullException">если массив пустой то выведет exception</exception>
        public static bool[] ToBoolArray(this BitArray bitArray)
        {
            if (bitArray == null)
                throw new ArgumentNullException(nameof(bitArray));

            bool[] array = new bool[bitArray.Length];

            for (int i = 0; i < bitArray.Length; i++)
            {
                array[i] = bitArray[i];
            }

            return array;
        }

        /// <summary>
        /// Создаёт массив byte из массива битов
        /// </summary>
        /// <param name="bits"></param>
        /// <returns>byte[]</returns>
        public static byte[] ToByteArray(this BitArray bits)
        {
            int numBytes = bits.Length / 8;
            if (bits.Length % 8 != 0)
            {
                numBytes++;
            }

            byte[] bytes = new byte[numBytes];
            bits.CopyTo(bytes, 0);
            return bytes;
        }

        /// <summary>
        /// Создаёт int[] из массива бит
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this BitArray bitArray)
        {
            int[] array = new int[bitArray.Length];

            for (int i = 0; i < bitArray.Length; i++)
            {
                array[i] = bitArray[i] ? 1 : 0;
            }

            return array;
        }

        /// <summary>
        /// Возвращает битовый массив, содержащий первые n битов из исходного массива.
        /// </summary>
        /// <param name="originalBits">Исходный битовый массив.</param>
        /// <param name="n">Количество битов, которые нужно взять из исходного массива.</param>
        /// <returns>Битовый массив, содержащий первые n битов из исходного массива.</returns>
        /// /// <exception cref="ArgumentNullException">если массив пустой то выведет exception</exception>
        public static BitArray GetFirstNBits(this BitArray bitArray, int n)
        {
            if (bitArray == null)
                throw new ArgumentNullException(nameof(bitArray));

            bool[] newArray = new bool[n];
            bool[] currBoolArray = bitArray.ToBoolArray();

            for (int i = 0; i < n; i++)
            {
                newArray[i] = currBoolArray[i];
            }

            return new BitArray(newArray);
        }

        /// <summary>
        /// Этот метод преобразует объект типа BitArray в строку, представляющую последовательность битов.
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns>Строку преобразованную из битового массива</returns>
        /// <exception cref="ArgumentNullException">Вызывается если массив пуст</exception>
        /// <exception cref="ArgumentException">Вызывается, когда длинна массива не кратно 8.</exception>
        public static string GetString(this BitArray bitArray)
        {
            if (bitArray == null)
                throw new ArgumentNullException(nameof(bitArray));

            if (bitArray.Length % 8 != 0)
                throw new ArgumentException("Длинна строки должна содержать делимое на 8 число бит.");

            StringBuilder sb = new StringBuilder(bitArray.Length / 8);

            for (int i = 0; i < bitArray.Length; i += 8)
            {
                int symbolValue = 0;

                for (int j = 0; j < 8; j++)
                {
                    symbolValue <<= 1;
                    if (bitArray[i + j])
                        symbolValue |= 1;
                }

                sb.Append((char)symbolValue);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Разбивает массив битов на равные части.
        /// </summary>
        /// <param name="bitArray">Массив битов для разделения.</param>
        /// <param name="partSize">Размер каждой части.</param>
        /// <returns>Список, содержащий части исходного массива битов.</returns>
        /// <exception cref="ArgumentNullException">Вызывается, если массив пуст.</exception>
        public static List<BitArray> SplitArrayIntoEqualParts(this BitArray bitArray, int partSize = 64)
        {
            if (bitArray == null)
                throw new ArgumentNullException(nameof(bitArray));

            List<BitArray> bitArrays = new List<BitArray>();

            int i = 0;
            int length = bitArray.Length;

            while (i < length)
            {
                int startIndex = i;
                List<bool> part = new List<bool>();

                if (bitArray.Length - i < partSize)
                {
                    length += (bitArray.Length - i) - partSize;

                    int paddingValue = partSize - (bitArray.Length - i);
                    BitArray paddingArray = PadTo16Bits(paddingValue);
                    bool[] paddingBoolArray = paddingArray.ToBoolArray();

                    for (int j = 0; j < partSize; j++)
                    {
                        if (startIndex + j > bitArray.Length - 1)
                        {
                            for (int k = 0; k < paddingBoolArray.Length; k++)
                            {
                                part.Add(paddingBoolArray[k]);
                            }
                            break;
                        }
                        else
                        {
                            part.Add(bitArray[startIndex + j]);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < partSize; j++)
                    {
                        part.Add(bitArray[startIndex + j]);
                    }
                }

                bitArrays.Add(new BitArray(part.ToArray()));
                i += partSize;
            }

            return bitArrays;
        }

        /// <summary>
        /// Дополняет целочисленное значение до представления BitArray фиксированной длины в 16 битов.
        /// </summary>
        /// <param name="value">Целочисленное значение для дополнения.</param>
        /// <returns>Представление BitArray дополненного целочисленного значения длиной 16 битов.</returns>
        public static BitArray PadTo16Bits(int value)
        {
            int numBytes = value / 8;
            if (value % 8 != 0)
            {
                numBytes++;
            }

            byte[] resultBytes = new byte[numBytes];

            byte byteValue = (byte)Convert.ToInt64(value.ToString(), 16);

            for (int i = 0; i < resultBytes.Length; i++)
            {
                resultBytes[i] = byteValue;
            }

            return BitArrayHelper.ConvertCorrectByteToBitArray(resultBytes);
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

            return (new BitArray(array)).ToCorrectBitArray();
        }

        /// <summary>
        /// Преобразует указанный объект BitArray в новый экземпляр BitArray, исправляя порядок его элементов.
        /// Для этого метод сначала конвертирует BitArray в массив boolean при помощи метода ToBoolArray(),
        /// затем разворачивает этот массив в обратном порядке и создает новый BitArray из полученного массива boolean-значений.
        /// </summary>
        /// <param name="bitArray">Исходный объект BitArray, который нужно скорректировать.</param>
        /// <returns>Новый объект BitArray с исправленным порядком элементов.</returns>
        public static BitArray ToCorrectBitArray(this BitArray bitArray)
        {
            int length = bitArray.Length;
            bool[] boolArray = new bool[length];

            // Скопируйте биты из исходного BitArray в логический массив.
            bitArray.CopyTo(boolArray, 0);

            // Поменяйте местами биты внутри каждого байта
            for (int i = 0; i < length / 8; i++)
            {
                Array.Reverse(boolArray, i * 8, 8);
            }

            return new BitArray(boolArray);
        }

        /// <summary>
        /// Разделяет битовый массив на равные части.
        /// </summary>
        /// <param name="bitArray">Исходный битовый массив.</param>
        /// <param name="part">Количество частей, на которые следует разделить массив.</param>
        /// <returns>Массив, содержащий равные порции битов из исходного массива.</returns>
        public static BitArray[] SplitBitsIntoEqualsPortions(this BitArray bitArray, int part)
        {
            if (bitArray == null)
                throw new ArgumentNullException(nameof(bitArray));

            int partSize = bitArray.Length / part;

            BitArray[] result = new BitArray[part];

            int index = 0;
            for (int i = 0; i < part; i++)
            {
                int startIndex = index;

                int currPartSize = Math.Min(partSize, bitArray.Length - index);

                bool[] bools = new bool[currPartSize];

                for (int j = 0; j < currPartSize; j++)
                {
                    bools[j] = bitArray[startIndex + j];
                }

                result[i] = (new BitArray(bools)).ToCorrectBitArray();
                index += currPartSize;
            }

            return result;
        }

        /// <summary>
        /// Метод выполняет сдвиг массива влево.
        /// </summary>
        /// <param name="bitArray">Экземпляр битового массива.</param>
        /// <param name="shiftValue">Значение сдвига.</param>
        /// <returns>Массив после выполнения сдвига.</returns>
        public static BitArray ShiftArrayLeft(this BitArray bitArray, int shiftValue)
        {
            int step = shiftValue % bitArray.Length;

            var tempArray = new bool[bitArray.Length];

            for (int i = 0; i < bitArray.Length; i++)
            {
                int index = (i - step < 0) ? bitArray.Length + i - step : i - step;
                tempArray[index] = bitArray[i];
            }

            for (int i = 0; i < bitArray.Length; i++)
            {
                bitArray[i] = tempArray[i];
            }

            return bitArray;
        }

        /// <summary>
        /// Метод для сдвига элементов в право
        /// </summary>
        /// <param name="bitArray">Экземпляр битового массива</param>
        /// <param name="shiftValue">Значение сдвига.</param>
        /// <returns>Массив после сдвига</returns>
        public static BitArray ShiftArrayRight(this BitArray bitArray, int shiftValue)
        {
            bool[] boolArray = new bool[bitArray.Length];

            for (int i = 0; i < boolArray.Length; i++) 
            {
                boolArray[(i + shiftValue) % boolArray.Length] = bitArray[i];
            }

            return new BitArray(boolArray);
        }

        /// <summary>
        /// Преобразует BitArray в его эквивалентное 32-битное целое число.
        /// </summary>
        /// <param name="bitArray">BitArray для преобразования.</param>
        /// <returns>Целое число, сформированное битами в bitArray.</returns>
        public static int ToInt32(this BitArray bitArray)
        {
            int result = 0;

            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                {
                    result += Convert.ToInt32(Math.Pow(2, i));
                }
            }

            return result;
        }

        /// <summary>
        /// Метод который делит массив по пополам 
        /// </summary>
        /// <param name="array">Экземпляр массива</param>
        /// <returns>Tuple состоящий из двух половин массива</returns>
        public static (BitArray, BitArray) SplitIntoTwoHalves(this BitArray array)
        {
            BitArray leftHalf = new BitArray(array.Length / 2);
            BitArray rightHalf = new BitArray(array.Length / 2);

            for (int i = 0; i < array.Length / 2; i++)
            {
                leftHalf[i] = array[i];
                rightHalf[i] = array[i + array.Length / 2];
            }

            return (leftHalf, rightHalf);
        }

        /// <summary>
        /// Возвращает правую половину массива
        /// </summary>
        /// <param name="bitArray">Экземпляр массива</param>
        /// <returns>Правую половину массива</returns>
        public static BitArray RightHalf(this BitArray bitArray)
        {
            return bitArray.SplitIntoTwoHalves().Item2;
        }

        /// <summary>
        /// Возвращает левую половину массива
        /// </summary>
        /// <param name="bitArray">Экземпляр массива</param>
        /// <returns>Левую половину массива</returns>
        public static BitArray LeftHalf(this BitArray bitArray)
        {
            return bitArray.SplitIntoTwoHalves().Item1;
        }

        /// <summary>
        /// Метод для склеиван6ия двух чвастей массива в оджин массив
        /// </summary>
        /// <param name="bitArray">Экземпляр битового массива</param>
        /// <param name="leftHalf">Левая часть массива</param>
        /// <param name="rightHalf">Правая часить массива</param>
        /// <exception cref="ArgumentException">Если сумма правой и левой части массива не меньше исходного массива</exception>
        public static void AssignHalves(this BitArray bitArray, BitArray leftHalf, BitArray rightHalf)
        {
            if (leftHalf.Length + rightHalf.Length != bitArray.Length)
            {
                throw new ArgumentException("Сумма длин левой и правой половин должна быть равна длине BitArray..");
            }

            // Присваиваем значения левой части
            for (int i = 0; i < leftHalf.Length; i++)
            {
                bitArray[i] = leftHalf[i];
            }

            // Присваиваем значения правой части
            for (int i = 0; i < rightHalf.Length; i++)
            {
                bitArray[i + leftHalf.Length] = rightHalf[i];
            }
        }

        /// <summary>
        /// Метод для получения массива бит в обратном порядке
        /// </summary>
        /// <param name="bitArray">Екземпляр массива</param>
        public static void Reverse(this BitArray bitArray)
        {
            int length = bitArray.Length;
            for (int i = 0; i < length / 2; i++)
            {
                bool temp = bitArray[i];
                bitArray[i] = bitArray[length - 1 - i];
                bitArray[length - 1 - i] = temp;
            }
        }

        /// <summary>
        /// Метод для получения подмассива бит из имеющегося
        /// </summary>
        /// <param name="bitArray">Массив бит</param>
        /// <param name="startIndex">Начальный индекс для получения подмассива</param>
        /// <returns>Массив начиная с startIndex и до конца массива</returns>
        public static BitArray SubBits(this BitArray bitArray, int startIndex = 0)
        {
            if (startIndex < 0 || startIndex >= bitArray.Length)
                throw new ArgumentException("Начальный индекс должен быть больше или равен 0 и меньше длины массива");

            int subLength = bitArray.Length - startIndex;
            bool[] output = new bool[subLength];

            for (int i = 0; i < subLength; i++)
            {
                output[i] = bitArray[i + startIndex];
            }

            return new BitArray(output);
        }
    }
}