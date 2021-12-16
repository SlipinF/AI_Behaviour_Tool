using UnityEngine;
using System;

namespace LoD.BT.Runtime {
  [Serializable]
  public class BehaviourNodeLinkData {
    public string outputGUID;
    public string inputGUID;
    public string outputPortName;
    public string inputPortName;
  }

}

