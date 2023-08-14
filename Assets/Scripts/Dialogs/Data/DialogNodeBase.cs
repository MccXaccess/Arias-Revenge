using UnityEditor;
using UnityEngine;

namespace Dialogs.Data
{
    public abstract class DialogNodeBase : ScriptableObject
    {
        [SerializeField]
        private GUID _guid;

        [HideInInspector]
        [SerializeField]
        private Vector2 _nodeViewPosition;

#if UNITY_EDITOR
        public GUID GUID => _guid;    

        public Vector2 NodeViewPosition => _nodeViewPosition;

        public void SetNodePosition(Vector2 nodeViewPosition) 
        {
            _nodeViewPosition = nodeViewPosition;
            AssetDatabase.SaveAssets();
        }

        public virtual void Initialize(GUID guid)
        {
            _guid = guid;
        }
#endif
    }
}