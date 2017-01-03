using UnityEngine;
using System.Collections;

public class InventoryItemLogic : UILogic {

    private InventoryItemModel m_inventoryItemModel;
    private InventoryItemView m_view;
    public InventoryItemLogic()
    {
        m_inventoryItemModel = new InventoryItemModel();
    }

    public void Initialize(InventoryItemView view)
    {
        m_view = view;
        ItemSource = m_inventoryItemModel;
        SetBinding<int>(InventoryItemModel.INVENTORYCOUNT, view.UpdateInventroyCount);
    }

    public void AddCount(int num)
    {
        m_inventoryItemModel.InventroyCount += num;
    }

    public void GoodTrigger(RoleBase hero)
    {
        Debug.logger.Log(m_view.m_goodType.ToString());
        switch (m_view.m_goodType)
        {
            case CommonDefine.GoodType.Food:
                hero.RoleActionFlag.AddFlag((long)StateDef.PlayerActionFlag.AddHP);
                hero.m_input.HP += 1;
                break;
            case CommonDefine.GoodType.Gold:
                Debug.logger.Log("gold trigger");
                //test code 
                AddCount(-1);
                break;
        }
    }

    public override void Release()
    {
        base.Release();
    }
}
