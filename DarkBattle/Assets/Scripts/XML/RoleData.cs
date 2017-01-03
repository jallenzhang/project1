using UnityEngine;
using System.Collections;

public class RoleData : XMLData<RoleData>
{
    public static readonly string fileName = "xml/role";

    public static RoleData GetRoleDataByID(int id)
    {
        if (!RoleData.dataMap.ContainsKey(id))
            return null;

        RoleData roleData = RoleData.dataMap[id];

        return roleData;
    }

    /// <summary>
    /// 人物名
    /// </summary>
    public string name { get; protected set; }

    public int style { get; protected set; }

    /// <summary>
    /// skill ID (600101 etc)
    /// </summary>
    public int skill1 { get; protected set; }

    public int skill2 { get; protected set; }

    public int skill3 { get; protected set; }

    public int skill4 { get; protected set; }

    public int skill5 { get; protected set; }

    public int skill6 { get; protected set; }

    public int skill7 { get; protected set; }

    public int hp { get; protected set; }

    public float agility { get; protected set; }
}
