using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool waiting;
    Coroutine C_StopTime;
    public void Stop(float duration)
    {
        if (C_StopTime != null) { 
            StopCoroutine(C_StopTime);
        }
        StartCoroutine(StopTime(duration));
    }
    IEnumerator StopTime(float duration)
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
    }
}
