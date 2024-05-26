using System.Collections.Generic;
using Godot;

namespace PixelTowns.Units;

public class AiEngine : AiAction.IObserver
{
    private readonly List<AiAction> aiActions = new();

    private AiAction currentAction;
    
    public void AddAction(AiAction action)
    {
        aiActions.Add(action);
    }

    public void Tick(double deltaTime)
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
            GD.PrintErr($"Wrong action completed! Current action = {currentAction.GetType().Name}, completed action = {action.GetType().Name}");
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
            float utility = aiActions[i].GetUtility();
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