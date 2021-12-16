using UnityEngine;

namespace LoD.BT {
  [LeafType("Action")]
  [GenericType(typeof(bool))]
  [GenericType(typeof(float))]
  [GenericType(typeof(int))]
  [GenericType(typeof(string))]
  [GenericType(typeof(Vector2))]
  [GenericType(typeof(Vector3))]
  [NodeHint("Sets a value in the behaviour tree blackboard.")]
  public class SetBlackboard<T> : Leaf {
    private string property;
    private T value;
    public SetBlackboard(string property, T value) {
      this.property = property;
      this.value = value;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      blackboard[property] = value;
      return NodeState.SUCCESS;
    }
  }
}

