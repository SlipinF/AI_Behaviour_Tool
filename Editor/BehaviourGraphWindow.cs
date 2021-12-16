using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;
using System.IO;
using UnityEditor.VersionControl;

namespace LoD.BT.Editor {
  [Serializable]
  public class BehaviourGraphWindow : EditorWindow {
    public static BehaviourGraphWindow currentWindow;

    private string _fileName;

    [MenuItem("Window/Legend Of Data/Behaviour Tree", false, 3011)]
    public static void ShowWindow() {
      BehaviourGraphWindow window = GetWindow<BehaviourGraphWindow>();
      window.titleContent = new GUIContent(text: "Behaviour Tree");
    }

    public BehaviourGraphView graphView { get; private set; }
    public Toolbar toolbar { get; private set; }
    public BlackboardProvider blackboardProvider { get; private set; }

    void OnEnable() {
      GenerateGraphView();
      GenerateToolbar();
      GenerateBlackboard();
    }

    void OnDisable() {
      rootVisualElement.Remove(graphView);
      rootVisualElement.Remove(toolbar);
      graphView.Remove(blackboardProvider.blackboard);
    }

    void GenerateGraphView() {
      graphView = new BehaviourGraphView(this);
      graphView.StretchToParentSize();

      rootVisualElement.Add(graphView);
    }

    void GenerateToolbar() {
      toolbar = new Toolbar();

      Button saveButton = new Button(() => {
        if(string.IsNullOrEmpty(_fileName)) {
          string absFilePath = EditorUtility.SaveFilePanel("Save Asset", "Assets/Resources", "New Behaviour Tree", "asset");

          _fileName = GetRelativePath(absFilePath, Application.dataPath);
        }
        var saveUtility = BehaviourGraphSaveUtility.GetInstance(graphView, blackboardProvider);
        saveUtility.SaveGraph(_fileName);
      });
      saveButton.text = "Save Asset";
      toolbar.Add(saveButton);

      Button openButton = new Button(() => {
        string absFilePath = EditorUtility.OpenFilePanelWithFilters("Open Asset", "Assets/Resources", new string[] { "Asset files", "asset" });

        if(string.IsNullOrEmpty(absFilePath)) {
          return;
        }

        _fileName = GetRelativePath(absFilePath, Application.dataPath);

        var saveUtility = BehaviourGraphSaveUtility.GetInstance(graphView, blackboardProvider);
        saveUtility.LoadGraph(_fileName);
      });
      openButton.text = "Open Asset";
      toolbar.Add(openButton);

      rootVisualElement.Add(toolbar);
    }

    void GenerateBlackboard() {
      blackboardProvider = new BlackboardProvider(graphView);
      blackboardProvider.blackboard.SetPosition(new Rect(0,position.height - 300,200,300));
      graphView.Add(blackboardProvider.blackboard);
    }

    string GetRelativePath(string filespec, string folder) {
      Uri pathUri = new Uri(filespec);
      // Folders must end in a slash
      if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString())) {
        folder += Path.DirectorySeparatorChar;
      }
      Uri folderUri = new Uri(folder);
      return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
    }
  }
}
