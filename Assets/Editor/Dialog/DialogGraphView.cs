using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Dialog
{
    public class DialogGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogGraphView, GraphView.UxmlTraits> { }

        public DialogGraphView()
        {
            

        }
    }
}