using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    public Text orderText;
    public Image image;
    public Image timerImage;

    // ���� ������ �ִ� �ֹ�
    public Data.Recipe recipe;

    private void Start()
    {
        int randomCnt = Random.Range(0, 3);
        image.sprite = Managers.Resource.Load<Sprite>($"Art/Customer/Customer_{randomCnt}");

        int randomRecipe = Random.Range(0, Managers.Data.availableRecipe.Count);
        recipe = Managers.Data.availableRecipe[randomRecipe];

        // �޴� �ؽ�Ʈ
        orderText.text = $"{recipe.korName} �ּ���.";

        StartCoroutine(CoTimer());
    }

    IEnumerator CoTimer()
    {
        while (0f <= timerImage.fillAmount)
        {
            timerImage.fillAmount -= 0.0035f;
            yield return new WaitForSeconds(0.1f);
        }

        orderText.text = "�̷��� �ʰ� ���ò��� �׳� ������!";
        // ����

        Destroy(gameObject, 2f);
    }

    public void ReviewText(Define.Level level)
    {
        StopCoroutine(CoTimer());
        switch (level)
        {
            case Define.Level.Perfect:
                orderText.text = $"���� ���ϴ� �ٷ� �װſ���!";
                break;
            case Define.Level.Good:
                orderText.text = $"���־��~ �����մϴ�.";
                break;
            case Define.Level.NotBad:
                orderText.text = $"��..����� �´°� ������..";
                break;
            case Define.Level.Wrong:
                orderText.text = $"����..��ü �� �ذ̴ϱ�?";
                break;
        }
        Destroy(gameObject, 2f);
    }
}
