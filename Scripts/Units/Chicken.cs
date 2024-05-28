using Godot;
using PixelTowns.Scripts.Units.AI;

namespace PixelTowns.Units;

public partial class Chicken : Animal
{
    [Export] private float timeToSit = 5000f;
	
	protected override void Init()
	{
		//timeWillSit = Time.GetTicksMsec() + timeToSit;
		
		// TODO AI modules
		Ai.AddAction(new WanderAction(this));
		Ai.AddAction(new IdleAction(this));
		//Ai.AddAction(new RestAction(this));
		
		Ai.SetUtilityRandomisationFactor(0.1f);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		//if (!isSitting && Time.GetTicksMsec() > timeWillSit)
		{
			//animPlayer.Play("Sit");
			//animPlayer.GetAnimation("Sit").LoopMode = Animation.LoopModeEnum.None;
			//isSitting = true;
		}
	}
}
