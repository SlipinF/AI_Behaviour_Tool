using UnityEngine;

namespace LoD.BT {
    [LeafType("Action")]
    public class FlyTowards : Leaf {
        GameObject endPoint;
        float movementSpeed;
        int threshold;

        public FlyTowards(GameObject endPoint, float movementSpeed, int threshold = 0) {
            this.endPoint = endPoint;
            this.movementSpeed = movementSpeed;
            this.threshold = threshold;
        }

        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyActor actor = (EnemyActor)blackboard["actor"];
            Vulnerable vulnerable = actor.Vulnerable();

            if (actor.transform.position == endPoint.transform.position) {
                return NodeState.SUCCESS;
            }
            if (vulnerable.Health() <= threshold) {
                Object.Destroy(actor.gameObject);
                return NodeState.FAILURE;
            }
            actor.transform.position = Vector2.MoveTowards(actor.transform.position, endPoint.transform.position, movementSpeed * Time.deltaTime);
            return NodeState.RUNNING;
        }
    }
}
