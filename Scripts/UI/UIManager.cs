using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace PixelTowns.UI;

public partial class UIManager : CanvasLayer
{
    [Export] public HUD HUD;
    [Export] public Array<UiBase> UiList;
    [Export] public Transition Transition;
    
    private UiBase openUi;
    
    public interface IObserver
    {
        void OnUiOpened();
        void OnAllUiClosed();
    }

    private readonly List<IObserver> observers = new();

    public void RegisterObserver(IObserver observer) => observers.Add(observer);
    public void UnregisterObserver(IObserver observer) => observers.Remove(observer);

    public void Init(GameData gameData)
    {
        HUD.Init(gameData);
    }

    public T GetUi<T>() where T : UiBase
    {
        foreach (var ui in UiList)
        {
            if (ui is T typedUi)
            {
                return typedUi;
            }
        }
        
        GD.PrintErr($"Failed to get UI: type {typeof(T).Name} was not registered on the {nameof(UIManager)}");
        return null;
    }
    
    public void OpenUi<T>(bool force = false) where T : UiBase
    {
        UiBase ui = GetUi<T>();
        if (ui == null) return;
        
        if (openUi != null && openUi != ui)
        {
            if (force)
            {
                openUi.Close();
            }
            else
            {
                GD.Print($"Cannot open UI of type {typeof(T).Name} because UI {openUi.Name} is already open.");
                return;
            }
        }

        openUi = ui;
        ui.Open();
        observers.ForEach(o => o.OnUiOpened());
    }

    public void OnCancelPressed()
    {
        if (openUi != null)
        {
            // TODO first pass this to the open UI. If it returns false, then we close the UI, otherwise it's handled by the UI already.
            openUi.Close();
            openUi = null;
            observers.ForEach(o => o.OnAllUiClosed());
        }
    }
}