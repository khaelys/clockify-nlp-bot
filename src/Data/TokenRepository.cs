﻿using System;
using System.Threading.Tasks;
using Azure;
using Azure.Security.KeyVault.Secrets;

namespace Bot.Data
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly SecretClient _secretClient;
        
        // TODO add caching

        public TokenRepository(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        public async Task<TokenData> ReadAsync(string id)
        {
            if (id == null) throw new ArgumentNullException(id);
            
            try
            {
                KeyVaultSecret secret = await _secretClient.GetSecretAsync(id);
                return new TokenData(secret.Name, secret.Value);
            }
            catch (RequestFailedException e)
            {
                if (e.Status == 404)
                {
                    throw new TokenNotFoundException("No token has been found with id " + id);
                }

                throw;
            }
        }

        public async Task<TokenData> WriteAsync(string value, string? id = null)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(value);
            
            string name = id ?? Guid.NewGuid().ToString();
            KeyVaultSecret secret = await _secretClient.SetSecretAsync(name, value);
            return new TokenData(secret.Name, secret.Value);
        }
    }
}