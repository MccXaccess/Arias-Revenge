using Dialogs.Data;

namespace Editor.Dialog.VisualElements
{
    internal sealed class DialogNodeView : UnityEditor.Experimental.GraphView.Node
    {
        private DialogNodeData _dialogNodeData;

        public DialogNodeView(DialogNodeData dialogNodeData)
        {
            _dialogNodeData = dialogNodeData; 
            title = _dialogNodeData.Text;
        }
    }
}