using UnityEngine;

namespace LoD.BT {
  [LeafType("Action")]
  [NodeHint("Waits for a given amount of time before returning a success. For instance, it can be used to pause the actor for a bit after an action.")]
  public class Wait : Leaf {
    private float startTime;
    private float duration;
    private bool started;

    public Wait(float duration) {
      this.duration = duration;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      //EnemyActor actor = (EnemyActor) blackboard["actor"];
     // PhysicState physics = actor.Physics();

      if(!started) {
        startTime = Time.deltaTime;
        started = true;
      }

      return Time.deltaTime - startTime >= duration
        ? NodeState.SUCCESS
        : NodeState.RUNNING;
    }

    public override void Reset() {
      started = false;
    }
  }
}
