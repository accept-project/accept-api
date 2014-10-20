using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace AcceptApi.Areas.Api.Models.Security
{
    /// <summary>
    /// hmac private/public key authentication(not used currently).
    /// </summary>
    public class HmacSha1
    {
        string _message;
        string _key;
        HMACSHA1 _hmacsha1;
        System.Text.ASCIIEncoding _encoding = new System.Text.ASCIIEncoding();
        byte[] _hashMessage;
        string _stringfiedHashMesssage;

        public HmacSha1(string message, string key)
        {
            _message = message;
            _key = key;             
        }

       
        public string CalculateHmac()
        {
            byte[] keyByte = _encoding.GetBytes(_key);
            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = _encoding.GetBytes(_message);

            _hashMessage = hmacsha1.ComputeHash(messageBytes);

           return  _stringfiedHashMesssage = ByteToString(_hashMessage);

        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
    
    }
}