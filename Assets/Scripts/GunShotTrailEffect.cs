using System;
using System.Collections;
using System.Collections.Generic;
using BDeshi.Utility;
using UnityEngine;

public class GunShotTrailEffect : MonoBehaviour
{
    private LineRenderer liner;
    public FiniteTimer trailTimer = new FiniteTimer(0, .16f);
    public Color baseColor;

    private void Awake()
    {
        liner = GetComponent<LineRenderer>();
        baseColor = liner.startColor;
        
        trailTimer.complete();
        disableTrail();
    }

    public void enableTrail(Vector3 shotOrigin, Vector3 shotEndPoint)
    {
        trailTimer.reset();
        liner.enabled = true;
        liner.SetPosition(0, shotOrigin);
        liner.SetPosition(1, shotEndPoint);

        liner.startColor = liner.endColor = baseColor;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!trailTimer.isComplete)
        {
            trailTimer.updateTimer(Time.deltaTime);
            liner.startColor = liner.endColor = Color.Lerp(baseColor, new Color(baseColor.r, baseColor.g, baseColor.b,0),  trailTimer.Ratio);

            if (trailTimer.isComplete)
            {
                disableTrail();
            }
        }
    }

    private void disableTrail()
    {
        liner.enabled = false;
    }
}
