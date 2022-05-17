using System;
using System.Collections.Generic;

namespace Spirebyte.Services.Identity.Application.Users.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Fullname { get; set; }
    public string Pic { get; set; }
    public IEnumerable<Guid> Roles { get; set; }
    public IEnumerable<string> Claims { get; set; }
    public DateTime CreatedAt { get; set; }
}