using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    // 너가 가지고있어라 메뉴 데이터
    protected Dictionary<string, Data.Stat> menuData = new Dictionary<string, Data.Stat>();
    protected Dictionary<string, Data.Upgrade> upgradeData = new Dictionary<string, Data.Upgrade>();

    // 업그래이드 쪽에서 누르면 갱신되도록?
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