using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Start : UI_Scene
{
    SceneFader sceneFader;

    enum Buttons
    {
        StartButton,
        DeleteButton,
        QuitButton,
    }
    enum Texts
    {
        StartText,
    }

    enum Images
    {
        Black,   
    }

    private void Awake()
    {
        sceneFader = GetComponent<SceneFader>();
    }

    private void Start()
    {
        Init();
        if (Managers.Data.CheckSaveFile())
            GetText((int)Texts.StartText).text = "이어하기";
        else
            GetText((int)Texts.StartText).text = "시작하기";
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        sceneFader.img = GetImage((int)Images.Black);
        BindEvent(GetButton((int)Buttons.StartButton).gameObject, (PointerEventData data) => { Play(); });
        BindEvent(GetButton((int)Buttons.DeleteButton).gameObject, (PointerEventData data) => { Delete(); });
        BindEvent(GetButton((int)Buttons.QuitButton).gameObject, (PointerEventData data) => { Quit(); });
    }

    public void Play()
    {
        Managers.Sound.Play("Effect/ring");
        // 데이터 불러오딩
        Managers.Data.LoadPlayerData();

        sceneFader.FadeTo("Game");
    }
    
    public void Delete()
    {
        Managers.UI.ShowPopupUI<UI_Warning>();
    }

    public void Quit()
    {
#if UNITY_EDITER
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
