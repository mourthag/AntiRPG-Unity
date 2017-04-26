using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class CharacterController : MonoBehaviour, IGOAP
{
    public float Speed = 1.0f;

    public float Health = 100;
    public string Name = "Character";

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	    CheckDeath();
	
	}

    public void Move()
    {
        Move(transform.forward);
    }

    public void Move(Vector3 direction)
    {
        direction.Normalize();

        transform.position += direction*Time.deltaTime*Speed;
    }

    public void Rotate(float degrees)
    {
        transform.Rotate(Vector3.forward, degrees);
    }

    public void Rotate(Vector3 target)
    {
        transform.rotation = Quaternion.LookRotation(target);
    }

    public bool CheckDeath()
    {
        if (!(Health <= 0)) return false;

        Die();
        return true;
    }

    public bool Damage(float amount)
    {
        Health -= amount;
        Debug.Log("Damage taken:" + Name + " has " + Health + " Health");
        return CheckDeath();
    }
         
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Attack(CharacterController target)
    {
    }

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        var worldState = new HashSet<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("KillPlayer", false)
        };

        return worldState;
    }

    public HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        var goals = new HashSet<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("KillPlayer", true)
        };

        return goals;
    }

    public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        Debug.Log(failedGoal.First() + "can not be reached!");
    }

    public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions)
    {
        Debug.Log("Plan found for goal: " + goal.FirstOrDefault().Key);
    }

    public void ActionsFinished()
    {
    }

    public void PlanAborted(GOAPAction aborterAction)
    {
    }

    public bool MoveAgent(GOAPAction nextAction)
    {
        transform.LookAt(nextAction.Target.transform);
        Move();

        nextAction.SetInRange((nextAction.Target.transform.position - transform.position).magnitude <= nextAction.Range);

        return nextAction.IsInRange();
    }
}
