using UnityEngine;
using System.Collections;

public class DarkBattleTimer {

    private static DarkBattleTimer s_instance = null;
    public static DarkBattleTimer Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new DarkBattleTimer();
            }

            return s_instance;
        }
    }

    private bool m_isRunning = true;
    private float m_pastTime = 0;

    private DarkBattleTimer()
    {
        m_isRunning = true;
        m_pastTime = 0;
    }

    public void Update(float timeDelta)
    {
        if (m_isRunning)
        {
            m_pastTime += timeDelta;
        }
    }

    public bool IsRunning
    {
        get
        {
            return m_isRunning;
        }
        set
        {
            m_isRunning = value;
        }
    }

    public float PastedTime
    {
        get
        {
            return m_pastTime;
        }
    }
}
