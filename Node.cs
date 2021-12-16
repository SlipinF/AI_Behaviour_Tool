using UnityEngine;
using System.Collections;

namespace LoD.BT {
  public abstract class Node {
    public Node() {}

    public abstract void Reset();

    public abstract NodeState Evaluate(Blackboard blackboard);
  }
}
