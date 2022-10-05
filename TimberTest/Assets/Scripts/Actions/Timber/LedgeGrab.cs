using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scenario;

namespace Timber
{
    [RequireComponent(typeof(Rigidbody))]
    public class LedgeGrab : MonoBehaviour
    {
        [Tooltip("Offset from transform.position where the ledge raycast will begin.")]
        [SerializeField]
        Vector3 _ledge_rc_offset;
        [Tooltip("Length of the ledge raycast.")]
        [SerializeField]
        float _ledge_rc_dist;

        [Tooltip("Movement speed while positioning to grab the ledge.")]
        [SerializeField]
        float _ledge_start_spd;
        [Tooltip("Movement speed while climbing the ledge.")]
        [SerializeField]
        float _ledge_climb_spd;

        Rigidbody _rigid;

        //ledge raycast layermask
        LayerMask _ledge_rc_layers;

        //if the character is currently climbing a ledge
        bool _isClimbing;
        //if the character finished climbing a ledge
        bool _isComplete;

        //information about the targeted ledge
        LedgeInfo _info;

        void Start()
        {
            _rigid = GetComponent<Rigidbody>();

            //layer 7 is the layer Ledge
            //this sets the layer mask as exclusively the Ledge layer
            _ledge_rc_layers = (1 << 7);
        }

        public bool LedgeCheck()
        {
            //raycast variables
            Ray ray = new Ray(transform.position + _ledge_rc_offset, transform.forward);
            RaycastHit hit;
            
            //checks if there's a valid ledge in front of the character
            if(Physics.Raycast(ray, out hit, _ledge_rc_dist, _ledge_rc_layers))
            {
                LedgeInfo LI = hit.transform.GetComponent<LedgeInfo>();

                if(LI != null)
                {
                    _info = LI;

                    _isClimbing = false;
                    _isComplete = false;

                    _rigid.isKinematic = true;

                    return true;
                }
            }

            return false;
        }
        #if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            //draws the ledge raycast on the screen
            Debug.DrawRay(transform.position + _ledge_rc_offset, transform.forward * _ledge_rc_dist, Color.blue);
        }
        #endif

        private void MoveToLedge(float time)
        {
            float step = _ledge_start_spd * time;

            transform.position = Vector3.MoveTowards(transform.position, _info.start_pos, step);

            if(Vector3.Distance(transform.position, _info.start_pos) < _info.start_dist) _isClimbing = true;
        }

        private void ClimbLedge(float time)
        {
            float step = _ledge_climb_spd * time;

            transform.position = Vector3.MoveTowards(transform.position, _info.end_pos, step);

            if(Vector3.Distance(transform.position, _info.end_pos) < _info.end_dist) _isComplete = true;
        }

        public bool Ledge(float time)
        {
            if(!_isClimbing) MoveToLedge(time);
            else
            {
                ClimbLedge(time);

                //use this in the state machine to check if the character continues climbing or not
                if(_isComplete)
                {
                    _rigid.isKinematic = false;

                    return true;
                }
            }
            return false;
        }

        public bool LedgeJump(float time)
        {
            if(!_isClimbing) MoveToLedge(time);
            else
            {
                _rigid.isKinematic = false;
                //_rigid.AddForce

                _isComplete = true;
            }

            if(_isComplete)
            {
                return true;
            }
            return false;
        }
    }
}