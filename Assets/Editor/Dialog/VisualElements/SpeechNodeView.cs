using Dialogs.Data;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Dialog.VisualElements
{
    internal class SpeechNodeView : Node
    {
        public Action<SpeechNodeView> onSelect;

        private SpeechNodeData _speechNodeData;
        protected Port _inputPort;
        protected Port _outputPort;

        public Port InputPort => _inputPort;
        public Port OutputPort => _outputPort;

        public SpeechNodeData speechNodeData => _speechNodeData;

        public SpeechNodeView(SpeechNodeData speechNodeData) : base("Assets/Editor/Dialog/DialogNodeView.uxml")
        {
            _speechNodeData = speechNodeData; 
            title = _speechNodeData.NodeTitle;
            viewDataKey = _speechNodeData.GUID.ToString();
            
            style.left = _speechNodeData.NodeViewPosition.x;
            style.top = _speechNodeData.NodeViewPosition.y;

            CreateGraphNodePorts();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            _speechNodeData.SetNodePosition(newPos.position);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            onSelect?.Invoke(this);
        }
        
        protected virtual void CreateGraphNodePorts()
        {
            _inputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(float));
            _inputPort.portName = "";
            _inputPort.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(_inputPort);

            _outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(float));
            _outputPort.portName = "";
            _outputPort.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(_outputPort);
        }
    }
}