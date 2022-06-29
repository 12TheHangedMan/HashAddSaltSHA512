using System;

namespace CodeLabs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HashAddSalt HS = new HashAddSalt();
            HS.Validator("00002", "12345");
        }
    }
}
