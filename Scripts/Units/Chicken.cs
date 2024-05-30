using Godot;
using PixelTowns.Scripts.Units.AI;

namespace PixelTowns.Units;

public partial class Chicken : Animal
{
	[Export] private StringName restAnimation;
	
	protected override void Init()
	{
		// TODO AI modules
		Ai.AddAction(new WanderAction(this));
		Ai.AddAction(new IdleAction(this));
		Ai.AddAction(new RestAction(this, OnRest));

		Ai.RandomiseUtilitySelection = true;
	}

	private void OnRest(bool isResting)
	{
		if (isResting)
		{
			Animator.AddOverride(restAnimation);	
		}
		else
		{
			Animator.RemoveOverride(restAnimation);	
		}
	}
}
