using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateEnemyInfo : MonoBehaviour
{
	[SerializeField] EnemyInfoSO infoSO;

	EnemyHP enemyHP = null;
	AsteroidMove asteroidMove = null;
	EnemyDroneMove droneMove = null;
	EnemyShipMove shipMove = null;
	CollisionWithPlayer collision = null;

	EnemyDropDealer dropDealer = null;

	EnemyProjectileShoot shooter = null;

	PowerUpDrop PowerUpDrop = null;

	SentinelAttack sentinelAttack = null;

	[Button]
	public void UpdateValues()
	{
		if (TryGetComponent(out enemyHP))
		{
			enemyHP.MaxHP = infoSO.MaxHP;
		}

		if (TryGetComponent(out asteroidMove))
		{
			asteroidMove.BaseSpeed = infoSO.Speed;
			asteroidMove.SpeedVariationPerc = infoSO.SpeedVarPerc;
		}

        if (TryGetComponent(out droneMove))
		{
			droneMove.BaseSpeed = infoSO.Speed;
			droneMove.SpeedVariationPerc = infoSO.SpeedVarPerc;
		}

        if (TryGetComponent(out shipMove))
		{
			shipMove.MaxYSpeed = infoSO.Speed;
			shipMove.MaxXSpeed = infoSO.XSpeed;
			shipMove.RotationChangeTime = infoSO.RotChangeTime;
			shipMove.RotationChangeTimeVar = infoSO.RotTimeVar;
		}

        if (TryGetComponent(out collision))
		{
			collision.Damage = infoSO.CollisionDamage;
			collision.ImpactVelocity = infoSO.ImpactVelocity;
		}

        if (TryGetComponent(out dropDealer))
        {
			dropDealer.DropsToSpawn = infoSO.DropsChances;
			dropDealer.MinDropsNum = infoSO.MinDrops;
			dropDealer.MaxDropsNum = infoSO.MaxDrops;
        }

        if (TryGetComponent(out shooter))
		{
			shooter.BaseShootCD = infoSO.ShootCD;
			shooter.ShootCDVariation = infoSO.ShootCdVar;
		}

		if(TryGetComponent(out PowerUpDrop))
		{
			PowerUpDrop.ChanceToDrop = infoSO.PuDropChance;
			PowerUpDrop.PuDrops = infoSO.PuDrops;
        }

		if(TryGetComponent(out sentinelAttack))
		{
            sentinelAttack.Range = infoSO.SentRange;
            sentinelAttack.Damage = infoSO.SentDamage;
            sentinelAttack.DamageInterval = infoSO.SentDamageInterval;
        }
    }
}