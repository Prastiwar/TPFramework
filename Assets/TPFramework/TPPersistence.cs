///**
//*   Authored by Tomasz Piowczyk
//*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
//*   Repository: https://github.com/Prastiwar/TPFramework 
//*/
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using System.Security.Cryptography;
//using System.Text;
//using UnityEngine;

//namespace TPFramework
//{
//    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
//    public sealed class PersistantPrefsAttribute : PropertyAttribute
//    {
//        internal readonly string key;
//        internal readonly object defaultVal;

//        private PersistantPrefsAttribute(string persistantKey, object defaultValue)
//        {
//            key = persistantKey;
//            defaultVal = defaultValue;
//        }

//        public PersistantPrefsAttribute(string persistantKey, int defaultValue) : this(persistantKey, defaultValue as object) { }
//        public PersistantPrefsAttribute(string persistantKey, string defaultValue) : this(persistantKey, defaultValue as object) { }
//        public PersistantPrefsAttribute(string persistantKey, float defaultValue) : this(persistantKey, defaultValue as object) { }
//        public PersistantPrefsAttribute(string persistantKey, bool defaultValue) : this(persistantKey, defaultValue as object) { }
//        public PersistantPrefsAttribute(string persistantKey) : this(persistantKey, null) { }
//    }

//    public sealed class TPPersistant
//    {
//        //[Serializable]
//        //private struct DataHolder
//        //{
//        //    internal readonly bool isInitialized;
//        //    public List<string> Keys;
//        //    public List<string> Values;

//        //    public DataHolder(int capacity = 10)
//        //    {
//        //        Keys = new List<string>(capacity);
//        //        Values = new List<string>(capacity);
//        //        isInitialized = true;
//        //    }

//        //    public void Add(string key, string value)
//        //    {
//        //        Keys.Add(key);
//        //        Values.Add(value);
//        //    }

//        //    public string Get(string key)
//        //    {
//        //        int length = Keys.Count;
//        //        for (int i = 0; i < length; i++)
//        //        {
//        //            if (Keys[i] == key)
//        //                return Values[i];
//        //        }
//        //        return null;
//        //    }
//        //}

//        static readonly string PasswordHash = "P@@Sw0rd";
//        static readonly string SaltKey = "S@LT&KEY";
//        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
//        private static readonly string saltKey = "osiembyt";

//        private static readonly Type typeNeeded = typeof(PersistantPrefsAttribute);

//        private static Type[] supportTypes = new Type[] {
//            typeof(int),
//            typeof(float),
//            typeof(string),
//            typeof(bool)
//        };

//        private static readonly Dictionary<FieldInfo, PersistantPrefsAttribute> reusableCollecton = new Dictionary<FieldInfo, PersistantPrefsAttribute>(32);
//        private static Dictionary<FieldInfo, PersistantPrefsAttribute> ReusableDictionary {
//            get {
//                reusableCollecton.Clear();
//                return reusableCollecton;
//            }
//        }

//        public static void SaveObjects(params object[] sources)
//        {
//            int length = sources.Length;
//            for (int i = 0; i < length; i++)
//            {
//                SaveObject(sources[i]);
//            }
//        }

//        public static void LoadObjects(params object[] sources)
//        {
//            int length = sources.Length;
//            for (int i = 0; i < length; i++)
//            {
//                LoadObject(sources[i]);
//            }
//        }

//        public static void SaveObject(object source)
//        {
//            var injectedFields = GetFieldsWithAttribute(source);

//            foreach (var fieldAtt in injectedFields)
//            {
//                PersistantPrefsAttribute att = fieldAtt.Value;
//                object fieldValue = fieldAtt.Key.GetValue(source);

//                if (!IsValidSafeCheck(att.defaultVal, fieldValue))
//                    return;

//                string cryptKey = Encrypt(att.key);
//                PlayerPrefs.SetString(Encrypt(att.key), Encrypt(fieldValue.ToString()));

//                if (fieldValue is int)
//                {
//                    PlayerPrefs.SetInt(cryptKey, (int)fieldValue);
//                }
//                else if (fieldValue is float)
//                {
//                    PlayerPrefs.SetFloat(cryptKey, (float)fieldValue);
//                }
//                else if (fieldValue is string)
//                {
//                    PlayerPrefs.SetString(cryptKey, (string)fieldValue);
//                }
//                else if (fieldValue is bool)
//                {
//                    PlayerPrefs.SetInt(cryptKey, (bool)fieldValue ? 1 : 0);
//                }
//            }
//        }

//        public static void LoadObject(object source)
//        {
//            var injectedFields = GetFieldsWithAttribute(source);
//            int i = 0;
//            foreach (var pair in injectedFields)
//            {
//                PersistantPrefsAttribute att = pair.Value;
//                object fieldValue = pair.Key.GetValue(source);

//                if (!IsValidSafeCheck(att.defaultVal, fieldValue))
//                    return;

//                bool hasDefault = att.defaultVal != null;
//                object newValue = null;
//                string cryptKey = Decrypt(att.key);
//                newValue = Convert.ChangeType(PlayerPrefs.GetString(Decrypt(att.key)), fieldValue.GetType().TypeInitializer.DeclaringType);

//                if (fieldValue is int)
//                {
//                    newValue = PlayerPrefs.GetInt(cryptKey, hasDefault ? (int)att.defaultVal : 0);
//                }
//                else if (fieldValue is float)
//                {
//                    newValue = PlayerPrefs.GetFloat(cryptKey, hasDefault ? (float)att.defaultVal : 0);
//                }
//                else if (fieldValue is string)
//                {
//                    newValue = PlayerPrefs.GetString(cryptKey, hasDefault ? (string)att.defaultVal : string.Empty);
//                }
//                else if (fieldValue is bool)
//                {
//                    newValue = PlayerPrefs.GetInt(cryptKey, hasDefault ? (int)att.defaultVal : 0) == 1 ? true : false;
//                }

//                pair.Key.SetValue(source, newValue);
//            }
//        }

//        private static Dictionary<FieldInfo, PersistantPrefsAttribute> GetFieldsWithAttribute(object source)
//        {
//            FieldInfo[] fields = source.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
//            int length = fields.Length;
//            var dictionary = ReusableDictionary;

//            for (int i = 0; i < length; i++)
//            {
//                if (IsFieldValid(fields[i]))
//                {
//                    dictionary[fields[i]] = (PersistantPrefsAttribute)fields[i].GetCustomAttributes(typeNeeded, false)[0];
//                }
//            }
//            return dictionary;
//        }

//        private static bool IsFieldValid(FieldInfo field)
//        {
//            if (!IsTypeSupported(field.FieldType) || !field.IsDefined(typeNeeded, false))
//                return false;
//            return true;
//        }

//        private static bool IsTypeSupported(Type objType)
//        {
//            int supportTypesLength = supportTypes.Length;
//            for (int i = 0; i < supportTypesLength; i++)
//            {
//                if (objType == supportTypes[i])
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        private static bool IsValidSafeCheck(object defaultValue, object fieldValue)
//        {
//            Type fieldType = fieldValue.GetType();
//            if (IsTypeSupportedSafeCheck(fieldType))
//            {
//                if (defaultValue != null)
//                {
//                    return IsDefaultValueValidSafeCheck(defaultValue, fieldType);
//                }
//                return true;
//            }
//            return false;
//        }

//        private static bool IsTypeSupportedSafeCheck(Type objType)
//        {
//            if (IsTypeSupported(objType))
//                return true;
//            Debug.LogError("Only int/float/bool/string are supported!");
//            return false;
//        }

//        private static bool IsDefaultValueValidSafeCheck(object defaultValue, Type shouldType)
//        {
//            if (defaultValue.GetType() == shouldType)
//                return true;
//            Debug.LogError("You're trying to set default value to field with different type");
//            return false;
//        }

//        //private static string Decrypt(string encryptedString)
//        //{
//        //    string result;
//        //    DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider {
//        //        Mode = CipherMode.ECB,
//        //        Padding = PaddingMode.PKCS7,
//        //        Key = Encoding.ASCII.GetBytes(saltKey)
//        //    };
//        //    using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(encryptedString)))
//        //    {
//        //        using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateDecryptor(), CryptoStreamMode.Read))
//        //        {
//        //            using (StreamReader sr = new StreamReader(cs, Encoding.ASCII))
//        //            {
//        //                result = sr.ReadToEnd();
//        //            }
//        //        }
//        //    }
//        //    return null;
//        //}

//        //private static string Encrypt(string plainText)
//        //{
//        //    string result;
//        //    DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider {
//        //        Mode = CipherMode.ECB,
//        //        Padding = PaddingMode.PKCS7,
//        //        Key = Encoding.ASCII.GetBytes(saltKey)
//        //    };
//        //    using (MemoryStream stream = new MemoryStream())
//        //    {
//        //        using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateEncryptor(), CryptoStreamMode.Write))
//        //        {
//        //            byte[] data = Encoding.Default.GetBytes(plainText);
//        //            cs.Write(data, 0, data.Length);
//        //            cs.FlushFinalBlock(); // <-- Add this
//        //            result = Convert.ToBase64String(stream.ToArray());
//        //        }
//        //    }
//        //    return result;
//        //}


//        //public static string Encrypt(string plainText)
//        //{
//        //    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
//        //    byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
//        //    var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
//        //    var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
//        //    byte[] cipherTextBytes;

//        //    using (var memoryStream = new MemoryStream())
//        //    {
//        //        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
//        //        {
//        //            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
//        //            cryptoStream.FlushFinalBlock();
//        //            cipherTextBytes = memoryStream.ToArray();
//        //            cryptoStream.Close();
//        //        }
//        //        memoryStream.Close();
//        //    }
//        //    return Convert.ToBase64String(cipherTextBytes);
//        //}
//        //public static string Decrypt(string encryptedText)
//        //{
//        //    byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
//        //    byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
//        //    var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
//        //    var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
//        //    var memoryStream = new MemoryStream(cipherTextBytes);
//        //    var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
//        //    byte[] plainTextBytes = new byte[cipherTextBytes.Length];

//        //    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
//        //    memoryStream.Close();
//        //    cryptoStream.Close();
//        //    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
//        //}



//        //public static string Encrypt(string toEncrypt, bool useHashing = true)
//        //{
//        //    byte[] keyArray;
//        //    byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

//        //    if (useHashing)
//        //    {
//        //        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
//        //        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(saltKey));
//        //        hashmd5.Clear();
//        //    }
//        //    else
//        //        keyArray = UTF8Encoding.UTF8.GetBytes(saltKey);

//        //    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider {
//        //        Key = keyArray,
//        //        Mode = CipherMode.ECB,
//        //        Padding = PaddingMode.PKCS7
//        //    };

//        //    ICryptoTransform cTransform = tdes.CreateEncryptor();
//        //    byte[] resultArray =
//        //      cTransform.TransformFinalBlock(toEncryptArray, 0,
//        //      toEncryptArray.Length);
//        //    tdes.Clear();
//        //    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
//        //}

//        //public static string Decrypt(string cipherString, bool useHashing = true)
//        //{
//        //    byte[] keyArray;
//        //    byte[] toEncryptArray = Convert.FromBase64String(cipherString);

//        //    if (useHashing)
//        //    {
//        //        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
//        //        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(saltKey));

//        //        hashmd5.Clear();
//        //    }
//        //    else
//        //    {
//        //        keyArray = UTF8Encoding.UTF8.GetBytes(saltKey);
//        //    }

//        //    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider {
//        //        Key = keyArray,
//        //        Mode = CipherMode.ECB,
//        //        Padding = PaddingMode.PKCS7
//        //    };

//        //    ICryptoTransform cTransform = tdes.CreateDecryptor();
//        //    byte[] resultArray = cTransform.TransformFinalBlock(
//        //                         toEncryptArray, 0, toEncryptArray.Length);
//        //    tdes.Clear();
//        //    return UTF8Encoding.UTF8.GetString(resultArray);
//        //}

//        public void Test()
//        {
//            var str = "String to be encrypted";
//            var password = "p@SSword";
//            var strEncryptred = Cipher.Encrypt(str, password);
//            var strDecrypted = Cipher.Decrypt(strEncryptred, password);
//        }
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
