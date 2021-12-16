using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoD.BT {
  [NodeHint("A sequence composite will keep evaluate its children one by one until as long as they return a success.")]
  public class Sequence : Composite {
    private int currentNode = 0;

    public Sequence(string name, List<Node> nodes) : base(name, nodes) {}

    public override NodeState Evaluate(Blackboard blackboard) {
      NodeState nodeState = nodes[currentNode].Evaluate(blackboard);

      switch(nodeState) {
        case NodeState.SUCCESS:
          currentNode++;
          break;

        case NodeState.FAILURE:
          return NodeState.FAILURE;
      }

      if (currentNode >= nodes.Count)  {
        return NodeState.SUCCESS;
      } else if(nodeState == NodeState.SUCCESS) {
        return Evaluate(blackboard);
      }

      return NodeState.RUNNING;
    }

    public override void Reset() {
      base.Reset();
      currentNode = 0;
    }
  }
}
