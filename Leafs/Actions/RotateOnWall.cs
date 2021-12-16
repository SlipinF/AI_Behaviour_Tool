using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("Rotate actor towards wall alligment")]
    public class RotateOnWall : Leaf {
        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];

            if(actor.rotationTimer > 0) {                
                return NodeState.FAILURE;
            }
            actor.IsWallClimbing = true;
            actor.transform.rotation = Quaternion.FromToRotation(Vector2.up,actor.hitNormal);
            actor.GetComponent<Rigidbody2D>().MovePosition(actor.hitPoint);
            actor.SetRotationTimer();
            return NodeState.SUCCESS;
        }

        public override void Reset() {
        }
    }
}

