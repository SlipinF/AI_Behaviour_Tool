using UnityEngine;

namespace LoD.BT {
  [LeafType("Action")]
  [NodeHint("An action node that makes the actor dive onto the player. The curves can be used to control the concrete motion of the actor on each axis.")]
  public class Dive : Leaf {
    private AnimationCurve diveXCurve;
    private AnimationCurve diveYCurve;
    private float diveDuration;
    private string diveAnimationName;
    private float pastPlayerDistance;

    private float startTime = -1;
    private Vector3 startPosition;
    private Vector3 toTarget;

    public Dive(float diveDuration,
                AnimationCurve diveXCurve,
                AnimationCurve diveYCurve,
                float pastPlayerDistance,
                string diveAnimationName) {
      this.diveDuration = diveDuration;
      this.diveXCurve = diveXCurve;
      this.diveYCurve = diveYCurve;
      this.pastPlayerDistance = pastPlayerDistance;
      this.diveAnimationName = diveAnimationName;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyActor actor = (EnemyActor) blackboard["actor"];
      PlayerActor player = (PlayerActor) blackboard["player"];
      PhysicState physics = actor.Physics();
      Animator animator = actor.Animator();

      if(startTime == -1) {
        animator.Play(diveAnimationName, 0, 0f);
        startTime = physics.GetTime();
        startPosition = actor.transform.position;
        float chargeDirection = Mathf.Sign(player.transform.position.x - startPosition.x);
        toTarget = player.Collider().bounds.center
          + new Vector3(pastPlayerDistance * chargeDirection, 0, 0)
          - actor.transform.position;
      }

      float rate = (physics.GetTime() - startTime) / diveDuration;

      if(rate >= 1) {
        physics.velocity = new Vector2();
        return NodeState.SUCCESS;
      }

      Vector3 nextPosition = startPosition + new Vector3(
        toTarget.x * diveXCurve.Evaluate(rate),
        toTarget.y * diveYCurve.Evaluate(rate),
        1);

      physics.velocity = (nextPosition - actor.transform.position) / physics.GetFixedDeltaTime();

      return NodeState.RUNNING;
    }

    public override void Reset() {
      startTime = -1;
      startPosition = new Vector3();
      toTarget = new Vector3();
    }
  }
}

