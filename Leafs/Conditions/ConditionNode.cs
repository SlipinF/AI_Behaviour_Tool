using System;
using UnityEngine;

namespace LoD.BT {
  public class ConditionNode : Leaf {
    private Func<Blackboard, bool> func;

    public ConditionNode(Func<Blackboard, bool> func) {
      if (func == null) {
        throw new Exception("A ConditionNode must have a function!");
      }
      this.func = func;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      return func(blackboard)
        ? NodeState.SUCCESS
        : NodeState.FAILURE;
    }
  }
}
