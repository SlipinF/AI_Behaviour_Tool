using System;

namespace LoD.BT {
  [AttributeUsage(AttributeTargets.Class)]
  public class LeafType : Attribute {
    public string type;

    public LeafType(string type) {
      this.type = type;
    }
  }
}
