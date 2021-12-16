using UnityEngine;

namespace LoD.BT {
    [LeafType("Action")]
    public class ChargeAttack : Leaf {
        GameObject endPoint;
        float movementSpeed;
        float timeBeforeRetreat;
        float timer = 0.5f;
        int threshold;

        public ChargeAttack(GameObject endPoint, float movementSpeed, float timeBeforeRetreat, int threshold = 0) {
            this.endPoint = endPoint;
            this.movementSpeed = movementSpeed;
            this.timeBeforeRetreat = timeBeforeRetreat;
            this.timer = timeBeforeRetreat;
            this.threshold = threshold;
        }

        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyActor actor = (EnemyActor)blackboard["actor"];
            Vulnerable vulnerable = actor.Vulnerable();

            timer -= Time.deltaTime;

            if (actor.transform.position == endPoint.transform.position) {
                return NodeState.SUCCESS;
            }
            if (timer < 0) {
                timer = timeBeforeRetreat;
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
