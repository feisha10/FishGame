using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITweenImageFill : UITween
{
    public override void WorkFunction(float process)
    {
        Img.fillAmount = Mathf.Lerp((float) StartValue, (float) EndValue, process);
    }

    public override void Reset()
    {
        Img.fillAmount = (float)_startValue;
    }

    private Image _image;

    public Image Img
    {
        get
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
            }
            return _image;
        }
    }
}
