using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeExplosion : MonoBehaviour
{
    [SerializeField]
    ParticleSystem m_spark;

    [SerializeField]
    ParticleSystem m_light;

    public void SetColorSprite(Sprite sprite)
    {
        Color color = GetColorFromSprite(sprite);
        var main = m_spark.main;
        main.startColor = color;
        main = m_light.main;
        main.startColor = color;
    }

    private Color GetColorFromSprite(Sprite sprite)
    {
        Color color = Color.white;
        switch (sprite.name)
        {
            case "1":
                ColorUtility.TryParseHtmlString("#0083E0", out color);
                break;
            case "2":
                ColorUtility.TryParseHtmlString("#DF7710", out color);
                break;                
            case "3":
                ColorUtility.TryParseHtmlString("#00CCFB", out color);
                break;
            case "4":
                ColorUtility.TryParseHtmlString("#6FBABF", out color);
                break;
            case "5":
                ColorUtility.TryParseHtmlString("#72D800", out color);
                break;
            case "6":
                ColorUtility.TryParseHtmlString("#FF00B1", out color);
                break;
            case "7":
                ColorUtility.TryParseHtmlString("#C61000", out color);
                break;
            default:
                ColorUtility.TryParseHtmlString("#8200A1", out color);
                break;
        }
        color.a = 0.5f;
        return color;
    }
}
