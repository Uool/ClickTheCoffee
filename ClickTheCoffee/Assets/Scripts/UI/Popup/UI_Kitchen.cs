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
    [HideInInspector] public Stuff currentStuff;     // 선택된 재료
    List<Stuff> stuffList = new List<Stuff>();      // 한 음료에 들어간 재료의 리스트
    Dictionary<Stuff, FillImage> fillObjDic = new Dictionary<Stuff, FillImage>();   // 재료가 얼마나 들어갔는지 체크하기 위한 딕셔너리
    Dictionary<string, float> checkDic = new Dictionary<string, float>();   // 재료 이름과, 양을 담아둬서 비교하기 위한 딕셔너리

    // MakeList (한번 만들어내면 그 이후에는 더 만들필요가 없지)
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
        _stuffText.text = "사용재료 :";
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

        // 본인을 제외하고 모두 비활성화
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
        // 클릭된 놈 정보 저장(이름),잠금여부, 체크표시(활성,비활성)
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
                // 해금 팝업창
                UI_Unlock ui_Unlock = Managers.UI.ShowPopupUI<UI_Unlock>();
                ui_Unlock.DefaultSetting(Get<Stuff>(idx));
                break;
            }
        }
    }

    public void ClickFillButton()
    {
        // 선택된 것이 없으면 return
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

        // 기존에 추가 된 재료가 있는지 체크
        if (stuffList.Contains(currentStuff))
        {
            // 비교할 카운트
            int count = 0;
            foreach (Stuff stuff in stuffList)
            {
                if (stuff == currentStuff)
                {
                    fillObjDic[stuff].currentAmount += 0.1f;
                    fillObjDic[stuff].CurrentFillStuff();
                }
                // stuff가 맨 처음 넣은 재료라면, 다른 재료들은 같이 올라가야한다.
                // 반대로 2번째에 넣은 재료라면, 2번째 이상의 음료들은 모두 + 0.1이 되어야 한다.
                else if (stuffCount < count)
                    fillObjDic[stuff].CurrentFillStuff();

                // 점검 끝나면 카운트 올려준다.
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
            stuffList.Add(currentStuff);    // 재료 리스트에 추가
            fillObjDic.Add(currentStuff, fillObj);
        }

        // Text 추가
        _stuffText.text = "사용재료 :";
        for (int i = 0; i < stuffList.Count; i++)
        {
            _stuffText.text += $"\n{stuffList[i].stuffName} {(fillObjDic[stuffList[i]].currentAmount * 100f)}%";
        }
    }

    void ClickCompleteButton()
    {
        // 음료를 다 만들지 못하면 안됌
        if (0.98f > _totalAmount)
            return;

        int totalStuffID = 0;

        foreach(Stuff stuff in stuffList)
        {
            float stuffAmount = 0f;
            totalStuffID += stuff.stuffID;
            stuffAmount = fillObjDic[stuff].currentAmount;

            // 현재 재료와 양을 기록해둔다.
            checkDic.Add(stuff.stuffName, stuffAmount);
        }

        // 레시피와 비교
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
            
            // 오차가 없이 완벽하다?
            if (errorRate <= 0.09f)
            {
                complete.DefaultSetting(recipe, Define.Level.Perfect);
                // 이 경우 플레이어 데이터에 새로운 언락 음료를 저장해놔야 한다.
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
            Debug.Log("이 음료는 레시피에 없습니다.");
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
        // 재료 삭제
        foreach(Stuff stuff in stuffList)
        {
            // 오브젝트 삭제
            Managers.Resource.Destroy(fillObjDic[stuff].gameObject);
            fillObjDic.Remove(stuff);
            checkDic.Remove(stuff.stuffName);
        }
        // 재료 리스트 삭제
        stuffList.RemoveRange(0, stuffList.Count);

        // 텍스트 정리
        _stuffText.text = "사용재료 :";
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
