using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // 추가적인 생성자
    public UI_Complete()
    {
        
    }
    public UI_Complete(Data.Recipe recipe, Define.Level level = Define.Level.Good)
    {
        
    }

    protected void Awake()
    {
        Init();
    }

    protected void Start()
    {
        
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
    }

    public void DefaultSetting(Data.Recipe recipe, Define.Level level = Define.Level.Good)
    {
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

}
