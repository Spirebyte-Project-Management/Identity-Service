﻿using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;

namespace Spirebyte.Services.Identity.Application.Commands;

[Contract]
public class SignUp : ICommand
{
    public SignUp(Guid userId, string email, string fullname, string pic, string password, string role,
        IEnumerable<string> permissions)
    {
        UserId = userId == Guid.Empty ? Guid.NewGuid() : userId;
        Email = email;
        Fullname = fullname;
        Pic = pic;
        Password = password;
        Role = role == string.Empty ? "User" : role;
        Permissions = permissions;
    }

    public Guid UserId { get; }
    public string Email { get; }
    public string Fullname { get; }
    public string Pic { get; }
    public string Password { get; }
    public string Role { get; }
    public IEnumerable<string> Permissions { get; }
}