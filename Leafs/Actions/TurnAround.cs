using UnityEngine;
using UnityEngine.Events;
using Gamekit2D;

namespace LoD.BT {
  [LeafType("Action")]
  [NodeHint("An action that changes the direction")]
  public class TurnAround : Leaf {
        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyBehaviour actor = (EnemyBehaviour)blackboard["actor"];
            Vector2 direction = new Vector2(Mathf.RoundToInt(actor.m_SpriteForward.x), 0);
            actor.UpdateFacing((int)direction.x);
            return NodeState.SUCCESS;
        }
        public override void Reset() {
        }
    }
}
  

