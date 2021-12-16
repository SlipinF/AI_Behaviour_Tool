using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoD.BT {
    [LeafType("Action")]
    [NodeHint("Makes the actor moves toward the player until the actor reach 'endDistance'. If 'moveOnYAxis' is enabled the actor will also move on the Y axis and gravity won't be applied durong the motion.")]
    public class Shoot : Leaf {
        private Vector3 shootDirection;
        private bool active = true;
        private float counter;
        private float _cooldown;

        public Shoot(float cooldown) {
            this._cooldown = cooldown;
            this.counter = _cooldown;
        }

        public override NodeState Evaluate(Blackboard blackboard) {
            EnemyActor actor = (EnemyActor)blackboard["actor"];
            PlayerActor player = (PlayerActor)blackboard["player"];
            Weapons _weapons = actor.GetComponent<Weapons>();
            PhysicState physics = actor.Physics();

            Vector3 toPlayer = player.transform.position - actor.transform.position;

            if (_weapons.HasPrimaryWeapon() && active && counter <= 0) {
             //_weapons.primaryWeapon.Fire(actor.transform.position, new Vector2(toPlayer.x, toPlayer.y), 0); 

             active = false;
             counter = _cooldown;       
             FMODUnity.RuntimeManager.PlayOneShot("event:/SFXEnemyShot1", actor.transform.position);
             return NodeState.SUCCESS;
            }

            counter -= physics.GetDeltaTime();
            return NodeState.RUNNING;
        }

        public override void Reset() {
            active = true;
        }
    }
}
