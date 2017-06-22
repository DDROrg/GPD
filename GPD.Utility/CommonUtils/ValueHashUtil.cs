using System;
using System.Security.Cryptography;

namespace GPD.Utility.CommonUtils
{
    /// <summary>
    /// Salted value hashing with PBKDF2-SHA1.
    /// </summary>
    public class ValueHashUtil
    {
        #region Declarations
        // The following constants may be changed without breaking existing hashes.
        private const int SALT_BYTES = 30;
        private const int HASH_BYTES = 40;
        private const int PBKDF2_ITERATIONS = 1000;

        private const int ITERATION_INDEX = 0;
        private const int SALT_INDEX = 1;
        private const int PBKDF2_INDEX = 2;
        #endregion Declarations

        #region Public Methods

        /// <summary>
        /// Creates a salted PBKDF2 hash of the password.
        /// </summary>
        /// <param name="valueToHash">Value to hash.</param>
        /// <returns>The hash of the password.</returns>
        public static string CreateHash(string valueToHash)
        {
            // Generate a random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTES];
            csprng.GetBytes(salt);

            // Hash the password and encode the parameters
            byte[] hash = PBKDF2(valueToHash, salt, PBKDF2_ITERATIONS, HASH_BYTES);

            return string.Format("{0}:{1}:{2}", PBKDF2_ITERATIONS, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        /// <summary>
        /// Validates a data given a hash of the correct one.
        /// </summary>
        /// <param name="valueToCheck">The string value to check.</param>
        /// <param name="goodHash">At hash of he correct value.</param>
        /// <returns>True if the valueToCheck is correct. False otherwise.</returns>
        public static bool ValidateHash(string valueToCheck, string goodHash)
        {
            // Extract the parameters from the hash
            char[] delimiter = { ':' };
            string[] split = goodHash.Split(delimiter);
            int iterations = int.Parse(split[ITERATION_INDEX]);
            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

            byte[] testHash = PBKDF2(valueToCheck, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Compares two byte arrays in length-constant time. This comparison
        /// method is used so that password hashes cannot be extracted from
        /// on-line systems using a timing attack and then attacked off-line.
        /// </summary>
        /// <param name="a">The first byte array.</param>
        /// <param name="b">The second byte array.</param>
        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="valueToHash">The string value to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The PBKDF2 iteration count.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] PBKDF2(string valueToHash, byte[] salt, int iterations, int outputBytes)
        {
            byte[] retunValue;

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(valueToHash, salt, iterations))
            {
                retunValue = pbkdf2.GetBytes(outputBytes);
            }

            return retunValue;
        }

        #endregion Private Methods

        #region Pubic Get/Set Properties
        public static int IterationsNumber
        {
            get
            {
                return PBKDF2_ITERATIONS;
            }
        }
        #endregion Pubic Get/Set Properties
    }
}