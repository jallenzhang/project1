using UnityEngine;
using System.Collections;

public class OverlayItemModel : NotifyPropChanged, INotifyPropChanged {
    public const string SELECTED = "overlay_selected";
    public const string CHANGABLE = "overlay_changable";
    public const string BLOODING = "overlay_blooding";
    public const string TAGED = "overlay_taged";
    public const string BUFF = "overlay_buff";
    public const string DEBUFF = "overlay_debuff";
    public const string POISON = "overlay_poison";
    public const string OVERLAYHP = "overlay_hp";
    public const string PRESSURE = "overlay_pressure";
    public const string AFFECTATTACK = "overlay_affectAttack";
    public const string AFFECTHLEP = "overlay_affectHelp";

    private bool m_isSelected = false;
    public bool IsSelected
    {
        get
        {
            return m_isSelected;
        }
        set
        {
            m_isSelected = value;
            OnPropertyChanged(SELECTED, m_isSelected);
        }
    }

    private bool m_isChangable = false;
    public bool IsChangable
    {
        get
        {
            return m_isChangable;
        }
        set
        {
            m_isChangable = value;
            OnPropertyChanged(CHANGABLE, m_isChangable);
        }
    }

    private bool m_isBlooding = false;
    public bool IsBlooding
    {
        get
        {
            return m_isBlooding;
        }
        set
        {
            if (m_isBlooding != value)
            {
                m_isBlooding = value;
                OnPropertyChanged(BLOODING, m_isBlooding);
            }
        }
    }

    private bool m_isTaged = false;
    public bool IsTaged
    {
        get
        {
            return m_isTaged;
        }
        set
        {
            if (m_isTaged != value)
            {
                m_isTaged = value;
                OnPropertyChanged(TAGED, m_isTaged);
            }
        }
    }

    private bool m_isBuff = false;
    public bool IsBuff
    {
        get
        {
            return m_isBuff;
        }
        set
        {
            if (m_isBuff != value)
            {
                m_isBuff = value;
                OnPropertyChanged(BUFF, m_isBuff);
            }
        }
    }

    private bool m_isDebuff = false;
    public bool IsDebuff
    {
        get
        {
            return m_isDebuff;
        }
        set
        {
            if (m_isDebuff != value)
            {
                m_isDebuff = value;
                OnPropertyChanged(DEBUFF, m_isDebuff);
            }
        }
    }

    private bool m_isPoison = false;
    public bool IsPoison
    {
        get
        {
            return m_isPoison;
        }
        set
        {
            if (m_isPoison != value)
            {
                m_isPoison = value;
                OnPropertyChanged(POISON, m_isPoison);
            }
        }
    }

    private float m_hp = 1;
    public float HP
    {
        get
        {
            return m_hp;
        }
        set
        {
            m_hp = value;
            OnPropertyChanged(OVERLAYHP, m_hp);
        }
    }

    private int m_pressure = 0;
    public int Pressure
    {
        get
        {
            return m_pressure;
        }
        set
        {
            m_pressure = value;
            OnPropertyChanged(PRESSURE, m_pressure);
        }
    }

    private bool m_affectAttack = false;
    public bool AffectAttack
    {
        get
        {
            return m_affectAttack;
        }
        set
        {
            m_affectAttack = value;
            OnPropertyChanged(AFFECTATTACK, m_affectAttack);
        }
    }

    private bool m_affectHelp = false;
    public bool AffectHelp
    {
        get
        {
            return m_affectHelp;
        }
        set
        {
            m_affectHelp = value;
            OnPropertyChanged(AFFECTHLEP, m_affectHelp);
        }
    }
}
