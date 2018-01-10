using System.Threading.Tasks;

namespace OpenEvents.Backend.Common
{
    public interface IAppInitializerTask
    {
        Task Initialize();
    }
}