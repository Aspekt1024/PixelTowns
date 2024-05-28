using System.Collections.Generic;
using Godot;

namespace PixelTowns.Units.AI;

public class AiEngine : AiAction.IObserver
{
    private readonly List<AiAction> aiActions = new();

    private AiAction currentAction;
    private float utilityRandomisationFactor;
    
    public void AddAction(AiAction action)
    {
        aiActions.Add(action);
    }

    /// <summary>
    /// Sets the randomisation for the utility to randomise each utility value by a percentage (where 0.1 = 10%)
    /// </summary>
    public void SetUtilityRandomisationFactor(float factor)
    {
        utilityRandomisationFactor = factor;
    }

    public void Tick(float deltaTime)
    {
        if (currentAction == null)
        {
            RunNextAction();
        }
        else
        {
            currentAction?.Tick(deltaTime);   
        }
    }

    public void OnActionComplete(AiAction action)
    {
        if (action != currentAction)
        {
            AiOverseer.LogError($"{action.Unit.Name} - wrong action completed! Current action = {currentAction.GetType().Name}, completed action = {action.GetType().Name}");
            return;
        }

        currentAction.UnregisterObserver(this);
        currentAction = null;
    }

    private void RunNextAction()
    {
        int index = -1;
        float maxUtility = float.MinValue;
        for (int i = 0; i < aiActions.Count; i++)
        {
            float utility = aiActions[i].GetUtility() * (1 + Random.Range(-1f, 1f) * utilityRandomisationFactor);
            if (utility > maxUtility)
            {
                maxUtility = utility;
                index = i;
            }
        }

        if (index >= 0)
        {
            currentAction = aiActions[index];
            currentAction.RegisterObserver(this);
            currentAction.Run();
        }
        else
        {
            GD.PrintErr($"No viable action was found. AiAction count = {aiActions.Count}");
        }
    }
}