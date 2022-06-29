using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace CodeLabs
{
    internal class HashAddSalt
    {
        string inputString { get; set; }
        public HashAddSalt(string inputString)
        {
            this.inputString = inputString;                   
        }
        public HashAddSalt() { }

        //Hash512 generates a Hashed string using SHA512
        public string Hash512(string salt, string targetString)
        {
            using (SHA512 shaEncry = new SHA512Managed())
            {
                byte[] targetBytes = Encoding.UTF8.GetBytes(salt + targetString);
                byte[] hashBytes = shaEncry.ComputeHash(targetBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        //SaltGenerate Module creates salt
        public string Salt()
        {
            Byte[] saltByte = new Byte[Math.Max(64, this.inputString.Length)];
            var rng = RandomNumberGenerator.Create();           
            rng.GetBytes(saltByte);                        
            return Convert.ToBase64String(saltByte);
        }

        //Under this line are validating/modifying functions
        bool ValidatingFlag = false;
        public Dictionary<string, SaltAndHash> UserDic = new Dictionary<string, SaltAndHash>()
        {
            { "00002", new SaltAndHash("QxxLKmk/8gOy/VhK6eHjpw4GuM2o","fZGNQyjrB1Snnk9nttfNhuFz6gKBRV0uYWW4Tw0Gv+uzO2alPPO1Q89fySQ/fmTJY9KXfHCH6U9G9d6uL11qfg==") }
        };
        public class SaltAndHash
        {
            public string salt { get; private set; }
            public string hash { get; private set; }
            public SaltAndHash(string salt, string hash)
            {
                this.hash = hash;
                this.salt = salt;
            }
        }
        //modify/add/remove hashcode and salt module     
        public void UpdateSaltHash(string ID, string PassWord)
        {            
            string salt = Salt();
            string hash = Hash512(salt, PassWord);
            SaltAndHash salt_hash = new SaltAndHash(salt, hash);
            if (!UserDic.ContainsKey(ID))
            {
                UserDic.Add(ID, salt_hash);
                try
                {
                    UserDic.Remove(ID);
                }
                catch (SystemException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                UserDic[ID] = salt_hash;
            }           
        }
        //validate ID/Password module
        public void Validator(string ID, string password)
        {
            if (UserDic.ContainsKey(ID))
            {
                if (UserDic[ID].hash == Hash512(UserDic[ID].salt, password)) ValidatingFlag = true;
            }
            if (ValidatingFlag)
            {
                Console.WriteLine("Login Success");
            }
            else
            {
                Console.WriteLine("Login Failure");
            }

        }
    }
}
