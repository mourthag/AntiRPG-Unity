using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Basic Class for all actions that any agent
/// can perform to achieve a goal.
/// </summary>
public abstract class GOAPAction : MonoBehaviour
{

    /// <summary>
    /// Preconditions that need to match so this action can be performed.
    /// </summary>
    private readonly HashSet<KeyValuePair<string, object>> _preconditions;
    /// <summary>
    /// Effects that performing this action has.
    /// </summary>
    private readonly HashSet<KeyValuePair<string, object>> _effects;

    /// <summary>
    /// For some actions the agent needs to be in range. Set to false if not needed.
    /// </summary>
    private bool _inRange = false;

    /// <summary>
    /// Instanciate and create preconditions and effects
    /// </summary>
    protected GOAPAction()
    {
        _preconditions = new HashSet<KeyValuePair<string, object>>();
        _effects = new HashSet<KeyValuePair<string, object>>();
    }

    /// <summary>
    /// Cost to perform this action.
    /// </summary>
    public float Cost = 1f;

    /// <summary>
    /// Maximum range to perform this action
    /// </summary>
    public float Range = 1f;

    /// <summary>
    /// Some Actions may target a GameObject. Others not, so this can be null.
    /// </summary>
    public GameObject Target = null;


    public HashSet<KeyValuePair<string, object>> Preconditions
    {
        get { return _preconditions; }
    }

    public HashSet<KeyValuePair<string, object>> Effects
    {
        get { return _effects; }
    }

    public void DoReset()
    {
        _inRange = false;
        Target = null;
        Reset();
    }

    public bool IsInRange()
    {
        return _inRange;
    }

    public void SetInRange(bool inRange)
    {
        _inRange = inRange;
    }

    public void AddPrecondition(string key, object value)
    {
        _preconditions.Add(new KeyValuePair<string, object>(key, value));
    }

    public void RemovePrecondition(string key)
    {
        var removePair = default(KeyValuePair<string, object>);

        foreach (var precondition in _preconditions.Where(precondition => precondition.Key.Equals(key)))
        {
            removePair = precondition;
        }
        if (!default(KeyValuePair<string, object>).Equals(removePair))
        {
            _preconditions.Remove(removePair);
        }
    }

    public void AddEffect(string key, object value)
    {
        _effects.Add(new KeyValuePair<string, object>(key, value));
    }

    public void RemoveEffect(string key)
    {
        var removePair = default(KeyValuePair<string, object>);

        foreach (var effect in _effects.Where(effect => effect.Key.Equals(key)))
        {
            removePair = effect;
        }
        if (!default(KeyValuePair<string, object>).Equals(removePair))
        {
            _effects.Remove(removePair);
        }

    }

    public abstract void Reset();

    public abstract bool IsDone();

    public abstract bool CheckProceduralPrecondition(GameObject agent);

    public abstract bool Perform(GameObject agent);

    public abstract bool RequiresInRange();
}

