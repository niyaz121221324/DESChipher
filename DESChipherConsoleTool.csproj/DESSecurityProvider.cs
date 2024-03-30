
using System.Xml.Linq;

namespace DESChipherConsoleTool
{
    public class DESSecurityProvider
    {
        private const int MAX_ROUND = 16;

        private readonly IInitialPermutator _initialPermutator;
        private readonly IKeyComperssionPermutator _keyComperssionPermutator;
        private readonly IFinalKeyCompressionPermutator _finalKeyCompressionPermutator;
        private readonly ISBoxPermutator _sBoxPermutator;
        private readonly IExpansionFunction _expansionFunction;
        private readonly IPFinalPermutator _landscapePermutator;
        private readonly IFinalPermutator _finalPermutator;

        public DESSecurityProvider(BitArray key, IInitialPermutator initialPermutatior = null,
            IFinalKeyCompressionPermutator finalKeyCompressionPermutator = null,
            IKeyComperssionPermutator keyComperssionPermutator = null, 
            IExpansionFunction expansionFunction = null,
            ISBoxPermutator sBoxPermutator = null,
            IPFinalPermutator landscapePermutator = null, 
            IFinalPermutator finalPermutator = null)
        {
            _initialPermutator = initialPermutatior ?? new InitialPermutator();
            _keyComperssionPermutator = keyComperssionPermutator ?? new KeyCompressionPermutator();
            _finalKeyCompressionPermutator = finalKeyCompressionPermutator ?? new FinalKeyCompressionPermutator();
            _expansionFunction = expansionFunction ?? new ExpansionFunction();
            _sBoxPermutator = sBoxPermutator ?? new SBoxPermutator();
            _landscapePermutator = landscapePermutator ?? new PFinalPermutator();
            _finalPermutator = finalPermutator ?? new FinalPermutator();
        }

        /// метод для шифрования текста алгоритмом DES
        /// </summary>
        /// <param name="input">Текст для шифрования</param>
        /// <param name="key">Ключ для шифрования</param>
        /// <returns>Шифр текст</returns>
        public string Encrypt(string input, BitArray key)
        {
            BitArray inputBitArray = BitArrayHelper.FromString(input);
            BitArray[] blocks = inputBitArray.SplitArrayIntoEqualParts(64).ToArray();

            BitArray[] keyBlocks = GenerateKey(key);

            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = _initialPermutator.InitialPermutate(blocks[i]);

                BitArray leftBlock = blocks[i].LeftHalf();
                BitArray rightBlock = blocks[i].RightHalf();

                BitArray previousRightBlock = rightBlock;
                BitArray previousLeftBlock = leftBlock;

                leftBlock = previousRightBlock;
                rightBlock = previousLeftBlock;

                for (int j = MAX_ROUND - 1; j >= 1; j--)
                {
                    leftBlock = previousRightBlock;
                    rightBlock = previousLeftBlock.Xor(Function(previousRightBlock, keyBlocks[j - 1]));

                    previousLeftBlock = leftBlock;
                    previousRightBlock = rightBlock;
                }

                blocks[i] = BitArrayHelper.MergeArrays(rightBlock, leftBlock);
                blocks[i] = _finalPermutator.Permutate(blocks[i]);
            }

            BitArray encryptedBitArray = BitArrayHelper.MergeArrays(blocks);
            return encryptedBitArray.GetString();
        }

        /// <summary>
        /// метод для дешифрования алгоритмом DES
        /// </summary>
        /// <param name="input">зашифрованный текст на вход</param>
        /// <param name="key"></param>
        /// <returns>Дешифрованный текст</returns>
        public string Decrypt(string input, BitArray key)
        {
            BitArray inputBitArray = BitArrayHelper.FromString(input);
            BitArray[] blocks = inputBitArray.SplitArrayIntoEqualParts(64).ToArray();

            BitArray[] keyBlocks = GenerateKey(key);

            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = _initialPermutator.InitialPermutate(blocks[i]);

                BitArray leftBlock = blocks[i].LeftHalf();
                BitArray rightBlock = blocks[i].RightHalf();

                BitArray previousRightBlock = rightBlock;
                BitArray previousLeftBlock = leftBlock;

                for (int j = MAX_ROUND - 1; j >= 1; j--)
                {
                    leftBlock = previousRightBlock;
                    rightBlock = previousLeftBlock.Xor(Function(previousRightBlock, keyBlocks[MAX_ROUND - j - 1]));

                    previousLeftBlock = leftBlock;
                    previousRightBlock = rightBlock;
                }

                leftBlock = previousLeftBlock.Xor(Function(previousRightBlock, keyBlocks.First()));
                rightBlock = previousRightBlock;

                blocks[i] = BitArrayHelper.MergeArrays(rightBlock, leftBlock);
                blocks[i] = _finalPermutator.Permutate(blocks[i]);
            }

            BitArray decryptedBitArray = BitArrayHelper.MergeArrays(blocks);
            return decryptedBitArray.GetString();
        }

        private BitArray Function(BitArray block, BitArray key)
        {
            BitArray expandedRightBlock = _expansionFunction.Expand(block);
            BitArray xoredBlock = expandedRightBlock.Xor(key);
            BitArray substitutedBlock = _sBoxPermutator.Permutate(xoredBlock);
            BitArray permutedBlock = _landscapePermutator.Permutate(substitutedBlock);

            return permutedBlock; 
        }

        private BitArray[] GenerateKey(BitArray key)
        {
            BitArray[] roundKeys = new BitArray[MAX_ROUND];
            key = _keyComperssionPermutator.Permutate(key);

            for (int round = 0; round < MAX_ROUND; round++)
            {
                BitArray[] keyHalves = key.SplitBitsIntoEqualsPortions(2);
                BitArray leftKeyHalf = keyHalves[0];
                BitArray rightKeyHalf = keyHalves[1];

                int shiftValue = GetShiftValue(round + 1);
                leftKeyHalf = leftKeyHalf.ShiftArrayLeft(shiftValue);
                rightKeyHalf = rightKeyHalf.ShiftArrayLeft(shiftValue); 

                BitArray fullShiftedKey = BitArrayHelper.MergeArrays(leftKeyHalf, rightKeyHalf);
                roundKeys[round] = _finalKeyCompressionPermutator.Permutate(fullShiftedKey);
            }

            return roundKeys;
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