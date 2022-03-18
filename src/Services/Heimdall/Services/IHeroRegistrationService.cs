using System.Threading.Tasks;
using Heimdall.Models;

namespace Heimdall.Services
{
    public interface IHeroRegistrationService
    {
        Task RegisterHeroAync(string name, string passowrd);
    }
}