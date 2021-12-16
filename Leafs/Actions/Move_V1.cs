using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("Makes the actor patrol from wall to wall")]
    public class Move_V1 : Leaf {
    private float speed;
    private bool edgeFalling;


    public Move_V1(float speed, bool edgeFalling) {
      this.speed = speed;
      this.edgeFalling = edgeFalling;
    }
    public override NodeState Evaluate(Blackboard blackboard) {
     EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
            if (actor.CheckForObstacle(actor.viewDistance, edgeFalling)) {
                //thisDebug.Log("moving Climb"); will inverse the move vector, and UpdateFacing will then flip the sprite & forward vector as moveVector will be in the other direction
                return NodeState.FAILURE;
            } else {
                if (actor.IsWallClimbing) {
                    if(actor.transform.rotation.x == 1) {
                        actor.SetMoveVector((actor.transform.right * -1) * speed);
                        actor.activeEdgecollider = actor.leftEdgeCollider;
                    } else {
                        actor.SetMoveVector(actor.transform.right * speed);
                        actor.activeEdgecollider = actor.rightEdgeCollider;
                    }
                    actor.EnemyMove();
                }
                else {
                    actor.SetMoveVector(actor.transform.right * speed);
                    actor.EnemyMove();
                }
                return NodeState.SUCCESS;
            }
        }
        public override void Reset() {
        }
    }
}
