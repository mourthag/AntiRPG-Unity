using System.Collections.Generic;


/// <summary>
/// Every Agent needs to implement this interface. It provides necessary
/// informations fo the planner.
/// 
/// It also provides an interface to enable communication between agent and planner 
/// for feedback on success or failure
/// </summary>
public interface IGOAP
{
    /// <summary>
    /// The starting state of the Agent and the world.
    /// Supply what states are needed
    /// </summary>
    /// <returns>Starting state of the Agent and the world</returns>
    HashSet<KeyValuePair<string, object>> GetWorldState();

    /// <summary>
    /// Give the planner a goal so it can plan a way to achieve these goals
    /// </summary>
    /// <returns>Goals that this Agent wants to achieve</returns>
    HashSet<KeyValuePair<string, object>> CreateGoalState();

    /// <summary>
    /// No actions can achieve the given goal
    /// </summary>
    /// <param name="failedGoal">Goal that can't be achieved</param>
    void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

    /// <summary>
    /// A plan was found for the given goal.
    /// </summary>
    /// <param name="goal">Goal that can be achieved</param>
    /// <param name="actions">Actions that need to be performed to achieve the given goal</param>
    void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GOAPAction> actions);

    /// <summary>
    /// All actions were executed successfull
    /// </summary>
    void ActionsFinished();

    /// <summary>
    /// An action caused the plan to abort
    /// </summary>
    /// <param name="aborterAction">Action that caused the abort</param>
    void PlanAborted(GOAPAction aborterAction);

    /// <summary>
    /// Called during update
    /// </summary>
    /// <param name="nextAction">The next action the agent moves toward</param>
    /// <returns>True if the next action is reached. False if it needs further steps</returns>
    bool MoveAgent(GOAPAction nextAction);
}
