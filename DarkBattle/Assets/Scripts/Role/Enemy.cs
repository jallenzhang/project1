using UnityEngine;
using System.Collections;
using Game.AIBehaviorTree;
using System;
using System.IO;

public class Enemy : MonoBehaviour {
    public RoleBase m_role;
    public SkinnedMeshRenderer skin;
    private int m_enemyId;

    private BTree tree;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (tree != null)
        {
            tree.Run(m_role.m_input);
        }
    }

    public void Init(RoleBase roleBase)
    {
        m_role = roleBase;
        m_role.RoleObject = gameObject;
        m_enemyId = roleBase.m_heroId;
        m_role.RoleActionFlag.AddFlag((long)StateDef.PlayerActionFlag.Idle);
        StartUp();
    }

    public void onUnSelect()
    {
        m_role.OverlayItemModel.IsSelected = false;
    }

    public void onSelect()
    {
        m_role.OverlayItemModel.IsSelected = true;
    }

    public void StartUp()
    {
        using (FileStream fs = new FileStream(Application.dataPath + "/Data/BTree/heroBTree.json", FileMode.Open))
        {
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);

            BTreeMgr.sInstance.Load(System.Text.Encoding.UTF8.GetString(buffer), m_enemyId.ToString());
            this.tree = BTreeMgr.sInstance.GetTree("Hero" + m_enemyId);
            if (this.tree == null)
                Debug.logger.LogError("BTREE", "can not load btree");
        }
    }
}
