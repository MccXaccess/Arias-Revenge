using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Dialog.VisualElements
{
    public class DialogGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogGraphView, GraphView.UxmlTraits> { }

        public DialogGraphView()
        {
            AddStyleSheets();

            Insert(0, new GridBackground());

            SetupGraphViewManipulators();
        }

        private void AddStyleSheets()
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Dialog/DialogGraphEditor.uss");
            styleSheets.Add(styleSheet);
        }

        private void SetupGraphViewManipulators()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }
    }
}