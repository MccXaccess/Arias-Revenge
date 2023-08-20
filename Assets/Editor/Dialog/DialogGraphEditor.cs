using Dialogs.Data;
using Editor.Dialog.VisualElements;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraphEditor : EditorWindow
{
    private DialogGraphView _dialogGraphView;
    private InspectorView _inspectorView;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Dialog Graph Editor/Editor...")]
    public static void ShowDialogEditor()
    {
        DialogGraphEditor dialogeditor = GetWindow<DialogGraphEditor>();
        dialogeditor.titleContent = new GUIContent("Dialog Graph Editor");
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
        switch (Selection.activeObject)
        {
            case ConversationData conversation:
                _dialogGraphView.PopulateView(conversation);
                break;
            case SpeechNodeData speechNode:
                break;
            default:
                _dialogGraphView.ClearView();
                _inspectorView.ClearView();
                break;
        }
    }

    private void LoadFromData(ConversationData conversation)
    {
        _dialogGraphView.PopulateView(conversation);
    }

    [OnOpenAsset]
    private static bool OnOpenAsset(int instanceID, int line)
    {
        string assetPath = AssetDatabase.GetAssetPath(instanceID);
        var conversation = AssetDatabase.LoadAssetAtPath<ConversationData>(assetPath);

        if(conversation is not null)
        {
            DialogGraphEditor dialogEditor = GetWindow<DialogGraphEditor>();
            dialogEditor.titleContent = new GUIContent("Dialog Graph Editor");
            dialogEditor.LoadFromData(conversation);
            return true;
        }

        return false;
    }
}
