using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Drink : UI_Popup
{
    enum Types
    {
        None,
        Coffee,
        Tea,
        Juice,
    }
    enum Buttons
    {
        MenuButton,
    }
    enum Texts
    {
        MenuName,
        MenuCost,
    }
    enum Images
    {
        Fill,
    }
    enum Gameobjects
    {
        LockPanel,
    }

    // Json���� �ҷ��� �����͵�
    [SerializeField] string drinkName = "";
    [SerializeField] int cost;
    [SerializeField] int level;             // ���Ḧ �� �� �ִ� �ܰ�
    [SerializeField] float totalAmount;
    [SerializeField] bool isLocked;
    
    // �̰������� ���
    [SerializeField] const float speed = 1f;
    [SerializeField] float upgrade;         // ��ư ������ ���
    [SerializeField] float currentAmount;

    Button _craftButton;
    Data.PlayerStat _player;
    Types _type = Types.None;
    float _playerSpeed;

    // Start is called before the first frame update
    protected void Start()
    {
        Init();

        _craftButton = GetButton((int)Buttons.MenuButton);
        // ��ư ������ �� �̺�Ʈ
        _craftButton.gameObject.BindEvent(OnButtonClicked);
    }

    public override void Init()
    {
        // ����� UI ���ε�
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(Gameobjects));

        //menuData = Managers.Data.DrinkDict;
        _player = Managers.Data.Playerdata;

        _type = (Types)Enum.Parse(typeof(Types), menuData[gameObject.name].type);

        drinkName = menuData[gameObject.name].korName;
        cost = menuData[gameObject.name].cost;
        level = menuData[gameObject.name].level;
        totalAmount = menuData[gameObject.name].totalAmount;

        // �÷��̾� �����Ͱ� �ʿ��� ����
        isLocked = menuData[gameObject.name].isLocked;

        // �޾ƿ� ������ ���� ���� ����
        GetText((int)Texts.MenuName).text = drinkName;
        GetText((int)Texts.MenuCost).text = $"���� : {cost}";

        // �⺻ ����
        CheckSpeed();
        CheckLevelUp();

        // �׼����� ����س��´�.
        UpgradeClicked += CheckSpeed;
        UpgradeClicked += CheckLevelUp;

        currentAmount = 0f;

        // �̰� �����?
        GetObject((int)Gameobjects.LockPanel).SetActive(isLocked);   
    }

    // �÷��̾��� ���� ������ ���� ���᷹���� ���Ͽ� ����
    void CheckLevelUp()
    {
        if (isLocked)
        {
            switch (_type)
            {
                case Types.Coffee:
                    if (level <= _player.menu_coffeeLevel)
                        isLocked = false;
                    break;
                case Types.Tea:
                    if (level <= _player.menu_TeaLevel)
                        isLocked = false;
                    break;
                case Types.Juice:
                    if (level <= _player.menu_JuiceLevel)
                        isLocked = false;
                    break;
            }
            GetObject((int)Gameobjects.LockPanel).SetActive(isLocked);
        }
    }

    void CheckSpeed()
    {
        switch (_type)
        {
            case Types.Coffee:
                _playerSpeed = Managers.Data.Playerdata.speed_CoffeeLevel;
                break;
            case Types.Tea:
                _playerSpeed = Managers.Data.Playerdata.speed_TeaLevel;
                break;
            case Types.Juice:
                _playerSpeed = Managers.Data.Playerdata.speed_JuiceLevel;
                break;
        }
    }

    public void OnButtonClicked(PointerEventData data)
    {
        // �ѹ� ������ �� �������� ��
        currentAmount += (speed + (speed * _playerSpeed * 0.2f)) / totalAmount;
        Mathf.Clamp(currentAmount, 0f, 1f);

        if (1f <= currentAmount)
        {
            currentAmount = 0f;
            // �մ��� �������ִ��� ���纸��
            Managers.Customers.CheckMenu(gameObject.name);
        }

        GetImage((int)Images.Fill).fillAmount = currentAmount;
    }

}