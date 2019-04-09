using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;

namespace SiteMonitoring.Services
{
    /// <summary>
    /// Сервис для работы с JWT токенами
    /// </summary>
    public class JWTService
    {
        /// <summary>
        /// Секретный ключ
        /// </summary>
        const string Secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        /// <summary>
        /// Создать токен
        /// </summary>
        public static string GenerateUserToken(string login, string password, string userid)
        {
            var payload = new Dictionary<string, object>
            {
                //Здесь должны быть клеймы
                { "login", login },
                { "password", password },
                { "userid", userid}
            };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, Secret);
        }

        /// <summary>
        /// Декдировать токен
        /// </summary>
        public static JWTTokenDict DecodeUserToken(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, Secret, verify: true);

                return JsonConvert.DeserializeObject<JWTTokenDict>(json);
            }
            catch (TokenExpiredException)
            {
                throw new Exception("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new Exception("Token has invalid signature");
            }
        }

        public class JWTTokenDict
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string Userid { get; set; }
        }
    }
}
