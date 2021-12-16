using UnityEngine;
using System;

namespace LoD.BT {
  public abstract class Decorator : Node {
    protected Node childNode;

    public Decorator(Node childNode) {
      if (childNode == null) {
        throw new Exception("A Decorator must have a child node!");
      }
      this.childNode = childNode;
    }

    public override void Reset() {
      childNode.Reset();
    }
  }
}
