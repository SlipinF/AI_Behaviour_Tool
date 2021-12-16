using UnityEngine;

namespace LoD.BT {
  [NodeHint("A repeater will keep evaluate its child whatever the result of its evaluation. Useful as an entry point in order to keep an evaluation loop running.")]
  public class Repeater : Decorator {
    public Repeater(Node childNode) : base(childNode) {}

    public override NodeState Evaluate(Blackboard blackboard) {
      NodeState state = childNode.Evaluate(blackboard);

      if (state != NodeState.RUNNING) {
        Reset();
      }
      return NodeState.SUCCESS;
    }
  }
}
