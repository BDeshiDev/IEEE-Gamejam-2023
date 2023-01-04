using FSM.GameState;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/*
Cheap method to ensure that manager scene is already loaded.
This will try to loadGameData the manager scene anyways even if the scene is loaded
e.g after changing levels
As long as the root objects in the manager scene are singletons(which they should be),
having the manager scene already loaded is not a problem
In general, ensure that everything in the manager scene goes to don'tdestroyonload
and that they are singletons
*/
namespace BDeshi.levelloading
{
    public class ManagerLoadEnsurer : MonoBehaviour
    {
        public static bool loadedManager = false;
        //can't  direcly call the manager fsm so keep an accessible

        public string managerSceneName = "ManagerScene";

        public UnityEvent preManagerSceneCallback;
        private void Awake()
        {
            Debug.Log("loadedManager = " + loadedManager);
            ensureLoad();
        }

        public  void ensureLoad()
        {
            if (!loadedManager)
            {
                loadedManager = true;
                preManagerSceneCallback.Invoke();
                // GameStateManager.initialStateID = GameStateManager.gameplayStateID;
                Debug.Log("GameStateManager.initialStateID = " + GameStateManager.initialStateID);
                SceneManager.LoadScene(managerSceneName, LoadSceneMode.Additive);
            }
        }

    }
}
