using UnityEngine;
using System.Collections;

public class DungeonBagPoint : DungeonPoint
{
    public DungeonBagPoint():base()
    {
        CheckPointType = CommonDefine.CheckPointType.bag;
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
