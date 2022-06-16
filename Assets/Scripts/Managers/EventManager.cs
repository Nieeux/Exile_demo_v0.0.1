using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action<int> EnterRandomevents;
    public void Randomevents(int id)
    {
        if (EnterRandomevents != null)
        {
            EnterRandomevents(id);
        }
    }
}
