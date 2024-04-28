using System.Text;
using System.Security.Cryptography;
using System.Text;

namespace APIAlunoTurma.Services
{
    public class ProtegeSenha
    {
        public static string GetSha256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2")); // Converte para hexadecimal
                }

                return builder.ToString();
            }
        }

        public static string SenhaHash(string senha)
        {
            string hash = GetSha256Hash(senha);
            return hash;
        }
    }
}
