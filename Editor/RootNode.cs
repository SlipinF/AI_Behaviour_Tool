using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace LoD.BT.Editor {
  public class RootNode : BehaviourNode {
    public RootNode(EdgeConnectorListener edgeConnectorListener) : base(edgeConnectorListener) {
      _nodeType = NodeType.Root;

      this.title = "Entry Point";
      capabilities &= ~Capabilities.Deletable;
      capabilities &= ~Capabilities.Movable;
      capabilities &= ~Capabilities.Selectable;
      AddPort("Root Node", Direction.Output, typeof(BehaviourNode), Port.Capacity.Single, titleContainer);
      RefreshExpandedState();
      RefreshPorts();
    }
  }
}

