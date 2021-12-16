using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using LoD.BT.Runtime;

namespace LoD.BT.Editor {
  public class BehaviourGraphSaveUtility {
    public static BehaviourGraphSaveUtility GetInstance(BehaviourGraphView graphView, BlackboardProvider blackboardProvider) {
      return new BehaviourGraphSaveUtility {
        graphView = graphView,
        blackboardProvider = blackboardProvider,
      };
    }

    BehaviourGraphView graphView;
    BlackboardProvider blackboardProvider;
    BehaviourTree treeCache;
    List<Edge> edges => graphView.edges.ToList();
    List<BehaviourNode> nodes => graphView.nodes.ToList().Cast<BehaviourNode>().ToList();

    public void SaveGraph(string fileName) {
      if(!edges.Any()) { return; }

      BehaviourTree container = ScriptableObject.CreateInstance<BehaviourTree>();

      SaveNodes(container);
      SaveProperties(container);

      AssetDatabase.CreateAsset(container, $"Assets/{fileName}");
      AssetDatabase.SaveAssets();
    }

    void SaveNodes(BehaviourTree container) {
      container.rootGUID = nodes[0].GUID;

      Edge[] connectedEdges = edges.Where(e => e.input.node != null).ToArray();
      foreach(Edge edge in connectedEdges) {
        BehaviourNode output = edge.output.node as BehaviourNode;
        BehaviourNode input = edge.input.node as BehaviourNode;

        container.nodeLinksData.Add(new BehaviourNodeLinkData {
          outputGUID = output.GUID,
          inputGUID = input.GUID,
          outputPortName = edge.output.portName,
          inputPortName = edge.input.portName,
        });
      }

      foreach(BehaviourNode node in nodes.Where(n => !n.IsRoot())) {
        container.nodesData.Add(node.ToNodeData());
      }
    }

    void SaveProperties(BehaviourTree container) {
      container.graphProperties = graphView.graphProperties.Select(p => new SerializedGraphProperty(p.name, new TypeData(p.value))).ToList();
    }

    public void LoadGraph(string fileName) {
      treeCache = (BehaviourTree)AssetDatabase.LoadAssetAtPath($"Assets/{fileName}", typeof(BehaviourTree));

      if(treeCache == null) {
        EditorUtility.DisplayDialog("File Not Found", "Target behaviour graph does not exist!", "Ok");
      }

      ClearProperties();
      ClearGraph();

      LoadProperties();
      CreateNodes();
      ConnectNodes();
    }

    void ClearProperties() {
      graphView.graphProperties.Clear();
      blackboardProvider.blackboard.Clear();
    }

    void ClearGraph() {
      nodes.Find(n => n.IsRoot()).GUID = treeCache.nodeLinksData[0].outputGUID;

      foreach(BehaviourNode node in nodes) {
        if(node.IsRoot()) { continue; }

        edges.Where(e => e.input.node == node).ToList().ForEach(e => graphView.RemoveElement(e));

        graphView.RemoveElement(node);
      }
    }

    void LoadProperties() {
      Type type = typeof(GraphProperty<>);

      List<GraphProperty> properties = treeCache.graphProperties.Select(p => {
        Type generic = type.MakeGenericType(new Type[]{ p.value.type });
        ConstructorInfo ctor = generic.GetConstructors()[0];
        return (GraphProperty) ctor.Invoke(new object[] { p.value.value, p.name });
      }).ToList();

      properties.ForEach(p => {
        blackboardProvider.AddInputRow(p);
      });
    }

    void CreateNodes() {
      nodes[0].GUID = treeCache.rootGUID;
      foreach(BehaviourNodeData nodeData in treeCache.nodesData) {
        BehaviourNode tempNode = CreateNode(nodeData);
        graphView.AddElement(tempNode);

        if(nodeData.nodeType == "Composite") {
          CompositeNode compositeNode = (CompositeNode) tempNode;
          for(int i = 1; i < nodeData.portCount; i++) {
            compositeNode.AddElementPort();
          }
        }
      }
    }

    BehaviourNode CreateNode(BehaviourNodeData nodeData) {
      BehaviourNode tempNode = new BehaviourNode();

      switch(nodeData.nodeType) {
        case "Decorator":
          DecoratorType decoratorType = (DecoratorType)Enum.Parse(typeof(DecoratorType), nodeData.decoratorType);
          tempNode = new DecoratorNode(decoratorType, graphView.edgeConnectorListener);
          break;

        case "Composite":
          CompositeType compositeType = (CompositeType)Enum.Parse(typeof(CompositeType), nodeData.compositeType);
          tempNode = new CompositeNode(compositeType, nodeData.name, graphView.edgeConnectorListener);
          break;

        case "Condition":
        case "Action":
          NodeType nodeType = (NodeType)Enum.Parse(typeof(NodeType), (string)nodeData.nodeType);

          tempNode = new LeafNode(
            nodeType,
            Type.GetType(nodeData.fullName),
            nodeData.name,
            nodeData.values,
            graphView.edgeConnectorListener);
          break;

        case "Property":
          GraphProperty property = graphView.GetGraphPropertyByName(nodeData.name);
          tempNode = new PropertyNode(property, graphView.edgeConnectorListener);
          break;

        default:
          throw new Exception($"Unknown node type {nodeData.nodeType}");
      }

      tempNode.GUID = nodeData.GUID;
      tempNode.SetPosition(new Rect(nodeData.nodePosition, graphView.DEFAULT_SIZE));

      return tempNode;
    }

    void ConnectNodes() {
      foreach(BehaviourNode node in nodes) {
        List<BehaviourNodeLinkData> connections = treeCache.nodeLinksData.Where(e => e.outputGUID == node.GUID).ToList();

        foreach(BehaviourNodeLinkData linkData in connections) {
          BehaviourNode targetNode = nodes.First(n => n.GUID == linkData.inputGUID);

          LinkNodes(node.GetOutputPortByName(linkData.outputPortName), targetNode.GetInputPortByName(linkData.inputPortName));
        }
      }
    }

    void LinkNodes(Port output, Port input) {
      Edge edge = new Edge(){
        output = output,
        input = input,
      };
      graphView.Connect(edge);
    }
  }
}

