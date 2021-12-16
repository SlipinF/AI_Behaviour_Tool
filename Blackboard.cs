using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoD.BT {
  public class Blackboard : Dictionary<string, object> {
    public Blackboard() : base() {
      BlackboardManager.Instance.SetDefaults(this);
    }
  }
}
