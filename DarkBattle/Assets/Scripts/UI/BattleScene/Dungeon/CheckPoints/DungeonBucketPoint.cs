using UnityEngine;
using System.Collections;

public class DungeonBucketPoint : DungeonPoint
{
    public DungeonBucketPoint() : base()
    {
        CheckPointType = CommonDefine.CheckPointType.bucket;
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
