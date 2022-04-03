using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_Kitchen : UI_Popup
{
    enum Buttons
    {
        MainButton,
        LiquidButton,
        FruitButton,
        EtcButton,
        FillButton,
        CancelButton,
        CompleteButton,
        GoCounterButton,
        MakeListButton,
    }
    enum GameObjects
    {
        MainPanel,
        LiquidPanel,
        FruitPanel,
        EtcPanel,
    }
    enum Transforms
    {
        FillCup,
    }

    enum Stuffs
    {
        Coffee, Choco, GreenTeaPowder, IceTeaPowder,
        Water, Milk, Vanilla_S, Choco_S, Honey_S, Condensed_Milk_S, Caramel_S, Ice, SparklingWater,
        Strawberry, Lemon, Grape, Orange,
        Sugar, Ginger, Cinnamomum, SweetPotato, IceCream, Oreo
    }

    enum Texts
    {
        StuffText,
    }

    // Check Stuff
    [HideInInspector] public Stuff currentStuff;     // ���õ� ���
    List<Stuff> stuffList = new List<Stuff>();      // �� ���ῡ �� ����� ����Ʈ
    Dictionary<Stuff, FillImage> fillObjDic = new Dictionary<Stuff, FillImage>();   // ��ᰡ �󸶳� ������ üũ�ϱ� ���� ��ųʸ�
    Dictionary<string, float> checkDic = new Dictionary<string, float>();   // ��� �̸���, ���� ��Ƶּ� ���ϱ� ���� ��ųʸ�

    // MakeList (�ѹ� ������ �� ���Ŀ��� �� �����ʿ䰡 ����)
    UI_MakeList _ui_MakeList;

    UI_Counter _ui_Counter;

    float _totalAmount;
    Text _stuffText;
    int _maxCount = Enum.GetValues(typeof(Stuffs)).Length;
    bool isStart = false;

    public void SetCounter(UI_Counter counter)
    {
        _ui_Counter = counter;
    }

    protected void Start()
    {
        Init();
    }

    public override void Init()
    {
        // Popup Canvas Create
        base.Init();

        // Select List
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Transform>(typeof(Transforms));
        Bind<Text>(typeof(Texts));

        // Stuff List
        Bind<Stuff>(typeof(Stuffs));

        BindingEvent();

        _stuffText = GetText((int)Texts.StuffText);
        _stuffText.text = "������ :";
    }

    void BindingEvent()
    {
        // Select Button
        BindEvent(GetButton((int)Buttons.MainButton).gameObject, (PointerEventData data) => { ClickSelectButton((int)GameObjects.MainPanel); });
        BindEvent(GetButton((int)Buttons.LiquidButton).gameObject, (PointerEventData data) => { ClickSelectButton((int)GameObjects.LiquidPanel); });
        BindEvent(GetButton((int)Buttons.FruitButton).gameObject, (PointerEventData data) => { ClickSelectButton((int)GameObjects.FruitPanel); });
        BindEvent(GetButton((int)Buttons.EtcButton).gameObject, (PointerEventData data) => { ClickSelectButton((int)GameObjects.EtcPanel); });

        // Stuff Button
        for (int i = 0; i < _maxCount; i++)
        {
            int temp = i;
            BindEvent(Get<Stuff>(temp).iconButton.gameObject, (PointerEventData data) => { ClickStuffIcon(temp); });
        }

        // Button
        BindEvent(GetButton((int)Buttons.FillButton).gameObject, (PointerEventData data) => { ClickFillButton(); });
        BindEvent(GetButton((int)Buttons.CancelButton).gameObject, (PointerEventData data) => { ResetAll(); });
        BindEvent(GetButton((int)Buttons.CompleteButton).gameObject, (PointerEventData data) => { ClickCompleteButton(); });
        BindEvent(GetButton((int)Buttons.GoCounterButton).gameObject, (PointerEventData data) => { ClickGoCounterButton(); });
        BindEvent(GetButton((int)Buttons.MakeListButton).gameObject, (PointerEventData data) => { ClickMakeListButton(); });
    }

    protected void Update()
    {
        if (!isStart)
        {
            // Start Panel
            ClickSelectButton((int)GameObjects.MainPanel);
            isStart = true;
        }
    }

    void ClickSelectButton(int idx)
    {
        Array array = Enum.GetValues(typeof(GameObjects));

        // ������ �����ϰ� ��� ��Ȱ��ȭ
        for (int i = 0; i < array.Length; i++)
        {
            if (i == idx)
                GetObject(i).SetActive(true);
            else
                GetObject(i).SetActive(false);
        }
    }

    void ClickStuffIcon(int idx)
    {
        // Ŭ���� �� ���� ����(�̸�),��ݿ���, üũǥ��(Ȱ��,��Ȱ��)
        for (int i = 0; i < _maxCount; i++)
        {
            if (i == idx && false == Get<Stuff>(idx).isLocked)
            {
                currentStuff = Get<Stuff>(idx);
                // CheckButton Active
                Get<Stuff>(idx).iconCheck.SetActive(true);
            }
            else if(i != idx && false == Get<Stuff>(idx).isLocked)
            {
                // CheckButton InActive
                Get<Stuff>(i).iconCheck.SetActive(false);
            }
            else if (true == Get<Stuff>(idx).isLocked)
            {
                // �ر� �˾�â
                UI_Unlock ui_Unlock = Managers.UI.ShowPopupUI<UI_Unlock>();
                ui_Unlock.DefaultSetting(Get<Stuff>(idx));
                break;
            }
        }
    }

    public void ClickFillButton()
    {
        // ���õ� ���� ������ return
        if (null == currentStuff)
            return;

        if (0.98f < _totalAmount)
            return;

        int stuffCount = 0;
  
        foreach (Stuff stuff in stuffList)
        {
            if (stuff != currentStuff)
                stuffCount++;
            else
                break;
        }

        // ������ �߰� �� ��ᰡ �ִ��� üũ
        if (stuffList.Contains(currentStuff))
        {
            // ���� ī��Ʈ
            int count = 0;
            foreach (Stuff stuff in stuffList)
            {
                if (stuff == currentStuff)
                {
                    fillObjDic[stuff].currentAmount += 0.1f;
                    fillObjDic[stuff].CurrentFillStuff();
                }
                // stuff�� �� ó�� ���� �����, �ٸ� ������ ���� �ö󰡾��Ѵ�.
                // �ݴ�� 2��°�� ���� �����, 2��° �̻��� ������� ��� + 0.1�� �Ǿ�� �Ѵ�.
                else if (stuffCount < count)
                    fillObjDic[stuff].CurrentFillStuff();

                // ���� ������ ī��Ʈ �÷��ش�.
                count++;
            }
            _totalAmount += 0.1f;
        }
        else
        {
            FillImage fillObj = Managers.Resource.Instantiate("UI/FillImage", Get<Transform>((int)Transforms.FillCup)).GetOrAddComponent<FillImage>();
            fillObj.currentAmount += 0.1f;
            fillObj.SetColor(currentStuff.color);

            if (stuffList.Count > 0)
            {
                fillObj.TotalFillStuff(_totalAmount);
                fillObj.transform.SetAsFirstSibling();
            }
            else
                fillObj.CurrentFillStuff();

            _totalAmount += 0.1f;
            stuffList.Add(currentStuff);    // ��� ����Ʈ�� �߰�
            fillObjDic.Add(currentStuff, fillObj);
        }

        // Text �߰�
        _stuffText.text = "������ :";
        for (int i = 0; i < stuffList.Count; i++)
        {
            _stuffText.text += $"\n{stuffList[i].stuffName} {(fillObjDic[stuffList[i]].currentAmount * 100f)}%";
        }
    }

    void ClickCompleteButton()
    {
        // ���Ḧ �� ������ ���ϸ� �ȉ�
        if (0.98f > _totalAmount)
            return;

        int totalStuffID = 0;

        foreach(Stuff stuff in stuffList)
        {
            float stuffAmount = 0f;
            totalStuffID += stuff.stuffID;
            stuffAmount = fillObjDic[stuff].currentAmount;

            // ���� ���� ���� ����صд�.
            checkDic.Add(stuff.stuffName, stuffAmount);
        }

        // �����ǿ� ��
        if (Managers.Data.RecipeDict.TryGetValue(totalStuffID, out Data.Recipe recipe))
        {
            float errorRate = 0f;

            for (int i = 0; i < checkDic.Count; i++)
            {
                string stuffName = recipe.korStuffList[i];
                float first = checkDic[stuffName];
                float second = recipe.amountList[i];

                if (0.98f <= first)
                    first = 1f;
                if (0.98f <= second)
                    second = 1f;

                errorRate += Mathf.Abs(first - second);
            }

            UI_Complete complete = Managers.UI.ShowPopupUI<UI_Complete>();
            complete.SetUI(_ui_Counter, this);
            
            // ������ ���� �Ϻ��ϴ�?
            if (errorRate <= 0.09f)
            {
                complete.DefaultSetting(recipe, Define.Level.Perfect);
                // �� ��� �÷��̾� �����Ϳ� ���ο� ��� ���Ḧ �����س��� �Ѵ�.
                Managers.Data.Playerdata.unlockDrinkList.Add(recipe.engName);
            }
            else if (0.1f <= errorRate && errorRate < 0.4f)
            {
                complete.DefaultSetting(recipe, Define.Level.Good);
            }
            else
            {
                complete.DefaultSetting(recipe, Define.Level.NotBad);
            }
        }
        else
        {
            Debug.Log("�� ����� �����ǿ� �����ϴ�.");
        }

        ResetAll();
    }

    void ClickGoCounterButton()
    {
        gameObject.SetActive(false);
    }

    void ClickMakeListButton()
    {
        if (null == _ui_MakeList)
        {
            _ui_MakeList = Managers.UI.ShowPopupUI<UI_MakeList>();
            _ui_MakeList.uI_Kitchen = this;
        }
        else
            _ui_MakeList.gameObject.SetActive(true);
    }

    void ResetAll()
    {
        // ��� ����
        foreach(Stuff stuff in stuffList)
        {
            // ������Ʈ ����
            Managers.Resource.Destroy(fillObjDic[stuff].gameObject);
            fillObjDic.Remove(stuff);
            checkDic.Remove(stuff.stuffName);
        }
        // ��� ����Ʈ ����
        stuffList.RemoveRange(0, stuffList.Count);

        // �ؽ�Ʈ ����
        _stuffText.text = "������ :";
        _totalAmount = 0f;
    }

    public Stuff GetStuff(string stuffName)
    {
        Stuff stuff;

        Stuffs enumStuff = (Stuffs)Enum.Parse(typeof(Stuffs), stuffName);
        stuff = Get<Stuff>((int)enumStuff);

        return stuff;
    }
}
