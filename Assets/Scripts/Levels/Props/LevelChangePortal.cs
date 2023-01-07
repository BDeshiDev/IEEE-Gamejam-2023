using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Input;
using Core.Misc;
using DG.Tweening;
using FSM.GameState;
using Sound;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangePortal : MonoBehaviour
{
    private bool hasTriggered = false;

    public int debugLevelChangeThreshold = 3;
    
    public Transform portalOrigin;
    public float pullbackFOV = 160;
    public float pulloutIime = .75f;
    public float pullInIime = .5f;
    IEnumerator triggerPullInEffect()
    {
        var cam = Camera.main;
        cam.transform.parent = null;
        SceneVarTracker.Instance.Player.gameObject.SetActive(false);

        cam.transform.position = new Vector3(cam.transform.position.x, portalOrigin.position.y,cam.transform.position.z) ;
        var camPullBackPos = cam.transform.position + (cam.transform.position - portalOrigin.position).normalized * 5;
        cam.transform.LookAt(portalOrigin);
        var fovStart = cam.fieldOfView;
        var fovZoomOutTimer = new FiniteTimer(1);
        // while (!fovZoomOutTimer.isComplete)
        // {
        //     fovZoomOutTimer.updateTimer(Time.deltaTime);
        //     cam.fieldOfView = Mathf.Lerp(fovStart, pullbackFOV, fovZoomOutTimer.Ratio);
        //     yield return null;
        // }
        yield return
        DOTween.Sequence()
            .Insert(0, cam.DOFieldOfView(pullbackFOV, pulloutIime))
            .Insert(0, cam.transform.DOMove(camPullBackPos, pulloutIime))
            .Insert(pulloutIime, cam.DOFieldOfView(10, pullInIime))
            .Insert(pulloutIime, cam.transform.DOMove(portalOrigin.position, pullInIime))
            .WaitForCompletion();

        // cam.fieldOfView = fovStart;
        // SFXManager.Instance.play(SFXManager.Instance.levelCompleteSFX);
        //
        // yield return cam.transform.DOMove(portalOrigin.position, .5f, true).WaitForCompletion();

        Debug.Log("done");
        
        loadNextLevel();
    }
        
        
    void handlePortalEntered()
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(triggerPullInEffect());
        }
    }

    private static void loadNextLevel()
    {
        //assume that the portal will only be placed on levels that have leveldata objects in them
        GameStateManager.Instance.loadScene(SceneVarTracker.Instance.CurLevelData.getNextSceneAfterThisLevel());
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