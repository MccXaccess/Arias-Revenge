using Dialogs.Data;
using UnityEditor.Experimental.GraphView;

namespace Editor.Dialog.VisualElements
{
    internal sealed class RootSpeechNodeView : SpeechNodeView
    {
        public RootSpeechNodeView(SpeechNodeData speechNodeData) : base(speechNodeData)
        {
        }

        protected override void CreateGraphNodePorts()
        {
            _outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            _outputPort.portName = "Output";
            outputContainer.Add(_outputPort);
        }
    }
}