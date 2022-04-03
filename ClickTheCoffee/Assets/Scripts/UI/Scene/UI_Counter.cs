using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Counter : UI_Scene
{
    enum Buttons
    {
        OptionButton,
        KitchenButton,
        DrinkButton,
    }
    enum Images
    {
        DayImage,
        DrinkButton,
    }
    enum Texts
    {
        MoneyText,
        DayText,
        TimeText,
        SalesText,
    }

    [HideInInspector]public Customer currentCustomer;
    public Transform CustomerParent;

    UI_Kitchen _ui_Kitchen;
    Button _drinkButton;

    Data.Recipe _currentDrink;
    Define.Level _level;
    int _salesCost;

    // 판매량 저장
    int _dailyCustomer;
    int _dailySales;
    int _dailyCost;

    // timer
    float _minute = 0f;
    float _hour = 8f;
    string _date = "AM";
    bool isOpen = true;
    
    void Start()
    {
        Init();
        StartCoroutine(CoEnterCustomer());
    }

    public override void Init()
    {
        base.Init();
        Binding();
        DefaultSetting();
    }

    void Binding()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        BindEvent(GetButton((int)Buttons.OptionButton).gameObject, (PointerEventData data) => { ClickOptionButton(); });
        BindEvent(GetButton((int)Buttons.KitchenButton).gameObject, (PointerEventData data) => { ClickKitchenButton(); });
        BindEvent(GetButton((int)Buttons.DrinkButton).gameObject, (PointerEventData data) => { SellDrink(currentCustomer); });
    }

    void DefaultSetting()
    {
        Managers.Sound.Play("Bgm/Sunny Days - Anno Domini Beats",Define.Sound.Bgm);

        _drinkButton = GetButton((int)Buttons.DrinkButton);
        _drinkButton.gameObject.SetActive(false);

        GetImage((int)Images.DayImage).sprite = Managers.Resource.Load<Sprite>("Art/ButtonIcon/Morning");
        GetText((int)Texts.DayText).text = $"{Managers.Data.Playerdata.day}일";
        RenewText();


        // Prefab Load
        if (null == _ui_Kitchen)
        {
            _ui_Kitchen = Managers.UI.ShowPopupUI<UI_Kitchen>();
            _ui_Kitchen.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isOpen)
        {
            RenewText();
            Timer();
        }
    }

    #region UI
    void ClickOptionButton()
    {
        UI_Option ui_Option = Managers.UI.ShowPopupUI<UI_Option>();

        // Pause
        Time.timeScale = 0f;
    }

    void ClickKitchenButton()
    {
        if (null != _ui_Kitchen)
        { 
            _ui_Kitchen.gameObject.SetActive(true);
            _ui_Kitchen.SetCounter(this);
        }
    }
    public void SettingDrink(Data.Recipe recipe, Define.Level level)
    {
        if (null == _currentDrink)
            _currentDrink = recipe;
        else
        {
            Debug.Log("_currentDrink is null");
            return;
        }
        _level = level;
        _drinkButton.gameObject.SetActive(true);
        GetImage((int)Images.DrinkButton).sprite = Managers.Resource.Load<Sprite>($"Art/Cafe_Img/{_currentDrink.engName}");
        _salesCost = _currentDrink.cost;
    }

    void SellDrink(Customer customer)
    {
        if (null == _currentDrink)
            return;
        if (null == customer)
            return;

        // Order Check
        if (customer.recipe.drinkID == _currentDrink.drinkID)
        {
            customer.ReviewText(_level);
            _dailySales += 1;

            switch (_level)
            {
                case Define.Level.Perfect:
                    Managers.Data.Playerdata.money += _salesCost;
                    _dailyCost += _salesCost;
                    break;
                case Define.Level.Good:
                    Managers.Data.Playerdata.money += (int)(_salesCost * 0.8f);
                    _dailyCost += (int)(_salesCost * 0.8f);
                    break;
                case Define.Level.NotBad:
                    Managers.Data.Playerdata.money += (int)(_salesCost * 0.6f);
                    _dailyCost += (int)(_salesCost * 0.6f);
                    break;
                case Define.Level.Wrong:
                    break;
                default:
                    break;
            }
        }
        
        // Reset
        _currentDrink = null;
        GetImage((int)Images.DrinkButton).sprite = null;
        _drinkButton.gameObject.SetActive(false);

        RenewText();
    }

    public void RenewText()
    {
        GetText((int)Texts.MoneyText).text = $"{Managers.Data.Playerdata.money}";
        GetText((int)Texts.SalesText).text = $"{_dailySales}개";
    }

    void Timer()
    {
        _minute += Time.deltaTime * 2f;
        if (60f <= _minute)
        {
            _hour += 1f;
            _minute = 0f;
        }    
        GetText((int)Texts.TimeText).text = string.Format("{0}:{1:N0}" + $"{_date}", _hour, _minute);

        // 시간에 따른 아이콘 변화
        if (12f <= _hour && 16f > _hour)
            GetImage((int)Images.DayImage).sprite = Managers.Resource.Load<Sprite>("Art/ButtonIcon/Afternoon");
        if (16f <= _hour)
            GetImage((int)Images.DayImage).sprite = Managers.Resource.Load<Sprite>("Art/ButtonIcon/Evening");

        // 게임 정산 창 뜨기
        if (18f <= _hour)
        {
            // 손님 입장 금지
            StopCoroutine(CoEnterCustomer());

            UI_CloseResult ui_Result = Managers.UI.ShowPopupUI<UI_CloseResult>();
            ui_Result.SetFader(GetComponent<SceneFader>());
            ui_Result.InputTextInfo(_dailyCustomer, _dailySales, _dailyCost);

            // 하루 증가
            Managers.Data.Playerdata.day++;
        }

    }
    #endregion

    #region Customer

    IEnumerator CoEnterCustomer()
    {
        while(isOpen)
        {
            if (null == currentCustomer)
            {
                GameObject go = Managers.Resource.Instantiate("UI/Customer", CustomerParent);
                currentCustomer = go.GetComponent<Customer>();
                _dailyCustomer++;
            }
            // 추후 나중에 손님 오는 속도 증가 되면 줄어들도록.
            yield return new WaitForSeconds(5f);
        }
    }
    #endregion
}
