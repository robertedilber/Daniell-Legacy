using Daniell.Runtime.Systems.Events;
using UnityEngine;

namespace Daniell.Runtime.Systems.Input
{
    [CreateAssetMenu(menuName ="Input/Dual Axis Input Event")]
    public class DualAxisInputEvent : ValueInputRelay<Vector2Event, Vector2> { }
}