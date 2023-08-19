using Dialogs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
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

            CreateDialogViewNodes(conversation);
            CreateDialogNodeEdges(conversation);
        }

        internal void ClearView()
        {
            _conversation = null;
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            // Remove Nodes
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

            // Create node edges
            for (int i = 0; i < graphViewChange.edgesToCreate?.Count; i++)
            {
                var edge = graphViewChange.edgesToCreate[i];
                var parentView = edge.output.node as SpeechNodeView;
                var childView = edge.input.node as SpeechNodeView;
                _conversation.AddConnection(parentView.speechNodeData, childView.speechNodeData);
            }
            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Add Dialog Node", (a) => AddDialogNode());
            evt.menu.AppendAction("Focus Root Node", (a) => FocusRootNode());
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

        private void FocusRootNode()
        {
            GridBackground.viewDataKey = _conversation.RootNode.GUID.ToString();
        }

        private void CreateDialogViewNodes(ConversationData conversation)
        {
            CreateDialogNodeView(conversation.RootNode);
            for (int i = 0; i < conversation.SpeechNodes.Count; i++)
                CreateDialogNodeView(conversation.SpeechNodes[i]);
        }

        private void CreateDialogNodeView(SpeechNodeData speechNodeData)
        {
            var newNodeView = new SpeechNodeView(speechNodeData);
            AddElement(newNodeView);
        }

        private void CreateDialogNodeEdges(ConversationData conversation)
        {
            CreateDialogNodeEdge(conversation.RootNode);

            for (int i = 0; i < conversation.SpeechNodes.Count; i++)
            {
                var speechNodeData = conversation.SpeechNodes[i];
                CreateDialogNodeEdge(speechNodeData);
            }
        }

        private void CreateDialogNodeEdge(SpeechNodeData speechNodeData)
        {
            var parentNodeView = GetNodeByGuid(speechNodeData.GUID.ToString()) as SpeechNodeView;

            for (int i = 0; i < speechNodeData.Connections?.Count; i++)
            {
                var child = speechNodeData.Connections[i];
                var childNodeView = GetNodeByGuid(child.GUID.ToString()) as SpeechNodeView;
                var newEdge = parentNodeView.OutputPort.ConnectTo(childNodeView.InputPort);
                AddElement(newEdge);
            }
        }
    }
}