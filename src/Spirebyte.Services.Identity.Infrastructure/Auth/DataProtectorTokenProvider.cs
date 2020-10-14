using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Convey.Auth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Spirebyte.Services.Identity.Application.Services.Interfaces;
using Spirebyte.Services.Identity.Core.Entities.Base;

namespace Spirebyte.Services.Identity.Infrastructure.Auth
{
    public class DataProtectorTokenProvider : IDataProtectorTokenProvider
    {
        private readonly JwtOptions _tokenOptions;
        protected IDataProtector Protector { get; private set; }
        public DataProtectorTokenProvider(IDataProtectionProvider dataProtectionProvider, JwtOptions tokenOptions)
        {
            _tokenOptions = tokenOptions;
            Protector = dataProtectionProvider.CreateProtector("DataProtectorTokenProvider");
        }

        public virtual async Task<string> GenerateAsync(string purpose, AggregateId userId, string securityStamp)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var ms = new MemoryStream();
            using (var writer = ms.CreateWriter())
            {
                writer.Write(DateTimeOffset.UtcNow);
                writer.Write(userId.ToString());
                writer.Write(purpose ?? "");

                writer.Write(securityStamp ?? "");
            }
            var protectedBytes = Protector.Protect(ms.ToArray());
            return Convert.ToBase64String(protectedBytes);
        }
        public virtual async Task<bool> ValidateAsync(string purpose, string token, AggregateId userId, string securityStamp)
        {
            try
            {
                var unprotectedData = Protector.Unprotect(Convert.FromBase64String(token));
                var ms = new MemoryStream(unprotectedData);
                using var reader = ms.CreateReader();

                var creationTime = reader.ReadDateTimeOffset();
                var expirationTime = creationTime + TimeSpan.FromMinutes(_tokenOptions.ExpiryMinutes);
                if (expirationTime < DateTimeOffset.UtcNow)
                {
                    return false;
                }

                var usrId = reader.ReadString();
                if (usrId != userId.ToString())
                {
                    // invalid userId
                    return false;
                }

                var purp = reader.ReadString();
                if (!string.Equals(purp, purpose))
                {
                    // invalidPurpose
                    return false;
                }

                var stamp = reader.ReadString();
                if (!string.Equals(stamp, securityStamp))
                {
                    //Invalid stamp
                    return false;
                }

                return true;
            }
            catch
            {
                // Do not leak exception
            }

            return false;
        }
    }

    internal static class StreamExtensions
    {
        private static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        public static BinaryReader CreateReader(this Stream stream)
        {
            return new BinaryReader(stream, DefaultEncoding, true);
        }

        public static BinaryWriter CreateWriter(this Stream stream)
        {
            return new BinaryWriter(stream, DefaultEncoding, true);
        }

        public static DateTimeOffset ReadDateTimeOffset(this BinaryReader reader)
        {
            return new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);
        }

        public static void Write(this BinaryWriter writer, DateTimeOffset value)
        {
            writer.Write(value.UtcTicks);
        }
    }
}
