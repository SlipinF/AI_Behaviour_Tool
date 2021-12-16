using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Graphing;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System;
using System.Collections.Generic;
using LoD.BT.Runtime;

namespace LoD.BT.Editor {
  public class CompositeNode : BehaviourNode {
    public CompositeType type;
    public string nodeName;
    public List<Port> elementPorts;

    public CompositeNode(CompositeType type, string nodeName = "", EdgeConnectorListener edgeConnectorListener = null) : base(edgeConnectorListener) {
      _nodeType = NodeType.Composite;
      this.type = type;
      this.nodeName = nodeName;
      this.elementPorts = new List<Port>();

      TextField nodeNameField = new TextField();
      nodeNameField.value = nodeName;
      nodeNameField.RegisterValueChangedCallback(OnNameChanged);
      propertiesContainer.Add(new PropertyRow("Name", nodeNameField));

      EnumField compositeTypes = new EnumField(type);
      compositeTypes.RegisterValueChangedCallback(OnTypeChanged);
      propertiesContainer.Add(new PropertyRow("Type", compositeTypes));

      Button addPort = new Button(() => AddElementPort());
      addPort.text = "Add Child";
      propertiesContainer.Add(addPort);

      UpdateTitle();
      AddParentPort();
      AddElementPort();
      RefreshPorts();
      RefreshExpandedState();
      SetNodeHint();
    }

    public void AddElementPort() {
      string portName = $"Element {elementPorts.Count}";
      Port port = AddPort(portName, Direction.Output, typeof(BehaviourNode));
      Button deleteButton = new Button(() => RemoveElementPort(port));
      deleteButton.text = "X";
      port.contentContainer.Add(deleteButton);
      elementPorts.Add(port);
    }

    public void RemoveElementPort(Port port) {
      List<Edge> connections = port.connections.ToList();
      BehaviourGraphView graphView = port.GetFirstAncestorOfType<BehaviourGraphView>();
      foreach(Edge e in connections) {
        port.Disconnect(e);
        e.input.Disconnect(e);
        graphView.RemoveElement(e);
      }
      elementPorts.Remove(port);
      outputContainer.Remove(port);
      RefreshPorts();
      RefreshExpandedState();
      RenameRemainingPorts();
    }

    void RenameRemainingPorts() {
      int i = 0;
      elementPorts.ForEach(p => p.portName = $"Element {i++}");
    }

    void OnTypeChanged(ChangeEvent<Enum> evt) {
      type = (CompositeType) evt.newValue;
      UpdateTitle();
      SetNodeHint();
    }

    void OnNameChanged(ChangeEvent<string> evt) {
      nodeName = evt.newValue;
      UpdateTitle();
    }

    void UpdateTitle() {
      title = string.IsNullOrEmpty(nodeName)
        ? $"{type} Composite"
        : nodeName;
    }

    public override BehaviourNodeData ToNodeData() {
      return new BehaviourNodeData {
        GUID = GUID,
        nodePosition = GetPosition().position,
        nodeType = NodeType.Composite.ToString(),
        compositeType = type.ToString(),
        portCount = elementPorts.Count,
        name = nodeName,
      };
    }
    void SetNodeHint() {
      Attribute[] attrs = Attribute.GetCustomAttributes(TypeUtils.GetCompositeType(type.ToString()));

      foreach (Attribute attr in attrs) {
        if (attr is NodeHint) {
          NodeHint a = (NodeHint) attr;
          tooltip = a.hint;
        }
      }
    }
  }
}

