using UnityEngine;

namespace LoD.BT {
  public class Leaf : Node {

    public override NodeState Evaluate(Blackboard blackboard) {
      return NodeState.FAILURE;
    }

    public override void Reset() {}
  }
}
