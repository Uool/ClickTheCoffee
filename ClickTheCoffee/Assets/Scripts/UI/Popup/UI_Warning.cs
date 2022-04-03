using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_Warning : UI_Popup
{
    enum Buttons
    {
        YesButton,
        NoButton,
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.YesButton).gameObject, (PointerEventData data) => 
        { Managers.Data.DeletePlayerData(); Managers.UI.ClosePopupUI(); Managers.Scene.LoadScene(Define.Scene.Start); });
        BindEvent(GetButton((int)Buttons.NoButton).gameObject, (PointerEventData data) => { Managers.UI.ClosePopupUI(); });
    }
}
