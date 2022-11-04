using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidCustomDrag : MonoBehaviour
    {
        Rigidbody _rigid;
        [SerializeField] float _dragMult;

        [SerializeField] Vector3 _dragDir;

        void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
        }

        public void CustomDrag()
        {
            _rigid.AddForce(new Vector3(-(_rigid.velocity.x * _dragDir.x), -(_rigid.velocity.y * _dragDir.y), -(_rigid.velocity.z * _dragDir.z)) * _dragMult);
        }

        void FixedUpdate()
        {
            CustomDrag();
        }
    }
}