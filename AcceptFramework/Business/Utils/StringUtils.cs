using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using AcceptFramework.Domain.Common;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace AcceptFramework.Business.Utils
{
    public static class StringUtils
    {
        public static string GenerateTinyHash(string input)
        {            
            int ms = input.GetHashCode();
            return string.Format("{0:X}", ms).ToLower();
        }

        /// <summary>
        /// creates a MD5 hash.
        /// </summary>
        public static string ToMD5(this string input)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(input);
            data = x.ComputeHash(data);

            string output = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                output += data[i].ToString("x2").ToLower();
            }

            return output;
        }

        public static string Generate32CharactersStringifiedGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string Generate32CharactersStringifiedGuid(string sourceUrl)
        {
            return string.Format("{0}_{1}", sourceUrl, Guid.NewGuid().ToString("N"));
        }

        public static bool EmailValidator(string email)
        {
            string emailPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            Match emailMatch = Regex.Match(email, emailPattern);
            return !string.IsNullOrEmpty(emailMatch.Value);
        }
       
    }
}
