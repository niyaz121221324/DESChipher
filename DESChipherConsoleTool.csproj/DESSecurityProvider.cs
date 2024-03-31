
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

        #region Шифрование алгоритмом DES
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
                EncryptBlock(ref blocks[i], keyBlocks);
                blocks[i] = _finalPermutator.Permutate(blocks[i]);
            }
            
            return MergeBlocksToString(blocks);
        }

        // Метод для шифрования алгоритмом DES
        private void EncryptBlock(ref BitArray block, BitArray[] keyBlocks)
        {
            BitArray leftBlock = block.LeftHalf();
            BitArray rightBlock = block.RightHalf();

            for (int j = 0; j < MAX_ROUND; j++)
            {
                ProcessEncryptRound(ref leftBlock, ref rightBlock, keyBlocks[j]);
            }

            block.AssignHalves(rightBlock, leftBlock);
        }

        private void ProcessEncryptRound(ref BitArray leftBlock, ref BitArray rightBlock, BitArray keyBlock)
        {
            BitArray functionResult = Function(rightBlock, keyBlock);
            BitArray temp = rightBlock;
            rightBlock = leftBlock.Xor(functionResult);
            leftBlock = temp;
        }
        #endregion 

        #region Дешифрование алгоритмом DES
        /// <summary>
        /// Дешифрует входной текст, зашифрованный с использованием алгоритма DES.
        /// </summary>
        /// <param name="input">Шифр текст</param>
        /// <param name="key">Ключ для дешифрования</param>
        /// <returns>Исходный текст</returns>
        public string Decrypt(string input, BitArray key)
        {
            BitArray[] blocks = PrepareBlocks(input);
            BitArray[] keyBlocks = GenerateKey(key);

            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = _initialPermutator.InitialPermutate(blocks[i]);
                DecryptBlock(blocks[i], keyBlocks);
                blocks[i] = _finalPermutator.Permutate(blocks[i]);
            }

            return MergeBlocksToString(blocks);
        }

        // Метод для дешифрования блока текста алгоритмом DES
        private void DecryptBlock(BitArray block, BitArray[] keyBlocks)
        {
            BitArray leftBlock = block.LeftHalf();
            BitArray rightBlock = block.RightHalf();

            for (int j = MAX_ROUND - 1; j >= 0; j--)
            {
                ProcessDecryptRound(ref leftBlock, ref rightBlock, keyBlocks[j]);
            }

            block.AssignHalves(rightBlock, leftBlock);
        }

        private void ProcessDecryptRound(ref BitArray leftBlock, ref BitArray rightBlock, BitArray keyBlock)
        {
            BitArray functionResult = Function(leftBlock, keyBlock);
            BitArray temp = leftBlock;
            leftBlock = rightBlock.Xor(functionResult);
            rightBlock = temp;
        }
        #endregion

        private BitArray[] PrepareBlocks(string input)
        {
            BitArray inputBitArray = BitArrayHelper.FromString(input);
            return inputBitArray.SplitArrayIntoEqualParts(64).ToArray();
        }

        private string MergeBlocksToString(BitArray[] blocks)
        {
            BitArray encryptedBitArray = BitArrayHelper.MergeArrays(blocks);
            return encryptedBitArray.GetString();
        }

        // Функция F для преобразования
        private BitArray Function(BitArray block, BitArray key)
        {
            BitArray expandedRightBlock = _expansionFunction.Expand(block);
            BitArray xoredBlock = expandedRightBlock.Xor(key);
            BitArray substitutedBlock = _sBoxPermutator.Permutate(xoredBlock);
            BitArray permutedBlock = _landscapePermutator.Permutate(substitutedBlock);
            return permutedBlock;
        }

        // Метод для генерации ключей для алгоритма DES
        private BitArray[] GenerateKey(BitArray key)
        {
            key = _keyComperssionPermutator.Permutate(key);

            BitArray[] roundKeys = new BitArray[MAX_ROUND];
            
            for (int round = 0; round < MAX_ROUND; round++)
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