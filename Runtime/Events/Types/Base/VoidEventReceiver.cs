using Daniell.Runtime.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VoidEventReceiver : EventReceiver
{
#if UNITY_EDITOR
    public override ScriptableEvent Event => _event;
#endif

    [SerializeField]
    [Tooltip("Event linked to this receiver")]
    private VoidEvent _event;

    [SerializeField]
    [Tooltip("Response called when the linked event is raised")]
    private UnityEvent _response;

    private void OnEnable()
    {
        _event.AddListener(OnEventReceived);
    }

    private void OnDisable()
    {
        _event.RemoveListener(OnEventReceived);
    }

    private void OnEventReceived()
    {
        _response.Invoke();
    }
}
