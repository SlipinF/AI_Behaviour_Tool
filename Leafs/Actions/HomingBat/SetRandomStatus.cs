using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("Makes the actor moves toward the player until the actor reach 'endDistance'. If 'moveOnYAxis' is enabled the actor will also move on the Y axis and gravity won't be applied durong the motion.")]
    public class SetRandomStatus : Leaf {
        public SetRandomStatus() {
        }

        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
            if(actor.randomTimer <= 0) {
             actor.SetRandomStatus(true);
             actor.randomDirection = new Vector2(Random.Range(-1,1), Random.Range(-1, 1));
            }          
            return NodeState.SUCCESS;
        }
        public override void Reset() {
        }
    }
}
