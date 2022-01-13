using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Daniell.Runtime.Systems.DialogueNodes
{
    public class DialogueReader : MonoBehaviour
    {
        public DialogueFile _dialogueFile;

        GraphNodeData startNode;
        GraphNodeData currentNode;

        public event Action OnDialogueStart;
        public event Action OnDialogueEnd;

        public void InitializeDialogue(DialogueFile dialogueFile)
        {
            _dialogueFile = dialogueFile;

            // Call on dialogue start
            OnDialogueStart?.Invoke();
            startNode = _dialogueFile.GetStartNode();
            currentNode = startNode;

            // Call next
            Next();
        }

        public bool Next()
        {
            // Return if there is no dialogue file loaded.
            if (_dialogueFile == null)
            {
                Debug.LogWarning("Dialogue file not loaded. Call 'InitializeDialogue()' first.");
                return false;
            }

            if (currentNode != null)
            {
                ProcessNode(currentNode);

                // Go to the next node
                _dialogueFile.TryGetNextNodeData(currentNode, out GraphNodeData nextNode);

                currentNode = nextNode;
                return true;
            }

            // End dialogue
            OnDialogueEnd?.Invoke();

            return false;
        }

        protected virtual void ProcessNode(GraphNodeData graphNodeData)
        {
            switch (graphNodeData)
            {
                case DialogueLineNodeData dialogueLineNodeData:
                    Debug.Log(dialogueLineNodeData.Line);
                    break;
            }
        }

        protected virtual void SendDataToCurrentNode()
        {

        }

        private void Start()
        {
            InitializeDialogue(_dialogueFile);
        }

        void OnInteract() => Next();
    }
}