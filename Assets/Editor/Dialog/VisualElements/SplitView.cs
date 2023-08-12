using UnityEngine.UIElements;

namespace Editor.Dialog.VisualElements
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits>{ };

        public SplitView()
        {
                
        }
    }
}