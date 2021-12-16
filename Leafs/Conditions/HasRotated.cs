using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
namespace LoD.BT {
    [LeafType("Condition")]
    [NodeHint("Check if object has rotated")]
    public class HasRotated : Leaf {
      public override NodeState Evaluate(Blackboard blackboard) {
            EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
            return actor.hasRotated
              ? NodeState.SUCCESS
              : NodeState.FAILURE;
        }
    }
}
