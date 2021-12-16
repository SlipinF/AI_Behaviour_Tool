using System;

namespace LoD.BT {
  [NodeHint(@"A decorator that wil start returning failure once the child node returned a success once.
  The child won't be evaluated after that.
  Reset do nothing as the purpose of this decorator is to ensure that the child node is only running once in the whole life of the tree.
  Typical use would be to trigger a custom attack/action once the actor's health drops below a given threshold.")]
  public class Once : Decorator {
    private bool childSucceededOnce = false;

    public Once(Node childNode) : base(childNode) {}

    public override NodeState Evaluate(Blackboard blackboard) {
      if(childSucceededOnce) {
        return NodeState.FAILURE;
      }

      NodeState childState = childNode.Evaluate(blackboard);
      if (childState == NodeState.SUCCESS) {
        childSucceededOnce = true;
      }
      return childState;
    }
  }
}
