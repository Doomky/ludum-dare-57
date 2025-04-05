﻿using System;
using System.Collections;
using UnityEngine;

public class CoroutineHelpers : MonoBehaviour
{
    public static float Timescale = 1;

    [SerializeField] 
    private static CoroutineHelpers _singleton;

    public static CoroutineHelpers Singleton
    {
        get
        {
            if (!_singleton)
            {
                GameObject singletonGo = new("Coroutine GameObject");
                _singleton = singletonGo.AddComponent<CoroutineHelpers>();
            }
            
            return _singleton;
        }
    }
    
    public static IEnumerator WaitForSeconds(float duration)
    {
        yield return new WaitForSeconds(duration / Timescale);
    }

    public static void Chain(params IEnumerator[] actions)
    {
        Singleton.StartCoroutine(ChainCoroutine(actions));
    }

    public static void DoDelayed(Action action, float delay, MonoBehaviour monoBehaviour = null)
    {
        if (monoBehaviour)
        {
            monoBehaviour.StartCoroutine(DoDelayedCoroutine(action, delay));
        }
        else
        {
            Singleton.StartCoroutine(DoDelayedCoroutine(action, delay));
        }
    }

    public static void Do(Action action)
    {
        Singleton.StartCoroutine(DoCoroutine(action));
    }

    private static IEnumerator ChainCoroutine(params IEnumerator[] actions)
    {
        foreach (IEnumerator action in actions)
        {
            yield return Singleton.StartCoroutine(action);
        }
    }

    private static IEnumerator DoDelayedCoroutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    private static IEnumerator WaitForSecondsCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
    }

    private static IEnumerator DoCoroutine(Action action)
    {
        action();
        yield return 0;
    }
}
