using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class GOAPAgent : MonoBehaviour
{

    private FSM _stateMachine;
    private FSM.FsmState _idleState;
    private FSM.FsmState _moveToState;
    private FSM.FsmState _performActionState;

    private HashSet<GOAPAction> _availableActions;
    private Queue<GOAPAction> _currentActions;
    private IGOAP _dataProvider;
    private GOAPPlanner _planner;

    public void Start()
    {
        _stateMachine = new FSM();
        _availableActions = new HashSet<GOAPAction>();
        _currentActions = new Queue<GOAPAction>();
        _planner = new GOAPPlanner();
        FindDataProvider();
        LoadActions();
        CreateIdleState();
        CreateMoveToState();
        CreatePerformActionState();
        _stateMachine.PushState(_idleState);
    }

    public void Update()
    {
        _stateMachine.Update(gameObject);
    }

    public void AddAction(GOAPAction action)
    {
        _availableActions.Add(action);
    }

    public GOAPAction GetAction(Type actionType)
    {
        return _availableActions.FirstOrDefault(action => action.GetType() == actionType);
    }

    public void RemoveAction(GOAPAction action)
    {
        _availableActions.Remove(action);
    }

    private bool HasActionPlan()
    {
        return _currentActions.Count > 0;
    }


    private void FindDataProvider()
    {
        foreach (var comp in gameObject.GetComponents(typeof (Component)))
        {
            if (!(comp is IGOAP)) continue;

            _dataProvider = (IGOAP) comp;
            return;
        }
    }

    private void LoadActions()
    {
        var actions = gameObject.GetComponents<GOAPAction>();
        foreach (var a in actions)
        {
            _availableActions.Add(a);
        }
    }

    private void CreateIdleState()
    {
        _idleState = (obj, fsm) =>
        {
            var worldState = _dataProvider.GetWorldState();
            var goal = _dataProvider.CreateGoalState();

            var plan = _planner.Plan(gameObject, _availableActions, worldState, goal);
            if (plan != null)
            {
                _currentActions = plan;
                _dataProvider.PlanFound(goal, plan);
                fsm.PopState();
                fsm.PushState(_performActionState);
            }
            else
            {
                _dataProvider.PlanFailed(goal);
                fsm.PopState();
                fsm.PushState(_idleState);
            }
        };
    }

    private void CreateMoveToState()
    {
        _moveToState = (obj, fsm) =>
        {
            // move the game object

            var action = _currentActions.Peek();

            //Requires in range but there is no target
            if (action.RequiresInRange() && action.Target == null)
            {
                fsm.PopState(); // move
                fsm.PopState(); // perform
                fsm.PushState(_idleState);
                return;
            }

            // get the agent to move itself
            if (_dataProvider.MoveAgent(action))
            {
                fsm.PopState();
            }
        };
    }

    private void CreatePerformActionState()
    {

        _performActionState = (obj, fsm) => {

                                                if (!HasActionPlan())
                                                {
                                                    fsm.PopState();
                                                    fsm.PushState(_idleState);
                                                    _dataProvider.ActionsFinished();
                                                    return;
                                                }

                                                var action = _currentActions.Peek();
                                                if (action.IsDone())
                                                {
                                                    _currentActions.Dequeue();
                                                }

                                                if (HasActionPlan())
                                                {
                                                    action = _currentActions.Peek();
                                                    var inRange = !action.RequiresInRange() || action.IsInRange();

                                                    if (inRange)
                                                    {
                                                        var success = action.Perform(obj);
                                                        if (success) return;
                                                        fsm.PopState();
                                                        fsm.PushState(_idleState);
                                                        CreateIdleState();
                                                        _dataProvider.PlanAborted(action);
                                                    }
                                                    else
                                                    {
                                                        fsm.PushState(_moveToState);
                                                    }
                                                }
                                                else
                                                {
                                                    fsm.PopState();
                                                    fsm.PushState(_idleState);
                                                    _dataProvider.ActionsFinished();
                                                }
        };
    }
}
