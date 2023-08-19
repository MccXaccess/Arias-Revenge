using Dialogs.Data;
using Editor.Dialog.VisualElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraphEditor : EditorWindow
{
    private DialogGraphView _dialogGraphView;
    private InspectorView _inspectorView;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Dialog Graph Editor/Editor...")]
    public static void ShowExample()
    {
        DialogGraphEditor wnd = GetWindow<DialogGraphEditor>();
        wnd.titleContent = new GUIContent("Dialog Graph Editor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML

        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        labelFromUXML.style.flexGrow = 1;
        labelFromUXML.style.flexShrink = 1;
        root.Add(labelFromUXML);


        // Add style sheet
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Dialog/DialogGraphEditor.uss");
        root.styleSheets.Add(styleSheet);

        _dialogGraphView = root.Q<DialogGraphView>();
        _inspectorView = root.Q<InspectorView>();

        _dialogGraphView.SetInspectorView(_inspectorView);
    }

    private void OnSelectionChange()
    {
        var conversation = Selection.activeObject as ConversationData;
        if (conversation is null)
        {
            _dialogGraphView.ClearView();
            _inspectorView.ClearView();
            return;
        }
        _dialogGraphView.PopulateView(conversation);
    }
}
