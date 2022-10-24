using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Movement))]
    public class Dash : MonoBehaviour
    {
        Rigidbody _rigid;
        Movement _move;

        [SerializeField]
        float _dash_f;
        bool _dash_isCD;
        [SerializeField]
        float _dash_cd;

        void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
            _move = GetComponent<Movement>();
        }

        public void DashEffect(Vector3 dir)
        {
            if(_dash_isCD || _move.Get_isStunned()) return;

            print("dash2");
            
            _rigid.AddForce(new Vector3(dir.x * _dash_f, 0, dir.z * _dash_f));
            StartCoroutine("DashCD");
        }

        private IEnumerator DashCD()
        {
            _dash_isCD = true;

            yield return new WaitForSeconds(_dash_cd);

            _dash_isCD = false;
        }

    }
}
