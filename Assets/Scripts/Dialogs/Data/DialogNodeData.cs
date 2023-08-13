using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialogs.Data
{
    public class DialogNodeData : ScriptableObject
    {
        [SerializeField]
        private GUID _guid;

        [SerializeField]
        private string _text;

        [SerializeField]
        private List<DialogNodeData> _connections;

        [HideInInspector]
        public Vector2 Position;

        public GUID GUID => _guid;
        public string Text => _text;

#if UNITY_EDITOR
        public void Initialize(GUID guid, string text)
        {
            _guid = guid;
            _text = text;

            AssetDatabase.SaveAssets();
        }

        public void AddConnection(DialogNodeData dialogNodeData)
        {
            _connections.Add(dialogNodeData);
        }

        public void RemoveConnection(DialogNodeData dialogNodeData)
        {
            _connections.Remove(dialogNodeData);
        }

        public List<DialogNodeData> Children => _connections;
#endif 
    }
}