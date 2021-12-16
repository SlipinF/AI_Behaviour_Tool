using UnityEngine;

namespace LoD.BT {
  [LeafType("Action")]
  [NodeHint("Makes the actor falls in a straight line until grounded. Rather than a velocity, this action uses a duration so you generally want to have an action prior this one that will position the actor in mid air.")]
  public class FastFall : Leaf {
    private float duration;
    private bool started = false;
    private float height;

    public FastFall(float duration, float height) {
      this.duration = duration;
      this.height = height;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyActor actor = (EnemyActor) blackboard["actor"];
      PhysicState physics = actor.Physics();

      if(physics.IsGrounded()) {
        return NodeState.SUCCESS;
      } else {
        if(!started) {
          started = true;
        }
        physics.velocity = new Vector2(0, height / duration * -1);
        return NodeState.RUNNING;
      }
    }

    public override void Reset() {
      started = false;
    }
  }
}

