using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;
using System.IO;
/// <summary>
/// 角色的U3D对象
/// </summary>
public class Hero : MonoBehaviour {
    public RoleBase m_role;
    public SkinnedMeshRenderer skin;
    private int m_heroId = 0;

    private BTree tree;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (tree != null)
        {
            tree.Run(m_role.m_input);
        }
	}

    public void Init(int heroId)
    {
        m_role = RoleManager.Instance.GetRoleById(heroId);
        m_role.RoleObject = gameObject;
        m_heroId = heroId;
        m_role.RoleActionFlag.AddFlag((long)StateDef.PlayerActionFlag.Idle);
        StartUp();
    }

    public void StartUp()
    {
        using (FileStream fs = new FileStream(Application.dataPath + "/Data/BTree/heroBTree.json", FileMode.Open))
        {
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            BTreeMgr.sInstance.Load(System.Text.Encoding.UTF8.GetString(buffer), m_heroId.ToString());
            this.tree = BTreeMgr.sInstance.GetTree("Hero" + m_heroId);
            if (this.tree == null)
                Debug.logger.LogError("BTREE", "can not load btree");
        }
    }

}
