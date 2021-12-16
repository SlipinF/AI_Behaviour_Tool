using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;


namespace LoD.BT {
    [LeafType("Condition")]
    [NodeHint("Check if is one the edge")]
    public class IsSidingEdge : Leaf {     
        public class IsWallClimbing : Leaf {
            public override NodeState Evaluate(Blackboard blackboard) {
                EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
                return actor.CheckForObstacle(actor.viewDistance)
                  ? NodeState.SUCCESS
                  : NodeState.FAILURE;
            }
        }
    }
}
