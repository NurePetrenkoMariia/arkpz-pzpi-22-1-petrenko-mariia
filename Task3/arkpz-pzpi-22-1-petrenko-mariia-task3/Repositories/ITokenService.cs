using Models;

namespace Repositories
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
