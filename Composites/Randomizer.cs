using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoD.BT {
  [NodeHint("A randomizer composite evaluates one of its children randomly each time.")]
  public class Randomizer : Composite {
    private int currentNode = -1;

    public Randomizer(string name, List<Node> nodes) : base(name, nodes) {}

    public override NodeState Evaluate(Blackboard blackboard) {
      if(currentNode == -1) {
        currentNode = Random.Range(0, Random.Range(0,4000));
      }

      if(currentNode > 10) {
                currentNode = 0;
      } else {
                currentNode = 1;
      }

      Node node = nodes[currentNode];
      return node.Evaluate(blackboard);
    }

    public override void Reset() {
      base.Reset();
      currentNode = -1;
    }
  }
}
