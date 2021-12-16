using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using UnityEditor;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine;

namespace LoD.BT.Editor {
  public sealed class BehaviourGraphView : GraphView {
    public static StyleSheet LoadStyleSheet(string text) {
      string path = string.Format(
        "{0}/Resources/uss/{1}.uss",
        "Assets/Scripts/BehaviourTree/Editor",
        text);
      return AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
    }

    public Vector2 ENTRY_POSITION = new Vector2(100, 100);
    public Vector2 DEFAULT_POSITION = new Vector2(200, 200);
    public Vector2 DEFAULT_SIZE = new Vector2(200, 100);

    public List<GraphProperty> graphProperties;

    private NodeSearchWindow searchWindowProvider;
    private BehaviourGraphWindow window;
    public EdgeConnectorListener edgeConnectorListener;

    public BehaviourGraphView(BehaviourGraphWindow window) {
      this.window = window;
      this.graphProperties = new List<GraphProperty>();
      this.graphViewChanged = OnGraphViewChanged;

      SetupZoom(0.125f, 8);

      styleSheets.Add(LoadStyleSheet("BehaviourGraphView"));
      styleSheets.Add(LoadStyleSheet("PropertyRow"));
      styleSheets.Add(LoadStyleSheet("Port"));
      styleSheets.Add(LoadStyleSheet("Blackboard"));
      styleSheets.Add(LoadStyleSheet("Nodes"));

      this.AddManipulator(new ContentDragger());
      this.AddManipulator(new SelectionDragger());
      this.AddManipulator(new RectangleSelector());

      AddSearchWindow();
      this.edgeConnectorListener = new EdgeConnectorListener(searchWindowProvider, window);
      searchWindowProvider.edgeConnectorListener = edgeConnectorListener;

      BehaviourNode entryNode = GenerateEntryPointNode();
      AddElement(entryNode);

      RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
      RegisterCallback<DragPerformEvent>(OnDragPerformEvent);
    }

    private BehaviourNode GenerateEntryPointNode() {
      RootNode node = new RootNode(edgeConnectorListener);

      node.SetPosition(new Rect(ENTRY_POSITION, DEFAULT_SIZE));

      return node;
    }

    public void AddElementAt(BehaviourNode element, Vector2 position) {
      element.SetPosition(new Rect(position, DEFAULT_SIZE));
      AddElement(element);
    }

    public void Connect(Edge edge) {
      edge.input.Connect(edge);
      edge.output.Connect(edge);
      AddElement(edge);

      if(edge.input.portType != typeof(BehaviourNode)) {
        PropertyBindedToPort(edge);
      }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
      List<Port> compatiblePorts = new List<Port>();
      ports.ForEach((port) => {
        if(startPort != port
           && startPort.node != port.node
           && startPort.portType == port.portType) {
          compatiblePorts.Add(port);
        }
      });
      return compatiblePorts;
    }

    public GraphProperty GetGraphPropertyByName(string name) {
      return graphProperties.Where(n => n.name == name).FirstOrDefault();
    }

    GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) {
      if(graphViewChange.elementsToRemove != null
         && graphViewChange.elementsToRemove.Any()) {
        List<Edge> removedEdges = graphViewChange.elementsToRemove.OfType<Edge>().ToList();

        removedEdges.ForEach(e => {
          if(e.input.portType != typeof(BehaviourNode)) {
            PropertyUnbindedFromPort(e);
          }
        });
      }

      return graphViewChange;
    }

    void PropertyBindedToPort(Edge edge) {
      PropertyRow row = edge.input.GetFirstAncestorOfType<PropertyRow>();
      PropertyNode node = (PropertyNode) edge.output.node;
      GraphProperty property = node.property;
      CopyPropertyValueToPort(property, edge.input);
      row.component.SetEnabled(false);
    }

    void PropertyUnbindedFromPort(Edge edge) {
      PropertyRow row = edge.input.GetFirstAncestorOfType<PropertyRow>();
      row.component.SetEnabled(true);
    }

    public void CopyPropertyValueToPort(GraphProperty property, Port port) {
      MethodInfo method = typeof(BehaviourGraphView).GetMethod("CopyTypedPropertyValueToPort");
      MethodInfo genericMethod = method.MakeGenericMethod(property.type);
      genericMethod.Invoke(this, new object[] { property, port });
    }

    public void CopyTypedPropertyValueToPort<T>(GraphProperty<T> property, Port port) {
      PropertyRow row = port.GetFirstAncestorOfType<PropertyRow>();
      BaseField<T> field = (BaseField<T>) row.component;
      field.value = property.defaultValue;
    }

    public void UpdateNodesBoundsToProperty(GraphProperty property) {
      List<PropertyNode> propertyNodes = nodes.ToList().OfType<PropertyNode>().Where(p => p.property == property).ToList();

      foreach(PropertyNode node in propertyNodes) {
        Edge[] connectedEdges = edges.ToList().Where(e => e.output.node == node).ToArray();
        foreach(Edge edge in connectedEdges) {
          Port port = edge.input;

          CopyPropertyValueToPort(property, port);
        }
      };
    }

    void AddSearchWindow() {
      searchWindowProvider = ScriptableObject.CreateInstance<NodeSearchWindow>();
      searchWindowProvider.Init(window, this);

      nodeCreationRequest = context => {
        // Nullify activeEdge and sourcePort that could have
        // been set in a previous cancelled operation
        searchWindowProvider.activeEdge = null;
        searchWindowProvider.sourcePort = null;
        SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindowProvider);
      };
    }

    void OnDragUpdatedEvent(DragUpdatedEvent e) {
      var selection = DragAndDrop.GetGenericData("DragSelection") as List<ISelectable>;
      bool dragging = false;
      if (selection != null) {
        // Blackboard
        if (selection.OfType<BlackboardFieldView>().Any()) {
          dragging = true;
        }
      }

      if (dragging) {
        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
      }
    }

    void OnDragPerformEvent(DragPerformEvent e) {
      Vector2 localPos = (e.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, e.localMousePosition);

      var selection = DragAndDrop.GetGenericData("DragSelection") as List<ISelectable>;
      if (selection != null) {
        // Blackboard
        if (selection.OfType<BlackboardFieldView>().Any()) {
          IEnumerable<BlackboardFieldView> fields = selection.OfType<BlackboardFieldView>();
          foreach (BlackboardFieldView field in fields) {
            AddElementAt(new PropertyNode(field.property, edgeConnectorListener), localPos);
            // CreateNode(field, localPos);
          }
        }
      }
    }
  }
}
