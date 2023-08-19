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
        private string _nodeTitle;

        [HideInInspector]
        [SerializeField]
        private Vector2 _nodeViewPosition;

        [HideInInspector]
        [SerializeField]
        private bool _isRootNode;

#if UNITY_EDITOR
        public GUID GUID => _guid;    
        public string NodeTitle => _nodeTitle;
        public Vector2 NodeViewPosition => _nodeViewPosition;
        public bool IsRootNode => _isRootNode;

        public virtual void Initialize(GUID guid)
        {
            _guid = guid;
            AssetDatabase.SaveAssets();
        }

        public void SetNodeTitle(string nodeTitle)
        {
            _nodeTitle = nodeTitle;
            AssetDatabase.SaveAssets();
        }

        public void SetNodePosition(Vector2 nodeViewPosition) 
        {
            _nodeViewPosition = nodeViewPosition;
            AssetDatabase.SaveAssets();
        }

        public void SetRootNode(bool isRootNode)
        {
            _isRootNode = isRootNode;
            AssetDatabase.SaveAssets();
        }
#endif
    }
}