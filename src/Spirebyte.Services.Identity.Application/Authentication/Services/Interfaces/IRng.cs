namespace Spirebyte.Services.Identity.Application.Authentication.Services.Interfaces;

public interface IRng
{
    string Generate(int length = 50, bool removeSpecialChars = true);
}