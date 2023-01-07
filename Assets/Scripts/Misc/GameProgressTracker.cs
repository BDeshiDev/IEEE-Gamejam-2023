using BDeshi.Utility;

namespace Core.Misc
{
    /// <summary>
    /// Contains variables for game progress that need to persist across levels
    /// and level reloads(hence can't be stored in non singletons)
    /// </summary>
    public class GameProgressTracker: MonoBehaviourLazySingleton<GameProgressTracker>
    {
        public bool hasPlayerDiedFromHealthpack = false;
        
    }
}