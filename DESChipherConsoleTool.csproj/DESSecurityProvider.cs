
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
        private readonly IPFinalPermutator _pBoxPermutator;
        private readonly IFinalPermutator _finalPermutator;

        public DESSecurityProvider(IInitialPermutator initialPermutatior = null,
            IFinalKeyCompressionPermutator finalKeyCompressionPermutator = null,
            IKeyComperssionPermutator keyComperssionPermutator = null, 
            IExpansionFunction expansionFunction = null,
            ISBoxPermutator sBoxPermutator = null,
            IPFinalPermutator pBoxPermutator = null, 
            IFinalPermutator finalPermutator = null)
        {
            _initialPermutator = initialPermutatior ?? new InitialPermutator();
            _keyComperssionPermutator = keyComperssionPermutator ?? new KeyCompressionPermutator();
            _finalKeyCompressionPermutator = finalKeyCompressionPermutator ?? new FinalKeyCompressionPermutator();
            _expansionFunction = expansionFunction ?? new ExpansionFunction();
            _sBoxPermutator = sBoxPermutator ?? new SBoxPermutator();
            _pBoxPermutator = pBoxPermutator ?? new PBoxPermutator();
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
                EncryptBlock(ref blocks[i], keyBlocks);
            }

            BitArray encryptedBitArray = BitArrayHelper.MergeArrays(blocks);
            return encryptedBitArray.GetString();
        }

        // Метод для шифрования алгоритмом DES
        private void EncryptBlock(ref BitArray block, BitArray[] keyBlocks)
        {
            block = _initialPermutator.InitialPermutate(block);

            BitArray leftBlock = block.LeftHalf();
            BitArray rightBlock = block.RightHalf();

            for (int j = 0; j < MAX_ROUND; j++)
            {
                BitArray functionResult = Function(rightBlock, keyBlocks[j]);
                BitArray temp = rightBlock;
                rightBlock = leftBlock.Xor(functionResult);
                leftBlock = temp;
            }

            block.AssignHalves(rightBlock, leftBlock); // меняем блоки местами в последнем раунде и объеденяем
            block = _finalPermutator.Permutate(block);
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
            BitArray[] keyBlocks = GenerateKey(key, false);

            for (int i = 0; i < blocks.Length; i++)
            {
                EncryptBlock(ref blocks[i], keyBlocks);
            }

            BitArray encryptedBitArray = BitArrayHelper.MergeArrays(blocks);
            return encryptedBitArray.GetString();
        }

        // Метод для дешифрования блока текста алгоритмом DES
        private void DecryptBlock(ref BitArray block, BitArray[] keyBlocks)
        {
            block = _finalPermutator.Permutate(block);
            
            // Меняем блоки местами в первом раунде для обратной функции сети фейстеля
            BitArray leftBlock = block.RightHalf();
            BitArray rightBlock = block.LeftHalf();

            for (int j = MAX_ROUND - 1; j >= 0; j--)
            {
                BitArray functionResult = Function(leftBlock, keyBlocks[j]);
                BitArray temp = leftBlock;
                leftBlock = rightBlock.Xor(functionResult);
                rightBlock = temp;
            }

            block.AssignHalves(leftBlock, rightBlock);
            block = _initialPermutator.InitialPermutate(block);
        }
        #endregion

        private BitArray[] PrepareBlocks(string input)
        {
            BitArray inputBitArray = BitArrayHelper.FromString(input);
            return inputBitArray.SplitArrayIntoEqualParts(64).ToArray();
        }

        // Выполняет функцию F для преобразования блока данных.
        // Входные параметры:
        // - block: блок данных, который подвергается преобразованию.
        // - key: ключ, используемый для преобразования блока.
        // Результат работы функции F:
        // - permutedBlock: преобразованный блок данных после выполнения всех этапов.
        private BitArray Function(BitArray block, BitArray key)
        {
            // Шаг 1: Расширение блока
            BitArray expandedRightBlock = _expansionFunction.Expand(block);

            // Шаг 2: Применение операции XOR к расширенному блоку и ключу
            BitArray xoredBlock = expandedRightBlock.Xor(key);

            // Шаг 3: Применение S-боксов для замены значений в блоке
            BitArray substitutedBlock = _sBoxPermutator.Permutate(xoredBlock);

            // Шаг 4: Перестановка значений в блоке с помощью P-бокса
            BitArray permutedBlock = _pBoxPermutator.Permutate(substitutedBlock);

            // Возвращаем преобразованный блок данных
            return permutedBlock;
        }

        // Метод для генерации ключей для алгоритма DES
        private BitArray[] GenerateKey(BitArray key, bool isEncryption = true)
        {
            key = _keyComperssionPermutator.Permutate(key);

            BitArray[] roundKeys = new BitArray[MAX_ROUND];

            for (int round = GetStartingRound(isEncryption); IsRoundConditionMet(round, isEncryption); round = UpdateRound(round, isEncryption))
            {
                BitArray leftKeyHalf = key.LeftHalf();
                BitArray rightKeyHalf = key.RightHalf();

                int shiftValue = GetShiftValue(round + 1);

                leftKeyHalf = leftKeyHalf.ShiftArrayLeft(shiftValue);
                rightKeyHalf = rightKeyHalf.ShiftArrayLeft(shiftValue);

                key.AssignHalves(leftKeyHalf, rightKeyHalf);

                roundKeys[round] = _finalKeyCompressionPermutator.Permutate(key);
            }

            return roundKeys;
        }

        private int GetStartingRound(bool isEncryption)
        {
            // Определяет начальный раунд в зависимости от режима работы (шифрование или расшифрование).
            // Для шифрования возвращает 0 (первый раунд), для расшифрования возвращает максимальное значение раунда (MAX_ROUND).
            return isEncryption ? 0 : MAX_ROUND;
        }

        private bool IsRoundConditionMet(int round, bool isEncryption)
        {
            // Проверяет, выполнено ли условие для продолжения цикла генерации ключа.
            // Для шифрования проверяет, если текущий раунд меньше максимального значения (MAX_ROUND).
            // Для расшифрования проверяет, если текущий раунд больше или равен 1.
            return isEncryption ? round < MAX_ROUND - 1 : round >= 0;
        }

        private int UpdateRound(int round, bool isEncryption)
        {
            // Обновляет счетчик раунда для следующей итерации цикла генерации ключа.
            // Для шифрования увеличивает счетчик раунда на 1, для расшифрования уменьшает счетчик раунда на 1.
            return isEncryption ? round + 1 : round - 1;
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