using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;

namespace Timber
{
    public class TimberInstance : MonoBehaviour
    {
        public static TimberInstance Instance {get; private set;}

        [HideInInspector]
        public InpReceiver timberInput;

        void Awake()
        {
            //sets global reference
            if (Instance == null) Instance = this;
            //if there's  already an instance, remove this
            else Destroy(gameObject);

            timberInput = GetComponent<InpReceiver>();
        }
    }
}