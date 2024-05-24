using PixelTowns.UI;

namespace PixelTowns;

public class GameState : UIManager.IObserver
{
    /// <summary>
    /// Called once the entire game is loaded and ready to begin playing
    /// </summary>
    public void StartGame()
    {
        GameManager.Input.EnableDefaultMaps();
        GameManager.UI.RegisterObserver(this);
    }

    public void OnUiOpened()
    {
        GameManager.Input.PlayerMap.Disable();
        GameManager.Input.UiMap.Enable();
    }

    public void OnAllUiClosed()
    {
        GameManager.Input.PlayerMap.Enable();
        GameManager.Input.UiMap.Disable();
    }
}