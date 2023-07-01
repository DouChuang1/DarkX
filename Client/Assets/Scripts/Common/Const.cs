using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TxtColor
{
    Red,
    Green,
    Blue,
    Yellow
}

public enum DamageType
{
    None,
    AD=1,
    AP=2
}

public class Const  {
    private const string ColorRed = "<color=#FF0000FF>";
    private const string ColorGreen = "<color=#00FF00FF>";
    private const string ColorBlue = "<color=#00B4FFFF>";
    private const string ColorYellow = "<color=#FFFF00FF>";
    private const string ColorEnd = "</color>";

    public static string Color(string str, TxtColor c)
    {
        string result = "";
        switch (c)
        {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
        }
        return result;
    }



    public const string SceneLogin = "SceneLogin";
	public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";
    public const string BGHuangYe = "bgHuangYe";
    
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenBtn = "uiOpenPage";

    public const string UILogin = "uiLoginBtn";

	public const string UIClickBtn = "uiClickBtn";
    public const string FBEnter = "fbitem";

    public const string SceneMainCity = "SceneMainCity";

    public const int ScreenPresetHeight = 750;
    public const int ScreenPresetWeight = 1136;

    public const int ScreenOpDis = 90;

    public const int PlayerMoveSpeed = 8;
    public const int MonsterMoveSpeed = 4;

    public const float AccelerSpeed = 5;
    public const int BlendIdle = 0;
    public const int BlendWalk = 1;

    public const int MainCityMapID = 10000;

    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;

    public const int ActionBorn = 0;
    public const int ActionDie = 100;
    public const int ActionHit = 101;

    public const int ActionDefault = -1;
}
