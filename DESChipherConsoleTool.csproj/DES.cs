
namespace DESChipherConsoleTool
{
    class DES
    {
        private string _key;
        private BitArray[] subkeys = new BitArray[16];

        public DES(string key)
        {
            _key = key;
            InitKeys(_key);
        }

        private void InitKeys(string key)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            BitArray keyBits = new BitArray(keyBytes);

            KeysGenerator keyGenerator = new KeysGenerator();
            subkeys = keyGenerator.GenerateKeys(keyBits, subkeys);
        }

        private BitArray FeistelFunction(BitArray R, BitArray K)
        {
            BitArray ER = ExpantionFunction.ExpansionPermutation(R);
            ER.Xor(K);
            BitArray SR = SBoxPermutator.Substitution(ER);
            return PBoxPermutator.Permutation(SR);
        }

        private void EncryptBlock(BitArray block, BitArray[] subkeys)
        {
            block = InitialPermutator.InitialPermutation(block);

            BitArray L = new BitArray(32);
            BitArray R = new BitArray(32);
            for (int i = 0; i < 32; i++)
            {
                L[i] = block[i];
                R[i] = block[i + 32];
            }

            for (int round = 0; round < 16; round++)
            {
                BitArray tempR = (BitArray)R.Clone();
                R = L.Xor(FeistelFunction(R, subkeys[round]));
                L = tempR;
                Console.WriteLine($"Encrypt Round {round + 1}");

                //Выводим значение каждого раунда
                BitArray roundRes = BitArrayHelper.MergeArrays(R, L);
                BitMatrix matrix = BitMatrix.FromBitArray(roundRes, 8, 8);
                matrix.Print();
                Console.WriteLine();
            }

            block = BitArrayHelper.MergeArrays(R, L);

            block = FinalPermutator.FinalPermutation(block);
        }

        public string Encrypt(string plaintext, string key)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            BitArray keyBits = new BitArray(keyBytes);

            byte[] plaintextBytes = Encoding.ASCII.GetBytes(plaintext);
            int length = plaintextBytes.Length;
            int paddedLength = ((length + 7) / 8) * 8;
            byte[] paddedBytes = new byte[paddedLength];
            Array.Copy(plaintextBytes, paddedBytes, length);

            BitArray ciphertextBits = new BitArray(paddedLength * 8);
            for (int i = 0; i < paddedLength; i += 8)
            {
                BitArray block = new BitArray(new byte[] {
                    paddedBytes[i],
                    paddedBytes[i + 1],
                    paddedBytes[i + 2],
                    paddedBytes[i + 3],
                    paddedBytes[i + 4],
                    paddedBytes[i + 5],
                    paddedBytes[i + 6],
                    paddedBytes[i + 7]
                });

                EncryptBlock(block, subkeys);

                for (int j = 0; j < 64; j++)
                {
                    ciphertextBits[i * 8 + j] = block[j];
                }
            }

            byte[] ciphertextBytes = new byte[ciphertextBits.Length / 8];
            ciphertextBits.CopyTo(ciphertextBytes, 0);

            return Convert.ToBase64String(ciphertextBytes);
        }

        private void DecryptBlock(BitArray block, BitArray[] subkeys)
        {
            block = InitialPermutator.InitialPermutation(block);

            BitArray L = new BitArray(32);
            BitArray R = new BitArray(32);
            for (int i = 0; i < 32; i++)
            {
                L[i] = block[i];
                R[i] = block[i + 32];
            }

            for (int round = 15; round >= 0; round--)
            {
                BitArray tempR = (BitArray)R.Clone();
                R = L.Xor(FeistelFunction(R, subkeys[round]));
                L = tempR;
            }

            block = BitArrayHelper.MergeArrays(R, L);

            block = FinalPermutator.FinalPermutation(block);
        }

        public string Decrypt(string ciphertext, string key)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            BitArray keyBits = new BitArray(keyBytes);

            byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);
            int length = ciphertextBytes.Length;

            BitArray plaintextBits = new BitArray(length * 8);
            for (int i = 0; i < length; i += 8)
            {
                BitArray block = new BitArray(new byte[] {
                    ciphertextBytes[i],
                    ciphertextBytes[i + 1],
                    ciphertextBytes[i + 2],
                    ciphertextBytes[i + 3],
                    ciphertextBytes[i + 4],
                    ciphertextBytes[i + 5],
                    ciphertextBytes[i + 6],
                    ciphertextBytes[i + 7]
                });

                DecryptBlock(block, subkeys);

                for (int j = 0; j < 64; j++)
                {
                    plaintextBits[i * 8 + j] = block[j];
                }
            }

            byte[] plaintextBytes = new byte[plaintextBits.Length / 8];
            plaintextBits.CopyTo(plaintextBytes, 0);

            int paddingLength = 0;
            for (int i = plaintextBytes.Length - 1; i >= 0 && plaintextBytes[i] == 0; i--)
            {
                paddingLength++;
            }

            return Encoding.ASCII.GetString(plaintextBytes, 0, plaintextBytes.Length - paddingLength);
        }
    }
}
