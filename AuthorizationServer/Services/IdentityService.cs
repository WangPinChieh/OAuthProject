using AuthorizationServer.Controllers;

namespace AuthorizationServer.Services
{
    public class IdentityService : IIdentityService
    {
        public bool Login(Identity identity)
        {
            return true;
        }
    }
}