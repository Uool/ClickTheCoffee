using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Complete : UI_Popup
{
    enum Buttons
    {
        GiveButton,
        ReturnButton,
    }
    enum Images
    {
        DrinkImage,
    }
    enum Texts
    {
        CoffeeNameText,
        CoffeeInfoText,
    }

    Animator _anim;
    string _imageName;
    string _infoText;
    Sprite _imageSprite;
    Define.Level _level = Define.Level.Good;
    Data.Recipe _currentDrink = new Data.Recipe();
    
    UI_Kitchen _ui_Kitchen;
    UI_Counter _ui_Counter;

    public void SetUI(UI_Counter counter, UI_Kitchen kitchen)
    {
        _ui_Kitchen = kitchen;
        _ui_Counter = counter;
    }

    protected void Awake()
    {
        Init();
    }

    public override void Init()
    {
        // Popup Canvas Create
        base.Init();

        _anim = GetComponent<Animator>();

        // Bind
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        // BindEvent
        BindEvent(GetButton((int)Buttons.ReturnButton).gameObject, (PointerEventData data) => { Managers.UI.ClosePopupUI(); });
        BindEvent(GetButton((int)Buttons.GiveButton).gameObject, (PointerEventData data) => { ClickGiveButton(); });
    }

    public void DefaultSetting(Data.Recipe recipe, Define.Level level = Define.Level.Good)
    {
        _currentDrink = recipe;
        _imageName = recipe.korName;
        _infoText = recipe.info;
        _imageSprite = Managers.Resource.Load<Sprite>($"Art/Cafe_Img/{recipe.engName}");
        _level = level;

        // Default Setting
        GetImage((int)Images.DrinkImage).sprite = _imageSprite;
        GetText((int)Texts.CoffeeNameText).text = _imageName;
        GetText((int)Texts.CoffeeInfoText).text = _infoText;
        PlayStarAnimation(_level);
    }

    void PlayStarAnimation(Define.Level level)
    {
        switch (level)
        {
            case Define.Level.Perfect:
                _anim.Play("Star_3");
                break;
            case Define.Level.Good:
                _anim.Play("Star_2");
                break;
            case Define.Level.NotBad:
                _anim.Play("Star_1");
                break;
            default:
                break;
        }
    }

    void ClickGiveButton()
    {
        // 어떤 레시피인지, Level이 얼마인지 저장을 해둬야 한다. 카운터에 전달해줘야돼
        _ui_Counter.SettingDrink(_currentDrink, _level);

        // 얘는 닫고, 부엌은 그냥 비활성화
        Managers.UI.ClosePopupUI();
        _ui_Kitchen.gameObject.SetActive(false);

    }
}
