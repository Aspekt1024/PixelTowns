using Godot;

namespace PixelTowns.Units;

public partial class Chicken : Animal
{
	[Export] private AnimationPlayer animPlayer;
    [Export] private float timeToSit = 5000f;

    private float timeWillSit;
    private bool isSitting;
	
	public override void _Ready()
	{
		timeWillSit = Time.GetTicksMsec() + timeToSit;
		animPlayer.Play("Idle");
	}

	public override void _Process(double delta)
	{
		if (!isSitting && Time.GetTicksMsec() > timeWillSit)
		{
			animPlayer.Play("Sit");
			animPlayer.GetAnimation("Sit").LoopMode = Animation.LoopModeEnum.None;
			isSitting = true;
		}
	}
}
