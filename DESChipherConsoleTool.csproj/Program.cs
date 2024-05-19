
public class Program
{
    public static void Main()
    {
        DES des = new DES();
        string key = "0123456789ABCDEF";

        string text = Console.ReadLine() ?? string.Empty;

        Console.WriteLine();
        string encryptText = des.Encrypt(text, key);
        Console.WriteLine($"Encrypted text is {encryptText}");

        string decryptText = des.Decrypt(encryptText, key);
        Console.WriteLine($"Decrypted text is {decryptText}");
    }
}