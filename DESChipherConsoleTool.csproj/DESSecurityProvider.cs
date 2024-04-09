
namespace DESChipherConsoleTool
{
    public class DESSecurityProvider
    {
        private const int MAX_ROUND = 16;

        private readonly InitialPermutator _initialPermutator;
        private readonly KeyCompressionPermutator _keyComperssionPermutator;
        private readonly FinalKeyCompressionPermutator _finalKeyCompressionPermutator;
        private readonly SBoxPermutator _sBoxPermutator;
        private readonly ExpansionFunction _expansionFunction;
        private readonly PBoxPermutator _pBoxPermutator;
        private readonly FinalPermutator _finalPermutator;

        public DESSecurityProvider()
        {
            _initialPermutator = new InitialPermutator();
            _keyComperssionPermutator = new KeyCompressionPermutator();
            _finalKeyCompressionPermutator = new FinalKeyCompressionPermutator();
            _expansionFunction = new ExpansionFunction();
            _sBoxPermutator = new SBoxPermutator();
            _pBoxPermutator = new PBoxPermutator();
            _finalPermutator = new FinalPermutator();
        }

        /// <summary>
        /// Метод для шифрования текста алгоритмом DES
        /// </summary>
        /// <param name="input">Текст для шифрования</param>
        /// <param name="key">Ключ для шифрования</param>
        /// <returns>Шифр текст</returns>
        public string Encrypt(string input, BitArray key)
        {
            BitArray[] blocks = BitArrayHelper.FromString(input).SplitArrayIntoEqualParts().ToArray();
            BitArray[] keys = GenerateKeys(key, true);

            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = _initialPermutator.InitialPermutate(blocks[i]);
                blocks[i] = PerformRound(blocks[i], keys, true);
                blocks[i] = _finalPermutator.Permutate(blocks[i]);
            }

            return BitArrayHelper.MergeArrays(blocks).GetString();
        }

        /// <summary>
        /// Метод для дешифрования текста алгоритмом DES
        /// </summary>
        /// <param name="input">Шифр текст</param>
        /// <param name="key">Ключ для дешифрвания</param>
        /// <returns>Расшифрованный текст</returns>
        public string Decrypt(string input, BitArray key)
        {
            BitArray[] blocks = BitArrayHelper.FromString(input).SplitArrayIntoEqualParts().ToArray();
            BitArray[] keys = GenerateKeys(key, false);

            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = _initialPermutator.InitialPermutate(blocks[i]);
                blocks[i] = PerformRound(blocks[i], keys, false);
                blocks[i] = _finalPermutator.Permutate(blocks[i]);
            }

            return BitArrayHelper.MergeArrays(blocks).GetString();
        }

        private BitArray PerformRound(BitArray block, BitArray[] keys, bool isEncrypt)
        {
            BitArray leftBlock = block.LeftHalf();
            BitArray rightBlock = block.RightHalf();

            for (int round = 0; round < MAX_ROUND; round++)
            {
                if (isEncrypt)
                {
                    BitArray previousRightBlock = rightBlock;
                    rightBlock = leftBlock.Xor(PerformFunction(rightBlock, keys[round]));
                    leftBlock = previousRightBlock;
                }
                else
                {
                    BitArray prevoiusLeftBlock = leftBlock;
                    leftBlock = rightBlock.Xor(PerformFunction(leftBlock, keys[round]));
                    rightBlock = prevoiusLeftBlock;
                }
            }

            block = BitArrayHelper.MergeArrays(leftBlock, rightBlock);
            return block;
        }

        private BitArray PerformFunction(BitArray block, BitArray key)
        {
            block = _expansionFunction.Expand(block);
            block = block.Xor(key);
            block = _sBoxPermutator.Permutate(block);
            block = _pBoxPermutator.Permutate(block);
            return block;
        }

        private BitArray[] GenerateKeys(BitArray key, bool isEncrypt)
        {
            BitArray[] keys = new BitArray[MAX_ROUND];

            int startRound = isEncrypt ? 0 : MAX_ROUND - 1;
            int step = isEncrypt ? 1 : -1;

            BitArray leftKeyBlock = key.LeftHalf();
            BitArray rightKeyBlock = key.RightHalf();

            for (int round = startRound; isEncrypt ? round < MAX_ROUND : round >= 0; round += step)
            {
                int shiftValue = GetShiftValue(round + 1);

                leftKeyBlock = leftKeyBlock.LeftShift(shiftValue);
                rightKeyBlock = rightKeyBlock.LeftShift(shiftValue);

                BitArray permutatedKey = BitArrayHelper.MergeArrays(leftKeyBlock, rightKeyBlock);

                int index = isEncrypt ? round : MAX_ROUND - (round + 1);
                keys[index] = _finalKeyCompressionPermutator.Permutate(permutatedKey);
            }

            return keys;
        }

        private int GetShiftValue(int round)
        {
            switch (round)
            {
                case 1:
                case 2:
                case 9:
                case 16:
                    return 1;
                default:
                    return 2;
            }
        }
    }
}