using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementSO",
				 menuName = "ScriptableObject/Movement")]
public class SO_Movement : ScriptableObject
{
    [Tooltip("Horizontal movement speed.")]
    public float move_spd;

    #region Jump
    [Tooltip("Ground jump initial force.")]
    public float jump_f;
    [Tooltip("Jump hold extra force.")]
    public float h_jump_f;
    [Tooltip("Air jump initial force.")]
    public float air_jump_f;
    [Tooltip("Air jump hold extra force.")]
    public float h_air_jump_f;
    [Tooltip("Number of air jumps.")]
    public int air_jumps;

    [Tooltip("Maximum time-window for hold jump.")]
    public float max_jump_charge;
    [Tooltip("Maximum time-window for air hold jump.")]
    public float air_max_jump_charge;

    [Tooltip("Minimum time between jumps.")]
    public float jump_cd;
    #endregion

    [Tooltip("Gravitational force.")]
    public float grav;

    [Tooltip("Rotation speed.")]
    public float rot_spd;
    
    [Tooltip("Stun effect duration.")]
    public float stun_duration;
}
