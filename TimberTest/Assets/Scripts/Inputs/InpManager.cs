using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Timber;

namespace Inputs
{
    public class InpManager : MonoBehaviour
    {
        //rewired player
        Player _rw_player;
        //which object is receiving the inputs
        InpReceiver _char;

        void Awake()
        {
            _rw_player = ReInput.players.GetPlayer(0);
        }

        void Update()
        {
            GetInputs();
        }

        //gets the inputs and sets them in the current controlled object
        private void GetInputs()
        {
            //if there's an object being controlled
            if(_char != null)
            {
                _char.h_move = _rw_player.GetAxis("H_Move");
                _char.v_move = _rw_player.GetAxis("V_Move");
                _char.jump = _rw_player.GetButtonDown("Jump");
                _char.jump_hold = _rw_player.GetButton("Jump");
                _char.action1 = _rw_player.GetButtonDown("Puppet");
                _char.action2 = _rw_player.GetButtonDown("Arm");
            }
            else
            {
                #if UNITY_EDITOR
                print("InpReceiver in ImpManager is null.");
                #endif

                _char = TimberInstance.Instance.timberInput;
            }
        }
    }
}