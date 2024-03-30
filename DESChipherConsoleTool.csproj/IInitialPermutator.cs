
namespace DESChipherConsoleTool
{
    public interface IInitialPermutator
    {
        /// <summary>
        /// Метод выполняет начальную перестановку в алгоритме DES.
        /// </summary>
        /// <param name="input">Входной блок данных для перестановки.</param>
        /// <returns>Битовый массив, представляющий результат начальной перестановки.</returns>
        BitArray InitialPermutate(BitArray input);
    }
}
