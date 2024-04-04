
class Program
{
    static void Main()
    {
        Console.Write("Введите текст для шифрования : ");
        string input = Console.ReadLine();

        Console.WriteLine("Входная строка в битовом представлении : ");
        PrintBitArrayInfo(input);

        BitMatrix[] blocks = BitMatrix.GetBlocksFromString(input);

        Console.WriteLine();
        for (int i = 0; i < blocks.Length; i++)
        {
            Console.WriteLine($"блок {i + 1}");
            PrintBitMatrix(blocks[i]);
        }

        BitArray key = BitArrayHelper.GenerateRandomBitArray();
        DESSecurityProvider dESSecurityProvider = new DESSecurityProvider();

        string keyWord = key.GetString();
        Console.WriteLine();
        Console.WriteLine($"Ключ в битовом представлении : {keyWord}");

        string encryptedText = dESSecurityProvider.Encrypt(input, key);
        Console.WriteLine();
        Console.WriteLine($"Зашифрованный текст : {encryptedText}");

        string decryptedText = dESSecurityProvider.Decrypt(encryptedText, key);
        Console.WriteLine();
        Console.WriteLine($"Расшифрованный текст : {decryptedText}");
    }

    static void PrintBitArrayInfo(string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            Console.Write($"{input[i]} => ");
            byte[] bytesTest = Encoding.ASCII.GetBytes(input[i].ToString());
            BitArray array = BitArrayHelper.ConvertCorrectByteToBitArray(bytesTest);
            PrintBitArray(array);
            Console.WriteLine();
        }
    }

    static void PrintBitMatrix(BitMatrix matrix)
    {
        for (int i = 0; i < matrix.Rows; i++)
        {
            for (int j = 0; j < matrix.Columns; j++)
            {
                Console.Write((matrix[i, j] ? "1" : "0") + " ");
            }
            Console.WriteLine();
        }
    }

    static void PrintBitArray(BitArray bitArray)
    {
        for (int i = 0; i < bitArray.Count; i++)
        {
            Console.Write((bitArray[i] ? "1" : "0") + " ");
        }
    }
}