
namespace DESChipherConsoleTool
{
    class DES
    {
        private static readonly int[] IP = {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        private static readonly int[] FP = {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25
        };

        private static readonly int[] E = {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        private static readonly int[,] SBox = {
            // S1
            {
                14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7,
                0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8,
                4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0,
                15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13
            },
            // S2
            {
                15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10,
                3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5,
                0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15,
                13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9
            },
            // S3
            {
                10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8,
                13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1,
                13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7,
                1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12
            },
            // S4
            {
                7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15,
                13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9,
                10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4,
                3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14
            },
            // S5
            {
                2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9,
                14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6,
                4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14,
                11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3
            },
            // S6
            {
                12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11,
                10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8,
                9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6,
                4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13
            },
            // S7
            {
                4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1,
                13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6,
                1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2,
                6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12
            },
            // S8
            {
                13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7,
                1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2,
                7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8,
                2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11
            }
        };

        private static readonly int[] P = {
            16, 7, 20, 21,
            29, 12, 28, 17,
            1, 15, 23, 26,
            5, 18, 31, 10,
            2, 8, 24, 14,
            32, 27, 3, 9,
            19, 13, 30, 6,
            22, 11, 4, 25
        };

        private BitArray InitialPermutation(BitArray input)
        {
            BitArray output = new BitArray(64);
            for (int i = 0; i < 64; i++)
            {
                output[i] = input[IP[i] - 1];
            }
            return output;
        }

        private BitArray FinalPermutation(BitArray input)
        {
            BitArray output = new BitArray(64);
            for (int i = 0; i < 64; i++)
            {
                output[i] = input[FP[i] - 1];
            }
            return output;
        }

        private BitArray ExpansionPermutation(BitArray input)
        {
            BitArray output = new BitArray(48);
            for (int i = 0; i < 48; i++)
            {
                output[i] = input[E[i] - 1];
            }
            return output;
        }

        private BitArray Substitution(BitArray input)
        {
            BitArray output = new BitArray(32);
            for (int i = 0; i < 8; i++)
            {
                int row = (input[i * 6] ? 2 : 0) | (input[i * 6 + 5] ? 1 : 0);
                int col = (input[i * 6 + 1] ? 8 : 0) | (input[i * 6 + 2] ? 4 : 0) | (input[i * 6 + 3] ? 2 : 0) | (input[i * 6 + 4] ? 1 : 0);
                int val = SBox[i, row * 16 + col];
                for (int j = 0; j < 4; j++)
                {
                    output[i * 4 + j] = (val & (1 << (3 - j))) != 0;
                }
            }
            return output;
        }

        private BitArray Permutation(BitArray input)
        {
            BitArray output = new BitArray(32);
            for (int i = 0; i < 32; i++)
            {
                output[i] = input[P[i] - 1];
            }
            return output;
        }

        private BitArray FeistelFunction(BitArray R, BitArray K)
        {
            BitArray ER = ExpansionPermutation(R);
            ER.Xor(K);
            BitArray SR = Substitution(ER);
            return Permutation(SR);
        }

        private static readonly int[] PC1 = {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        private static readonly int[] PC2 = {
            14, 17, 11, 24, 1, 5, 3, 28,
            15, 6, 21, 10, 23, 19, 12, 4,
            26, 8, 16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56,
            34, 53, 46, 42, 50, 36, 29, 32
        };

        private static readonly int[] Shifts = {
            1, 1, 2, 2, 2, 2, 2, 2,
            1, 2, 2, 2, 2, 2, 2, 1
        };

        private BitArray Permute(BitArray input, int[] table)
        {
            BitArray output = new BitArray(table.Length);
            for (int i = 0; i < table.Length; i++)
            {
                output[i] = input[table[i] - 1];
            }
            return output;
        }

        private BitArray LeftShift(BitArray input, int count)
        {
            BitArray output = new BitArray(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[(i + count) % input.Length];
            }
            return output;
        }

        public void GenerateKeys(BitArray key, BitArray[] subkeys)
        {
            BitArray permutedKey = Permute(key, PC1);
            BitArray C = new BitArray(28);
            BitArray D = new BitArray(28);

            for (int i = 0; i < 28; i++)
            {
                C[i] = permutedKey[i];
                D[i] = permutedKey[i + 28];
            }

            for (int i = 0; i < 16; i++)
            {
                C = LeftShift(C, Shifts[i]);
                D = LeftShift(D, Shifts[i]);

                BitArray combined = new BitArray(56);
                for (int j = 0; j < 28; j++)
                {
                    combined[j] = C[j];
                    combined[j + 28] = D[j];
                }

                subkeys[i] = Permute(combined, PC2);
            }
        }

        private void EncryptBlock(BitArray block, BitArray[] subkeys)
        {
            block = InitialPermutation(block);

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
                BitArray roundRes = BitArrayHelper.MergeArrays(R, L);
                BitMatrix matrix = BitMatrix.FromBitArray(roundRes, 8, 8);
                matrix.Print();
                Console.WriteLine();
            }

            for (int i = 0; i < 32; i++)
            {
                block[i] = R[i];
                block[i + 32] = L[i];
            }

            block = FinalPermutation(block);
        }

        public string Encrypt(string plaintext, string key)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            BitArray keyBits = new BitArray(keyBytes);

            BitArray[] subkeys = new BitArray[16];
            GenerateKeys(keyBits, subkeys);

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
            block = InitialPermutation(block);

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

            for (int i = 0; i < 32; i++)
            {
                block[i] = R[i];
                block[i + 32] = L[i];
            }

            block = FinalPermutation(block);
        }

        public string Decrypt(string ciphertext, string key)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            BitArray keyBits = new BitArray(keyBytes);

            BitArray[] subkeys = new BitArray[16];
            GenerateKeys(keyBits, subkeys);

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
