using UnityEngine;

namespace LoD.BT {
  [LeafType("Action")]
  [NodeHint("Makes an actor jumps. The jump height and distance are defined using the 'hopParams' vector and are relative to the current actor's orientation (meaning a negative x axis value will result in a backward jump).")]
  public class Hop : Leaf {
    private AnimationCurve curve;
    private float duration;
    private float startTime;
    private float direction;
    private bool running;
    private Vector2 hopParams;
    private Vector2 lastPos;

    public Hop(float duration, Vector2 hopParams) {
      this.duration = duration;
      this.hopParams = hopParams;
      curve = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(0.5f, 1, 0.2f, 0.2f),
        new Keyframe(1, 0));
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyActor actor = (EnemyActor) blackboard["actor"];
      PhysicState physics = actor.Physics();

      if(!running) {
        running = true;
        startTime = physics.GetTime();
        direction = physics.orientation;
      }

      float rate = (physics.GetTime() - startTime) / duration;

      Vector2 nextPos = new Vector2(
        rate * direction * hopParams.x,
        hopParams.y * curve.Evaluate(rate));

      physics.velocity = (nextPos - lastPos) / physics.GetFixedDeltaTime();
      lastPos = nextPos;

      if(rate >= 1) {
        running = false;
        physics.velocity = new Vector2();
        return NodeState.SUCCESS;
      } else {
        return NodeState.RUNNING;
      }
    }

    public override void Reset() {
      running = false;
      lastPos = new Vector2();
    }
  }
}
