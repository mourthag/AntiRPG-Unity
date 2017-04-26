using UnityEngine;

public class MeeleeAttackAction : GOAPAction {

    private bool _recovered;
    private float _startTime;

    public float Damage = 1.0f;
    public float RecoveryTime;

    public MeeleeAttackAction()
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
            Target.GetComponent<CharacterController>().Damage(Damage);
        }
        if(Time.time - _startTime > RecoveryTime)
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
