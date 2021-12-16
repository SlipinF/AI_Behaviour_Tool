using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("Makes the actor moves towards wall")]
    public class Move_Patroll : Leaf {
        private float speed;
        private bool edgeFalling;


        public Move_Patroll(float speed, bool edgeFalling) {
            this.speed = speed;
            this.edgeFalling = edgeFalling;
        }
        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
            if (actor.IsToTurnAround(actor.viewDistance, edgeFalling)) {
                //thisDebug.Log("moving Climb"); will inverse the move vector, and UpdateFacing will then flip the sprite & forward vector as moveVector will be in the other direction
                return NodeState.FAILURE;
            } else {
                actor.SetMoveVector(new Vector2(speed * actor.m_SpriteForward.x, -actor.gravity));
                actor.EnemyMove();
               return NodeState.SUCCESS;
            }
        }
        public override void Reset() {
        }
    }
}
