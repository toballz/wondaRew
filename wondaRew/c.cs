  
using System.IO; 
using System.Security.Cryptography; 
using System.Windows; 
 

namespace wondaRew
{
    internal class C
    {  

        

        private static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (var rng =   RandomNumberGenerator.Create())
            {
                for (int i = 0; i < 30; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }
            return data;
        }
        private static int AES_KeySize = 256;
        private static int AES_BlockSize = 128; 

        public static string? EncryptByteStringorAnything_Aes(
        string inputFile, string outputFile, string password, string pwdSalt1, byte[] pwdSalt2)
        {
            try
            {
                // Generate a random salt
                byte[] randomSalt = GenerateRandomSalt();

                // Create output file stream
                using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                {
                    // Write the random salt to the start of the output file
                    fsCrypt.Write(randomSalt, 0, randomSalt.Length);

                    // Combine password and salts to create passwordBytes
                    byte[] passwordSalt1Bytes = System.Text.Encoding.UTF8.GetBytes(password + pwdSalt1);
                    byte[] passwordBytes = new byte[passwordSalt1Bytes.Length + pwdSalt2.Length];
                    Buffer.BlockCopy(passwordSalt1Bytes, 0, passwordBytes, 0, passwordSalt1Bytes.Length);
                    Buffer.BlockCopy(pwdSalt2, 0, passwordBytes, passwordSalt1Bytes.Length, pwdSalt2.Length);

                    // Set up AES encryption algorithm
                    using (Aes AES = Aes.Create())
                    {
                        AES.KeySize = AES_KeySize;
                        AES.BlockSize = AES_BlockSize;
                        AES.Padding = PaddingMode.PKCS7;
                        AES.Mode = CipherMode.CFB;

                        // Derive the key and IV using Rfc2898DeriveBytes with a secure hash algorithm
                        using (var keyDerivation = new Rfc2898DeriveBytes(passwordBytes, randomSalt, 50000, HashAlgorithmName.SHA256))
                        {
                            AES.Key = keyDerivation.GetBytes(AES.KeySize / 8);
                            AES.IV = keyDerivation.GetBytes(AES.BlockSize / 8);
                        }

                        // Create CryptoStream for encryption
                        using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            // Read the input file in chunks and encrypt
                            using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                            {
                                byte[] buffer = new byte[1048576]; // 1MB buffer size
                                int bytesRead;

                                while ((bytesRead = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    cs.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception a)
            {
                return a.Message;
            }
            return null;
        }

        public static string? DecryptByteStringorAnything_Aes(
        string inputFile, string outputFile, string password, string pwdSalt1, byte[] pwdSalt2)
        {
            // Combine password and salts to create passwordBytes
            byte[] passwordSalt1Bytes = System.Text.Encoding.UTF8.GetBytes(password + pwdSalt1);
            byte[] passwordBytes = new byte[passwordSalt1Bytes.Length + pwdSalt2.Length];
            Buffer.BlockCopy(passwordSalt1Bytes, 0, passwordBytes, 0, passwordSalt1Bytes.Length);
            Buffer.BlockCopy(pwdSalt2, 0, passwordBytes, passwordSalt1Bytes.Length, pwdSalt2.Length);

            // Prepare to read the encrypted file
            byte[] salt = new byte[GenerateRandomSalt().Length]; // Must match the encryption salt size
            using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
            {
                // Read the salt from the start of the file
                fsCrypt.Read(salt, 0, salt.Length);

                using (Aes AES = Aes.Create())
                {
                    AES.KeySize = AES_KeySize;
                    AES.BlockSize = AES_BlockSize;
                    AES.Padding = PaddingMode.PKCS7;
                    AES.Mode = CipherMode.CFB;

                    // Derive the key and IV using the same method as encryption
                    using (var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000, HashAlgorithmName.SHA256))
                    {
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                    }

                    // Set up CryptoStream for decryption
                    using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read))
                    using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                    {
                        byte[] buffer = new byte[1048576]; // 1MB buffer
                        int bytesRead;

                        try
                        {
                            while ((bytesRead = cs.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fsOut.Write(buffer, 0, bytesRead);
                            }
                        }
                        catch (CryptographicException ex)
                        {
                            return ("Decryption failed. Incorrect password or corrupted file: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            return ("An error occurred during decryption: " + ex.Message);
                        }
                    }
                }
            }return null;
        }
    }
}