using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CombatLog : MonoBehaviour
{
    // Updated on EnemyHP
    [ReadOnly] public int FrontalLasersTotalDamage = 0;
    [ReadOnly] public int SpreadLasersTotalDamage = 0;
    [ReadOnly] public int LateralLasersTotalDamage = 0;
    [ReadOnly] public int BackLasersTotalDamage = 0;
    [ReadOnly] [GUIColor("lightblue")] public int LasersTotalDamage = 0;
    [Space] // IonStreamScript
    [ReadOnly] public int BombTotalDamage = 0;
    [Space]
    [ReadOnly] public int IonStreamTotalDamage = 0;
    [Space] // DroneAttackScript
    [ReadOnly] public int Drone1TotalDamage = 0;
    [ReadOnly] public int Drone2TotalDamage = 0;
    [ReadOnly] public int Drone3TotalDamage = 0;
    [ReadOnly] [GUIColor("lightblue")] public int DronesTotalDamage = 0;
    [Space]
    [ReadOnly] [GUIColor("lightpurple")] public int TotalDamageDealt = 0;
    [Space] //ShieldStrenght
    [ReadOnly] public int FrontShieldTotalBlocked = 0;
    [ReadOnly] public int RightShieldTotalBlocked = 0;
    [ReadOnly] public int LeftShieldTotalBlocked = 0;
    [ReadOnly] public int BackShieldTotalBlocked = 0;
    [ReadOnly] [GUIColor("lightblue")] public int ShieldsTotalBlocked = 0;
    [Space] //PlayerHP/PlayerHeal
    [ReadOnly] public int TotalDamageHealed = 0;
    [ReadOnly] public int TotalDamageTaken = 0;

    static public CombatLog Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

#if UNITY_EDITOR 
    private void Update()
    {
        SumValues();
    }
#endif

    public void SumValues()
    {
        LasersTotalDamage = FrontalLasersTotalDamage+SpreadLasersTotalDamage+LateralLasersTotalDamage+BackLasersTotalDamage;
        DronesTotalDamage = Drone1TotalDamage+Drone2TotalDamage+Drone3TotalDamage;
        ShieldsTotalBlocked = FrontShieldTotalBlocked+RightShieldTotalBlocked+LeftShieldTotalBlocked+BackShieldTotalBlocked;

        TotalDamageDealt = LasersTotalDamage + DronesTotalDamage + IonStreamTotalDamage + BombTotalDamage;
    }
}
