using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Upgrade : UI_Popup
{
    enum Types
    {
        None,
        IncreasePop,
        NewCoffee,
        NewTea,
        NewJuice,
        SpeedCoffee,
        SpeedTea,
        SpeedJuice,
    }
    enum Buttons
    {
        UpgradeButton,
    }

    enum Texts
    {
        UpgradeName,
        UpgradeCost,
        UpgradeInfo,
        UpgradeCount,
        ButtonText,
    }

    // 업그레이드 완료 시 잠글 패널
    enum Gameobjects
    {
        LockPanel,
    }

    [SerializeField] string upgradeName = "";
    [SerializeField] string upgradeInfo = "";
    [SerializeField] int level;
    [SerializeField] int cost;
    [SerializeField] float totalLevel;

    // 조작 버튼
    Button _upgradeButton;

    Types _type = Types.None;
    Data.PlayerStat player;
    int _originalCost = 1000;

    protected void Start()
    {
        Init();
        _upgradeButton = GetButton((int)Buttons.UpgradeButton);
        _upgradeButton.gameObject.BindEvent(OnButtonClicked);
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(Gameobjects));

        // DataManager에서 정보를 받아옴
        //upgradeData = Managers.Data.UpgradeDict;
        player = Managers.Data.Playerdata;

        // 강화타입
        _type = (Types)Enum.Parse(typeof(Types), gameObject.name);

        // 정보 입력
        upgradeName = upgradeData[gameObject.name].korUpgradeName;
        upgradeInfo = upgradeData[gameObject.name].info;
        // 각 업그레이드 레벨
        level = ReturnLevel(_type);
        totalLevel = upgradeData[gameObject.name].totalLevel;

        //가격 계산
        cost = upgradeData[gameObject.name].cost;
        CalcCost(level);        

        GetText((int)Texts.UpgradeName).text = upgradeName;
        GetText((int)Texts.UpgradeInfo).text = upgradeInfo;
        GetText((int)Texts.UpgradeCount).text = $"{level} / {totalLevel}";

        // 일단 열어둬
        GetObject((int)Gameobjects.LockPanel).SetActive(false);
    }

    private void Update()
    {
        // 돈없으면 업글 못해야지
        if (cost > player.money)
        {
            GetText((int)Texts.ButtonText).text = "잔액부족!";
            GetText((int)Texts.ButtonText).fontSize = 45;
            GetButton((int)Buttons.UpgradeButton).interactable = false;
        }
        else
        {
            GetText((int)Texts.ButtonText).text = "강화";
            GetText((int)Texts.ButtonText).fontSize = 60;
            GetButton((int)Buttons.UpgradeButton).interactable = true;
        }

    }

    void IncreaseLevel(Types types, int levelUpCount = 0)
    {
        switch (types)
        {
            case Types.IncreasePop:
                player.popLevel += levelUpCount;
                break;  
            case Types.NewCoffee:
                player.menu_coffeeLevel += levelUpCount;
                break;
            case Types.NewTea:
                player.menu_TeaLevel += levelUpCount;
                break;
            case Types.NewJuice:
                player.menu_JuiceLevel += levelUpCount;
                break;
            case Types.SpeedCoffee:
                player.speed_CoffeeLevel += levelUpCount;
                break;
            case Types.SpeedTea:
                player.speed_TeaLevel += +levelUpCount;
                break;
            case Types.SpeedJuice:
                player.speed_JuiceLevel += levelUpCount;
                break;
        }
    }

    int ReturnLevel(Types types)
    {
        switch (types)
        {
            case Types.IncreasePop:
                return player.popLevel;
            case Types.NewCoffee:
                return player.menu_coffeeLevel;
            case Types.NewTea:
                return player.menu_TeaLevel;
            case Types.NewJuice:
                return player.menu_JuiceLevel;
            case Types.SpeedCoffee:
                return player.speed_CoffeeLevel;
            case Types.SpeedTea:
                return player.speed_TeaLevel;
            case Types.SpeedJuice:
                return player.speed_JuiceLevel;
            default:
                return 0;
        }
    }

    // 금액 계산
    void CalcCost(int upgradeCount)
    {
        cost += (int)(_originalCost * 0.7f) * upgradeCount;
        GetText((int)Texts.UpgradeCost).text = $"가격 : {cost}";
    }

    public void OnButtonClicked(PointerEventData data)
    {
        if (!GetButton((int)Buttons.UpgradeButton).interactable)
            return;

        // 레벨 증가
        Managers.Data.Playerdata.money -= cost;
        IncreaseLevel(_type, 1);
        level = ReturnLevel(_type);
        // 가격 계산
        CalcCost(level);

        // 최대 강화시 잠금
        if (level == totalLevel)
        {
            GetObject((int)Gameobjects.LockPanel).SetActive(true);
            GetText((int)Texts.UpgradeCost).text = "강화 Max!";
        }

        GetText((int)Texts.UpgradeCount).text = $"{level} / {totalLevel}";

        // 이거 Drink에서 함수 작동하도록 한건데 될려나?0
        if (UpgradeClicked != null)
            UpgradeClicked.Invoke();
    }
}