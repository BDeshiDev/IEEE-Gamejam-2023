using UnityEngine;

namespace Core.Misc
{
    public class SkipToNextLevelHelper:MonoBehaviour
    {
        public void skipToNextLevel()
        {
            var portal = GameObject.FindObjectOfType<LevelChangePortal>();
            if (portal != null)
            {
                portal.handlePortalEntered();
            }
        }
    }
}