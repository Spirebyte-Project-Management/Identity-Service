namespace Spirebyte.Services.Identity.Application.Services.Interfaces;

public interface IRng
{
    string Generate(int length = 50, bool removeSpecialChars = true);
}