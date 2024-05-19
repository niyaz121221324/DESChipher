
public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.Write("Input key values : ");
            string key = Console.ReadLine() ?? string.Empty;
            DES des = new DES(key);

            Console.Write("Input text to encrypt : ");
            string text = Console.ReadLine() ?? string.Empty;

            Console.WriteLine();
            string encryptText = des.Encrypt(text, key);
            Console.WriteLine($"Encrypted text is {encryptText}");

            string decryptText = des.Decrypt(encryptText, key);
            Console.WriteLine($"Decrypted text is {decryptText}");
            Console.WriteLine();
        }
    }
}