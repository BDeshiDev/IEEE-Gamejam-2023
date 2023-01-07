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
        private PlayerEntity player;
        private LevelData levelData;
        private void Start()
        {
            player = SceneVarTracker.Instance.Player;
            levelData = SceneVarTracker.Instance.CurLevelData;
        }

        private void Update()
        {
            if (levelData.KillHeight >= player.transform.position.y && !player.HealthComponent.IsEmpty)
            {
                Debug.Log("player killed for being too short. Just like real life");
                player.forceKill();
            }
        }

        private void OnDrawGizmos()
        {
            if (levelData != null)
            {
                Gizmos.color = new Color(1,0,0,.5f);
                Gizmos.DrawCube(new Vector3(0, levelData.KillHeight, 0), new Vector3(100,1,100));
            }
            
        }
    }
}