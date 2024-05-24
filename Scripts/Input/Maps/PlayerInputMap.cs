using System.Collections.Generic;
using Godot;
using PixelTowns.InventoryManagement;

namespace PixelTowns;

public class PlayerInputMap : GameInputMap
{
    private const string MoveLeft = "moveLeft";
    private const string MoveRight = "moveRight";
    private const string MoveUp = "moveUp";
    private const string MoveDown = "moveDown";
    
    private const string Interact = "interact";
    private const string Use = "use";
    
    public interface IObserver
    {
        void OnInteractPressed();
        void OnUsePressed();
    }

    private readonly List<IObserver> observers = new();

    public void RegisterObserver(IObserver observer) => observers.Add(observer);
    public void UnregisterObserver(IObserver observer) => observers.Remove(observer);
    
    private static readonly List<string> PlayerMap = new()
    {
        MoveUp, MoveDown, MoveLeft, MoveRight,
        Interact, Use,
    };

    protected override List<string> Actions => PlayerMap;

    protected override void OnTick()
    {
        if (Input.IsActionJustPressed(Interact)) observers.ForEach(o => o.OnInteractPressed());
        if (Input.IsActionJustPressed(Use)) observers.ForEach(o => o.OnUsePressed());
    }

    public Vector2 GetMovementAxis()
    {
        if (!IsEnabled) return Vector2.Zero;
        
        Vector2 inputAxis = new Vector2()
        {
            X = Input.GetActionStrength(MoveRight) - Input.GetActionStrength(MoveLeft),
            Y = Input.GetActionStrength(MoveDown) - Input.GetActionStrength(MoveUp)
        };
        return inputAxis.Normalized();
    }

    protected override void OnInputEvent(InputEvent @event)
	{
		if (@event is InputEventKey keyInput)
		{
			if (keyInput.Pressed)
			{
				switch (keyInput.Keycode)
				{
					case Key.Key1:
						GameManager.UI.GetUi<InventoryManager>().SelectToolbeltSlot(0);
						break;
					case Key.Key2:
						GameManager.UI.GetUi<InventoryManager>().SelectToolbeltSlot(1);
						break;
					case Key.Key3:
						GameManager.UI.GetUi<InventoryManager>().SelectToolbeltSlot(2);
						break;
					case Key.Key4:
						GameManager.UI.GetUi<InventoryManager>().SelectToolbeltSlot(3);
						break;
					case Key.Key5:
						GameManager.UI.GetUi<InventoryManager>().SelectToolbeltSlot(4);
						break;
					case Key.Key6:
						GameManager.UI.GetUi<InventoryManager>().SelectToolbeltSlot(5);
						break;
					case Key.Key7:
						GameManager.UI.GetUi<InventoryManager>().SelectToolbeltSlot(6);
						break;
					case Key.Key8:
						GameManager.UI.GetUi<InventoryManager>().SelectToolbeltSlot(7);
						break;
					case Key.Key9:
						GameManager.UI.GetUi<InventoryManager>().SelectToolbeltSlot(8);
						break;
					case Key.Key0:
						GameManager.UI.GetUi<InventoryManager>().SelectToolbeltSlot(9);
						break;
				}
			}
		}
    }
}