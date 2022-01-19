namespace Spirebyte.Services.Identity.Application.Services.Interfaces;

public interface IPasswordService
{
    bool IsValid(string hash, string password);
    string Hash(string password);
}