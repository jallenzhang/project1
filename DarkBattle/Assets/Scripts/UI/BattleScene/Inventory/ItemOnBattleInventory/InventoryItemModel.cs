using UnityEngine;
using System.Collections;

public class InventoryItemModel : NotifyPropChanged, INotifyPropChanged {

    public const string INVENTORYCOUNT = "battle_inventoryCount";


    private int m_inventroyCount;
    public int InventroyCount
    {
        get
        {
            return m_inventroyCount;
        }
        set
        {
            m_inventroyCount = value;
            m_inventroyCount = Mathf.Max(0, m_inventroyCount);
            OnPropertyChanged(INVENTORYCOUNT, m_inventroyCount);
        }
    }
}
