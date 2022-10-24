using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;
using Actions;
using Timber;

namespace Enemy
{
    [RequireComponent(typeof(InpReceiver))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(LedgeGrab))]
    [RequireComponent(typeof(Dash))]
    public class EnemyTestControl : MonoBehaviour
    {
        //character's states
        enum State
        {
            Move,
            Ledge
        }
        //current state
        State currState = State.Move;
        
        
        InpReceiver _input;
        Movement _move;
        LedgeGrab _ledge;
        Dash _dash;

        Rigidbody _rigid;

        bool _jump_tapped, _holding_jump;

        void Awake()
        {
            _input = GetComponent<InpReceiver>();
            _move = GetComponent<Movement>();
            _ledge = GetComponent<LedgeGrab>();
            _dash = GetComponent<Dash>();

            _rigid = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if(_input.isControlled == false) return;

            StateMachine(currState);
        }

        #region movement
        #region basic movement
        private void Move(float time)
        {
            //sets the direction based on the player's input
            Vector3 dir = new Vector3(_input.h_move, 0, _input.v_move).normalized;
            _move.Move(dir, time);
            _move.Rotate(dir, Time.deltaTime);
            Dash(dir);
        }

        private void Jump()
        {
            if(_input.jump)
            {
                //_jump_tapped prevents the buffer from causing multiple jumps out of a single button press
                if(!_jump_tapped)
                {
                    //initial jump effect
                    _move.Jump();

                    _jump_tapped = true;
                }
            }
            else
            {
                _jump_tapped = false;

                //hold jump effect
                if(_holding_jump)
                {
                    //stop hold jump
                    if(!_input.jump_hold)
                    {
                        _holding_jump = false;

                        _move.StopCoroutine("JumpHolding");
                    }
                }
                else
                {
                    //start hold jump
                    if(_input.jump_hold)
                    {
                        _holding_jump = true;

                        _move.StartCoroutine("JumpHolding");
                    }
                }
            }
        }
        #endregion

        private void LedgeCheck()
        {
            if(_rigid.velocity.y < -0.1f)
            {
                if(_ledge.LedgeCheck())
                {
                    currState = State.Ledge;
                }
            }
        }

        public void Dash(Vector3 dir)
        {
            if(_input.action2)
            {
                print("dash");
                _dash.DashEffect(dir);

                _jump_tapped = true;
            }
        }
        #endregion

        #region state machine

        private void StateMachine(State state)
        {
            switch(state)
            {
                case State.Move:
                    MoveState();
                    break;
                
                case State.Ledge:
                    LedgeState();
                    break;
            }
        }

        private void MoveState()
        {
            Move(Time.deltaTime);
            Jump();
            
            LedgeCheck();
        }

        private void LedgeState()
        {
            if(_ledge.Ledge(Time.deltaTime) == true)
                currState = State.Move;
        }
        #endregion

        void FixedUpdate()
        {
            //gravity
            _move.Gravity();
        }
    }
}
