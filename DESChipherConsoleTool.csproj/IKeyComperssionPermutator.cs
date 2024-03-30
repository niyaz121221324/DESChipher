
namespace DESChipherConsoleTool
{
    public interface IKeyComperssionPermutator
    {
        /// <summary>
        /// Производит перестановку битов входного блока данных согласно определенной таблице сжатия ключа.
        /// </summary>
        /// <param name="input">Входной блок данных, который нужно переставить.</param>
        /// <returns>Битовый массив, представляющий результат перестановки.</returns>
        BitArray Permutate(BitArray input);
    }
}
