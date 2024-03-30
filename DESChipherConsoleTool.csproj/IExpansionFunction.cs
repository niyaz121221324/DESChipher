
namespace DESChipherConsoleTool
{
    public interface IExpansionFunction
    {
        // <summary>
        /// Расширяет входной массив <paramref name="input"/> согласно функции расширения, используемой в алгоритме.
        /// </summary>
        /// <param name="input">Входной массив, для расширения.</param>
        /// <returns>Расширенный массив, полученная в результате применения функции расширения.</returns>
        BitArray Expand(BitArray input);
    }
}
