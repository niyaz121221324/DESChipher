
namespace DESChipherConsoleTool
{
    public interface ISBoxPermutator
    {
        /// <summary>
        /// Метод реализующий перестановку s-блоками алгоритма des
        /// </summary>
        /// <param name="input">Битовый массив на вход для преоброзования</param>
        /// <returns>Преобразованный битовый массив</returns>
        BitArray Permutate(BitArray input);
    }
}
