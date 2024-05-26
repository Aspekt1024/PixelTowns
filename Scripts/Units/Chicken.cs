using Godot;

namespace PixelTowns.Units;

public partial class Chicken : Animal
{
	[Export] private AnimationPlayer animPlayer;
    [Export] private float timeToSit = 5000f;

    private readonly AiEngine ai = new ();
	
	public override void _Ready()
	{
		//timeWillSit = Time.GetTicksMsec() + timeToSit;
		animPlayer.Play("Idle");
		
		ai.AddAction(new WanderAction()); // TODO modules
	}

	public override void _Process(double delta)
	{
		ai.Tick(delta);
		
		
		//if (!isSitting && Time.GetTicksMsec() > timeWillSit)
		{
			//animPlayer.Play("Sit");
			//animPlayer.GetAnimation("Sit").LoopMode = Animation.LoopModeEnum.None;
			//isSitting = true;
		}
	}
}
