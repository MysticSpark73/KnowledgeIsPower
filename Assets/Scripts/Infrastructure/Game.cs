using Services.Input;

namespace Infrastructure
{
    public class Game
    {
        public static IInputService InputService;

        public Game()
        {
            RegisterInputService();
        }

        private void RegisterInputService()
        {
#if UnityEditor
            InputService = new StandaloneInputService();
#else
            InputService = new MobileInputService();
#endif
        }
    }
}