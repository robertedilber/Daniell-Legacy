using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daniell.Runtime.RuntimeMonitor
{
    public static class RuntimeDataMonitor
    {
        private static List<Func<object>> _monitoredValues = new List<Func<object>>();

        public static event Action<List<Func<object>>> OnValueListUpdated;

        public static void MonitorValue(Func<object> valueDelegate)
        {
            _monitoredValues.Add(valueDelegate);
            OnValueListUpdated?.Invoke(_monitoredValues);
        }
    }
}