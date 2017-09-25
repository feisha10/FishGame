using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITweenTextNum : UITween
{
    public override void WorkFunction(float process)
    {
        Text.text = ((int)Mathf.Lerp((int)StartValue, (int)EndValue, process)).ToString();
    }

    public override void Reset()
    {
        Text.text = _startValue.ToString();
    }

    private Text _text;

    public Text Text
    {
        get
        {
            if (_text == null)
            {
                _text = GetComponent<Text>();
            }
            return _text;
        }
    }
}
