
namespace DESChipherConsoleTool
{
    public class KeyCompressionPermutator
    {
        /// <summary>
        /// Производит перестановку битов входного блока данных согласно определенной таблице сжатия ключа.
        /// </summary>
        /// <param name="input">Входной блок данных, который нужно переставить.</param>
        /// <returns>Битовый массив, представляющий результат перестановки.</returns>
        public BitArray Permutate(BitArray input)
        {
            if (input.Length != 64)
                throw new ArgumentException("Размер ключа должен быть не менее 64 бит");

            List<bool> result = new List<bool>();

            for (int i = 0; i < input.Length; i++)
            {
                if ((i + 1) % 8 != 0) // пропускаем каждый 8 бит для сжатия ключа дл 56 бит
                {
                    result.Add(input[i]);
                }
            }

            return new BitArray(result.ToArray());
        }
    }
}
