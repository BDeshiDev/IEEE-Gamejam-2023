using UnityEngine;
using UnityEngine.Rendering;

namespace Core.Misc
{
    public static class ApplicationInitializer {
 
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeAfterSceneLoad() {
            DebugManager.instance.enableRuntimeUI = false;
        }
 
    }
}