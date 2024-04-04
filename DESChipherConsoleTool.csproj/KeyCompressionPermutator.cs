
namespace DESChipherConsoleTool
{
    public class KeyCompressionPermutator : IKeyComperssionPermutator
    {
        private readonly int[] keyPC1 = 
        {
            55, 47, 39, 31, 23, 15,  7,
            54, 46, 38, 30, 22, 14,  6,
            53, 45, 37, 29, 21, 13,  5,
            52, 44, 36, 28, 20, 12,  4,
            51, 43, 35, 27, 19, 11,  3,
            50, 42, 34, 26, 18, 10,  2,
            49, 41, 33, 25, 17,  9,  1,
            48, 40, 32, 24, 16,  8,  0
        };

        //public BitArray Permutate(BitArray input)
        //{
        //    bool[] bools = new bool[keyPC1.Length];

        //    for (int i = 0; i < bools.Length; i++)
        //    {
        //        bools[i] = input[keyPC1[i]];
        //    }

        //    return new BitArray(bools);
        //}

        public BitArray Permutate(BitArray input)
        {
            if (input.Length != 64)
                throw new ArgumentException("Размер ключа должен быть не менее 64 бит");

            List<bool> result = new List<bool>();

            for (int i = 0; i < input.Length; i++)
            {
                if (i + 1 % 8 != 0)
                {
                    result.Add(input[i]);
                }
            }

            return new BitArray(result.ToArray());
        }
    }
}
