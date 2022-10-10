using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Inputs;

namespace Test
{
    public class SwapTest : MonoBehaviour
    {
        [SerializeField]
        InpReceiver[] _inputs;

        int _isPlayer = 0;

        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                _isPlayer++;
                if(_isPlayer > 1) _isPlayer = 0;
                
                InpManager.Instance.ChangeReceiver(_inputs[_isPlayer]);
            }
        }
    }
}