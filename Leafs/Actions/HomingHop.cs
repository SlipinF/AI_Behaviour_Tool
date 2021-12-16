using UnityEngine;

namespace LoD.BT {
  [LeafType("Action")]
  [NodeHint("Makes the actor jumps toward the player.")]
  public class HomingHop : Leaf {
    private AnimationCurve curve;
    private float duration;
    private float startTime;
    private float direction;
    private bool jumping;
    private float hopHeight;
    private float lastY;
    private float distance;

    public HomingHop(float duration, float hopHeight) {
      this.duration = duration;
      this.hopHeight = hopHeight;
      curve = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(1, 1, 0.2f, 0.2f));
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyActor actor = (EnemyActor) blackboard["actor"];
      PlayerActor player = (PlayerActor) blackboard["player"];
      PhysicState physics = actor.Physics();

      if(!jumping) {
        jumping = true;
        startTime = physics.GetTime();
        direction = physics.orientation;
        distance = player.transform.position.x - actor.transform.position.x;
      }

      float rate = (physics.GetTime() - startTime) / duration;
      float nextY = hopHeight * curve.Evaluate(rate);

      physics.velocity = new Vector2(
        distance / duration / physics.Friction().x,
        (nextY - lastY) / physics.GetFixedDeltaTime()
      );

      lastY = nextY;

      if(rate >= 1) {
        jumping = false;
        physics.velocity = new Vector2();
        return NodeState.SUCCESS;
      } else {
        return NodeState.RUNNING;
      }
    }

    public override void Reset() {
      jumping = false;
      lastY = 0;
      distance = 0;
    }
  }
}
