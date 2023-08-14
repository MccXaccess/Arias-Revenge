using Dialogs.Data;
using System.Collections.Generic;
using System.Linq;
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
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (_conversation.RootNode == null)
                _conversation.Initialize();

            CreateDialogNodeView(_conversation.RootNode);

            for (int i = 0; i < conversation.SpeechNodes.Count; i++)
                CreateDialogNodeView(conversation.SpeechNodes[i]);
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            for (int i = 0; i < graphViewChange.elementsToRemove?.Count; i++)
            {
                var node = graphViewChange.elementsToRemove[i] as SpeechNodeView;
                if (node is not null)
                    _conversation.DeleteNode(node.speechNodeData);

                var edge = graphViewChange.elementsToRemove[i] as Edge;
                if (edge is not null)
                {
                    var outputView = edge.output.node as SpeechNodeView;
                    var inputView = edge.input.node as SpeechNodeView;
                    _conversation.RemoveConnection(outputView.speechNodeData, inputView.speechNodeData);
                }
            }

            for (int i = 0; i < graphViewChange.edgesToCreate?.Count; i++)
            {
                var edge = graphViewChange.edgesToCreate[i];
                var parentView = edge.output.node.userData as SpeechNodeData;
                var childView = edge.input.node.userData as SpeechNodeData;

                _conversation.AddConnection(parentView, childView);
            }
            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Add Dialog Node", (a) => AddDialogNode());
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports
                .ToList()
                .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node)
                .ToList();
        }

        private void AddDialogNode()
        {
            var dialogNode = _conversation.CreateNode();
            CreateDialogNodeView(dialogNode);
        }

        private void CreateDialogNodeView(SpeechNodeData speechNodeData)
        {
            var newNodeView = new SpeechNodeView(speechNodeData);
            AddElement(newNodeView);

            for (int i = 0; i < speechNodeData.Connections.Count; i++)
            {
                var child = speechNodeData.Connections[i];
                var childNodeView = GetNodeByGuid(child.GUID.ToString());

                newNodeView.outputContainer.Add(childNodeView.inputContainer[0]);
            }
        }
    }
}