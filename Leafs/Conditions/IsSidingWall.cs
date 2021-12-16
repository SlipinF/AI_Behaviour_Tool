using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
  [LeafType("Condition")]
  [NodeHint("Returns a success when this actor is colliding with a wall.")]
  public class IsSidingWall : Leaf {
    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
      CharacterController2D controller = (CharacterController2D)blackboard["charController"];
        return actor.CheckForObstacle(actor.viewDistance)
        ? NodeState.SUCCESS
        : NodeState.FAILURE;
    }
  }
}
