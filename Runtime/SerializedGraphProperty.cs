using System;

namespace LoD.BT.Runtime {
  [Serializable]
  public class SerializedGraphProperty {
    public string name;
    public TypeData value;

    public SerializedGraphProperty(string name, TypeData value) {
      this.name = name;
      this.value = value;
    }
  }
}

