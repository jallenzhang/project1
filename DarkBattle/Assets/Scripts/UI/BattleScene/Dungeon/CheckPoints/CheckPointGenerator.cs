using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPointGenerator {
    public static DungeonPoint CreateCheckPoint()
    {
        List<RandomObject> objs = new List<RandomObject>();
        foreach(KeyValuePair<int,CheckPointData> kv in CheckPointData.dataMap)
        {
            RandomObject ro = new RandomObject();
            ro.ItemId = kv.Key;
            ro.Weight = kv.Value.weight;
            objs.Add(ro);
        }

        List<RandomObject> retObjs = ProjectHelper.GetRandomList<RandomObject>(objs, 1);
        if (retObjs == null)
            Debug.logger.LogError("DungeonPoint", "random objet occurs error");

        CommonDefine.CheckPointType cp = (CommonDefine.CheckPointType)retObjs[0].ItemId;
        switch(cp)
        {
            case CommonDefine.CheckPointType.bag:
                return new DungeonBagPoint();
            case CommonDefine.CheckPointType.bucket:
                return new DungeonBucketPoint();
            case CommonDefine.CheckPointType.coffin:
                return new DungeonCoffinPoint();
            case CommonDefine.CheckPointType.None:
                return new DungeonPoint();
        }

        return new DungeonPoint();
    }
}
