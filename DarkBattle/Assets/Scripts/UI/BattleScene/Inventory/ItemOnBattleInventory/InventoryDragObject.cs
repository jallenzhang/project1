using UnityEngine;
using System.Collections;

public class InventoryDragObject : UIDragDropItem {

    //public 
    private int count = 0;
    private bool isChangedDepth = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
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
                transform.parent = surface.transform;
                transform.localPosition = Vector3.zero;
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
                transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
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
