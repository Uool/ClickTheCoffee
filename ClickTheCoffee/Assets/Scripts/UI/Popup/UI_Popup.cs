using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    // �ʰ� �������־�� �޴� ������
    protected Dictionary<string, Data.Stat> menuData = new Dictionary<string, Data.Stat>();
    protected Dictionary<string, Data.Upgrade> upgradeData = new Dictionary<string, Data.Upgrade>();

    // ���׷��̵� �ʿ��� ������ ���ŵǵ���?
    protected static Action UpgradeClicked = null;

    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}