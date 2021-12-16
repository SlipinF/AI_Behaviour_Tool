using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace LoD.BT.Runtime
{
    [Serializable]
    public class BehaviourTree : ScriptableObject
    {
        public string rootGUID;
        public List<BehaviourNodeData> nodesData = new List<BehaviourNodeData>();
        public List<BehaviourNodeLinkData> nodeLinksData = new List<BehaviourNodeLinkData>();
        public List<SerializedGraphProperty> graphProperties;

        public Node GetTree()
        {
            BehaviourNodeLinkData rootLink = nodeLinksData.Where(e => e.outputGUID == rootGUID).First();
            BehaviourNodeData root = nodesData.Where(n => n.GUID == rootLink.inputGUID).First();
            return InstantiateNode(root);
        }
        public Node InstantiateNode(BehaviourNodeData nodeData)
        {
            Type type;
            ConstructorInfo ctor;
            switch (nodeData.nodeType)
            {
                case "Decorator":
                    BehaviourNodeLinkData childConnection = nodeLinksData.Where(e => e.outputGUID == nodeData.GUID).First();
                    Node child = nodesData.Where(n => n.GUID == childConnection.inputGUID).Select(n => InstantiateNode(n)).First();
                    type = TypeUtils.GetDecoratorType(nodeData.decoratorType);
                    ctor = type.GetConstructors()[0];
                    return (Node)ctor.Invoke(new object[] { child });
                case "Composite":
                    List<BehaviourNodeLinkData> connections = nodeLinksData.Where(e => e.outputGUID == nodeData.GUID).ToList();
                    List<Node> children = connections
                      .Select(l => nodesData.Where(n => n.GUID == l.inputGUID).First())
                      .Select(n => InstantiateNode(n))
                      .ToList();

                    type = TypeUtils.GetCompositeType(nodeData.compositeType);
                    ctor = type.GetConstructors()[0];
                    return (Node)ctor.Invoke(new object[] { nodeData.name, children });
                case "Action":
                case "Condition":
                    type = Type.GetType(nodeData.fullName);
                    ctor = type.GetConstructors()[0];
                    return (Node)ctor.Invoke(nodeData.values.Values.ToArray());
                default:
                    throw new Exception($"Unknown node type {nodeData.nodeType}");
            }
        }
    }
}
