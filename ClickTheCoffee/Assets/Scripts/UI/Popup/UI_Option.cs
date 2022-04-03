using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Option : UI_Popup
{
    enum Buttons
    {
        CloseButton,
    }

    enum Toggles
    {
        BGMOnToggle,
        BGMOffToggle,
        EffectOnToggle,
        EffectOffToggle,
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Toggle>(typeof(Toggles));

        // 창닫기
        BindEvent(GetButton((int)Buttons.CloseButton).gameObject, (PointerEventData data) => { Managers.UI.ClosePopupUI(); Time.timeScale = 1f; });

        // Toggle 이벤트
        BindEvent(Get<Toggle>((int)Toggles.BGMOnToggle).gameObject, (PointerEventData data) => { ControlToggle(Toggles.BGMOnToggle); });
        BindEvent(Get<Toggle>((int)Toggles.BGMOffToggle).gameObject, (PointerEventData data) => { ControlToggle(Toggles.BGMOffToggle); });
        BindEvent(Get<Toggle>((int)Toggles.EffectOnToggle).gameObject, (PointerEventData data) => { ControlToggle(Toggles.EffectOnToggle); });
        BindEvent(Get<Toggle>((int)Toggles.EffectOffToggle).gameObject, (PointerEventData data) => { ControlToggle(Toggles.EffectOffToggle); });

        // 사운드 초기 설정
        if (!Managers.Sound.isBgmMute)
        {
            Get<Toggle>((int)Toggles.BGMOnToggle).isOn = true;
            Get<Toggle>((int)Toggles.BGMOffToggle).isOn = false;
        }
        else
        {
            Get<Toggle>((int)Toggles.BGMOnToggle).isOn = false;
            Get<Toggle>((int)Toggles.BGMOffToggle).isOn = true;
        }

        if (!Managers.Sound.isEffectMute)
        {
            Get<Toggle>((int)Toggles.EffectOnToggle).isOn = true;
            Get<Toggle>((int)Toggles.EffectOffToggle).isOn = false;
        }
        else
        {
            Get<Toggle>((int)Toggles.EffectOnToggle).isOn = false;
            Get<Toggle>((int)Toggles.EffectOffToggle).isOn = true;
        }
    }

    void ControlToggle(Toggles type)
    {
        switch (type)
        {
            case Toggles.BGMOnToggle:
                Managers.Sound.isBgmMute = false;
                Get<Toggle>((int)type).isOn = true;
                Get<Toggle>((int)Toggles.BGMOffToggle).isOn = false;
                Managers.Sound.Play("Bgm/Meeka - Steve Adams", Define.Sound.Bgm);
                break;
            case Toggles.BGMOffToggle:
                Managers.Sound.isBgmMute = true;
                Get<Toggle>((int)type).isOn = true;
                Get<Toggle>((int)Toggles.BGMOnToggle).isOn = false;
                Managers.Sound.StopBgm();
                break;
            case Toggles.EffectOnToggle:
                Managers.Sound.isEffectMute = false;    
                Get<Toggle>((int)type).isOn = true;
                Get<Toggle>((int)Toggles.EffectOffToggle).isOn = false;
                Managers.Sound.EffectSoundCtrl(true);
                break;
            case Toggles.EffectOffToggle:
                Managers.Sound.isEffectMute = true;
                Get<Toggle>((int)type).isOn = true;
                Get<Toggle>((int)Toggles.EffectOnToggle).isOn = false;
                Managers.Sound.EffectSoundCtrl(false);
                break;
        }
    }
}
