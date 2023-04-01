using UnityEngine;

namespace Core.Misc
{
    public class GameobjectToggleHelper : MonoBehaviour
    {
        public GameObject go;
        public void toggle()
        {
            go.SetActive(!go.activeSelf);
        }
    }
}