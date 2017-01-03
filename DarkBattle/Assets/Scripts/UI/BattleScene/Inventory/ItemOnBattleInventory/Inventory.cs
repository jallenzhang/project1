using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
    public GameObject[] cells;

    private bool m_isFind = false;

    void Start()
    {
        //test code
        AddItem(CommonDefine.GoodType.Supply_torch, 2);
    }
	
    public void AddItem(CommonDefine.GoodType goodType, int num = 0)
    {
        if(num > 0)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].transform.childCount > 0)
                {
                    InventoryItemView inventoryItem = cells[i].GetComponentInChildren<InventoryItemView>();
                    if (inventoryItem.m_goodType == goodType)
                    {
                        m_isFind = true;
                        inventoryItem.AddCount(num);
                        break;
                    }
                }
                else
                {
                    m_isFind = false;
                }
            }

            if (!m_isFind)
            {
                for (int j = 0; j < cells.Length; j++)
                {
                    if (cells[j].transform.childCount == 0)
                    {
                        GameObject go = NGUITools.AddChild(cells[j], (GameObject)ResMgr.Instance.LoadAssetFromResource("Prefabs/UI/Inventory/InventoryItem"));
                        go.GetComponent<UISprite>().spriteName = CommonDefine.GoodNameDic[goodType];
                        go.transform.localPosition = Vector3.zero;
                        InventoryItemView dragObjTemp = go.GetComponentInChildren<InventoryItemView>();
                        dragObjTemp.m_goodType = goodType;
                        dragObjTemp.AddCount(num);
                        break;
                    }
                }
            }
        }
    }
}
