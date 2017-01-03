using UnityEngine;
using System.Collections;

public class AbilityLogic : UILogic {

	// Use this for initialization
	void Start () {
	
	}

    public void Initialize(AbilityView view)
    {
        ItemSource = RoleManager.Instance;
        SetBinding<RoleBase>(RoleManager.CURRENTHERO, view.UpdateAbilitiesView);
    }

    public void onSelectAbility(AbilityBase ability)
    {
        if (ability != null 
            && ability.IsValid()
            && GameData.Instance.BattleSceneActionFlag.HasFlag((long)StateDef.BattleActionFlag.OnChoosingSkill))
        {
            ability.onSelect();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
