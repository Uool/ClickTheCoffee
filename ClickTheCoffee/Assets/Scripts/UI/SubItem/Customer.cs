using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public Text orderText;
    public Image image;
    public Image timerImage;

    // 내가 가지고 있는 주문
    public Data.Recipe recipe;

    private void Start()
    {
        int randomCnt = Random.Range(0, 3);
        image.sprite = Managers.Resource.Load<Sprite>($"Art/Customer/Customer_{randomCnt}");

        int randomRecipe = Random.Range(0, Managers.Data.availableRecipe.Count);
        recipe = Managers.Data.availableRecipe[randomRecipe];

        // 메뉴 텍스트
        orderText.text = $"{recipe.korName} 주세요.";

        StartCoroutine(CoTimer());
    }

    IEnumerator CoTimer()
    {
        while (0f <= timerImage.fillAmount)
        {
            timerImage.fillAmount -= 0.0035f;
            yield return new WaitForSeconds(0.1f);
        }

        orderText.text = "이렇게 늦게 나올꺼면 그냥 갈래요!";
        // 사운드

        Destroy(gameObject, 2f);
    }

    public void ReviewText(Define.Level level)
    {
        StopCoroutine(CoTimer());
        switch (level)
        {
            case Define.Level.Perfect:
                orderText.text = $"제가 원하는 바로 그거예요!";
                break;
            case Define.Level.Good:
                orderText.text = $"맛있어요~ 감사합니다.";
                break;
            case Define.Level.NotBad:
                orderText.text = $"음..음료는 맞는거 같은데..";
                break;
            case Define.Level.Wrong:
                orderText.text = $"에엥..대체 뭘 준겁니까?";
                break;
        }
        Destroy(gameObject, 2f);
    }
}
