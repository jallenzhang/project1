using UnityEngine;
using System.Collections;

public class InventoryItemView : MonoBehaviour {
    public UILabel CountLabel;
    [HideInInspector]
    public CommonDefine.GoodType m_goodType = CommonDefine.GoodType.None;

    private InventoryItemLogic m_inventoryItemLogic = null;

    void Awake()
    {
        if (m_inventoryItemLogic == null)
            m_inventoryItemLogic = new InventoryItemLogic();

        m_inventoryItemLogic.Initialize(this);
    }

	// Use this for initialization
	void Start () {
        if (GetComponent<UIButton>() == null)
            gameObject.AddComponent<UIButton>();

        EventDelegate.Add(GetComponent<UIButton>().onClick, delegate()
        {
            GoodTrigger(RoleManager.Instance.SelectedHero);
        });
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddCount(int num)
    {
        m_inventoryItemLogic.AddCount(num);
    }

    public void UpdateInventroyCount(int num)
    {
        if (num == 0)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        CountLabel.text = num.ToString();
    }

    public void GoodTrigger(RoleBase role)
    {
        m_inventoryItemLogic.GoodTrigger(role);
    }

    void OnDestroy()
    {
        if (m_inventoryItemLogic != null)
        {
            m_inventoryItemLogic.Release();
            m_inventoryItemLogic = null;
        }
    }

}
