using Daniell.Runtime.RuntimeMonitor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RuntimeDataMonitorWindow : EditorWindow
{
    private List<VisualElement> elements = new List<VisualElement>();

    [MenuItem("Daniell/Runtime Data Monitor")]
    public static void Open()
    {
        var window = GetWindow<RuntimeDataMonitorWindow>();
        window.titleContent = new GUIContent("Runtime Data Monitor");
    }

    private void OnEnable()
    {
        RuntimeDataMonitor.OnValueListUpdated += OnValueListUpdated;
    }

    private void OnDisable()
    {
        RuntimeDataMonitor.OnValueListUpdated -= OnValueListUpdated;
    }

    private void OnValueListUpdated(List<System.Func<object>> obj)
    {

    }
}
