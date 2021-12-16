using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("Detect if any bullet object is close to actor")]
    public class IsInDanger : Leaf {
        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
            
            return NodeState.SUCCESS;
        }
        public override void Reset() {
        }
    }
}
