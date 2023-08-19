using Dialogs.Data;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Dialog.VisualElements
{
    internal sealed class SpeechNodeView : Node
    {
        public Action<SpeechNodeView> onSelect;

        private SpeechNodeData _speechNodeData;
        private Port _inputPort;
        private Port _outputPort;

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

            //_speechNodeData.SetNodePosition()
            if(_speechNodeData.IsRootNode is false)
                CreateInputPort();
            CreateOutputPort();
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

        private void CreateInputPort()
        {
            _inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            _inputPort.portName = "Input";
            inputContainer.Add(_inputPort);
        }

        private void CreateOutputPort()
        {
            _outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            _outputPort.portName = "Output";
            outputContainer.Add(_outputPort);
        }
    }
}