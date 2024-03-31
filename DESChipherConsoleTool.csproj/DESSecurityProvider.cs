
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

        /// <summary>
        /// Шифрует входной текст с использованием алгоритма DES.
        /// </summary>
        /// <param name="input">Текст для шифрования</param>
        /// <param name="key">Ключ для шифрования</param>
        /// <returns>Шифр текст</returns>
        public string Encrypt(string input, BitArray key)
        {
            BitArray[] blocks = PrepareBlocks(input);
            BitArray[] keyBlocks = GenerateKey(key);

            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = _initialPermutator.InitialPermutate(blocks[i]);
                EncryptBlock(blocks[i], keyBlocks);
                blocks[i] = _finalPermutator.Permutate(blocks[i]);
            }
            
            return MergeBlocksToString(blocks);
        }

        /// <summary>
        /// Дешифрует входной текст, зашифрованный с использованием алгоритма DES.
        /// </summary>
        /// <param name="input">Шифр текст</param>
        /// <param name="key">Ключ для дешифрования</param>
        /// <returns>Исходный текст</returns>
        public string Decrypt(string input, BitArray key)
        {
            BitArray[] blocks = PrepareBlocks(input);
            BitArray[] keyBlocks = GenerateKey(key, false);

            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = _finalPermutator.Permutate(blocks[i]);
                DecryptBlock(blocks[i], keyBlocks);
                blocks[i] = _initialPermutator.InitialPermutate(blocks[i]);
            }

            return MergeBlocksToString(blocks);
        }

        private BitArray[] PrepareBlocks(string input)
        {
            BitArray inputBitArray = BitArrayHelper.FromString(input);
            return inputBitArray.SplitArrayIntoEqualParts(64).ToArray();
        }

        private void EncryptBlock(BitArray block, BitArray[] keyBlocks)
        {
            BitArray leftBlock = block.LeftHalf();
            BitArray rightBlock = block.RightHalf();

            for (int j = 0; j < MAX_ROUND; j++)
            {
                ProcessRound(ref leftBlock, ref rightBlock, keyBlocks[j]);
            }

            block.AssignHalves(rightBlock, leftBlock);
        }

        private void DecryptBlock(BitArray block, BitArray[] keyBlocks)
        {
            BitArray leftBlock = block.LeftHalf();
            BitArray rightBlock = block.RightHalf();

            for (int j = 0; j < MAX_ROUND; j++)
            {
                ProcessRound(ref leftBlock, ref rightBlock, keyBlocks[j]);
            }

            block.AssignHalves(rightBlock, leftBlock);
        }

        private void ProcessRound(ref BitArray leftBlock, ref BitArray rightBlock, BitArray keyBlock)
        {
            BitArray functionResult = Function(rightBlock, keyBlock);
            BitArray temp = rightBlock;
            rightBlock = leftBlock.Xor(functionResult);
            leftBlock = temp;
        }

        private string MergeBlocksToString(BitArray[] blocks)
        {
            BitArray encryptedBitArray = BitArrayHelper.MergeArrays(blocks);
            return encryptedBitArray.GetString();
        }

        private BitArray Function(BitArray block, BitArray key)
        {
            BitArray expandedRightBlock = _expansionFunction.Expand(block);
            BitArray xoredBlock = expandedRightBlock.Xor(key);
            BitArray substitutedBlock = _sBoxPermutator.Permutate(xoredBlock);
            BitArray permutedBlock = _landscapePermutator.Permutate(substitutedBlock);
            return permutedBlock;
        }

        private BitArray[] GenerateKey(BitArray key, bool isEncrypt = true)
        {
            key = _keyComperssionPermutator.Permutate(key);

            BitArray[] roundKeys = new BitArray[MAX_ROUND];

            int startRound = isEncrypt ? 0 : MAX_ROUND - 1;
            int endRound = isEncrypt ? MAX_ROUND : -1;
            int roundStep = isEncrypt ? 1 : -1;

            for (int round = startRound; round != endRound; round += roundStep)
            {
                roundKeys[round] = GenerateRoundKey(key, round);
            }

            return roundKeys;
        }

        private BitArray GenerateRoundKey(BitArray key, int round)
        {
            BitArray leftKeyHalf = key.LeftHalf();
            BitArray rightKeyHalf = key.RightHalf();

            int shiftValue = GetShiftValue(round + 1);

            leftKeyHalf = leftKeyHalf.ShiftArrayLeft(shiftValue);
            rightKeyHalf = rightKeyHalf.ShiftArrayLeft(shiftValue);

            key.AssignHalves(leftKeyHalf, rightKeyHalf);

            return _finalKeyCompressionPermutator.Permutate(key);
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