﻿using BDeshi.levelloading;
using Core.Input;
using UnityEditor;
using UnityEngine;

namespace Core.Misc
{
    #if UNITY_EDITOR
    /// <summary>
    /// Unity playmode with no domain reload/scene reload can cause problems with static stuff
    /// But it's too nice to give up.
    /// Call cleanup funcs here for now.
    /// TODO: Make an attribute to fetch and call automatically
    /// </summary>
    [InitializeOnLoad]
    public static class PlayStateCleaner
    {
        static PlayStateCleaner()
        {
            EditorApplication.playModeStateChanged += ModeChanged;
        }

        static void ModeChanged(PlayModeStateChange playModeState)
        {
            if (playModeState == PlayModeStateChange.ExitingPlayMode)
            {
                ManagerLoadEnsurer.loadedManager = false;
                InputManager.PlayModeExitCleanUp();
                PlayerEntity.PlayModeExitCleanUp();
                SceneVarTracker.PlayModeExitCleanup();
                
                GameProgressTracker.PlayModeExitCleanup();
            }
            else if (playModeState == PlayModeStateChange.ExitingEditMode)
            {
                ManagerLoadEnsurer.loadedManager = false;

                SceneVarTracker.PlayModeEnterCleanup();
                GameProgressTracker.PlayModeEnterCleanup();

            }
        }
    }
    #endif
}