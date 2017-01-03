using UnityEngine;
using System.Collections;

public class DungeonCoffinPoint : DungeonPoint
{
    public DungeonCoffinPoint():base()
    {
        CheckPointType = CommonDefine.CheckPointType.coffin;
    }

    public override string DrawIcon()
    {
        return "marker_curio";
    }

    public override string DrawTexture()
    {
        return base.DrawTexture();
    }

    public override void Perform()
    {
        base.Perform();
    }
}
