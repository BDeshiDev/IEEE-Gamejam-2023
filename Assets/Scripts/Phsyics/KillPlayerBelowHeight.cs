using System;
using Core.Misc;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// Kill player if y pos drop below value
    /// Just a lazy way of ensuring that the player dies when he falls off 
    /// </summary>
    public class KillPlayerBelowHeight: MonoBehaviour
    {

        private void Update()
        {
            if (SceneVarTracker.Instance.Player != null)
            {
                if (SceneVarTracker.Instance.CurLevelData.KillHeight >= SceneVarTracker.Instance.Player.transform.position.y
                    && !SceneVarTracker.Instance.Player.HealthComponent.IsEmpty)
                {
                    Debug.Log("player killed for being too short. Just like real life");
                    SceneVarTracker.Instance.Player.forceKill();
                }
            }
            
        }
        //
        // private void OnDrawGizmos()
        // {
        //     if (SceneVarTracker.Instance.Player != null)
        //     {
        //         Gizmos.color = new Color(1,0,0,.5f);
        //         Gizmos.DrawCube(new Vector3(0, SceneVarTracker.Instance.CurLevelData.KillHeight, 0), new Vector3(100,1,100));
        //     }
        // }
    }
}