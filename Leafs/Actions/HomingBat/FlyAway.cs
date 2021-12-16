using UnityEngine;

namespace LoD.BT {
    [LeafType("Action")]
    public class FlyAway : Leaf {
        private Vector2 flyAwayDistance;
        private AnimationCurve flyAwayXCurve;
        private AnimationCurve flyAwayYCurve;
        private float flyAwayDuration;
        private string flyAwayAnimationName;

        private float startTime = -1;
        private Vector3 startPosition;
        private Vector3 toTarget;

        public FlyAway(float flyAwayDuration,
                      AnimationCurve flyAwayXCurve,
                      AnimationCurve flyAwayYCurve,
                      Vector2 flyAwayDistance,
                      string flyAwayAnimationName) {
            this.flyAwayDuration = flyAwayDuration;
            this.flyAwayXCurve = flyAwayXCurve;
            this.flyAwayYCurve = flyAwayYCurve;
            this.flyAwayDistance = flyAwayDistance;
            this.flyAwayAnimationName = flyAwayAnimationName;
        }

        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyActor actor = (EnemyActor)blackboard["actor"];
            PlayerActor player = (PlayerActor)blackboard["player"];
            float maxY = (float)blackboard["originY"];
            PhysicState physics = actor.Physics();
            Animator animator = actor.Animator();

            if (startTime == -1) {
                animator.Play(flyAwayAnimationName, 0, 0f);
                startTime = physics.GetTime();
                startPosition = actor.transform.position;
                float flyAwayDirection = Mathf.Sign(physics.LastMovement().x);

                toTarget = player.Collider().bounds.center
                  + new Vector3(
                      flyAwayDistance.x * flyAwayDirection,
                      flyAwayDistance.y, 0)
                  - actor.transform.position;
            }

            float rate = (physics.GetTime() - startTime) / flyAwayDuration;

            if (rate >= 1) {
                physics.velocity = new Vector2();
                return NodeState.SUCCESS;
            }

            Vector3 nextPosition = startPosition + new Vector3(
              toTarget.x * flyAwayXCurve.Evaluate(rate),
              toTarget.y * flyAwayYCurve.Evaluate(rate),
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
