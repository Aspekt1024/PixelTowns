using Godot;
using System;

public partial class Chicken : Sprite2D
{
	[Export] private AnimationPlayer animPlayer;
    [Export] private float timeToSit = 5000f;

    private float timeWillSit;
    private bool isSitting;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timeWillSit = Time.GetTicksMsec() + timeToSit;
		animPlayer.Play("Idle");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
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
