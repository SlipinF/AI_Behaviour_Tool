using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using LoD.BT.Extensions;

namespace LoD.BT.Editor {
  public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider {
    private BehaviourGraphWindow window;
    private BehaviourGraphView graphView;

    public EdgeConnectorListener edgeConnectorListener;
    public Edge activeEdge;
    public Port sourcePort;

    public void Init(BehaviourGraphWindow window, BehaviourGraphView graphView) {
      this.window = window;
      this.graphView = graphView;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {

      List<SearchTreeEntry> entries = new List<SearchTreeEntry>() {
        new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),

        Group("Composites"),
        Entry(ContentFor(typeof(Sequence)), NodeType.Composite, CompositeType.Sequence),
        Entry(ContentFor(typeof(Selector)), NodeType.Composite, CompositeType.Selector),
        Entry(ContentFor(typeof(Parallel)), NodeType.Composite, CompositeType.Parallel),
        Entry(ContentFor(typeof(Randomizer)), NodeType.Composite, CompositeType.Randomizer),

        Group("Decorators"),
        Entry(ContentFor(typeof(Inverter)), NodeType.Decorator, DecoratorType.Inverter),
        Entry(ContentFor(typeof(Repeater)), NodeType.Decorator, DecoratorType.Repeater),
        Entry(ContentFor(typeof(Succeeder)), NodeType.Decorator, DecoratorType.Succeeder),
        Entry(ContentFor(typeof(Once)), NodeType.Decorator, DecoratorType.Once),
      };

      entries.Add(Group("Conditions"));
      GetClassesByLeafType("Condition").ForEach(t => {
        entries.Add(Entry(ContentFor(t), NodeType.Condition, t));
      });

      entries.Add(Group("Actions"));
      GetClassesByLeafType("Action").ForEach(t => {
        entries.Add(Entry(ContentFor(t), NodeType.Action, t));
      });

      if(graphView.graphProperties.Any()) {
        entries.Add(Group("Properties"));
        graphView.graphProperties.ForEach(p => {
          entries.Add(Entry(new GUIContent(p.name), NodeType.Property, p));
        });
      }

      return entries;
    }

    SearchTreeGroupEntry Group(string groupName) {
      return new SearchTreeGroupEntry(new GUIContent(groupName), 1);
    }

    SearchTreeEntry Entry(GUIContent content, NodeType type, object userData = null) {
      return new SearchTreeEntry(content) {
        userData = new NodeSetup {
          type = type,
          userData = userData,
          name = content.text,
        },
        level = 2,
      };
    }

    GUIContent ContentFor(Type type) {
      GUIContent content = new GUIContent(type.GetFriendlyName());
      Attribute[] attrs = Attribute.GetCustomAttributes(type);

      foreach (Attribute attr in attrs) {
        if (attr is NodeHint) {
          NodeHint a = (NodeHint) attr;
          content.tooltip = a.hint;
        }
      }
      return content;
    }

    public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context) {

      Vector2 worldMousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);

      Vector2 localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);

      NodeSetup nodeSetup = (NodeSetup) entry.userData;
      BehaviourNode newNode = null;
      switch(nodeSetup.type) {
        case NodeType.Composite:
          newNode = new CompositeNode((CompositeType) nodeSetup.userData, "", edgeConnectorListener);
          break;

        case NodeType.Decorator:
          newNode = new DecoratorNode((DecoratorType) nodeSetup.userData, edgeConnectorListener);
          break;

        case NodeType.Condition:
        case NodeType.Action:
          newNode = new LeafNode(nodeSetup.type, (Type) nodeSetup.userData, nodeSetup.name, null, edgeConnectorListener);
          break;

        case NodeType.Property:
          newNode = new PropertyNode((GraphProperty) nodeSetup.userData, edgeConnectorListener);
          break;

        default:
          throw new Exception($"Unknown node type {nodeSetup.type}");
      }

      graphView.AddElementAt(newNode, localMousePosition);
      if(activeEdge != null) {
        activeEdge.output = sourcePort;
        activeEdge.input = newNode.GetInputPortByType(typeof(BehaviourNode));
        graphView.Connect(activeEdge);

        activeEdge = null;
        sourcePort = null;
      }

      return true;
    }

    List<Type> GetClassesByLeafType(string leafType) {
      return AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(t => t.GetTypes())
        .Where(t =>
          t.IsClass
          && t.Namespace == "LoD.BT"
          && LeafTypeIs(t, leafType))
        .Select(t => ExpandGenericTypes(t))
        .SelectMany(t => t)
        .ToList();
    }

    bool LeafTypeIs(Type type, string leafType) {
      Attribute[] attrs = Attribute.GetCustomAttributes(type);

      foreach (Attribute attr in attrs) {
        if (attr is LeafType) {
          LeafType a = (LeafType) attr;
          return a.type == leafType;
        }
      }

      return false;
    }

    List<Type> ExpandGenericTypes(Type type) {
      List<GenericType> genericTypes = GenericTypeAttributes(type);

      return genericTypes.Count > 0
        ? genericTypes.Select(t => type.MakeGenericType(new Type[]{ t.type })).ToList()
        : new List<Type> { type };
    }

    List<GenericType> GenericTypeAttributes(Type type) {
      Attribute[] attrs = Attribute.GetCustomAttributes(type);
      List<GenericType> genericTypes = new List<GenericType>();

      foreach (Attribute attr in attrs) {
        if (attr is GenericType) {
          genericTypes.Add(attr as GenericType);
        }
      }

      return genericTypes;
    }
  }

  internal class NodeSetup {
    public NodeType type;
    public object userData;
    public string name;
  }
}
