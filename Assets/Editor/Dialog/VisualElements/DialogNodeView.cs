using Dialogs.Data;
using UnityEditor.Experimental.GraphView;

namespace Editor.Dialog.VisualElements
{
    internal sealed class DialogNodeView : UnityEditor.Experimental.GraphView.Node
    {
        private DialogNodeData _dialogNodeData;
        private Port _inputPort;
        private Port _outputPort;

        public DialogNodeData DialogNodeData => _dialogNodeData;

        public DialogNodeView(DialogNodeData dialogNodeData)
        {
            _dialogNodeData = dialogNodeData; 
            title = _dialogNodeData.Text;
            viewDataKey = _dialogNodeData.GUID.ToString();

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