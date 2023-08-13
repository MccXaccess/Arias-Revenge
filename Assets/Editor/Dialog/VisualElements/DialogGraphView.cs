using Dialogs.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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

            for (int i = 0; i < conversation.DialogNodesData.Count; i++)
                CreateDialogNodeView(conversation.DialogNodesData[i]);
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            for (int i = 0; i < graphViewChange.elementsToRemove?.Count; i++)
            {
                var node = graphViewChange.elementsToRemove[i] as DialogNodeView;
                if (node is not null)
                    _conversation.DeleteNode(node.DialogNodeData);

                var edge = graphViewChange.elementsToRemove[i] as Edge;
                if (edge is not null)
                {
                    var outputView = edge.output.node as DialogNodeView;
                    var inputView = edge.input.node as DialogNodeView;
                    _conversation.RemoveConnection(outputView.DialogNodeData, inputView.DialogNodeData);
                }
            }

            for (int i = 0; i < graphViewChange.edgesToCreate?.Count; i++)
            {
                var edge = graphViewChange.edgesToCreate[i];
                var parentView = edge.output.node;
                var childView = edge.input.node;

                _conversation.AddConnection(parentView.userData as DialogNodeData, childView.userData as DialogNodeData);
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

        private void CreateDialogNodeView(DialogNodeData dialogNode)
        {
            var newNodeView = new DialogNodeView(dialogNode);
            AddElement(newNodeView);

            for (int i = 0; i < dialogNode.Children.Count; i++)
            {
                var child = dialogNode.Children[i];
                var childNodeView = GetNodeByGuid(child.GUID.ToString());

                newNodeView.outputContainer.Add(childNodeView.inputContainer[0]);
            }
        }
    }
}