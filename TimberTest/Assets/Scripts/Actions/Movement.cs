using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {
        [Tooltip("Movement stats.")]
        [SerializeField]
        SO_Movement _so_move;

        Rigidbody _rigid;

        //if this character is stunned
        bool isStunned;

        [Tooltip("Offset from transform.position where the jump raycast will begin.")]
        [SerializeField]
        Vector3 _jump_rc_offset;
        [Tooltip("Length of the jump raycast.")]
        [SerializeField]
        float _jump_rc_dist;

        //jump raycast layermask
        LayerMask _jump_rc_layers;

        //if the jump started while the character was grounded
        bool _grounded;
        //how many air jumps the character can still make
        int _curr_air_jumps;
        //the current timer of hold jump
        float _curr_jump_charge;
        //if jump is currently in cooldown
        bool _jump_isCD;

        void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
        }

        void Start()
        {
            _curr_air_jumps = _so_move.air_jumps;

            //layer 3 is the layer Player, layer 6 is the layer Character
            //this sets the layer mask as everything but those two layers
            _jump_rc_layers = ~((1 << 3) | (1 << 6));
        }

        #region basic movement
        //changes the character's horizontal velocity
        public void Move(Vector3 dir, float time)
        {
            if(isStunned || dir == Vector3.zero) return;
//https://www.youtube.com/watch?v=BNiAt0HnC5M
            //_rigid.velocity = new Vector3(dir.x * _so_move.move_spd, _rigid.velocity.y, dir.z * _so_move.move_spd);
            Vector3 vel = dir * _so_move.move_spd;
            vel += vel.normalized * 0.2f * _rigid.drag;

            float force = Mathf.Clamp(_so_move.move_f, -_rigid.mass / time, _rigid.mass / time);

            if(_rigid.velocity.magnitude == 0)
                _rigid.AddForce(vel * force, _so_move.move_f_mode);
            else
            {
                var velProjectedToTarget = (vel.normalized * Vector3.Dot(vel, _rigid.velocity) / vel.magnitude);
                _rigid.AddForce((vel - velProjectedToTarget) * force, _so_move.move_f_mode);
            }
        }
        public void Rotate(Vector3 dir, float time)
        {
            if(isStunned || dir.magnitude < 0.1f) return;

            //target rotation
			Quaternion newRot = Quaternion.LookRotation(dir, Vector3.up);
			//rotates until the target rotation is reached
			transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, _so_move.rot_spd * time);
        }
        #endregion

        #region jump
        //checks if the player is grounded
        public bool JumpCheck()
        {
            if(isStunned) return false;

            //raycast variables
            Ray ray = new Ray(transform.position + _jump_rc_offset, -Vector3.up);
            RaycastHit hit;

            //checks if there's a valid collider below the character
            if(Physics.Raycast(ray, out hit, _jump_rc_dist, _jump_rc_layers))
                return true;
            return false;
        }
        #if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            //draws the jump raycast on the screen
            Debug.DrawRay(transform.position + _jump_rc_offset, -Vector3.up * _jump_rc_dist, Color.green);
        }
        #endif

        //initial jump (1 frame only)
        private void InitialJump(bool grounded)
        {
            _rigid.AddForce(Vector3.up * (grounded ? _so_move.jump_f : _so_move.air_jump_f));
        }
        //!!!used in a coroutine with yield return new WaitForFixedUpdate()
        private void HoldJump(bool grounded)
        {
            _rigid.AddForce(Vector3.up * (grounded ? _so_move.h_jump_f : _so_move.h_air_jump_f), _so_move.jump_f_mode);
        }
        
        public void Jump()
        {
            //stops the player from mashing jump to self-yeet into the sky
            if(_jump_isCD) return;

            //if grounded
            if(JumpCheck())
            {
                _grounded = true;
                _curr_air_jumps = _so_move.air_jumps;
                _curr_jump_charge = _so_move.max_jump_charge;

                //grounded jump
                InitialJump(true);
            }
            else
            {
                //if character still has air jumps left
                if(_curr_air_jumps > 0)
                {
                    _grounded = false;
                    _curr_air_jumps--;
                    _curr_jump_charge = _so_move.air_max_jump_charge;

                    //air jump
                    InitialJump(false);
                }
            }

            //puts the jump on cooldown
            StartCoroutine("JumpCooldown");
        }
        public IEnumerator JumpHolding()
        {
            //until the max duration
            while(_curr_jump_charge > 0)
            {
                //once per FixedUpdate
                yield return new WaitForFixedUpdate();

                //add force to the held jump
                HoldJump(_grounded);

                _curr_jump_charge -= Time.fixedDeltaTime;
            }
        }

        //jump cooldown timer
        private IEnumerator JumpCooldown()
        {
            _jump_isCD = true;

            yield return new WaitForSeconds(_so_move.jump_cd);

            _jump_isCD = false;
        }
        #endregion

        //stun cooldown timer
        public IEnumerator Stun()
        {
            isStunned = true;

            yield return new WaitForSeconds(_so_move.stun_duration);

            isStunned = false;
        }

        //!!!used in FixedUpdate()
        //set Rigidbody Use Gravity as false in the inspector, as this will be used instead
        public void Gravity()
        {
            _rigid.AddForce(-Vector3.up * _so_move.grav);
        }

        public bool Get_isStunned()
        {
            return isStunned;
        }
    }
}