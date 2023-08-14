using Dialogs.Data;
using UnityEditor.Experimental.GraphView;

namespace Editor.Dialog.VisualElements
{
    internal sealed class SpeechNodeView : Node
    {
        private SpeechNodeData _speechNodeData;
        private Port _inputPort;
        private Port _outputPort;

        public SpeechNodeData speechNodeData => _speechNodeData;

        public SpeechNodeView(SpeechNodeData speechNodeData)
        {
            _speechNodeData = speechNodeData; 
            title = _speechNodeData.Text;
            viewDataKey = _speechNodeData.GUID.ToString();

            CreateInputPort();
            CreateOutputPort();
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