using UnityEngine;
using System.Collections;

public class OverlayItemView : MonoBehaviour {
    public GameObject selectedObj;
    public GameObject changableObj;
    public GameObject affectAttackObj;
    public GameObject affectHelpObj;
    public UIGrid grid;
    public GameObject bloodArea;
    public GameObject pressure1st;
    public GameObject pressure2nd;

    private OverlayItemLogic m_logic = null;
    private float m_currentHP = 1;
	// Use this for initialization
	void Start () {
        
	}

    public void Init(OverlayItemModel model)
    {
        if (m_logic == null)
            m_logic = new OverlayItemLogic();

        m_logic.Initialize(this, model);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        if (m_logic != null)
            m_logic.Release();
    }

    public void SelectedChanged(bool selected)
    {
        selectedObj.SetActive(selected);
    }

    public void ChangableChanged(bool changed)
    {
        changableObj.SetActive(changed);
    }

    public void IsBlood(bool blood)
    {
        if (blood)
        {
            GameObject obj = NGUITools.AddChild(grid.gameObject, (GameObject)ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/Overlay/status"));
            obj.name = "blood";
            obj.GetComponent<UISprite>().spriteName = "tray_bleed";
            StartCoroutine(RefreshGrid());
        }
        else
        {
            Transform target = grid.transform.Find("blood");
            if (target != null)
            {
                NGUITools.Destroy(target.gameObject);
                StartCoroutine(RefreshGrid());
            }
        }
    }

    public void IsTaged(bool taged)
    {
        if (taged)
        {
            GameObject obj = NGUITools.AddChild(grid.gameObject, (GameObject)ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/Overlay/status"));
            obj.name = "tag";
            obj.GetComponent<UISprite>().spriteName = "tray_tag";
            StartCoroutine(RefreshGrid());
        }
        else
        {
            Transform target = grid.transform.Find("tag");
            if (target != null)
            {
                NGUITools.Destroy(target.gameObject);
                StartCoroutine(RefreshGrid());
            }
        }
    }

    public void IsBuff(bool buff)
    {
        if (buff)
        {
            GameObject obj = NGUITools.AddChild(grid.gameObject, (GameObject)ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/Overlay/status"));
            obj.name = "buff";
            obj.GetComponent<UISprite>().spriteName = "tray_buff";
            StartCoroutine(RefreshGrid());
        }
        else
        {
            Transform target = grid.transform.Find("buff");
            if (target != null)
                NGUITools.Destroy(target.gameObject);
        }
    }

    public void IsDebuff(bool debuff)
    {
        if (debuff)
        {
            GameObject obj = NGUITools.AddChild(grid.gameObject, (GameObject)ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/Overlay/status"));
            obj.name = "debuff";
            obj.GetComponent<UISprite>().spriteName = "tray_debuff";
            StartCoroutine(RefreshGrid());
        }
        else
        {
            Transform target = grid.transform.Find("debuff");
            if (target != null)
                NGUITools.Destroy(target.gameObject);
        }
    }

    public void IsPoison(bool poison)
    {
        if (poison)
        {
            GameObject obj = NGUITools.AddChild(grid.gameObject, (GameObject)ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/Overlay/status"));
            obj.name = "poison";
            obj.GetComponent<UISprite>().spriteName = "tray_poison";
            StartCoroutine(RefreshGrid());
        }
        else
        {
            Transform target = grid.transform.Find("poison");
            if (target != null)
            {
                NGUITools.Destroy(target.gameObject);
                StartCoroutine(RefreshGrid());
            }
        }
    }

    public void IsAffectAttack(bool affected)
    {
        affectAttackObj.SetActive(affected);
    }

    public void IsAffectHelp(bool affected)
    {
        affectHelpObj.SetActive(affected);
    }

    public void HPChanged(float hp)
    {
        StartCoroutine(HPGradualChange(m_currentHP, hp, 0.5f));
    }

    /// <summary>
    /// 0-10 1st pressure bar
    /// 11-20 2nd pressure bar
    /// </summary>
    /// <param name="pressure"></param>
    public void PressureChanged(int pressure)
    {
        if (pressure <= 10)
        {
            pressure1st.GetComponent<UIProgressBar>().value = (float)pressure / 10.0f;
            pressure2nd.GetComponent<UIProgressBar>().value = 0;
        }
        else
        {
            pressure1st.GetComponent<UIProgressBar>().value = 1;
            pressure2nd.GetComponent<UIProgressBar>().value = (float)(pressure - 10) / 10.0f;
        }
    }

    IEnumerator HPGradualChange(float from, float to, float duration )
    {
        float rate = 1f / duration;
        float tmp = 0;

        while (tmp < 1.0f)
        {
            tmp += rate * Time.deltaTime;
            bloodArea.GetComponent<UIProgressBar>().value = Mathf.Lerp(from, to, tmp);
            yield return null;
        }
        m_currentHP = to;
    }

    IEnumerator RefreshGrid()
    {
        yield return new WaitForEndOfFrame();
        grid.repositionNow = true;
        grid.Reposition();
    }
}
