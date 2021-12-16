using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace LoD.BT.Editor {
  public class EdgeConnectorListener : IEdgeConnectorListener {
    NodeSearchWindow searchWindowProvider;
    BehaviourGraphWindow editorWindow;

    public EdgeConnectorListener(NodeSearchWindow searchWindowProvider, BehaviourGraphWindow editorWindow) {
      this.searchWindowProvider = searchWindowProvider;
      this.editorWindow = editorWindow;
    }

    public void OnDropOutsidePort(Edge edge, Vector2 position){
      if(edge.output.portType == typeof(BehaviourNode)) {
      searchWindowProvider.activeEdge = edge;
      searchWindowProvider.sourcePort = edge.output;
      SearchWindow.Open(new SearchWindowContext(position + editorWindow.position.position), searchWindowProvider);
      }
    }

    public void OnDrop(GraphView graphView, Edge edge){
      ((BehaviourGraphView) graphView).Connect(edge);
    }
  }
}
