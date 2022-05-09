using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class WalkEvent : UnityEvent<int> { }

public class Pedometer : MonoBehaviour
{
    public WalkEvent onWalk = new WalkEvent();
    public TextMeshProUGUI txt_Steps;
    public int steps = 0;

    public float highLimit = 0.1f;
    public float lowLimit = 0.005f;
    public bool isHigh = false;

    public float filterHigh = 10.0f;
    public float filterLow = 0.1f;
    public float curAcc = 0f;
    public float avgAcc = 0f;
    public float delta = 0;

    public int counter = 30;
    public int waitTime = 30;
    public int oldSteps;
    public bool isCountable;
    public bool isWalkable = true;

    bool isAccOn;
    
    public TextMeshProUGUI txt_isHigh, txt_delta, txt_curAcc, txt_avgAcc;
    
    private void Awake()
    {
        onWalk = new WalkEvent();
        oldSteps = steps;
        txt_Steps.text = steps.ToString();
    }
    
    public void Init()
    {
        isWalkable = true;
    }
    
    private void FixedUpdate()
    {
        if (isWalkable)
        {
            if (isAccOn)
            {
                curAcc = Mathf.Lerp(curAcc, Input.acceleration.magnitude, Time.deltaTime * filterHigh);
                avgAcc = Mathf.Lerp(avgAcc, Input.acceleration.magnitude, Time.deltaTime * filterLow);
                delta = curAcc - avgAcc;

                if (!isHigh)
                {
                    if (delta > highLimit)
                    {
                        isHigh = true;
                        steps++;
                        if (onWalk != null)
                        {
                            Debug.Log("[Pedometer] onWalk : " + steps);
                            onWalk.Invoke(steps);
                        }
                    }
                }
                else
                {
                    if (delta < lowLimit)
                    {
                        isHigh = false;
                    }
                }
            }
        }
    }

    public void Play()
    {
        StopCoroutine(CheckAcc());
        StartCoroutine(CheckAcc());
    }

    public void Stop()
    {
        isWalkable = false;
    }

    IEnumerator CheckAcc()
    {
        while (avgAcc < 0.9f)
        {
            avgAcc = Input.acceleration.magnitude;
            yield return null;
        }
        isAccOn = true;
    }
}
