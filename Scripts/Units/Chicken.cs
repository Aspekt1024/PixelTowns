using Godot;

namespace PixelTowns.Units;

public partial class Chicken : Animal
{
	[Export] private AnimationPlayer animPlayer;
    [Export] private float timeToSit = 5000f;
	
	protected override void Init()
	{
		//timeWillSit = Time.GetTicksMsec() + timeToSit;
		animPlayer.Play("Idle");
		
		// TODO AI modules
		Ai.AddAction(new WanderAction(this));
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
