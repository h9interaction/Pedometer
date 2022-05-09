using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pedometer : MonoBehaviour
{
    public float lowLimit = 0.005f;
    public float highLimit = 0.1f;

    public float steps = 0.0f;

    public bool stateH = false;

    public float fHigh = 10.0f;
    public float fLow = 0.1f;

    private float curAcc = 0.0f;
    private float avgAcc;

    private float delta;

    public Text gStepCount;

    public static bool isWalking = false;
    public float currentTime;
    public float walkingTime;

    void Start()
    {
        avgAcc = Input.acceleration.magnitude;
    }

    void FixedUpdate()
    {
        curAcc = Mathf.Lerp(curAcc, Input.acceleration.magnitude, Time.deltaTime * fHigh);
        avgAcc = Mathf.Lerp(avgAcc, Input.acceleration.magnitude, Time.deltaTime * fLow);

        delta = curAcc - avgAcc;

        if (!stateH)
        {
            if (delta > highLimit)
            {
                stateH = true;
                currentTime = Time.time;
                walkingTime = currentTime + 0.75f;
                steps++;
                gStepCount.text = "Steps: " + steps;
            }
        }
        else
        {
            if (delta < lowLimit)
            {
                stateH = false;
            }
        }
    }

    void Update()
    {
        if (stateH)
        {
            isWalking = true;
        }
        if (isWalking)
        {
            if (Time.time > walkingTime)
            {
                isWalking = false;
            }
        }
    }
}