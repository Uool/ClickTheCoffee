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
        CompleteButton,
    }
    enum GameObjects
    {
        MainPanel,
        LiquidPanel,
    }
    enum Transforms
    {
        FillCup,
    }

    enum Stuffs
    {
        Coffee,
        Choco,
        Water,
        Milk,
    }

    // Check Stuff
    Stuff currentStuff;     // 선택된 재료
    List<Stuff> stuffList = new List<Stuff>();      // 한 음료에 들어간 재료의 리스트
    Dictionary<Stuff, FillImage> fillObjDic = new Dictionary<Stuff, FillImage>();   // 재료가 얼마나 들어갔는지 체크하기 위한 딕셔너리

    Dictionary<string, float> checkDic = new Dictionary<string, float>();   // 재료 이름과, 양을 담아둬서 비교하기 위한 딕셔너리
    float totalAmount;

    protected void Start()
    {
        Init();

        // Start Panel
        ClickSelectButton((int)GameObjects.MainPanel);
    }

    public override void Init()
    {
        // Popup Canvas Create
        base.Init();

        // Select List
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Transform>(typeof(Transforms));

        // Stuff List
        Bind<Stuff>(typeof(Stuffs));

        BindingEvent();
    }

    void BindingEvent()
    {
        // Select Button
        BindEvent(GetButton((int)Buttons.MainButton).gameObject, (PointerEventData data) => { ClickSelectButton((int)GameObjects.MainPanel); });
        BindEvent(GetButton((int)Buttons.LiquidButton).gameObject, (PointerEventData data) => { ClickSelectButton((int)GameObjects.LiquidPanel); });
        //BindEvent(GetButton((int)Buttons.FruitButton).gameObject, (PointerEventData data) => { ClickSelectButton((int)GameObjects.JuiceMenu); });
        //BindEvent(GetButton((int)Buttons.EtcButton).gameObject, (PointerEventData data) => { ClickSelectButton((int)GameObjects.UpgradeMenu); });

        // Stuff Button
        BindEvent(Get<Stuff>((int)Stuffs.Coffee).iconButton.gameObject, (PointerEventData data) => { ClickStuffIcon((int)Stuffs.Coffee); });
        BindEvent(Get<Stuff>((int)Stuffs.Choco).iconButton.gameObject, (PointerEventData data) => { ClickStuffIcon((int)Stuffs.Choco); });
        BindEvent(Get<Stuff>((int)Stuffs.Water).iconButton.gameObject, (PointerEventData data) => { ClickStuffIcon((int)Stuffs.Water); });
        BindEvent(Get<Stuff>((int)Stuffs.Milk).iconButton.gameObject, (PointerEventData data) => { ClickStuffIcon((int)Stuffs.Milk); });

        // FillButton
        BindEvent(GetButton((int)Buttons.FillButton).gameObject, (PointerEventData data) => { ClickFillButton(); });

        // CompleteButton
        BindEvent(GetButton((int)Buttons.CompleteButton).gameObject, (PointerEventData data) => { ClickCompleteButton(); });
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
        Array array = Enum.GetValues(typeof(Stuffs));

        // 클릭된 놈 정보 저장(이름),잠금여부, 체크표시(활성,비활성)
        for (int i = 0; i < array.Length; i++)
        {
            if (i == idx && !Get<Stuff>(idx).isLocked)
            {
                currentStuff = Get<Stuff>(idx);
                // CheckButton Active
                Get<Stuff>(idx).iconCheck.SetActive(true);
            }
            else if(i != idx && !Get<Stuff>(idx).isLocked)
            {
                // CheckButton InActive
                Get<Stuff>(i).iconCheck.SetActive(false);
            }
            else if (Get<Stuff>(idx).isLocked)
            {
                // 해금 팝업창
            }
        }
    }

    void ClickFillButton()
    {
        // 선택된 것이 없으면 return
        if (null == currentStuff)
            return;

        if (0.98f < totalAmount)
            return;

        int stuffCount = 0;
        // 얘 몇번째 재료냐?
        foreach (Stuff stuff in stuffList)
        {
            if (stuff != currentStuff)
                stuffCount++;
            else
                break;
        }

        // 이 재료는 기존에 추가된 적이 있습니까?
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
            totalAmount += 0.1f;
        }
        else  // 단 한번도 추가된 적이 없다고?
        {
            FillImage fillObj = Managers.Resource.Instantiate("UI/FillImage", Get<Transform>((int)Transforms.FillCup)).GetOrAddComponent<FillImage>();
            fillObj.currentAmount += 0.1f;
            fillObj.SetColor(currentStuff.color);

            if (stuffList.Count > 0)
            {
                fillObj.TotalFillStuff(totalAmount);
                fillObj.transform.SetAsFirstSibling();
            }
            else
                fillObj.CurrentFillStuff();

            totalAmount += 0.1f;
            stuffList.Add(currentStuff);    // 재료 리스트에 추가
            fillObjDic.Add(currentStuff, fillObj);
        }
    }

    void ClickCompleteButton()
    {
        int totalStuffID = 0;
        
        // 완성된 음료를 분석 후, 새로운 팝업창에 전달을 해줘야 함.
        // 지금 재료가 뭐 나왔냐?
        foreach(Stuff stuff in stuffList)
        {
            float stuffAmount = 0f;
            // 재료의 아이디를 다 더해줍시다
            totalStuffID += stuff.stuffID;
            // 재료의 양을 확인
            stuffAmount = fillObjDic[stuff].currentAmount;

            // 현재 재료와 양을 기록해둔다.
            checkDic.Add(stuff.stuffName, stuffAmount);
        }

        // 비교
        // 레시피를 비교해서 뒤져봐
        if (Managers.Data.RecipeDict.TryGetValue(totalStuffID, out Data.Recipe recipe))
        {
            float errorRate = 0f;

            // 얼마나 비율이 잘 맞는지? 
            for (int i = 0; i < checkDic.Count; i++)
            {
                // 재료 이름 뽑아옴
                string stuffName = recipe.stuffList[i];
                float first = checkDic[stuffName];
                float second = recipe.amountList[i];

                // 이거 해결법 모르겠다
                if (0.98f <= first)
                    first = 1f;
                if (0.98f <= second)
                    second = 1f;

                errorRate += Mathf.Abs(first - second);
            }

            UI_Complete complete = Managers.UI.ShowPopupUI<UI_Complete>();
            
            // 오차가 없이 완벽하다?
            if (errorRate <= 0.09f)
            {
                Debug.Log("오차가 없이 완벽한 음료입니다!");
                complete.DefaultSetting(recipe, Define.Level.Perfect);
            }
            else if (0.1f <= errorRate && errorRate < 0.4f)
            {
                Debug.Log("양 조절이 미묘하게 실패한 듯 하군요?");
                complete.DefaultSetting(recipe, Define.Level.Good);
            }
            else
            {
                Debug.Log("양 조절이 실패한 듯 하군요?");
                complete.DefaultSetting(recipe, Define.Level.NotBad);
            }

            // 이 음료는 무슨 음료인지 뽑아내봐
            Debug.Log($"이 음료는 {recipe.korName} 입니다.");
        }
        else
        {
            Debug.Log("이 음료는 레시피에 없습니다.");
        }

        ResetAll();
    }

    void ResetAll()
    {
        // 재료 삭제
        foreach(Stuff stuff in stuffList)
        {
            fillObjDic.Remove(stuff);
            checkDic.Remove(stuff.stuffName);
        }
        // 재료 리스트 삭제
        stuffList.RemoveRange(0, stuffList.Count);
    }
}
