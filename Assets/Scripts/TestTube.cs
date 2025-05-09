using UnityEngine;

public class TestTube : MonoBehaviour
{
    public SpriteRenderer[] slots = new SpriteRenderer[4]; // Assign in Inspector bottom to top
    public int fillLevel = 0;

    public Color GetTopColor()
    {
        if (fillLevel == 0) return Color.clear;
        return slots[fillLevel - 1].color;
    }

    public bool CanPourInto(TestTube target)
    {
        if (fillLevel == 0)
        {
            Debug.Log($"{name}: Cannot pour because source is empty.");
            return false;
        }

        if (target.fillLevel >= 4)
        {
            Debug.Log($"{name} → {target.name}: Cannot pour because target is full.");
            return false;
        }

        Color sourceColor = GetTopColor();
        Color targetColor = target.GetTopColor();

        if (target.fillLevel == 0)
        {
            Debug.Log($"{name} → {target.name}: Target is empty, can pour.");
            return true;
        }

        if (sourceColor == targetColor)
        {
            Debug.Log($"{name} → {target.name}: Top colors match ({sourceColor}), can pour.");
            return true;
        }

        Debug.Log($"{name} → {target.name}: Top colors don't match ({sourceColor} vs {targetColor})");
        return false;
    }


    public void PourInto(TestTube target)
    {
        if (!CanPourInto(target)) return;

        Color topColor = GetTopColor();
        int pourCount = 0;

        // Count how many top layers are the same color
        for (int i = fillLevel - 1; i >= 0; i--)
        {
            if (slots[i].enabled && slots[i].color == topColor)
                pourCount++;
            else
                break;
        }

        // Limit by target space
        pourCount = Mathf.Min(pourCount, 4 - target.fillLevel);

        for (int i = 0; i < pourCount; i++)
        {
            // Remove from this tube
            fillLevel--;
            slots[fillLevel].color = Color.clear;
            slots[fillLevel].enabled = false;

            // Add to target tube
            target.slots[target.fillLevel].color = topColor;
            target.slots[target.fillLevel].enabled = true;
            target.fillLevel++;
        }
    }
    public void SetInitialColors(Color[] colors)
    {
        fillLevel = colors.Length;

        for (int i = 0; i < colors.Length; i++)
        {
            slots[i].color = colors[i];
            slots[i].enabled = true;
        }

        for (int i = colors.Length; i < 4; i++)
        {
            slots[i].color = Color.clear;
            slots[i].enabled = false;
        }
    }

}
