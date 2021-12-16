using UnityEngine;

namespace LoD.BT {
  [LeafType("Condition")]
  [NodeHint("Returns a success when the player is currently grounded.")]
  public class PlayerIsGrounded : Leaf {
    public PlayerIsGrounded() {}

    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyActor actor = (EnemyActor) blackboard["actor"];
      PlayerActor player = (PlayerActor) blackboard["player"];
      PhysicState playerPhysics = player.GetComponent<PhysicState>();

      return playerPhysics.IsGrounded()
        ? NodeState.SUCCESS
        : NodeState.FAILURE;
    }
  }
}
