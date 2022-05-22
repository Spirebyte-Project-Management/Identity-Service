namespace Spirebyte.Services.Identity.Application.Users.DTO
{
    public class UserProviderDeleteApiDto<TKey>
    {
        public TKey UserId { get; set; }

        public string ProviderKey { get; set; }

        public string LoginProvider { get; set; }
    }
}







