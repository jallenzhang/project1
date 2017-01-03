using UnityEngine;
using System.Collections;

public class OverlayItemLogic : UILogic {
    public void Initialize(OverlayItemView view, OverlayItemModel model)
    {
        ItemSource = model;

        SetBinding<bool>(OverlayItemModel.SELECTED, view.SelectedChanged);
        SetBinding<bool>(OverlayItemModel.CHANGABLE, view.ChangableChanged);
        SetBinding<bool>(OverlayItemModel.BLOODING, view.IsBlood);
        SetBinding<bool>(OverlayItemModel.BUFF, view.IsBuff);
        SetBinding<bool>(OverlayItemModel.TAGED, view.IsTaged);
        SetBinding<bool>(OverlayItemModel.DEBUFF, view.IsDebuff);
        SetBinding<bool>(OverlayItemModel.POISON, view.IsPoison);
        SetBinding<float>(OverlayItemModel.OVERLAYHP, view.HPChanged);
        SetBinding<int>(OverlayItemModel.PRESSURE, view.PressureChanged);
        SetBinding<bool>(OverlayItemModel.AFFECTATTACK, view.IsAffectAttack);
        SetBinding<bool>(OverlayItemModel.AFFECTHLEP, view.IsAffectHelp);
    }
}
