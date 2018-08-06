/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using TPFramework.Core;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace TPFramework.Unity
{
    public class TPPersistantPrefs : TPPersistant
    {
        private static readonly CspParameters crpyter = new CspParameters {
            KeyContainerName = "ThisIsAKey"  // This is the key used to encrypt and decrypt can be anything.
        };

        private static readonly RSACryptoServiceProvider provider = new RSACryptoServiceProvider(crpyter);

        private static readonly HashSet<Type> supportedTypes = new HashSet<Type>() {
            typeof(int),
            typeof(float),
            typeof(string),
            typeof(bool)
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void PersistantPrefs()
        {
            Instance = new TPPersistantPrefs();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        protected override HashSet<Type> GetSupportedTypes()
        {
            return supportedTypes;
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        protected override object LoadValue(PersistantAttribute attribute, object objectValue, bool loadDefault)
        {
            string decrypt = Decrypt(PlayerPrefs.GetString(attribute.Key));
            if (string.IsNullOrEmpty(decrypt))
            {
                return loadDefault ? attribute.DefaultValue : null;
            }
            return Convert.ChangeType(decrypt, objectValue.GetType());
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        protected override void SaveValue(PersistantAttribute attribute, object saveValue)
        {
            PlayerPrefs.SetString(attribute.Key, Encrypt(saveValue.ToString()));
        }

        private string Encrypt(string value)
        {
            byte[] encryptBytes = provider.Encrypt(Encoding.UTF8.GetBytes(value), false);
            return Convert.ToBase64String(encryptBytes);
        }

        private string Decrypt(string cryptedValue)
        {
            return string.IsNullOrEmpty(cryptedValue) ? string.Empty : Encoding.UTF8.GetString(provider.Decrypt(Convert.FromBase64String(cryptedValue), false));
        }
    }
}

//    public sealed class TPPersistant
//    {
//        static readonly string PasswordHash = "P@@Sw0rd";
//        static readonly string SaltKey = "S@LT&KEY";
//        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
//        private static readonly string saltKey = "osiembyt";

//private static string Decrypt(string encryptedString)
//{
//    string result;
//    DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider {
//        Mode = CipherMode.ECB,
//        Padding = PaddingMode.PKCS7,
//        Key = Encoding.ASCII.GetBytes(saltKey)
//    };
//    using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(encryptedString)))
//    {
//        using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateDecryptor(), CryptoStreamMode.Read))
//        {
//            using (StreamReader sr = new StreamReader(cs, Encoding.ASCII))
//            {
//                result = sr.ReadToEnd();
//            }
//        }
//    }
//    return null;
//}

//private static string Encrypt(string plainText)
//{
//    string result;
//    DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider {
//        Mode = CipherMode.ECB,
//        Padding = PaddingMode.PKCS7,
//        Key = Encoding.ASCII.GetBytes(saltKey)
//    };
//    using (MemoryStream stream = new MemoryStream())
//    {
//        using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateEncryptor(), CryptoStreamMode.Write))
//        {
//            byte[] data = Encoding.Default.GetBytes(plainText);
//            cs.Write(data, 0, data.Length);
//            cs.FlushFinalBlock(); // <-- Add this
//            result = Convert.ToBase64String(stream.ToArray());
//        }
//    }
//    return result;
//}


//public static string Encrypt(string plainText)
//{
//    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
//    byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
//    var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
//    var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
//    byte[] cipherTextBytes;

//    using (var memoryStream = new MemoryStream())
//    {
//        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
//        {
//            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
//            cryptoStream.FlushFinalBlock();
//            cipherTextBytes = memoryStream.ToArray();
//            cryptoStream.Close();
//        }
//        memoryStream.Close();
//    }
//    return Convert.ToBase64String(cipherTextBytes);
//}
//public static string Decrypt(string encryptedText)
//{
//    byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
//    byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
//    var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
//    var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
//    var memoryStream = new MemoryStream(cipherTextBytes);
//    var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
//    byte[] plainTextBytes = new byte[cipherTextBytes.Length];

//    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
//    memoryStream.Close();
//    cryptoStream.Close();
//    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
//}



//public static string Encrypt(string toEncrypt, bool useHashing = true)
//{
//    byte[] keyArray;
//    byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

//    if (useHashing)
//    {
//        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
//        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(saltKey));
//        hashmd5.Clear();
//    }
//    else
//        keyArray = UTF8Encoding.UTF8.GetBytes(saltKey);

//    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider {
//        Key = keyArray,
//        Mode = CipherMode.ECB,
//        Padding = PaddingMode.PKCS7
//    };

//    ICryptoTransform cTransform = tdes.CreateEncryptor();
//    byte[] resultArray =
//      cTransform.TransformFinalBlock(toEncryptArray, 0,
//      toEncryptArray.Length);
//    tdes.Clear();
//    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
//}

//public static string Decrypt(string cipherString, bool useHashing = true)
//{
//    byte[] keyArray;
//    byte[] toEncryptArray = Convert.FromBase64String(cipherString);

//    if (useHashing)
//    {
//        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
//        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(saltKey));

//        hashmd5.Clear();
//    }
//    else
//    {
//        keyArray = UTF8Encoding.UTF8.GetBytes(saltKey);
//    }

//    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider {
//        Key = keyArray,
//        Mode = CipherMode.ECB,
//        Padding = PaddingMode.PKCS7
//    };

//    ICryptoTransform cTransform = tdes.CreateDecryptor();
//    byte[] resultArray = cTransform.TransformFinalBlock(
//                         toEncryptArray, 0, toEncryptArray.Length);
//    tdes.Clear();
//    return UTF8Encoding.UTF8.GetString(resultArray);
//}

//public void Test()
//{
//    var str = "String to be encrypted";
//    var password = "p@SSword";
//    var strEncryptred = Cipher.Encrypt(str, password);
//    var strDecrypted = Cipher.Decrypt(strEncryptred, password);
//}
//    }
//}

//public static class Cipher
//{
//    /// <summary>
//    /// Encrypt a string.
//    /// </summary>
//    /// <param name="plainText">String to be encrypted</param>
//    /// <param name="password">Password</param>
//    public static string Encrypt(string plainText, string password)
//    {
//        if (plainText == null)
//        {
//            return null;
//        }

//        if (password == null)
//        {
//            password = String.Empty;
//        }

//        // Get the bytes of the string
//        var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
//        var passwordBytes = Encoding.UTF8.GetBytes(password);

//        // Hash the password with SHA256
//        passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

//        var bytesEncrypted = Cipher.Encrypt(bytesToBeEncrypted, passwordBytes);

//        return Convert.ToBase64String(bytesEncrypted);
//    }

//    /// <summary>
//    /// Decrypt a string.
//    /// </summary>
//    /// <param name="encryptedText">String to be decrypted</param>
//    /// <param name="password">Password used during encryption</param>
//    /// <exception cref="FormatException"></exception>
//    public static string Decrypt(string encryptedText, string password)
//    {
//        if (encryptedText == null)
//        {
//            return null;
//        }

//        if (password == null)
//        {
//            password = String.Empty;
//        }

//        // Get the bytes of the string
//        var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
//        var passwordBytes = Encoding.UTF8.GetBytes(password);

//        passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

//        var bytesDecrypted = Cipher.Decrypt(bytesToBeDecrypted, passwordBytes);

//        return Encoding.UTF8.GetString(bytesDecrypted);
//    }

//    private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
//    {
//        byte[] encryptedBytes = null;

//        // Set your salt here, change it to meet your flavor:
//        // The salt bytes must be at least 8 bytes.
//        var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

//        using (MemoryStream ms = new MemoryStream())
//        {
//            using (RijndaelManaged AES = new RijndaelManaged())
//            {
//                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

//                AES.KeySize = 256;
//                AES.BlockSize = 128;
//                AES.Key = key.GetBytes(AES.KeySize / 8);
//                AES.IV = key.GetBytes(AES.BlockSize / 8);

//                AES.Mode = CipherMode.CBC;

//                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
//                {
//                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
//                    cs.Close();
//                }

//                encryptedBytes = ms.ToArray();
//            }
//        }

//        return encryptedBytes;
//    }

//    private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
//    {
//        byte[] decryptedBytes = null;

//        // Set your salt here, change it to meet your flavor:
//        // The salt bytes must be at least 8 bytes.
//        var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

//        using (MemoryStream ms = new MemoryStream())
//        {
//            using (RijndaelManaged AES = new RijndaelManaged())
//            {
//                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

//                AES.KeySize = 256;
//                AES.BlockSize = 128;
//                AES.Key = key.GetBytes(AES.KeySize / 8);
//                AES.IV = key.GetBytes(AES.BlockSize / 8);
//                AES.Mode = CipherMode.CBC;

//                using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
//                {
//                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
//                    cs.Close();
//                }

//                decryptedBytes = ms.ToArray();
//            }
//        }

//        return decryptedBytes;
//    }
//}
