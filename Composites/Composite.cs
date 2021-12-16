using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoD.BT {
  public abstract class Composite : Node {
    public string name;
    protected List<Node> nodes = new List<Node>();

    public Composite(string name, List<Node> nodes) {
      this.name = name;
      this.nodes = nodes;
    }

    public override void Reset() {
      foreach(Node node in nodes) {
        node.Reset();
      }
    }
  }
}
