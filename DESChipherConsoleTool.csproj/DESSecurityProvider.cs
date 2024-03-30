﻿
namespace DESChipherConsoleTool
{
    public class DESSecurityProvider
    {
        private const int MAX_ROUND = 16;

        private readonly IInitialPermutator _ipTablePermutator;
        private readonly IKeyComperssionPermutator _keyComperssionPermutator;
        private readonly IFinalKeyCompressionPermutator _finalKeyCompressionPermutator;
        private readonly ISBoxPermutator _sBoxPermutator;
        private readonly IExpansionFunction _expansionFunction;
        private readonly IPFinalPermutator _landscapePermutator;
        private readonly IFinalPermutator _finalPermutator;

        public DESSecurityProvider(BitArray key,IInitialPermutator ipTablePermutator = null,
            IFinalKeyCompressionPermutator finalKeyCompressionPermutator = null,
            IKeyComperssionPermutator keyComperssionPermutator = null, 
            IExpansionFunction expansionFunction = null,
            ISBoxPermutator sBoxPermutator = null,
            IPFinalPermutator landscapePermutator = null, 
            IFinalPermutator finalPermutator = null)
        {
            _ipTablePermutator = ipTablePermutator ?? new InitialPermutator();
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
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            BitArray inputBitArray = new BitArray(inputBytes);
            BitArray[] blocks = inputBitArray.SplitArrayIntoEqualParts(64).ToArray();

            for (int i = 0; i < blocks.Length; i++)
            {
                EncryptBlock(ref blocks[i], key);
            }

            BitArray encryptedBitArray = BitArrayHelper.MergeArrays(blocks);
            return encryptedBitArray.GetString();
        }

        private void EncryptBlock(ref BitArray block, BitArray key)
        {
            block = _ipTablePermutator.InitialPermutate(block);

            BitArray[] bitArrays = block.SplitBitsIntoEqualsPortions(2);
            BitArray leftBlock = bitArrays[0];
            BitArray rightBlock = bitArrays[1];

            BitArray[] keyBlocks = GenerateKey(key);

            BitArray previousRighBlock = rightBlock;
            BitArray previousLeftBlock = leftBlock;

            for (int i = 1; i <= MAX_ROUND - 1; i++)
            {
                leftBlock = previousRighBlock;
                rightBlock = previousLeftBlock.Xor(Function(previousRighBlock, keyBlocks[i - 1]));

                previousLeftBlock = leftBlock;
                previousRighBlock = rightBlock;
            }

            var temp = leftBlock;
            leftBlock = leftBlock.Xor(Function(rightBlock, keyBlocks[keyBlocks.Length - 1]));
            rightBlock = temp;

            block = BitArrayHelper.MergeArrays(rightBlock, leftBlock);
            block = _finalPermutator.Permutate(block);
        }

        private BitArray Function(BitArray block, BitArray key)
        {
            BitArray expandedRightBlock = _expansionFunction.Expand(block);
            BitArray xoredBlock = expandedRightBlock.Xor(key);
            BitArray substitutedBlock = _sBoxPermutator.Permutate(xoredBlock);
            BitArray permutedBlock = _landscapePermutator.Permutate(substitutedBlock);

            return permutedBlock; 
        }

        /// <summary>
        /// метод для дешифрования алгоритмом DES
        /// </summary>
        /// <param name="input">зашифрованный текст на вход</param>
        /// <param name="key"></param>
        /// <returns>Дешифрованный текст</returns>
        public string Decrypt(string input, BitArray key)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            BitArray inputBitArray = new BitArray(inputBytes);
            BitArray[] blocks = inputBitArray.SplitArrayIntoEqualParts(64).ToArray();

            for (int i = 0; i < blocks.Length; i++)
            {
                DecryptBlock(ref blocks[i], key);
            }

            BitArray decryptedBitArray = BitArrayHelper.MergeArrays(blocks);
            return decryptedBitArray.GetString();
        }

        private void DecryptBlock(ref BitArray block, BitArray key)
        {
            block = _finalPermutator.Permutate(block);

            BitArray[] bitArrays = block.SplitBitsIntoEqualsPortions(2);
            BitArray leftBlock = bitArrays[0];
            BitArray rightBlock = bitArrays[1];

            BitArray[] keyBlocks = GenerateKey(key);

            for (int round = MAX_ROUND; round >= 0; round--)
            {
                BitArray previousLeftBlock = leftBlock.Clone() as BitArray;
                leftBlock = rightBlock.Clone() as BitArray;
                rightBlock = previousLeftBlock.Xor(Function(rightBlock, keyBlocks[round - 1]));
            }

            block = BitArrayHelper.MergeArrays(leftBlock, rightBlock); 
            block = _ipTablePermutator.InitialPermutate(block);
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