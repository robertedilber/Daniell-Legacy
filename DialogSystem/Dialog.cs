using UnityEngine;

namespace Daniell.DialogSystem
{
    [CreateAssetMenu(fileName = "Dialog object", menuName = "Dialog")]
    public class Dialog : ScriptableObject
    {
        public DialogLine DialogLine;
    }
}