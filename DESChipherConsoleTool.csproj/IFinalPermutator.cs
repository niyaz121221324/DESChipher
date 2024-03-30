
namespace DESChipherConsoleTool
{
    public interface IFinalPermutator
    {
        /// <summary>
        /// Метод реализующий конечную перестановку в алгоритме DES
        /// </summary>
        /// <param name="input">Массив бит на вход</param>
        /// <returns>Массив бит после перестановки</returns>
        BitArray Permutate(BitArray input);
    }
}
