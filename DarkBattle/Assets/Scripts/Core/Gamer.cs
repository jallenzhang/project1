using UnityEngine;
using System.Collections;
using System;

public class Gamer : MonoBehaviour {
    public BattleScene battleSce;
    public GameObject hideRoleArea;
    [Tooltip("创建的随机地图是否支持回路")]
    public bool Loop = false;
    
    private ResMgr m_resMgr;
    private HeroGenerator m_heroGenerator;
    private EnemyGenerator m_enemyGenerator;
    private bool readDataSuccess = false;
    private static Gamer s_instance = null;
    public static Gamer Instance
    {
        get
        {
            return s_instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        s_instance = this;
        if (m_resMgr == null)
            m_resMgr = gameObject.AddComponent<ResMgr>();

        if (m_heroGenerator == null)
            m_heroGenerator = gameObject.AddComponent<HeroGenerator>();

        if (m_enemyGenerator == null)
            m_enemyGenerator = gameObject.AddComponent<EnemyGenerator>();

        Init();
    }

	// Use this for initialization
	void Start () {
        
	}

    /// <summary>
    /// read data from db and Init Hero on battle
    /// </summary>
    void Init()
    {
        if (GameData.Instance.FetchData())
        {
            readDataSuccess = true;

            RoleManager.Instance.InitHerosData(hideRoleArea);
        }
    }
	
	// Update is called once per frame
	void Update () {
        DarkBattleTimer.Instance.Update(Time.deltaTime);
	}

    void OnGUI()
    {
        //if (GUI.Button(new Rect(0, 0, 100, 40), "Fighting"))
        //{
        //    GameData.Instance.BattleSceneActionFlag.AddFlag((long)StateDef.BattleActionFlag.InFighting);
        //}
        //if (GUI.Button(new Rect(0, 60, 100, 40), "End Fight"))
        //{
        //    GameData.Instance.BattleSceneActionFlag.RemoveFlag((long)StateDef.BattleActionFlag.InFighting);
        //}

        if (readDataSuccess)
        {
            //if (GUI.Button(new Rect(0, 240, 100, 40), "Name:123"))
            //{
            //    GameData.Instance.GetCurrentUserInfo().Name = "123";
            //}

            //if (GUI.Button(new Rect(320, 0, 100, 40), "Add Role Soldier"))
            //{
            //    HeroGenerator.Instance.Generator(CommonDefine.RoleType.Soldier, 1);
            //}

            //if (GUI.Button(new Rect(320, 60, 100, 40), "Add Role Nun"))
            //{
            //    HeroGenerator.Instance.Generator(CommonDefine.RoleType.Nun, 2);
            //}

            //if (GUI.Button(new Rect(320, 120, 100, 40), "Add YeCha"))
            //{
            //    HeroGenerator.Instance.Generator(CommonDefine.RoleType.YeCha, 3);
            //}

            //if (GUI.Button(new Rect(160, 0, 100, 40), "Show GuiMianJunzi"))
            //{
            //    Enemy enemy = EnemyGenerator.Instance.Generator(CommonDefine.RoleType.GuiMianJunZi, UnityEngine.Random.Range(5, 9));
            //}

            //if (GUI.Button(new Rect(160, 0, 100, 40), "Left"))
            //{
            //    RoleManager.Instance.MoveRoleToLeft(RoleManager.Instance.SelectedHero.m_role, 1);
            //}

            //if (GUI.Button(new Rect(160, 60, 100, 40), "Right"))
            //{
            //    RoleManager.Instance.MoveRoleToRight(RoleManager.Instance.SelectedHero.m_role, 2);
            //}

            if (GUI.Button(new Rect(160, 120, 100, 40), "Blood"))
            {
                RoleManager.Instance.SelectedHero.OverlayItemModel.IsBlooding = true;
            }

            if (GUI.Button(new Rect(160, 180, 100, 40), "Poison"))
            {
                RoleManager.Instance.SelectedHero.OverlayItemModel.IsPoison = true;
            }

            if (GUI.Button(new Rect(160, 240, 100, 40), "Buffer"))
            {
                RoleManager.Instance.SelectedHero.OverlayItemModel.IsBuff = true;
            }

            //if (GUI.Button(new Rect(320, 180, 100, 40), "Into battle"))
            //{
                //if (!DungeonGenerator.Instance.CreateDungeon(5, 3, 8, StateDef.DungeonType.cove))
                //{
                //    Debug.logger.LogError("Dungeon", "Create Dungeon Map Failed!!!");
                //    return;
                //}
                //CameraFade.StartAlphaFade(Color.black, false, 3, 0, () => {
                //    battleSce.gameObject.SetActive(true);
                //    DungeonGenerator.Instance.IntoRoom(DungeonGenerator.Instance.DungeonRooms[0]); });
            //}
        }
    }
}
