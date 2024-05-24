using System.Collections.Generic;

namespace PixelTowns;

public class UiInputMap : GameInputMap
{


    private static readonly List<string> UiMap = new()
    {

    };
        
    protected override List<string> Actions => UiMap;


}