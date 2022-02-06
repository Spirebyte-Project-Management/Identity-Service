﻿using System;
using System.Linq;
using System.Security.Cryptography;
using Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;

namespace Spirebyte.Services.Identity.Infrastructure.Auth;

internal sealed class Rng : IRng
{
    private static readonly string[] SpecialChars = { "/", "\\", "=", "+", "?", ":", "&" };

    public string Generate(int length = 50, bool removeSpecialChars = true)
    {
        using var rng = new RNGCryptoServiceProvider();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        var result = Convert.ToBase64String(bytes);

        return removeSpecialChars
            ? SpecialChars.Aggregate(result, (current, chars) => current.Replace(chars, string.Empty))
            : result;
    }
}