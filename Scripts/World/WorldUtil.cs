using Godot;

namespace PixelTowns.World;

public static class WorldUtil
{
    public const int PixelsPerUnit = 16;
    
    public static Vector2 SnapToGrid(Vector2 input)
    {
        float x =  input.X - input.X % PixelsPerUnit + Mathf.Sign(input.X) * PixelsPerUnit * 0.5f;
        float y = input.Y - input.Y % PixelsPerUnit + Mathf.Sign(input.Y) * PixelsPerUnit * 0.5f;
        return new Vector2(x, y);
    }
    
    public static Vector2 RandomCellInRadius(Vector2 centrePoint, int cellRadius, bool snapToGrid = true)
    {
        Vector2 point = centrePoint + Random.InUnitCircle(cellRadius * PixelsPerUnit);
        if (snapToGrid)
        {
            return SnapToGrid(point);   
        }

        return point;
    }
}