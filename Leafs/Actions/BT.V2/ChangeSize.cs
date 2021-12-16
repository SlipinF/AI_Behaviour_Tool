using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("Change size of the actor")]
    public class ChangeSize : Leaf {
        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
           // actor.Debuger();
            actor.GetComponent<SpriteRenderer>().color += new Color(0.1f, 0.1f, 0.1f);
            return NodeState.SUCCESS;
        }
        public override void Reset() {
        }
    }
}

