using System;
using System.Collections;
using System.Collections.Generic;
using Core.Input;
using Core.Misc;
using FSM.GameState;
using Sound;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangePortal : MonoBehaviour
{
    private bool hasTriggered = false;

    public int debugLevelChangeThreshold = 3;
    void handlePortalEntered()
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            SFXManager.Instance.play(SFXManager.Instance.levelCompleteSFX);
            //assume that the portal will only be placed on levels that have leveldata objects in them
            GameStateManager.Instance.loadScene(SceneVarTracker.Instance.CurLevelData.getNextSceneAfterThisLevel());
        }
    }

    private void Start()
    {
        InputManager.debugButton1.addPerformedCallback(gameObject, handleDebugCalled);
    }

    /// <summary>
    /// skip to the next level for developer convenience
    /// </summary>
    private void handleDebugCalled()
    {
        if (--debugLevelChangeThreshold <= 0)
        {
            handlePortalEntered();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("portal " + other);
        handlePortalEntered();
    }
}