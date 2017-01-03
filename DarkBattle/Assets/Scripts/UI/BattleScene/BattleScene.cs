using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleScene : MonoBehaviour {
    public GameObject m_backgroundView;
    public GameObject RoleContainer;
    public GameObject UIContainer;

    public float SceneMoveSpeed = 1f;
    private float m_speedDelta = 0f;

    private int m_targetMask;
    void Awake()
    {
        m_targetMask = LayerMask.GetMask("PlayerLayer");
    }
	// Use this for initialization
	void Start () {
        Init();
	}

    void Init()
    {
        if (!DungeonGenerator.Instance.CreateDungeon(5, 3, 8, StateDef.DungeonType.cove))
        {
            Debug.logger.LogError("Dungeon", "Create Dungeon Map Failed!!!");
            return;
        }
        CameraFade.StartAlphaFade(Color.black, false, 3, 0, () => {
            DungeonGenerator.Instance.IntoRoom(DungeonGenerator.Instance.DungeonRooms[0]);
            UIContainer.SetActive(true);
            RoleContainer.SetActive(true);
        });
    }

    void UpdatePlayerStatus()
    {
        if (!GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.InFighting))
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.A))
            {
                foreach (RoleBase player in RoleManager.Instance.RolesInBattle())
                {
                    player.RoleActionFlag.RemoveFlag((long)StateDef.PlayerActionFlag.Idle);
                    player.RoleActionFlag.AddFlag((long)StateDef.PlayerActionFlag.WalkBackward);
                    m_speedDelta = 1f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                foreach (RoleBase player in RoleManager.Instance.RolesInBattle())
                {
                    player.RoleActionFlag.RemoveFlag((long)StateDef.PlayerActionFlag.WalkBackward);
                    player.RoleActionFlag.AddFlag((long)StateDef.PlayerActionFlag.Idle);
                    m_speedDelta = 0f;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                foreach (RoleBase player in RoleManager.Instance.RolesInBattle())
                {
                    player.RoleActionFlag.RemoveFlag((long)StateDef.PlayerActionFlag.Idle);
                    player.RoleActionFlag.AddFlag((long)StateDef.PlayerActionFlag.WalkForward);
                    m_speedDelta = -1f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                foreach (RoleBase player in RoleManager.Instance.RolesInBattle())
                {
                    player.RoleActionFlag.RemoveFlag((long)StateDef.PlayerActionFlag.WalkForward);
                    player.RoleActionFlag.AddFlag((long)StateDef.PlayerActionFlag.Idle);
                    m_speedDelta = 0f;
                }
            }
#else

#endif
        }
        else
        {

        }
    }

    void SelectPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_targetMask))
        {
            if (GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.OnExchanging))
            {
                if (hit.transform.GetComponent<Hero>().m_role != RoleManager.Instance.SelectedHero
                    && hit.transform.GetComponent<Hero>().m_role.OverlayItemModel.IsChangable)
                {
                    GameData.Instance.BattleSceneActionFlag.RemoveFlag((long)StateDef.BattleActionFlag.OnExchanging);
                    //当在战斗模式下，换位置也是选择skill的一种
                    GameData.Instance.BattleSceneActionFlag.RemoveFlag((long)StateDef.BattleActionFlag.OnChoosingSkill);
                    foreach (KeyValuePair<int, RoleBase> kv in RoleManager.Instance.RoleInBattleDic)
                    {
                        kv.Value.OverlayItemModel.IsChangable = false;
                    }
                    RoleManager.Instance.ExchangeRoles(hit.transform.GetComponent<Hero>().m_role, RoleManager.Instance.SelectedHero);
                }
            }
            else
            {
                if (GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.OnChoosingSkill))
                {
                    //已经有角色进入选技能阶段不同切换
                    if (hit.transform.GetComponent<Hero>() != null 
                        && hit.transform.GetComponent<Hero>().m_role != RoleManager.Instance.SelectedHero
                        && hit.transform.GetComponent<Hero>().m_role.OverlayItemModel.AffectHelp)
                    {
                        GameData.Instance.BattleSceneActionFlag.RemoveFlag((long)StateDef.BattleActionFlag.OnChoosingSkill);
                        GameData.Instance.BattleSceneActionFlag.AddFlag((long)StateDef.BattleActionFlag.OnAttacking);
                        RoleManager.Instance.AffectRole(hit.transform.GetComponent<Hero>().m_role);
                    }

                    if (hit.transform.GetComponent<Enemy>() != null 
                        && hit.transform.GetComponent<Enemy>().m_role != RoleManager.Instance.SelectedHero
                        && hit.transform.GetComponent<Enemy>().m_role.OverlayItemModel.AffectAttack)
                    {
                        GameData.Instance.BattleSceneActionFlag.RemoveFlag((long)StateDef.BattleActionFlag.OnChoosingSkill);
                        GameData.Instance.BattleSceneActionFlag.AddFlag((long)StateDef.BattleActionFlag.OnAttacking);
                        RoleManager.Instance.AffectRole(hit.transform.GetComponent<Enemy>().m_role);
                    }
                }
                else
                {
                    if (hit.transform.GetComponent<Hero>() != null)
                        RoleManager.Instance.SelectedHero = hit.transform.GetComponent<Hero>().m_role;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        UpdatePlayerStatus();
        SelectPlayer();
	}

    void LateUpdate()
    {
        m_backgroundView.GetComponent <BackgroundView>().Move(m_speedDelta * SceneMoveSpeed);//.transform.Translate(m_speedDelta * SceneMoveSpeed * Time.deltaTime, 0, 0, Space.Self);
    }
}
