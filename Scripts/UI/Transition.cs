using System;
using Godot;

namespace PixelTowns.UI;

public partial class Transition : CanvasLayer
{
    [Export] private CanvasModulate canvasModulate;
    
    private const float FadeDuration = 1f;

    private event Action onTransitionCompleteCallback = delegate { }; 

    private enum State
    {
        None = 0,
        FadeToBlack = 100,
        FadeToScene = 200,
        Blackout = 300,
    }
    
    private State state;
    private float ratio;

    public override void _Ready()
    {
        state = State.None;
        ratio = 0f;
        canvasModulate.Color = new Color(1f, 1f, 1f, ratio);
    }

    public void FadeToBlack(Action onComplete)
    {
        if (state is State.FadeToBlack or State.Blackout)
        {
            onComplete?.Invoke();
            return;
        }
        state = State.FadeToBlack;
        onTransitionCompleteCallback = onComplete;
    }

    public void FadeToScene(Action onComplete)
    {
        if (state is State.FadeToScene or State.None)
        {
            onComplete?.Invoke();
            return;
        }
        state = State.FadeToScene;
        onTransitionCompleteCallback = onComplete;
    }

    public override void _Process(double delta)
    {
        if (state is State.Blackout or State.None) return;

        float fadeMultiplier = state == State.FadeToBlack ? 1f : -1f;
        ratio += (float) delta * fadeMultiplier / FadeDuration;

        if (ratio >= 1f && state == State.FadeToBlack)
        {
            ratio = 1f;
            state = State.Blackout;
            onTransitionCompleteCallback?.Invoke();
            onTransitionCompleteCallback = null;
        }
        else if (ratio <= 0f && state == State.FadeToScene)
        {
            ratio = 0f;
            state = State.None;
            onTransitionCompleteCallback?.Invoke();
            onTransitionCompleteCallback = null;
        }

        canvasModulate.Color = new Color(1f, 1f, 1f, ratio);
    }
}