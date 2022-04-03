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
    Stuff currentStuff;     // ���õ� ���
    List<Stuff> stuffList = new List<Stuff>();      // �� ���ῡ �� ����� ����Ʈ
    Dictionary<Stuff, FillImage> fillObjDic = new Dictionary<Stuff, FillImage>();   // ��ᰡ �󸶳� ������ üũ�ϱ� ���� ��ųʸ�

    Dictionary<string, float> checkDic = new Dictionary<string, float>();   // ��� �̸���, ���� ��Ƶּ� ���ϱ� ���� ��ųʸ�
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
        Array array = Enum.GetValues(typeof(Stuffs));

        // Ŭ���� �� ���� ����(�̸�),��ݿ���, üũǥ��(Ȱ��,��Ȱ��)
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
                // �ر� �˾�â
            }
        }
    }

    void ClickFillButton()
    {
        // ���õ� ���� ������ return
        if (null == currentStuff)
            return;

        if (0.98f < totalAmount)
            return;

        int stuffCount = 0;
        // �� ���° ����?
        foreach (Stuff stuff in stuffList)
        {
            if (stuff != currentStuff)
                stuffCount++;
            else
                break;
        }

        // �� ���� ������ �߰��� ���� �ֽ��ϱ�?
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
            totalAmount += 0.1f;
        }
        else  // �� �ѹ��� �߰��� ���� ���ٰ�?
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
            stuffList.Add(currentStuff);    // ��� ����Ʈ�� �߰�
            fillObjDic.Add(currentStuff, fillObj);
        }
    }

    void ClickCompleteButton()
    {
        int totalStuffID = 0;
        
        // �ϼ��� ���Ḧ �м� ��, ���ο� �˾�â�� ������ ����� ��.
        // ���� ��ᰡ �� ���Գ�?
        foreach(Stuff stuff in stuffList)
        {
            float stuffAmount = 0f;
            // ����� ���̵� �� �����ݽô�
            totalStuffID += stuff.stuffID;
            // ����� ���� Ȯ��
            stuffAmount = fillObjDic[stuff].currentAmount;

            // ���� ���� ���� ����صд�.
            checkDic.Add(stuff.stuffName, stuffAmount);
        }

        // ��
        // �����Ǹ� ���ؼ� ������
        if (Managers.Data.RecipeDict.TryGetValue(totalStuffID, out Data.Recipe recipe))
        {
            float errorRate = 0f;

            // �󸶳� ������ �� �´���? 
            for (int i = 0; i < checkDic.Count; i++)
            {
                // ��� �̸� �̾ƿ�
                string stuffName = recipe.stuffList[i];
                float first = checkDic[stuffName];
                float second = recipe.amountList[i];

                // �̰� �ذ�� �𸣰ڴ�
                if (0.98f <= first)
                    first = 1f;
                if (0.98f <= second)
                    second = 1f;

                errorRate += Mathf.Abs(first - second);
            }

            UI_Complete complete = Managers.UI.ShowPopupUI<UI_Complete>();
            
            // ������ ���� �Ϻ��ϴ�?
            if (errorRate <= 0.09f)
            {
                Debug.Log("������ ���� �Ϻ��� �����Դϴ�!");
                complete.DefaultSetting(recipe, Define.Level.Perfect);
            }
            else if (0.1f <= errorRate && errorRate < 0.4f)
            {
                Debug.Log("�� ������ �̹��ϰ� ������ �� �ϱ���?");
                complete.DefaultSetting(recipe, Define.Level.Good);
            }
            else
            {
                Debug.Log("�� ������ ������ �� �ϱ���?");
                complete.DefaultSetting(recipe, Define.Level.NotBad);
            }

            // �� ����� ���� �������� �̾Ƴ���
            Debug.Log($"�� ����� {recipe.korName} �Դϴ�.");
        }
        else
        {
            Debug.Log("�� ����� �����ǿ� �����ϴ�.");
        }

        ResetAll();
    }

    void ResetAll()
    {
        // ��� ����
        foreach(Stuff stuff in stuffList)
        {
            fillObjDic.Remove(stuff);
            checkDic.Remove(stuff.stuffName);
        }
        // ��� ����Ʈ ����
        stuffList.RemoveRange(0, stuffList.Count);
    }
}
