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
    
    // �޴���
    enum Gameobjects
    {
        CoffeeMenu,
        TeaMenu,
        JuiceMenu,
        UpgradeMenu,
    }

    // Ŀ�Ǹ޴��� �����̵� ������ �޾ƿ´�
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
        Bind<GameObject>(typeof(Gameobjects));     // �޴���
        Bind<ScrollRect>(typeof(ScrollRects));

        // �ʱ� ���� (Ŀ�� ��� ������)
        DefaultSetting();
    }

    void DefaultSetting()
    {
        BindEvent(GetButton((int)Gameobjects.CoffeeMenu).gameObject, (PointerEventData data) => { ClickSelectButton((int)Gameobjects.CoffeeMenu); });
        BindEvent(GetButton((int)Gameobjects.TeaMenu).gameObject, (PointerEventData data) => { ClickSelectButton((int)Gameobjects.TeaMenu); });
        BindEvent(GetButton((int)Gameobjects.JuiceMenu).gameObject, (PointerEventData data) => { ClickSelectButton((int)Gameobjects.JuiceMenu); });
        BindEvent(GetButton((int)Gameobjects.UpgradeMenu).gameObject, (PointerEventData data) => { ClickSelectButton((int)Gameobjects.UpgradeMenu); });
    }

    // SelectButton�� ������ ��� �ൿ�ؾ� �ϴ� �Լ�
    void ClickSelectButton(int idx)
    {
        Array array = Enum.GetValues(typeof(Gameobjects));

        // ������ �����ϰ� ��� ��Ȱ��ȭ
        for (int i = 0; i < array.Length; i++)
        {
            if (i == idx)
            {
                GetObject(i).SetActive(true);
                // ��ũ�� ����
                Get<ScrollRect>((int)ScrollRects.MenuBackground).content = GetObject(i).GetComponent<RectTransform>();
            }
            else
                GetObject(i).SetActive(false);
        }        
    }

}
