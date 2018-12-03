using System;
using System.Security.Cryptography;
using System.Text;

namespace PharmaTab
{
    class Cryptage
    {

        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        /// <summary>
        /// renvoie la version hachée (en chaîne de caractères) du mot de passe clair passé en paramètre
        /// </summary>
        /// <param name="clearPassword"> le mot de passe à chiffrer </param>
        /// <returns> string </returns>
        public static string ProtectPassword(string clearPassword)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(clearPassword);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);

            return Convert.ToBase64String(outputBuffer);
        }

        /// <summary>
        /// renvoie la version claire (en chaîne de caractères) du mot de passe haché passé en paramètre
        /// </summary>
        /// <param name="protectedPassword"> le mot de passe à déchiffrer </param>
        /// <returns> string </returns>
        public static string UnprotectPassword(string protectedPassword)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(protectedPassword);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);

            return Encoding.Unicode.GetString(outputBuffer);
        }
    }
}