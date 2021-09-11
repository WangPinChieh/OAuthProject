using AuthorizationServer.Controllers;

namespace AuthorizationServer.Services
{
    public interface IIdentityService
    {
        bool Login(Identity identity);
    }
}