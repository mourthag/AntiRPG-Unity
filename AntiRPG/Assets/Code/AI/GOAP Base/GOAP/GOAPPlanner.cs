using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GOAPPlanner  {

    /**
* Plan what sequence of actions can fulfill the goal.
* Returns null if a plan could not be found, or a list of the actions
* that must be performed, in order, to fulfill the goal.
*/
    public Queue<GOAPAction> Plan(GameObject agent,
        HashSet<GOAPAction> availableActions,
        HashSet<KeyValuePair<string, object>> worldState,
        HashSet<KeyValuePair<string, object>> goal)
    {
        // reset the actions so we can start fresh with them
        foreach (var action in availableActions)
        {
            action.DoReset();
        }

        // check what actions can run using their checkProceduralPrecondition
        var usableActions = new HashSet<GOAPAction>();
        foreach (var action in availableActions.Where(action => action.CheckProceduralPrecondition(agent)))
        {
            usableActions.Add(action);
        }

        // we now have all actions that can run, stored in usableActions

        // build up the tree and record the leaf nodes that provide a solution to the goal.
        var leaves = new List<Node>();

        // build graph
        var start = new Node(null, 0, worldState, null);
        var success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            // oh no, we didn't get a plan
            Debug.Log("NO PLAN");
            return null;
        }

        // get the cheapest leaf
        Node cheapest = null;
        foreach (var leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else
            {
                if (leaf.RunningCost < cheapest.RunningCost)
                    cheapest = leaf;
            }
        }

        // get its node and work back through the parents
        var result = new List<GOAPAction>();
        var n = cheapest;
        while (n != null)
        {
            if (n.Action != null)
            {
                result.Insert(0, n.Action); // insert the action in the front
            }
            n = n.Parent;
        }
        // we now have this action list in correct order

        var queue = new Queue<GOAPAction>();
        foreach (var a in result)
        {
            queue.Enqueue(a);
        }

        // hooray we have a plan!
        return queue;
    }

    /**
	* Returns true if at least one solution was found.
	* The possible paths are stored in the leaves list. Each leaf has a
	* 'runningCost' value where the lowest cost will be the best action
	* sequence.
	*/
    private bool BuildGraph(Node parent, List<Node> leaves, HashSet<GOAPAction> usableActions, HashSet<KeyValuePair<string, object>> goal)
    {
        var foundOne = false;

        // go through each action available at this node and see if we can use it here
        foreach (var action in usableActions)
        {

            // if the parent state has the conditions for this action's preconditions, we can use it here
            if (!InState(action.Preconditions, parent.State)) continue;
            // apply the action's effects to the parent state
            var currentState = PopulateState(parent.State, action.Effects);
            //Debug.Log(GoapAgent.prettyPrint(currentState));
            var node = new Node(parent, parent.RunningCost + action.Cost, currentState, action);

            if (InState(goal, currentState))
            {
                // we found a solution!
                leaves.Add(node);
                foundOne = true;
            }
            else
            {
                // not at a solution yet, so test all the remaining actions and branch out the tree
                var subset = ActionSubset(usableActions, action);
                var found = BuildGraph(node, leaves, subset, goal);
                if (found)
                    foundOne = true;
            }
        }

        return foundOne;
    }

    /**
	* Create a subset of the actions excluding the removeMe one. Creates a new set.
	*/
    private HashSet<GOAPAction> ActionSubset(HashSet<GOAPAction> actions, GOAPAction removeMe)
    {
        var subset = new HashSet<GOAPAction>();
        foreach (var a in actions.Where(a => !a.Equals(removeMe)))
        {
            subset.Add(a);
        }
        return subset;
    }

    /**
	* Check that all items in 'test' are in 'state'. If just one does not match or is not there
	* then this returns false.
	*/
    private bool InState(HashSet<KeyValuePair<string, object>> test, HashSet<KeyValuePair<string, object>> state)
    {
        var allMatch = true;
        foreach (var t in test)
        {
            var match = Enumerable.Contains(state, t);
            if (!match)
                allMatch = false;
        }
        return allMatch;
    }

    /**
	* Apply the stateChange to the currentState
	*/
    private HashSet<KeyValuePair<string, object>> PopulateState(HashSet<KeyValuePair<string, object>> currentState, HashSet<KeyValuePair<string, object>> stateChange)
    {
        var state = new HashSet<KeyValuePair<string, object>>();
        // copy the KVPs over as new objects
        foreach (var s in currentState)
        {
            state.Add(new KeyValuePair<string, object>(s.Key, s.Value));
        }

        foreach (var change in stateChange)
        {
            // if the key exists in the current state, update the Value
            var exists = Enumerable.Contains(state, change);

            if (exists)
            {
                state.RemoveWhere((KeyValuePair<string, object> kvp) => kvp.Key.Equals(change.Key));
                var updated = new KeyValuePair<string, object>(change.Key, change.Value);
                state.Add(updated);
            }
            // if it does not exist in the current state, add it
            else
            {
                state.Add(new KeyValuePair<string, object>(change.Key, change.Value));
            }
        }
        return state;
    }

    /**
	* Used for building up the graph and holding the running costs of actions.
	*/
    private class Node
    {
        public readonly Node Parent;
        public readonly float RunningCost;
        public readonly HashSet<KeyValuePair<string, object>> State;
        public readonly GOAPAction Action;

        public Node(Node parent, float runningCost, HashSet<KeyValuePair<string, object>> state, GOAPAction action)
        {
            Parent = parent;
            RunningCost = runningCost;
            State = state;
            Action = action;
        }
    }
}
