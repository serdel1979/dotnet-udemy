using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using WebApiAutores.DTOs;

namespace WebApiAutores.Services
{
    public class HashService
    {

        public ResultadoHash Hash(string txtPlano)
        {
            var sal = new byte[16];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(sal);
            }

            return getHash(txtPlano, sal);
        }

        public ResultadoHash getHash(string txtPlano, byte[] sal)
        {
            var llaveDerivada = KeyDerivation.Pbkdf2(password: txtPlano, salt: sal,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 36);

            var hash = Convert.ToBase64String(llaveDerivada);
            return new ResultadoHash()
            {
                Hash = hash,
                Sal = sal
            };
        }
    }
}
