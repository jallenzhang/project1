using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoleView : MonoBehaviour {
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public Transform pos5;
    public Transform pos6;
    public Transform pos7;
    public Transform pos8;

    public OverlayView overlayView;
	// Use this for initialization
	void Start () {
        EventService.Instance.GetEvent<RolePositionChangeEvent>().Subscribe(InitRolePosition);
        EventService.Instance.GetEvent<AddEnemyInBattleEvent>().Subscribe(UpdateRole);
        UpdateRoles();
	}

    void UpdateRoles()
    {
        foreach (KeyValuePair<int, RoleBase> kv in RoleManager.Instance.RoleInBattleDic)
        {
            UpdateRole(kv.Value, false);
        }
    }

    void UpdateRole(RoleBase role, bool needTween)
    {
        UpdateRolePosition(role, needTween);
        UpdateOverlayItem(role);
    }

    void UpdateRolePosition(RoleBase role, bool needTween = true)
    {
        if (role.m_playerPosition == 0)
            role.RoleObject.transform.parent = pos1;
        else if (role.m_playerPosition == 1)
            role.RoleObject.transform.parent = pos2;
        else if (role.m_playerPosition == 2)
            role.RoleObject.transform.parent = pos3;
        else if (role.m_playerPosition == 3)
            role.RoleObject.transform.parent = pos4;
        else if (role.m_playerPosition == 4)
            role.RoleObject.transform.parent = pos5;
        else if (role.m_playerPosition == 5)
            role.RoleObject.transform.parent = pos6;
        else if (role.m_playerPosition == 6)
            role.RoleObject.transform.parent = pos7;
        else if (role.m_playerPosition == 7)
            role.RoleObject.transform.parent = pos8;

        if (needTween)
            StartCoroutine(Move(role.RoleObject, role.RoleObject.transform.localPosition, Vector3.zero, 1f));
        else
            role.RoleObject.transform.localPosition = Vector3.zero;

        role.RoleObject.transform.localScale = Vector3.one;
    }

    void InitRolePosition()
    {
        foreach(KeyValuePair<int,RoleBase> kv in RoleManager.Instance.RoleInBattleDic)
        {
            UpdateRolePosition(kv.Value);
        }
    }

    void UpdateOverlayItem(RoleBase role)
    {
        role.InitOverlayItem(overlayView.gameObject);
    }
	
    IEnumerator Move(GameObject target, Vector3 from, Vector3 to, float duration)
    {
        float rate = 1f / duration;
        float tmp = 0;

        while (tmp < 1.0f)
        {
            tmp += rate * Time.deltaTime;
            target.transform.localPosition = Vector3.Lerp(from, to, tmp);
            yield return null;
        }

        DarkBattleTimer.Instance.IsRunning = true;
    }

	// Update is called once per frame
	void Update () {
        UpdateOverlayPositions();
	}

    void OnDestroy()
    {
        EventService.Instance.GetEvent<RolePositionChangeEvent>().Unsubscribe(InitRolePosition);
        EventService.Instance.GetEvent<AddEnemyInBattleEvent>().Unsubscribe(UpdateRole);
    }

    private void UpdateOverlayPositions()
    {
        foreach (KeyValuePair<int, RoleBase> kv in RoleManager.Instance.RoleInBattleDic)
        {
            kv.Value.UpdateOverlayItemPosition(overlayView);
        }
    }
}
