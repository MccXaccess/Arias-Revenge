using System;
using UnityEditor;
using UnityEngine;

namespace Dialogs.Data
{
    public class DialogNodeData : ScriptableObject
    {
        [SerializeField]
        private Guid _guid;

        [SerializeField]
        private string _text;

        public Guid Guid => _guid;
        public string Text => _text;

#if UNITY_EDITOR
        public void Initialize(Guid guid, string text)
        {
            _guid = guid;
            _text = text;

            AssetDatabase.SaveAssets();
        }
#endif 
    }
}