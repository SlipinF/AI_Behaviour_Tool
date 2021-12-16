using System;

namespace LoD.BT {

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class GenericType : Attribute {
    public Type type;

    public GenericType(Type type) {
      this.type = type;
    }
  }
}
