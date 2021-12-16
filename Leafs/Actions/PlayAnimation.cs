using UnityEngine;

namespace LoD.BT {
  [LeafType("Action")]
  [NodeHint("Plays an animation on the actor's animator and will return a success when the animation ends.")]
  public class PlayAnimation : Leaf {
    private Animator animator;
    private string animation;
    private bool complete = false;
    private bool running = false;

    protected EnemyActor actor;

    public PlayAnimation(string animation) {
      this.animation = animation;
    }

    public override NodeState Evaluate(Blackboard blackboard) {
      actor = (EnemyActor) blackboard["actor"];

      if(!running) {
        Animator animator = actor.Animator();
        animator.Play(animation, 0, 0f);
        actor.OnAnimationComplete += OnAnimationComplete;
        running = true;
      }

      if(complete) {
        actor.OnAnimationComplete -= OnAnimationComplete;
        complete = false;
        running = false;
        OnComplete();
        return NodeState.SUCCESS;
      }

      return NodeState.RUNNING;
    }
    protected virtual void OnComplete() {}

    protected void OnAnimationComplete() {
      complete = true;
    }
  }
}
