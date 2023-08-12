using Dialogs.Data;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Dialog.VisualElements
{
    public class DialogGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogGraphView, GraphView.UxmlTraits> { }

        public GridBackground GridBackground { get; private set; }

        private ConversationData _conversation;

        public DialogGraphView()
        {
            AddStyleSheets();

            SetupGraphViewManipulators();

            GridBackground = new GridBackground();
            GridBackground.StretchToParentWidth();
            Insert(0, GridBackground);
        }

        private void AddStyleSheets()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Dialog/DialogGraphEditor.uss");
            styleSheets.Add(styleSheet);
        }

        private void SetupGraphViewManipulators()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        internal void PopulateView(ConversationData conversation)
        {
            _conversation = conversation;
            DeleteElements(graphElements);

            if (_conversation.RootNode == null)
                _conversation.Initialize();

            ViewDialogNode(_conversation.RootNode);

            for (int i = 0; i < conversation.DialogNodesData.Count; i++)
                ViewDialogNode(conversation.DialogNodesData[i]);
        }

        private void ViewDialogNode(DialogNodeData dialogNode)
        {
            var newNode = new DialogNodeView(dialogNode);
            AddElement(newNode);
        }
    }
}