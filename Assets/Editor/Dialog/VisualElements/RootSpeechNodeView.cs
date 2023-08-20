using Dialogs.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Dialog.VisualElements
{
    internal sealed class RootSpeechNodeView : SpeechNodeView
    {
        public RootSpeechNodeView(SpeechNodeData speechNodeData) : base(speechNodeData)
        {
        }

        protected override void CreateGraphNodePorts()
        {
            _outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(float));
            _outputPort.portName = "";
            _outputPort.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(_outputPort);
        }
    }
}