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

    // Load Data
    public string stuffName = "";
    public int cost = 0;
    public int stuffID;
    public Color color;
    public bool isLocked;

    // Check
    [HideInInspector] public Button iconButton;
    [HideInInspector] public GameObject iconCheck;
    [HideInInspector] public GameObject lockBackground;

    // JsonData


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

        iconButton = GetButton((int)Buttons.IconButton);
        iconCheck = GetObject((int)GameObjects.IconCheck);
        lockBackground = GetObject((int)GameObjects.LockBackground);

        // Load Json Data
        stuffData = Managers.Data.StuffDict;
        stuffName = stuffData[gameObject.name].korName;
        cost = stuffData[gameObject.name].cost;
        stuffID = stuffData[gameObject.name].stuffID;
        color = new Color(stuffData[gameObject.name].color[0], stuffData[gameObject.name].color[1], stuffData[gameObject.name].color[2]);
        isLocked = stuffData[gameObject.name].isLocked;

        IconSetting();
    }

    void IconSetting()
    {
        // Inactive CheckIcon
        iconCheck.SetActive(false);
        // Check Lock
        if (false == isLocked)
            lockBackground.SetActive(false);
    }
}
