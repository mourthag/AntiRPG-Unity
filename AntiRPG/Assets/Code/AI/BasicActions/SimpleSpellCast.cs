using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpellCast : GOAPAction {

    private bool _recovered;
    private float _startTime;

    public float Damage = 1.0f;
    public float RecoveryTime;
    public GameObject ProjectilePrefab;

    public SimpleSpellCast()
    {
        AddEffect("KillPlayer", true);
        SetInRange(false);
        Target = null;
        _startTime = 0;
        _recovered = false;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        Target = FindObjectOfType<PlayerController>().gameObject;
        return Target != null;
    }

    public override bool IsDone()
    {
        return _recovered;
    }

    public override bool Perform(GameObject agent)
    {
        if(_startTime == 0)
        {
            _startTime = Time.time;

            var spawnPosition = transform.position + Vector3.up * 2;

            var projectile = Instantiate(ProjectilePrefab, spawnPosition, Quaternion.identity, transform);
            projectile.transform.LookAt(Target.transform);

            var projectileScript = projectile.GetComponent<SimpleProjectile>();
            projectileScript.Damage = Damage;
        }
        if (Time.time - _startTime > RecoveryTime)
        {
            _recovered = true;
        }
        return true;

    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        SetInRange(false);
        Target = null;
        _startTime = 0;
        _recovered = false;
    }
}
