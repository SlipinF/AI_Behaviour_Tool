using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace LoD.BT {
  [NodeHint(@"A parallel composite runs all its children at the same time and will only return a success if all the children returned a success.
  It will considered running as long as no failures where returned by a child or while some children are still running.
  It can be used to run a sub tree as long as as a condition is true, leading to an interuption if the condition failed.")]
  public class Parallel : Composite {
    private List<Node> pendingNodes;

    public Parallel(string name, List<Node> nodes) : base(name, nodes) {
      pendingNodes = new List<Node>(nodes);
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      List<Node> runningNodes = new List<Node>();
      bool hadFailure = false;

      foreach (Node node in pendingNodes) {
        NodeState childState = node.Evaluate(blackboard);
        switch (childState) {
          case NodeState.FAILURE:
            hadFailure = true;
            break;
          case NodeState.RUNNING:
            runningNodes.Add(node);
            break;
        }
      }

      if (hadFailure) {
        return NodeState.FAILURE;
      } else if (runningNodes.Count == 0) {
        return NodeState.SUCCESS;
      } else {
        pendingNodes = runningNodes;
        return NodeState.RUNNING;
      }
    }

    public override void Reset() {
      base.Reset();
      pendingNodes = new List<Node>(nodes);
    }
  }
}


