using Infrastructure.Services;

namespace Services
{
    public interface IRandomService : IService
    {
        int Next(int min, int max);
    }
}