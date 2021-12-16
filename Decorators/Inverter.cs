using System;

namespace LoD.BT {
  [NodeHint("An inverter node returns the inverse of its child evaluation results. A success becomes a failure and a failure becomes a success. A running is returned unchanged.")]
  public class Inverter : Decorator {
    public Inverter(Node childNode) : base(childNode) {}

    public override NodeState Evaluate(Blackboard blackboard) {
      NodeState childState = childNode.Evaluate(blackboard);
      if (childState == NodeState.FAILURE) {
        return NodeState.SUCCESS;
      } else if (childState == NodeState.SUCCESS) {
        return NodeState.FAILURE;
      } else {
        return childState;
      }
    }
  }
}
