using UnityEngine;
using UnityEngine.UI;

public class FillImage : MonoBehaviour
{
    public Image _image;

    public float currentAmount = 0;
    float Amount = 0.1f;
    
    public void SetColor(Color _color)
    {
        _image.color = _color;
    }
    public void CurrentFillStuff()
    {
        _image.fillAmount += Amount;
    }

    public void TotalFillStuff(float amount)
    {
        _image.fillAmount = currentAmount + amount;
    }
}
