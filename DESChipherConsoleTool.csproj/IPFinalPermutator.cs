
namespace DESChipherConsoleTool
{
    /// <summary>
    /// P-Перестановка
    /// </summary>
    public interface IPFinalPermutator
    {
        /// <summary>
        /// Метод реализующий перестановку P в алгоритме DES 
        /// </summary>
        /// <param name="input">битовый массив на вход</param>
        /// <returns>массив после перестановки</returns>
        BitArray Permutate(BitArray input);
    }
}
