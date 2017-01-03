using UnityEngine;
using System.Collections;

public class HeroOnBattleInventoryCell : MonoBehaviour {
    public int cellPos;
	// Use this for initialization
	void Start () {
        EventService.Instance.GetEvent<HeroOnBattleInventoryCellEvent>().Subscribe(OnHeroOnBattleInventoryCellChanged);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnHeroOnBattleInventoryCellChanged(bool changed)
    {
        if (transform.childCount > 0)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Collider>().enabled = changed;
            }
        }
    }

    void OnDestroy()
    {
        EventService.Instance.GetEvent<HeroOnBattleInventoryCellEvent>().Unsubscribe(OnHeroOnBattleInventoryCellChanged);
    }
}
