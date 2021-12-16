using UnityEngine;
using Gamekit2D;


namespace LoD.BT {
  [LeafType("Condition")]
  [NodeHint("Returns a success if the actor has a line of sight to the player. This condition will perform a ray cast on bothe the player layer and the obstacles layer so as to detect when the map obstructs the line of sight.")]
  public class HasLineOfSight : Leaf {
    private float distance;
    private bool ignoreObstacles;

    public HasLineOfSight(float distance, bool ignoreObstacles = false) {
      this.distance = distance;
      this.ignoreObstacles = ignoreObstacles;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyBehaviour actor = (EnemyBehaviour) blackboard["actor"];
      PlayerCharacter player = (PlayerCharacter) blackboard["player"];     
      BoxCollider2D collider = actor.GetComponent<BoxCollider2D>();
      LayerMask playerMask = (LayerMask) blackboard["playerMask"];

      Vector3 origin = collider.bounds.center;
      Vector3 direction = (player.GetComponent<Collider2D>().bounds.center - origin).normalized;

            RaycastHit2D playerHit = Physics2D.Raycast(origin, direction, distance,playerMask);

      if(playerHit.collider != null) {
        if(!ignoreObstacles) {
          LayerMask obstaclesMask = (LayerMask) blackboard["obstaclesMask"];
          RaycastHit2D obstaclesHit = Physics2D.Raycast(origin, direction, distance, obstaclesMask);

          if(obstaclesHit.collider != null &&
             obstaclesHit.distance < playerHit.distance) {
            return NodeState.FAILURE;
          } else {
            return NodeState.SUCCESS;
          }
        } else {
          return NodeState.SUCCESS;
        }
      } else {
        return NodeState.FAILURE;
      }
    }
  }
}
