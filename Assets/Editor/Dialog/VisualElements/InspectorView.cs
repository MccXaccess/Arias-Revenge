using UnityEngine.UIElements;

namespace Editor.Dialog.VisualElements
{
    public class InspectorView : VisualElement
    {
        private UnityEditor.Editor _editor;

        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        public InspectorView() { }

        internal void OnSpeechNodeSelected(SpeechNodeView speechNodeView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(_editor);
            _editor = UnityEditor.Editor.CreateEditor(speechNodeView.speechNodeData);
            IMGUIContainer container = new IMGUIContainer(() => _editor.OnInspectorGUI());
            Add(container);
        }

        internal void ClearView()
        {
            Clear();
        }
    } 
}