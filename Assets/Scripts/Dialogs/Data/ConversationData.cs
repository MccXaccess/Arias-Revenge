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

        public void Initialize()
        {
            _dialogNodesData = new List<DialogNodeData>();
            _rootNode = CreateNode(typeof(DialogNodeData));
            AssetDatabase.SaveAssets();
        }

        public DialogNodeData CreateNode(System.Type type)
        {
            var dialogNode = ScriptableObject.CreateInstance<DialogNodeData>();
            _dialogNodesData.Add(dialogNode);

            dialogNode.Initialize(System.Guid.NewGuid(), "New Dialog Node Text");

            AssetDatabase.AddObjectToAsset(dialogNode, this);
            AssetDatabase.SaveAssets();

            return dialogNode;
        }

        public void DeleteNode(DialogNodeData dialogNode)
        {
            _dialogNodesData.Remove(_rootNode);
            AssetDatabase.RemoveObjectFromAsset(dialogNode); 
            AssetDatabase.SaveAssets();
        }
    }   
}