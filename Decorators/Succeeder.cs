using UnityEngine;

namespace LoD.BT {
  [NodeHint("The succeeder decorator will keep returns a success despite its child evaluation resulting in a failure.")]
  public class Succeeder : Decorator {
    public Succeeder(Node childNode) : base(childNode) {}

    public override NodeState Evaluate(Blackboard blackboard) {
      NodeState state = childNode.Evaluate(blackboard);

      if (state == NodeState.RUNNING)
        return NodeState.RUNNING;

      return NodeState.SUCCESS;
    }
  }
}
