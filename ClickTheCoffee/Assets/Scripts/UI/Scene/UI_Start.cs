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

        Managers.Sound.StopBgm();
    }

    public void Play()
    {
        Managers.Sound.Play("Effect/ring");
        // 데이터 불러오딩
        Managers.Data.LoadPlayerData();

        // 데이터 적용하기 (재료 / 음료해금 / 구매)
        for(int i = 0; i < Managers.Data.Playerdata.unlockStuffList.Count; i++)
        {
            // 재료 이름 찾아서 있으면 그놈 언락
            string stuffName = Managers.Data.Playerdata.unlockStuffList[i];
            if (Managers.Data.StuffDict.TryGetValue(stuffName, out Data.Stuff stuff))
                stuff.isLocked = false;
            else
                Debug.Log($"해당 재료를 찾을 수 없습니다 : {stuffName}");            
        }
        

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
