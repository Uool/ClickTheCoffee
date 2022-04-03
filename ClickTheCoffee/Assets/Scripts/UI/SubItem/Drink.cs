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

    // Json에서 불러올 데이터들
    [SerializeField] string drinkName = "";
    [SerializeField] int cost;
    [SerializeField] int level;             // 음료를 열 수 있는 단계
    [SerializeField] float totalAmount;
    [SerializeField] bool isLocked;
    
    // 이곳에서만 사용
    [SerializeField] const float speed = 1f;
    [SerializeField] float upgrade;         // 버튼 누르는 배수
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
        // 버튼 눌렀을 시 이벤트
        _craftButton.gameObject.BindEvent(OnButtonClicked);
    }

    public override void Init()
    {
        // 사용할 UI 바인딩
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

        // 플레이어 데이터가 필요한 영역
        isLocked = menuData[gameObject.name].isLocked;

        // 받아온 정보를 통해 정보 변경
        GetText((int)Texts.MenuName).text = drinkName;
        GetText((int)Texts.MenuCost).text = $"가격 : {cost}";

        // 기본 세팅
        CheckSpeed();
        CheckLevelUp();

        // 액션으로 등록해놓는다.
        UpgradeClicked += CheckSpeed;
        UpgradeClicked += CheckLevelUp;

        currentAmount = 0f;

        // 이거 열어요?
        GetObject((int)Gameobjects.LockPanel).SetActive(isLocked);   
    }

    // 플레이어의 음료 레벨과 현재 음료레벨과 비교하여 오픈
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
        // 한번 눌렀을 때 차오르는 양
        currentAmount += (speed + (speed * _playerSpeed * 0.2f)) / totalAmount;
        Mathf.Clamp(currentAmount, 0f, 1f);

        if (1f <= currentAmount)
        {
            currentAmount = 0f;
            // 손님이 가지고있는지 맞춰보기
            Managers.Customers.CheckMenu(gameObject.name);
        }

        GetImage((int)Images.Fill).fillAmount = currentAmount;
    }

}