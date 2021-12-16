using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoD.BT {
  [NodeHint("A selector composite will keep evaluate its children one by one until one returns a success.")]
  public class Selector : Composite {
    private int currentNode = 0;

    public Selector(string name, List<Node> nodes) : base(name, nodes) {}

    public override NodeState Evaluate(Blackboard blackboard) {
      if(currentNode >= nodes.Count) {
        return NodeState.FAILURE;
      }

      NodeState state = nodes[currentNode].Evaluate(blackboard);

      switch (state) {
        case NodeState.FAILURE:
          currentNode++;
          return Evaluate(blackboard);

        case NodeState.SUCCESS:
          return NodeState.SUCCESS;
      }
      return NodeState.RUNNING;
    }

    public override void Reset() {
      base.Reset();
      currentNode = 0;
    }
  }
}
