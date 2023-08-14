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
        private SpeechNodeData _rootNode;

        [SerializeField]
        private List<SpeechNodeData> _speechNodes;

        public SpeechNodeData RootNode => _rootNode;
        public List<SpeechNodeData> SpeechNodes => _speechNodes;

#if UNITY_EDITOR
        public void Initialize()
        {
            _speechNodes = new List<SpeechNodeData>();
            _rootNode = CreateNodeInner("root");
            AssetDatabase.SaveAssets();
        }

        public SpeechNodeData CreateNode()
        {
            var dialogNode = CreateNodeInner($"node-{_speechNodes.Count}");
            _speechNodes.Add(dialogNode);

            return dialogNode;
        }

        public void DeleteNode(SpeechNodeData dialogNode)
        {
            _speechNodes.Remove(_rootNode);
            AssetDatabase.RemoveObjectFromAsset(dialogNode); 
            AssetDatabase.SaveAssets();
        }

        public void AddConnection(SpeechNodeData sourceNode, SpeechNodeData targetNode)
        {
            if (sourceNode is null || targetNode is null)
                return;
            sourceNode.AddConnection(targetNode);
            AssetDatabase.SaveAssets();
        }

        public void RemoveConnection(SpeechNodeData sourceNode, SpeechNodeData targetNode)
        {
            if (sourceNode is null || targetNode is null)
                return;
            sourceNode.RemoveConnection(targetNode);
            AssetDatabase.SaveAssets();
        }

        private SpeechNodeData CreateNodeInner(string name)
        {
            var dialogNode = ScriptableObject.CreateInstance<SpeechNodeData>();
            dialogNode.name = name;
            dialogNode.Initialize(GUID.Generate());

            AssetDatabase.AddObjectToAsset(dialogNode, this);
            AssetDatabase.SaveAssets();

            return dialogNode;
        }
#endif
    }   
}