using System;
using System.Collections;
using System.Collections.Generic;
using Core.Misc;
using FSM.GameState;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangePortal : MonoBehaviour
{
    
    void handlePortalEntered()
    {
        //assume that the portal will only be placed on levels that have leveldata objects in them
        GameStateManager.Instance.loadLevel(SceneVarTracker.Instance.CurLevelData.getNextSceneAfterThisLevel());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("portal " + other);
        handlePortalEntered();
    }
}