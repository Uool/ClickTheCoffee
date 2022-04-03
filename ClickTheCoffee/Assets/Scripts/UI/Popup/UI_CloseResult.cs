using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI_CloseResult : UI_Popup
{
    SceneFader sceneFader;
    enum Texts
    {
        CustomerCountText,
        DrinkCountText,
        RevenueCountText
    }
    enum Buttons
    {
        NextButton
    }

    public void SetFader(SceneFader fader)
    {
        sceneFader = fader;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        BindEvent(GetButton((int)Buttons.NextButton).gameObject, (PointerEventData data) => { NextDay(); });
    }

    public void InputTextInfo(int customer, int drink, int cost)
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetText((int)Texts.CustomerCountText).text = $"{customer}��";
        GetText((int)Texts.DrinkCountText).text = $"{drink}��";
        GetText((int)Texts.RevenueCountText).text = $"{cost}��";
    }

    void NextDay()
    {
        // ���̺� �� ������
        Managers.Data.SaveJson<Data.PlayerStat>(Managers.Data.Playerdata);
        sceneFader.FadeTo("Game");
        ClosePopupUI();
    }
}
