using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResMgr : MonoBehaviour {
    private static ResMgr s_Instance = null;
    private Dictionary<string, AssetPack> m_dicAsset = new Dictionary<string, AssetPack>();

    void Awake()
    {
        s_Instance = this;
    }

    public static ResMgr Instance
    {
        get
        {
            return s_Instance;
        }
    }

    public static bool IsValid
    {
        get
        {
            return s_Instance != null;
        }
    }

    public UnityEngine.Object LoadAssetFromResource(string assetName, bool isKeepInMemory = false, System.Type type = null)
    {
        AssetPack assetPack = null;
        if (!m_dicAsset.TryGetValue(assetName, out assetPack) || assetPack == null)
        {
            assetPack = _LoadAssetFromResource(assetName, isKeepInMemory, type);
            if (assetPack != null) AddAssetToTile(assetName);
        }
        //如果指明了要keepInMemory//
        if (assetPack != null) assetPack.isKeepInMemory = isKeepInMemory ? isKeepInMemory : assetPack.isKeepInMemory;
        return assetPack.asset;
    }

    /// <summary>
    /// 将资源加入层级
    /// </summary>
    /// <param name="assetName"></param>
    private void AddAssetToTile(string assetName)
    {
        if (m_assetStack.Count == 0)
        {
            m_assetStack.Push(new List<string>());
        }
        List<string> assetTile = m_assetStack.Peek();
        if (!assetTile.Contains(assetName))
        {
            assetTile.Add(assetName);
        }
    }

    private AssetPack _LoadAssetFromResource(string assetName, bool isKeepInMemory, System.Type type)
    {
        AssetPack ret = null;
        UnityEngine.Object asset = null;
        if (type != null)
        {
            asset = Resources.Load(assetName, type);
        }
        else
        {
            asset = Resources.Load(assetName);
        }

        if (asset != null)
        {
            if (!m_dicAsset.ContainsKey(assetName))
            {
                ret = new AssetPack(asset, isKeepInMemory);
                m_dicAsset.Add(assetName, ret);
            }
            else
            {
                ret = m_dicAsset[assetName];
                Debug.logger.LogWarning("ResMgr","ResMgr._LoadAssetFromResource: asset name->" + assetName + ", already in asset pool");
            }
        }
        else
        {
            Debug.logger.LogError("ResMgr", "ResMgr._LoadAssetFromResource: assetName->" + assetName + ", finds no asset!");
        }
        return ret;
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// 每一层的资源使用统计
    /// </summary>
    private Stack<List<string>> m_assetStack = new Stack<List<string>>();

    private class AssetPack
    {
        /// <summary>
        /// 是否常驻内存
        /// </summary>
        public bool isKeepInMemory = false;

        /// <summary>
        /// 资源
        /// </summary>
        public UnityEngine.Object asset = null;

        /// <summary>
        /// 有多少层再使用本资源
        /// </summary>
        public int stackCount = 0;

        public AssetPack(UnityEngine.Object asse, bool keepInMemory)
        {
            asset = asse;
            isKeepInMemory = keepInMemory;
        }
    }
}
