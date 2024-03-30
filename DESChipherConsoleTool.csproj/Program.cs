
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
            Console.WriteLine($"block {i + 1}");
            PrintBitMatrix(blocks[i]);
        }

        Console.WriteLine();
        Console.WriteLine("Введите ключевое слово оно должно быть равно 8 символов : ");
        string keyWord = Console.ReadLine();

        if (keyWord.Length != 8)
            throw new ArgumentException("Ключ должен быть размером 64 бита");

        BitArray key = BitArrayHelper.FromString(keyWord);
        DESSecurityProvider dESSecurityProvider = new DESSecurityProvider(key);

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