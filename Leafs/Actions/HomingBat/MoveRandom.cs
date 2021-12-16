using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("Makes the actor moves toward the player until the actor reach 'endDistance'. If 'moveOnYAxis' is enabled the actor will also move on the Y axis and gravity won't be applied durong the motion.")]
    public class MoveRandom : Leaf {
        private float speed;
        private float endDistance;
        private Vector3 moveDirection;
        private bool moveOnYAxis;
        private string moveCloserAnimationName;
        private bool hasStartedMoving = false;


        public MoveRandom(float speed, float distance, string moveCloserAnimationName, bool moveOnYAxis = false) {
            this.endDistance = distance;
            this.speed = speed;
            this.moveOnYAxis = moveOnYAxis;
            this.moveCloserAnimationName = moveCloserAnimationName;
        }

        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
            PlayerCharacter player = (PlayerCharacter)blackboard["player"];
            if (!hasStartedMoving) {
                hasStartedMoving = true;
            }
            if (actor.isInRandomStatus) {
                actor.SetMoveVector(actor.randomDirection * speed);
                actor.EnemyMove();
                return NodeState.SUCCESS;
            } 
            else {
                return NodeState.FAILURE;
            }
        }

        public override void Reset() {
            hasStartedMoving = false;
        }
    }
}
