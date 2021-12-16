using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using LoD.BT.Runtime;

namespace LoD.BT.Editor {
  public class PropertyNode : BehaviourNode {
    public GraphProperty property;

    public PropertyNode(GraphProperty property, EdgeConnectorListener edgeConnectorListener = null) : base(edgeConnectorListener) {
      _nodeType = NodeType.Property;
      this.property = property;

      title = property.name;
      AddPort("Value", Direction.Output, property.value.GetType(), Port.Capacity.Multi, titleContainer);
      RefreshExpandedState();
      RefreshPorts();
    }

    public override BehaviourNodeData ToNodeData() {
      return new BehaviourNodeData {
        GUID = GUID,
        nodePosition = GetPosition().position,
        nodeType = nodeType.ToString(),
        name = property.name,
      };
    }
  }
}

