using System;
using UnityEngine;

namespace LoD.BT {
  public class ActionNode : Leaf {
    private Func<Blackboard, NodeState> func;

    public ActionNode(Func<Blackboard, NodeState> func) {
      if (func == null) {
        throw new Exception("An ActionNode must have a function!");
      }
      this.func = func;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      return func(blackboard);
    }
  }
}
