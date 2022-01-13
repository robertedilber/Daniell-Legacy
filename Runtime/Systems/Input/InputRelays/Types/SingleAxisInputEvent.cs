using Daniell.Runtime.Systems.Events;
using UnityEngine;

namespace Daniell.Runtime.Systems.Input
{
    [CreateAssetMenu(menuName = "Input/Single Axis Input Event")]
    public class SingleAxisInputEvent : ValueInputRelay<FloatEvent, float> { }
}