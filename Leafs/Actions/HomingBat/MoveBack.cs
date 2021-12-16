using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("Makes the actor")]
    public class MoveBack : Leaf {

        private float speed;
        private float endDistance;
        private Vector3 moveDirection;
        private bool moveOnYAxis;
        private string moveCloserAnimationName;
        private bool hasStartedMoving = false;

        public MoveBack(float speed, float distance, string moveCloserAnimationName, bool moveOnYAxis = false) {
            this.endDistance = distance;
            this.speed = speed;
            this.moveOnYAxis = moveOnYAxis;
            this.moveCloserAnimationName = moveCloserAnimationName;
        }

        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
            PlayerCharacter player = (PlayerCharacter)blackboard["player"];
            if(actor.randomTimer <= 0) {
               /* if(new Vector3(Mathf.Round(actor.transform.position.x), Mathf.Round(actor.transform.position.y),0) != actor.startingPosition) {
                    Vector3 toPlayer = actor.startingPosition - actor.transform.position;
                    moveDirection = toPlayer.normalized;
                    actor.SetMoveVector(moveDirection * speed);
                    actor.EnemyMove();
                    return NodeState.SUCCESS;
                } */
            }
            return NodeState.RUNNING;
        }

        public override void Reset() {
            hasStartedMoving = false;
        }

    }
}
