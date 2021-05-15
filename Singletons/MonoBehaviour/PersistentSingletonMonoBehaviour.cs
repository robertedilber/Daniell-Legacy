﻿using UnityEngine;

public abstract class PersistentSingletonMonoBehaviour<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}