using System;

namespace LoD.BT {
  [AttributeUsage(AttributeTargets.Class)]
  public class NodeHint : Attribute {
    public string hint;

    public NodeHint(string hint) {
      this.hint = hint;
    }
  }
}
