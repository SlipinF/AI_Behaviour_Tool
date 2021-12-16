using UnityEngine;

namespace LoD.BT {
  [LeafType("Condition")]
  [NodeHint("Returns a success when the player is behind this actor based on its current orientation.")]
  public class PlayerIsBehind : Leaf {
    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyActor actor = (EnemyActor) blackboard["actor"];
      PlayerActor player = (PlayerActor) blackboard["player"];
      PhysicState physics = actor.Physics();

      Vector3 toPlayer = player.transform.position - actor.transform.position;

      return Mathf.Sign(toPlayer.x) != physics.orientation
        ? NodeState.SUCCESS
        : NodeState.FAILURE;
    }
  }
}
