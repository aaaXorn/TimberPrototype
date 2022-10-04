using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;
using Actions;

namespace Timber
{
    public class TimberControl : MonoBehaviour
    {
        InpReceiver _input;
        Movement _move;

        bool _jump_tapped, _holding_jump;

        void Awake()
        {
            _input = GetComponent<InpReceiver>();
            _move = GetComponent<Movement>();
        }

        void Update()
        {
            Move();
            Jump();
        }

        void Move()
        {
            //sets the direction based on the player's input
            Vector3 dir = new Vector3(_input.h_move, 0, _input.v_move).normalized;
            _move.Move(dir);
            _move.Rotate(dir, Time.deltaTime);
        }

        void Jump()
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

        void FixedUpdate()
        {
            //gravity
            _move.Gravity();
        }
    }
}