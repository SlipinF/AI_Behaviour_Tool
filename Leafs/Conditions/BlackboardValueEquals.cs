using UnityEngine;
using System.Collections.Generic;

namespace LoD.BT {
  [LeafType("Condition")]
  [GenericType(typeof(bool))]
  [GenericType(typeof(float))]
  [GenericType(typeof(int))]
  [GenericType(typeof(string))]
  [GenericType(typeof(Vector2))]
  [GenericType(typeof(Vector3))]
  [NodeHint("Compares a value in the behaviour tree blackboard and returns a success if it equals the provided value.")]
  public class BlackboardValueEquals<T> : Leaf {
    private string property;
    private T value;

    public BlackboardValueEquals(string property, T value) {
      this.property = property;
      this.value = value;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      return EqualityComparer<T>.Default.Equals((T) blackboard[property], value)
        ? NodeState.SUCCESS
        : NodeState.FAILURE;
    }
  }
}
