using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("Make the actor climb the wall")]
    public class ClimbWall : Leaf {
        private float speed;
        private bool edgeFalling;
       
        public ClimbWall(float speed, bool edgeFalling) {
            this.speed = speed;
            this.edgeFalling = edgeFalling;
        }
        public override NodeState Evaluate(Blackboard blackboard) {
        EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
        PlayerCharacter player = (PlayerCharacter)blackboard["player"];
            if (actor.CheckForObstacle(actor.viewDistance, edgeFalling)) {
                //this will inverse the move vector, and UpdateFacing will then flip the sprite & forward vector as moveVector will be in the other direction
                return NodeState.FAILURE;
            } else {
                actor.SetMoveVector(new Vector2(0, speed * -actor.m_SpriteForward.x));
                actor.EnemyMove();
                return NodeState.SUCCESS;
            }
    }
    public override void Reset() {
    }

    }
}

