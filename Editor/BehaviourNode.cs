using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;
using GraphNode = UnityEditor.Experimental.GraphView.Node;
using System.IO;
using UnityEditor.VersionControl;
using LoD.BT.Runtime;

namespace LoD.BT.Editor {
  public class BehaviourNode : GraphNode {
    public string GUID;

    protected NodeType _nodeType;

    public NodeType nodeType { get { return _nodeType; } }

    public VisualElement propertiesContainer;
    public List<Port> inputPorts = new List<Port>();
    public List<Port> outputPorts = new List<Port>();

    public EdgeConnectorListener edgeConnectorListener;

    public BehaviourNode(EdgeConnectorListener edgeConnectorListener = null) {
      GUID = Guid.NewGuid().ToString();
      this.edgeConnectorListener = edgeConnectorListener;

      propertiesContainer = new VisualElement() {
        name = "propertyContainer"
      };

      mainContainer.Add(propertiesContainer);
      propertiesContainer.PlaceInFront(titleContainer);
    }

    public bool IsRoot() {
      return _nodeType == NodeType.Root;
    }

    public Port GetOutputPortByName(string name) {
      return outputPorts.Where(p => p.portName == name).FirstOrDefault();
    }

    public Port GetOutputPortByType(Type type) {
      return outputPorts.Where(p => p.portType == type).FirstOrDefault();
    }

    public Port GetInputPortByName(string name) {
      return inputPorts.Where(p => p.portName == name).FirstOrDefault();
    }

    public Port GetInputPortByType(Type type) {
      return inputPorts.Where(p => p.portType == type).FirstOrDefault();
    }

    public Port GeneratePort(string portName, Direction portDirection, Type type = null, Port.Capacity capacity = Port.Capacity.Single) {
      if(type == null) {
        type = typeof(float);
      }

      Port port = CustomPort.Create(Orientation.Horizontal, portDirection, capacity, type, edgeConnectorListener);

      port.portName = portName;
      port.visualClass = $"type{type.Name}";

      return port;
    }

    public Port AddParentPort() {
      Port port = AddPort("Parent", Direction.Input, typeof(BehaviourNode));
      return port;
    }

    public Port AddPort(string portName, Direction portDirection, Type type = null, Port.Capacity capacity = Port.Capacity.Single, VisualElement container = null) {

      Port port = GeneratePort(portName, portDirection, type, capacity);

      if(portDirection == Direction.Input) {
        inputPorts.Add(port);
        (container != null ? container : inputContainer).Add(port);
      } else {
        outputPorts.Add(port);
        (container != null ? container : outputContainer).Add(port);
      }

      return port;
    }

    public virtual BehaviourNodeData ToNodeData() {
      return new BehaviourNodeData {
        GUID = GUID,
        nodePosition = GetPosition().position,
      };
    }
  }
}
