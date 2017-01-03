using UnityEngine;
using System.Collections;

public class SettingManager {
    private const string USERID = "USERID";

    private static SettingManager s_instance = null;
    public static SettingManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new SettingManager();
            }

            return s_instance;
        }
    }

    public int UserID
    {
        get
        {
            return PlayerPrefs.HasKey(USERID) ? PlayerPrefs.GetInt(USERID) : 1;
        }
        set
        {
            PlayerPrefs.SetInt(USERID, value);
        }
    }
}
