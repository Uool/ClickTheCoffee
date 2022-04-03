using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class UI_Menu : UI_Popup
{
    enum SelectButtons
    {
        Select_Coffee,
        Select_Tea,
        Select_Juice,
        Select_Upgrade,
    }   
    
    // 메뉴판
    enum Gameobjects
    {
        CoffeeMenu,
        TeaMenu,
        JuiceMenu,
        UpgradeMenu,
    }

    // 커피메뉴판 슬라이드 때문에 받아온다
    enum ScrollRects
    {
        MenuBackground,
    }

    protected void Start()
    {
        Init();

        ClickSelectButton((int)Gameobjects.CoffeeMenu);
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(SelectButtons));
        Bind<GameObject>(typeof(Gameobjects));     // 메뉴판
        Bind<ScrollRect>(typeof(ScrollRects));

        // 초기 세팅 (커피 목록 나오기)
        DefaultSetting();
    }

    void DefaultSetting()
    {
        BindEvent(GetButton((int)Gameobjects.CoffeeMenu).gameObject, (PointerEventData data) => { ClickSelectButton((int)Gameobjects.CoffeeMenu); });
        BindEvent(GetButton((int)Gameobjects.TeaMenu).gameObject, (PointerEventData data) => { ClickSelectButton((int)Gameobjects.TeaMenu); });
        BindEvent(GetButton((int)Gameobjects.JuiceMenu).gameObject, (PointerEventData data) => { ClickSelectButton((int)Gameobjects.JuiceMenu); });
        BindEvent(GetButton((int)Gameobjects.UpgradeMenu).gameObject, (PointerEventData data) => { ClickSelectButton((int)Gameobjects.UpgradeMenu); });
    }

    // SelectButton을 눌렀을 경우 행동해야 하는 함수
    void ClickSelectButton(int idx)
    {
        Array array = Enum.GetValues(typeof(Gameobjects));

        // 본인을 제외하고 모두 비활성화
        for (int i = 0; i < array.Length; i++)
        {
            if (i == idx)
            {
                GetObject(i).SetActive(true);
                // 스크롤 설정
                Get<ScrollRect>((int)ScrollRects.MenuBackground).content = GetObject(i).GetComponent<RectTransform>();
            }
            else
                GetObject(i).SetActive(false);
        }        
    }

}
