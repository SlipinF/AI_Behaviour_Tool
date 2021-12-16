using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Graphing;
using UnityEditor.Experimental.GraphView;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using LoD.BT.Runtime;

namespace LoD.BT.Editor {
  public class LeafNode : BehaviourNode {
    public Type type;
    public Dictionary<string, Type> schema;
    public Dictionary<string, object> values;

    public LeafNode(NodeType nodeType, Type type, string name = "", Dictionary<string, object> values = null, EdgeConnectorListener edgeConnectorListener = null) : base(edgeConnectorListener) {
      _nodeType = nodeType;
      this.type = type;

      schema = new Dictionary<string, Type>();
      ConstructorInfo ctor = type.GetConstructors()[0];
      ParameterInfo[] pars = ctor.GetParameters();

      if(values != null) {
        this.values = values;
      } else {
        this.values = new Dictionary<string, object>();
        foreach(ParameterInfo p in pars) {
          this.values[p.Name] = TypeUtils.DefaultValueForType(p.ParameterType);
        }
      }

      foreach(ParameterInfo p in pars) {
        schema[p.Name] = p.ParameterType;

        object value;
        this.values.TryGetValue(p.Name, out value);

        BuildProperty(p.Name, p.ParameterType, value);
      }

      title = name;

      SetNodeHint();
      AddParentPort();
      RefreshExpandedState();
      RefreshPorts();
    }

    void BuildProperty(string name, Type type, object value) {
      MethodInfo method = typeof(LeafNode).GetMethod("BuildPropertyRow");
      MethodInfo genericMethod = method.MakeGenericMethod(type);
      genericMethod.Invoke(this, new object[] { name, value });
    }

    public void BuildPropertyRow<T>(string name, object value) {
      BaseField<T> field = TypeUtils.FieldForType<T>();

      if(value != null) {
        field.value = (T) value;
      }
      field.RegisterValueChangedCallback((ChangeEvent<T> evt) => {
        values[name] = evt.newValue;
      });

      Port port = GeneratePort(name, Direction.Input, typeof(T));
      inputPorts.Add(port);

      propertiesContainer.Add(new PropertyRow(name, field, port));
    }

    public override BehaviourNodeData ToNodeData() {
      return new BehaviourNodeData {
        GUID = GUID,
        nodePosition = GetPosition().position,
        nodeType = nodeType.ToString(),
        fullName = type.AssemblyQualifiedName,
        name = title,
        values = values,
      };
    }

    void SetNodeHint() {
      Attribute[] attrs = Attribute.GetCustomAttributes(type);

      foreach (Attribute attr in attrs) {
        if (attr is NodeHint) {
          NodeHint a = (NodeHint) attr;
          tooltip = a.hint;
        }
      }
    }
  }
}

