using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dialogs.Data
{
    [CreateAssetMenu(
        fileName = "New Conversation", 
        menuName = "Dialog/Conversation")]
    public class ConversationData : ScriptableObject
    {
        [SerializeField]
        private DialogNodeData _rootNode;

        [SerializeField]
        private List<DialogNodeData> _dialogNodesData;

        public DialogNodeData RootNode => _rootNode;
        public List<DialogNodeData> DialogNodesData => _dialogNodesData;

#if UNITY_EDITOR
        public void Initialize()
        {
            _dialogNodesData = new List<DialogNodeData>();
            _rootNode = CreateNodeInner("root");
            AssetDatabase.SaveAssets();
        }

        public DialogNodeData CreateNode()
        {
            var dialogNode = CreateNodeInner($"node {_dialogNodesData.Count}");
            _dialogNodesData.Add(dialogNode);

            return dialogNode;
        }

        public void DeleteNode(DialogNodeData dialogNode)
        {
            _dialogNodesData.Remove(_rootNode);
            AssetDatabase.RemoveObjectFromAsset(dialogNode); 
            AssetDatabase.SaveAssets();
        }

        public void AddConnection(DialogNodeData sourceNode, DialogNodeData targetNode)
        {
            if (sourceNode is null || targetNode is null)
                return;
            sourceNode.AddConnection(targetNode);
            AssetDatabase.SaveAssets();
        }

        public void RemoveConnection(DialogNodeData sourceNode, DialogNodeData targetNode)
        {
            if (sourceNode is null || targetNode is null)
                return;
            sourceNode.RemoveConnection(targetNode);
            AssetDatabase.SaveAssets();
        }

        private DialogNodeData CreateNodeInner(string name)
        {
            var dialogNode = ScriptableObject.CreateInstance<DialogNodeData>();
            dialogNode.name = name;
            dialogNode.Initialize(GUID.Generate(), "New Dialog Node Text");

            AssetDatabase.AddObjectToAsset(dialogNode, this);
            AssetDatabase.SaveAssets();

            return dialogNode;
        }
#endif
    }   
}