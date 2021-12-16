using GraphBlackboard = UnityEditor.Experimental.GraphView.Blackboard;
using LoD.BT.Extensions;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace LoD.BT.Editor {
  public class BlackboardProvider {
    public GraphBlackboard blackboard;
    private BehaviourGraphView graphView;
    private Dictionary<GraphProperty, BlackboardRow> inputRows;

    public BlackboardProvider(BehaviourGraphView graphView) {
      this.graphView = graphView;
      inputRows = new Dictionary<GraphProperty, BlackboardRow>();

      blackboard = new GraphBlackboard();
      blackboard.subTitle = "Graph Blackboard";
      blackboard.Add(new BlackboardSection() { title = "Properties" });
      blackboard.addItemRequested = AddItemRequested;
      blackboard.editTextRequested = EditTextRequested;
    }

    void AddItemRequested(GraphBlackboard blackboard) {
      var gm = new GenericMenu();
      AddPropertyItems(gm);
      gm.ShowAsContext();
    }

    void EditTextRequested(GraphBlackboard blackboard, VisualElement element, string newName) {
      BlackboardFieldView field = (BlackboardFieldView) element;
      string oldName = field.name;

      if(graphView.graphProperties.Any(p => p.name == newName)) {
        EditorUtility.DisplayDialog("Error", "This property name already exists, please chose a different one!", "Ok");
        return;
      }

      field.property.name = newName;
      field.text = newName;
    }

    void AddPropertyItems(GenericMenu gm) {
      gm.AddItem(new GUIContent("bool"), false, () => AddInputRow(new GraphProperty<bool>(false), true));
      gm.AddItem(new GUIContent("float"), false, () => AddInputRow(new GraphProperty<float>(0), true));
      gm.AddItem(new GUIContent("int"), false, () => AddInputRow(new GraphProperty<int>(0), true));
      gm.AddItem(new GUIContent("string"), false, () => AddInputRow(new GraphProperty<string>(""), true));
      gm.AddItem(new GUIContent("Vector2"), false, () => AddInputRow(new GraphProperty<Vector2>(new Vector2()), true));
      gm.AddItem(new GUIContent("Vector3"), false, () => AddInputRow(new GraphProperty<Vector3>(new Vector3()), true));
      gm.AddItem(new GUIContent("AnimationCurve"), false, () => AddInputRow(new GraphProperty<AnimationCurve>(null), true));
    }

    public void AddInputRow(GraphProperty input, bool create = false, int index = -1) {
      if (inputRows.ContainsKey(input)) { return; }

      string localInputName = input.name;
      while(graphView.graphProperties.Any(i => i.name == localInputName)) {
        localInputName = $"{localInputName}(1)";
      };

      input.name = localInputName;

      BlackboardFieldView field = new BlackboardFieldView(input, graphView, null, input.name, input.type.GetFriendlyName());
      BlackboardRow row = new BlackboardRow(field, field.defaultValueView);

      blackboard.Add(row);
      graphView.graphProperties.Add(input);
      // field.RegisterCallback<AttachToPanelEvent>(UpdateSelectionAfterUndoRedo);
    }
  }
}
