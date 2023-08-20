using Dialogs.Data;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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

        public SpeechNodeView(SpeechNodeData speechNodeData)
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
            _outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            _outputPort.portName = "Output";
            outputContainer.Add(_outputPort);

            _inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            _inputPort.portName = "Input";
            inputContainer.Add(_inputPort);
        }
    }
}