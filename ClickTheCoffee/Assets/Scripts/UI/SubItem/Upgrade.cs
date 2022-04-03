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

    // ���׷��̵� �Ϸ� �� ��� �г�
    enum Gameobjects
    {
        LockPanel,
    }

    [SerializeField] string upgradeName = "";
    [SerializeField] string upgradeInfo = "";
    [SerializeField] int level;
    [SerializeField] int cost;
    [SerializeField] float totalLevel;

    // ���� ��ư
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

        // DataManager���� ������ �޾ƿ�
        //upgradeData = Managers.Data.UpgradeDict;
        player = Managers.Data.Playerdata;

        // ��ȭŸ��
        _type = (Types)Enum.Parse(typeof(Types), gameObject.name);

        // ���� �Է�
        upgradeName = upgradeData[gameObject.name].korUpgradeName;
        upgradeInfo = upgradeData[gameObject.name].info;
        // �� ���׷��̵� ����
        level = ReturnLevel(_type);
        totalLevel = upgradeData[gameObject.name].totalLevel;

        //���� ���
        cost = upgradeData[gameObject.name].cost;
        CalcCost(level);        

        GetText((int)Texts.UpgradeName).text = upgradeName;
        GetText((int)Texts.UpgradeInfo).text = upgradeInfo;
        GetText((int)Texts.UpgradeCount).text = $"{level} / {totalLevel}";

        // �ϴ� �����
        GetObject((int)Gameobjects.LockPanel).SetActive(false);
    }

    private void Update()
    {
        // �������� ���� ���ؾ���
        if (cost > player.money)
        {
            GetText((int)Texts.ButtonText).text = "�ܾ׺���!";
            GetText((int)Texts.ButtonText).fontSize = 45;
            GetButton((int)Buttons.UpgradeButton).interactable = false;
        }
        else
        {
            GetText((int)Texts.ButtonText).text = "��ȭ";
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

    // �ݾ� ���
    void CalcCost(int upgradeCount)
    {
        cost += (int)(_originalCost * 0.7f) * upgradeCount;
        GetText((int)Texts.UpgradeCost).text = $"���� : {cost}";
    }

    public void OnButtonClicked(PointerEventData data)
    {
        if (!GetButton((int)Buttons.UpgradeButton).interactable)
            return;

        // ���� ����
        Managers.Data.Playerdata.money -= cost;
        IncreaseLevel(_type, 1);
        level = ReturnLevel(_type);
        // ���� ���
        CalcCost(level);

        // �ִ� ��ȭ�� ���
        if (level == totalLevel)
        {
            GetObject((int)Gameobjects.LockPanel).SetActive(true);
            GetText((int)Texts.UpgradeCost).text = "��ȭ Max!";
        }

        GetText((int)Texts.UpgradeCount).text = $"{level} / {totalLevel}";

        // �̰� Drink���� �Լ� �۵��ϵ��� �Ѱǵ� �ɷ���?0
        if (UpgradeClicked != null)
            UpgradeClicked.Invoke();
    }
}