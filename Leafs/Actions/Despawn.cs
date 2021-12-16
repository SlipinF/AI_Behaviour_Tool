using UnityEngine;
using static UnityEngine.Object;

namespace LoD.BT {
  [LeafType("Action")]
  [NodeHint("Destroys the current actor's gameobject.")]
  public class Despawn : Leaf {
    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyActor actor = (EnemyActor) blackboard["actor"];

      Object.Destroy(actor.gameObject);

      return NodeState.SUCCESS;
    }
  }
}
