using UnityEngine;
using LoD;
using Gamekit2D;

namespace LoD.BT {
  public class BlackboardManager : Singleton<BlackboardManager> {
    public LayerMask playerMask;
    public LayerMask obstaclesMask;
    public GameObjectRuntimeSet playerSet;

    public void SetDefaults(Blackboard blackboard) {
      blackboard["player"] = PlayerCharacter.PlayerInstance;//playerSet.First().GetComponent<PlayerCharacter>();
      blackboard["playerMask"] = BlackboardManager.Instance.playerMask;
      blackboard["obstaclesMask"] = BlackboardManager.Instance.obstaclesMask;
    }
  }
}

