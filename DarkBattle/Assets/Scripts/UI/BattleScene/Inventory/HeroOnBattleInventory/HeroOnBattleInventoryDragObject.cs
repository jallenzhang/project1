using UnityEngine;
using System.Collections;

public class HeroOnBattleInventoryDragObject : UIDragDropItem
{
    private int count = 0;
    private bool isChangedDepth = false;

    //protected override void Start()
    //{
    //    base.Start();
    //}

    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        EventService.Instance.GetEvent<HeroOnBattleInventoryCellEvent>().Publish(false);
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        EventService.Instance.GetEvent<HeroOnBattleInventoryCellEvent>().Publish(true);
        //Debug.logger.Log("surface name is: " + surface.name + " surface tag is: " + surface.tag);
        if (surface)
        {
            if ("item".Equals(surface.tag) || "item" == surface.tag)
            {
                Transform _parent = surface.transform.parent;
                surface.transform.parent = transform.parent;
                surface.transform.localPosition = Vector3.zero;

                transform.parent = _parent;
                transform.localPosition = Vector3.zero;

                UISprite sprite = transform.GetComponent<UISprite>();
                UILabel label = transform.GetComponentInChildren<UILabel>();
                sprite.depth -= 2;
                label.depth -= 2;
                isChangedDepth = false;
            }
            else if ("cell".Equals(surface.tag) || "cell" == surface.tag)
            {
                if (surface.transform.childCount != 0)
                {
                    Transform trans = surface.transform.GetChild(0);
                    trans.parent = transform.parent;
                    trans.localPosition = Vector3.zero;
                    if (transform.parent.GetComponent<HeroOnBattleInventoryCell>() != null)
                    {
                        //在战斗列表里调整位置
                        trans.GetComponent<HeroOnBattleInventoryView>().roleBase.m_playerPosition = transform.parent.GetComponent<HeroOnBattleInventoryCell>().cellPos;
                    }
                    else
                    {
                        //从战斗列表里置换到英雄列表里
                        RoleManager.Instance.RemoveRoleInBattle(trans.GetComponent<HeroOnBattleInventoryView>().roleBase.m_heroId);
                        trans.GetComponent<HeroOnBattleInventoryView>().roleBase = null;
                    }
                }

                transform.parent = surface.transform;
                transform.localPosition = Vector3.zero;
                if (transform.GetComponent<HeroOnBattleInventoryView>().roleBase == null)
                {
                    //从英雄列表拖入到上战列表里
                    RoleManager.Instance.CreateHero(transform.GetComponent<HeroOnBattleInventoryView>().roleInfo, surface.GetComponent<HeroOnBattleInventoryCell>().cellPos);
                }
                else
                {
                    //在战斗列表里调整位置
                    transform.GetComponent<HeroOnBattleInventoryView>().roleBase.m_playerPosition = surface.GetComponent<HeroOnBattleInventoryCell>().cellPos;
                }

                RoleManager.Instance.UpdateRolesOnBattleInDB();
            }
            else if ("Player".Equals(surface.tag) || "Player" == surface.tag)
            {
                Hero hero = surface.GetComponentInParent<Hero>();
                if (hero != null)
                {
                    GetComponent<InventoryItemView>().GoodTrigger(hero.m_role);
                }
            }
            else
            {
                //从战斗列表拖到用户拥有的英雄列表里
                transform.localPosition = Vector3.zero;
                RoleManager.Instance.RemoveRoleInBattle(transform.GetComponent<HeroOnBattleInventoryView>().roleInfo.id);
                transform.GetComponent<HeroOnBattleInventoryView>().roleBase = null;
                RoleManager.Instance.UpdateRolesOnBattleInDB();
            }
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
        transform.GetComponent<UISprite>().enabled = false;
        transform.GetComponent<UISprite>().enabled = true;
    }

    protected override void OnDragDropMove(Vector2 delta)
    {
        base.OnDragDropMove(delta);
        if (!isChangedDepth)
        {
            isChangedDepth = true;
            UISprite sprite = transform.GetComponent<UISprite>();
            UILabel label = transform.GetComponentInChildren<UILabel>();
            if (sprite != null)
                sprite.depth += 2;
            if (label != null)
                label.depth += 2;
        }
    }
}
