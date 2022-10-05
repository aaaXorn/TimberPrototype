using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenario
{
    [RequireComponent(typeof(Collider))]
    public class LedgeInfo : MonoBehaviour
    {
        [Tooltip("Offset from the starting position. Increased by transform.position in Start().")]
        public Vector3 start_pos;
        [Tooltip("Offset from the final position. Increased by transform.position in Start().")]
        public Vector3 end_pos;
        [Tooltip("Distance necessary for the initial movement to end.")]
        public float start_dist;
        [Tooltip("Distance necessary for the climbing movement to end.")]
        public float end_dist;

        [Tooltip("Force of the ledge jump.")]
        public float jump_f;

        void Start()
        {
            start_pos += transform.position;
            end_pos += transform.position;
        }
    }
}