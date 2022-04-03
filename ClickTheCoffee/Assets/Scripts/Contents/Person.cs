
using System;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour
{
    enum DrinkTypes
    {
        Coffee,
        Tea,
        Juice,
        MaxCount,
    }

    enum Coffee
    {
        Espresso = 1,
        Americano,
        CafeLatte,
        VainillaLatte,
        CafeMocha,
    }

    enum Tea
    {
        IceTea = 1,
        GreenTea,
        RedTea,
        MilkTea,
        MintTea,
    }

    enum Juice
    {
        Apple = 1,
        Grape,
        Banana,
        Watermelon,
        Strawberry,
    }

    // ���ϴ� �޴��� �׸�
    public Image _Image;
    // �޴��� ���� (������ ���� �� ���ؾ� �Ǵ� ����)
    public Data.Stat _drinkInfo;
    // �갡 ���ִ� �ڸ�
    public PositionAble _Position;

    Sprite[] _drinkSprites;
    Sprite[] _personSprites;
    bool isActive = false;

    private void OnEnable()
    {
        // �� �̹���
        _personSprites = Resources.LoadAll<Sprite>("Art/Customer/customer");
        int randomNum = UnityEngine.Random.Range(0, 50);
        GetComponent<Image>().sprite = _personSprites[randomNum];

        // Ǯ������ �����鼭 Ȱ��ȭ �� �� ���� ������ ����Ǿ�� �Ѵ�.
        int wantType = UnityEngine.Random.Range((int)DrinkTypes.Coffee, (int)DrinkTypes.MaxCount);
        string typeName = Enum.GetName(typeof(DrinkTypes), wantType);
        int wantLevel = UnityEngine.Random.Range(1, PlayerDrinkLevel(wantType) + 1);
        
        DrinkTypes types = (DrinkTypes)Enum.Parse(typeof(DrinkTypes), typeName);
        // �մ��� ���ϴ� �����̸��� �� �̸��� �ش��ϴ� sprite �߰�
        string drinkName = FindDrinkSpriteAndName(types, wantLevel);

        // �� �մ��� ���ϴ� ������ ������ ����ִ�.
        //_drinkInfo = Managers.Data.DrinkDict[drinkName];
        _Image.sprite = _drinkSprites[_drinkInfo.imageNumber];

        if (isActive)
            Start();
    }

    private void Start()
    {
        isActive = true;
        // �Ŵ����� �մ� �־�д�
        Managers.Customers._person.Add(this);

        // ��ġ ����
        for (int i = 0; i < 5; i++)
        {
            GameObject go = GameObject.Find($"Pos_{i}");
            if (false == go.transform.GetComponent<PositionAble>().isUsing)
            {
                _Position = go.transform.GetComponent<PositionAble>();
                _Position.isUsing = true;
                transform.position = _Position.transform.position;
                break;
            }
        }        
    }

    int PlayerDrinkLevel(int type)
    {
        switch (type)
        {
            case 0:
                return Managers.Data.Playerdata.menu_coffeeLevel;
            case 1:
                return Managers.Data.Playerdata.menu_TeaLevel;
            case 2:
                return Managers.Data.Playerdata.menu_JuiceLevel;
            default:
                return 1;
        }
    }

    string FindDrinkSpriteAndName(DrinkTypes type, int level)
    {
        string name = "";
        switch (type)
        {
            case DrinkTypes.Coffee:
                _drinkSprites = Resources.LoadAll<Sprite>("Art/Cafe_img/CoffeeImg");
                name = Enum.GetName(typeof(Coffee), level);
                break;
            case DrinkTypes.Tea:
                _drinkSprites = Resources.LoadAll<Sprite>("Art/Cafe_img/Tea_Re");
                name = Enum.GetName(typeof(Tea), level);
                break;
            case DrinkTypes.Juice:
                _drinkSprites = Resources.LoadAll<Sprite>("Art/Cafe_img/Juice_Re");
                name = Enum.GetName(typeof(Juice), level);
                break;
        }

        return name;
    }
}
