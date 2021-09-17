using System;
using System.Collections;
using System.Collections.Generic;

namespace Daniell.DialogSystem
{
    [Serializable]
    public class DialogLine
    {
        public Character Character;
        public string Text;
    }

    [Serializable]
    public class DialogChoice : DialogLine
    {
        public string this[int i] => _choices[i];
        public int Length => _choices.Length;

        private string[] _choices;
    }
}