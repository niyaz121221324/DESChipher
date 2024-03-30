
namespace DESChipherConsoleTool
{
    public interface IFinalKeyCompressionPermutator
    {
        /// <summary>
        /// Переставляет биты входного массива с использованием определенного алгоритма сжатия ключа.
        /// </summary>
        /// <param name="input">Входной битовый массив для перестановки.</param>
        /// <returns>Переставленный битовый массив.</returns>
        BitArray Permutate(BitArray input);
    }
}
