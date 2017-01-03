using UnityEngine;
using System.Collections;

public class ConfigManager {
    private static ConfigManager s_instance = null;
    public static ConfigManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new ConfigManager();

            return s_instance;
        }
    }

    public void Init()
    {
        LoadCoreConfig();
    }

    private void LoadCoreConfig()
    {
        LoadBehaviourTreeConfigs();
    }

    private void LoadBehaviourTreeConfigs()
    {

    }
}
