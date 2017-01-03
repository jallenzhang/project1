using UnityEngine;
using System.Collections;

public class CommonFlag {

    /// <summary>
    /// 标志值
    /// </summary>
    private long m_uValue = 0;

    public long Value
    {
        get { return m_uValue; }
        set { m_uValue = value; }
    }

    /// <summary>
    /// 增加一个标志, 非线程安全
    /// </summary>
    /// <param name="uFlag">要增加的标志</param>
    /// <returns>增加标志后的值</returns>
    public long AddFlag(long uFlag)
    {
        m_uValue |= uFlag;
        return m_uValue;
    }

    /// <summary>
    /// 移除一个标志, 非线程安全
    /// </summary>
    /// <param name="uFlag">要移除的标志</param>
    /// <returns>移除标志后的值</returns>
    public long RemoveFlag(long uFlag)
    {
        m_uValue &= ~uFlag;
        return m_uValue;
    }

    /// <summary>
    /// 是否有某标志
    /// </summary>
    /// <param name="uFlag">要检查的标志</param>
    /// <returns>如果有返回true，否则返回false</returns>
    public bool HasFlag(long uFlag)
    {
        return ((m_uValue & uFlag) != 0);
    }
}
