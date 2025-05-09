using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidLayer : MonoBehaviour
{
    public Color color;

    public void SetColor(Color newColor)
    {
        color = newColor;
        GetComponent<SpriteRenderer>().color = newColor;
    }
}

