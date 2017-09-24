using UnityEngine;
using System.Collections;

public static class GameSetting {

    static public void Init()
    {
        LAYER_VALUE_UI = LayerMask.NameToLayer(LAYER_NAME_UI);
        LAYER_MASK_UI = LayerMask.GetMask(LAYER_NAME_UI);

        LAYER_VALUE_DEFAULT = LayerMask.NameToLayer(LAYER_NAME_DEFAULT);
        LAYER_MASK_DEFAULT = LayerMask.GetMask(LAYER_NAME_DEFAULT);

        LAYER_VALUE_HERO = LayerMask.NameToLayer(LAYER_NAME_HERO);
        LAYER_MASK_HERO = LayerMask.GetMask(LAYER_NAME_HERO);
    }

    #region layers
    // Layer Name
    static public string LAYER_NAME_DEFAULT = "Default";
    static public string LAYER_NAME_UI = "UI";
    static public string LAYER_NAME_HERO = "Hero";

    // Layer Value
    static public int LAYER_VALUE_DEFAULT = 0;
    static public int LAYER_VALUE_UI = 0;
    static public int LAYER_VALUE_HERO = 0;

    // Layer Mask
    static public int LAYER_MASK_DEFAULT = 0;
    static public int LAYER_MASK_UI = 0;
    static public int LAYER_MASK_HERO = 0;

    #endregion
}
