
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

    // 원하는 메뉴의 그림
    public Image _Image;
    // 메뉴의 정보 (음식을 냈을 때 비교해야 되는 정보)
    public Data.Stat _drinkInfo;
    // 얘가 서있는 자리
    public PositionAble _Position;

    Sprite[] _drinkSprites;
    Sprite[] _personSprites;
    bool isActive = false;

    private void OnEnable()
    {
        // 내 이미지
        _personSprites = Resources.LoadAll<Sprite>("Art/Customer/customer");
        int randomNum = UnityEngine.Random.Range(0, 50);
        GetComponent<Image>().sprite = _personSprites[randomNum];

        // 풀링에서 나오면서 활성화 될 때 마다 정보가 변경되어야 한다.
        int wantType = UnityEngine.Random.Range((int)DrinkTypes.Coffee, (int)DrinkTypes.MaxCount);
        string typeName = Enum.GetName(typeof(DrinkTypes), wantType);
        int wantLevel = UnityEngine.Random.Range(1, PlayerDrinkLevel(wantType) + 1);
        
        DrinkTypes types = (DrinkTypes)Enum.Parse(typeof(DrinkTypes), typeName);
        // 손님이 원하는 음료이름과 그 이름에 해당하는 sprite 추가
        string drinkName = FindDrinkSpriteAndName(types, wantLevel);

        // 이 손님이 원하는 음료의 정보가 담겨있다.
        //_drinkInfo = Managers.Data.DrinkDict[drinkName];
        _Image.sprite = _drinkSprites[_drinkInfo.imageNumber];

        if (isActive)
            Start();
    }

    private void Start()
    {
        isActive = true;
        // 매니저에 손님 넣어둔다
        Managers.Customers._person.Add(this);

        // 위치 생성
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
