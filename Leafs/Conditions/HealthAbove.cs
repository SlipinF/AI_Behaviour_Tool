using UnityEngine;

namespace LoD.BT {
  [LeafType("Condition")]
  [NodeHint("Compare this actor's health with the defined value using the > operator.")]
  public class HealthAbove : Leaf {
    private int threshold;

    public HealthAbove(int threshold = 0) {
      this.threshold = threshold;
    }
    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyActor actor = (EnemyActor) blackboard["actor"];
      Vulnerable vulnerable = actor.Vulnerable();

      return vulnerable.Health() > threshold
        ? NodeState.SUCCESS
        : NodeState.FAILURE;
    }
  }
}
