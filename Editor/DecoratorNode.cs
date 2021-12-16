using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Graphing;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System;
using LoD.BT.Runtime;

namespace LoD.BT.Editor {
  public class DecoratorNode : BehaviourNode {
    public DecoratorType type;

    public DecoratorNode(DecoratorType type, EdgeConnectorListener edgeConnectorListener = null) : base(edgeConnectorListener) {
      _nodeType = NodeType.Decorator;
      this.type = type;

      EnumField compositeTypes = new EnumField(type);
      compositeTypes.RegisterValueChangedCallback(OnTypeChanged);
      propertiesContainer.Add(new PropertyRow("Type", compositeTypes));

      title = type.ToString();

      AddParentPort();
      AddPort("Child", Direction.Output, typeof(BehaviourNode));
      RefreshExpandedState();
      RefreshPorts();
      SetNodeHint();
    }

    void OnTypeChanged(ChangeEvent<Enum> evt) {
      type = (DecoratorType) evt.newValue;
      title = type.ToString();
      SetNodeHint();
    }

    public override BehaviourNodeData ToNodeData() {
      return new BehaviourNodeData {
        GUID = GUID,
        nodePosition = GetPosition().position,
        nodeType = NodeType.Decorator.ToString(),
        decoratorType = type.ToString(),
      };
    }
    void SetNodeHint() {
      Attribute[] attrs = Attribute.GetCustomAttributes(TypeUtils.GetDecoratorType(type.ToString()));

      foreach (Attribute attr in attrs) {
        if (attr is NodeHint) {
          NodeHint a = (NodeHint) attr;
          tooltip = a.hint;
        }
      }
    }
  }
}

