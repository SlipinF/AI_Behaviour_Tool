using UnityEngine;
using Gamekit2D;

namespace LoD.BT {
  [LeafType("Action")]
  [NodeHint("Makes the actor moves toward the player until the actor reach 'endDistance'. If 'moveOnYAxis' is enabled the actor will also move on the Y axis and gravity won't be applied durong the motion.")]
  public class MoveCloser : Leaf {
    private float speed;
    private float endDistance;
    private Vector3 moveDirection;
    private bool moveOnYAxis;
    private string moveCloserAnimationName;
    private bool hasStartedMoving = false;


    public MoveCloser(float speed, float distance, string moveCloserAnimationName, bool moveOnYAxis = false) {
      this.endDistance = 1;
      this.speed = speed;
      this.moveOnYAxis = moveOnYAxis;
      this.moveCloserAnimationName = moveCloserAnimationName;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      EnemyBehaviour actor = (EnemyBehaviour) blackboard["actor"];
      PlayerCharacter player = (PlayerCharacter)blackboard["player"];

      Vector3 toPlayer = player.transform.position - actor.transform.position;
      moveDirection = toPlayer.normalized;



      if(!hasStartedMoving) {
        hasStartedMoving = true;
      }
      if(actor.isInRandomStatus) {
       return NodeState.FAILURE;
      }  
        float distance = toPlayer.magnitude;
         if (distance >= endDistance) {
         actor.SetMoveVector(moveDirection * speed);
         actor.EnemyMove();
         return NodeState.SUCCESS;
       }
        return NodeState.RUNNING;
    }

    public override void Reset() {
      hasStartedMoving = false;
    }
  }
}
