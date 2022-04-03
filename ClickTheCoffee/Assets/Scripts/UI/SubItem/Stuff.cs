using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Stuff : UI_Base
{
    enum Buttons
    {
        IconButton,
    }
    enum GameObjects
    {
        IconCheck,
        LockBackground,
    }
    enum Texts
    {
        IconText,
    }
    enum Images
    {
        IconButton,
    }

    // Load Data
    [HideInInspector] public string stuffName = "";
    [HideInInspector] public string stuffEngName = "";
    [HideInInspector] public int cost = 0;
    [HideInInspector] public int stuffID;
    [HideInInspector] public Color color;
    [HideInInspector] public bool isLocked;

    // Check
    public Button iconButton;
    public GameObject iconCheck;
    public GameObject lockBackground;

    Sprite _buttonSprite;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));;
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        // Load Json Data
        stuffData = Managers.Data.StuffDict;
        stuffName = stuffData[gameObject.name].korName;
        stuffEngName = stuffData[gameObject.name].engName;
        cost = stuffData[gameObject.name].cost;
        stuffID = stuffData[gameObject.name].stuffID;
        color = new Color(stuffData[gameObject.name].color[0], stuffData[gameObject.name].color[1], stuffData[gameObject.name].color[2]);
        isLocked = stuffData[gameObject.name].isLocked;

        // 버튼 이미지, 텍스트 자동삽입
        _buttonSprite = Managers.Resource.Load<Sprite>($"Art/ButtonIcon/{stuffEngName}");
        GetImage((int)Images.IconButton).sprite = _buttonSprite;
        GetText((int)Texts.IconText).text = stuffName;

        IconSetting();
    }

    public void IconSetting()
    {
        // Inactive CheckIcon
        iconCheck.SetActive(false);
        // Check Lock
        if (false == isLocked)
            lockBackground.SetActive(false);
    }
}
