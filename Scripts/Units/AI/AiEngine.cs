using System.Collections.Generic;
using System.Linq;
using Godot;

namespace PixelTowns.Units.AI;

public class AiEngine : AiAction.IObserver
{
    private readonly List<AiAction> aiActions = new();

    private AiAction currentAction;
    public bool RandomiseUtilitySelection = false;
    
    public void AddAction(AiAction action)
    {
        aiActions.Add(action);
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
        if (RandomiseUtilitySelection)
        {
            index = GetRandomisedUtilityIndex();
        }
        else
        {
            index = GetMaxUtilityIndex();
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

    private int GetMaxUtilityIndex()
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
        return index;
    }

    private int GetRandomisedUtilityIndex()
    {
        if (!aiActions.Any()) return -1;
        
        float totalUtility = 0f;
        float[] utilities = new float[aiActions.Count];
        for (int i = 0; i < aiActions.Count; i++)
        {
            utilities[i] = aiActions[i].GetUtility();
            totalUtility += utilities[i];
        }

        float rand = Random.Range(0f, totalUtility);
        float counter = 0f;
        for (int i = 0; i < utilities.Length; i++)
        {
            counter += utilities[i];
            if (counter >= rand)
            {
                return i;
            }
        }

        AiOverseer.LogError("Something strange happened with the randomised utility selection randomiser. Maybe check on it.");
        return GetMaxUtilityIndex();
    }
}