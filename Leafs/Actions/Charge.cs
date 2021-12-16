using UnityEngine;

namespace LoD.BT {
  [LeafType("Action")]
  [NodeHint("An action that makes the actor charge the player in a straight line.")]
  public class Charge : Leaf {
    private float maxDistance;
    private float pastPlayerDistance;
    private float speed;
    private float distanceTravelled = 0;
    private bool hasChargeDirection = false;
    private Vector3 chargeDirection;

    public Charge(float speed, float pastPlayerDistance) {
      this.speed = speed;
      this.pastPlayerDistance = pastPlayerDistance;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyActor actor = (EnemyActor) blackboard["actor"];
      PlayerActor player = (PlayerActor) blackboard["player"];
      PhysicState physics = actor.Physics();

      if(!hasChargeDirection) {
        hasChargeDirection = true;
        chargeDirection = (player.transform.position - actor.transform.position).normalized;
        maxDistance = Mathf.Abs(player.transform.position.x - actor.transform.position.x) + pastPlayerDistance;
      }

      if(physics.IsSidingWall()) {
        physics.velocity.x = 0;
        return NodeState.SUCCESS;
      }

      distanceTravelled += Mathf.Abs(physics.LastMovement().x);

      if(distanceTravelled >= maxDistance) {
        physics.velocity.x = 0;
        return NodeState.SUCCESS;
      }

      physics.velocity.x = Mathf.Sign(chargeDirection.x) * speed;

      return NodeState.RUNNING;
    }

    public override void Reset() {
      distanceTravelled = 0;
      hasChargeDirection = false;
    }
  }
}
