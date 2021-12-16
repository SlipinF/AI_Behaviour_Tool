using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("An action that sets a wall climb status")]
    public class SetWallClimbingStatus : Leaf {
       public override NodeState Evaluate(Blackboard blackboard) {
            EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
            actor.canWallClimb = true;
            return NodeState.SUCCESS;
        }
        public override void Reset() {

        }
    }
}


