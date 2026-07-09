using Infrastructure.Services;

namespace UI.Services.Windows
{
    public interface IWindowsService : IService
    {
        void Show(WindowType type);
    }
}