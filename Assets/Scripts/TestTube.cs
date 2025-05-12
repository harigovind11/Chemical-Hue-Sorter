using UnityEngine;
using System.Collections;
using DG.Tweening;
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

    public IEnumerator AnimatePourTo(TestTube target)
    {
        if (!CanPourInto(target)) yield break;

        Color topColor = GetTopColor();
        int pourCount = 0;

        for (int i = fillLevel - 1; i >= 0; i--)
        {
            if (slots[i].enabled && slots[i].color == topColor) pourCount++;
            else break;
        }

        pourCount = Mathf.Min(pourCount, 4 - target.fillLevel);

        // === Tilt source tube ===
        float angle = (target.transform.position.x > transform.position.x) ? -25f : 25f;
        yield return RotateTube(transform, angle, 0.15f);

        for (int i = 0; i < pourCount; i++)
        {
            int sourceIndex = fillLevel - 1;
            int targetIndex = target.fillLevel;

            SpriteRenderer sourceSlot = slots[sourceIndex];
            SpriteRenderer targetSlot = target.slots[targetIndex];

            // Create a temporary visual clone
            GameObject temp = new GameObject("PouringBlock");
            SpriteRenderer sr = temp.AddComponent<SpriteRenderer>();

            // Match visual properties
            sr.sprite = sourceSlot.sprite;
            sr.color = sourceSlot.color;
            sr.sortingOrder = 10;
            sr.drawMode = sourceSlot.drawMode;
            sr.size = sourceSlot.size;
            sr.transform.localScale = sourceSlot.transform.lossyScale;


            temp.transform.position = sourceSlot.transform.position;

            Vector3 endPos = targetSlot.transform.position;

            // Animate movement
            float t = 0f;
            float duration = 0.25f;
            while (t < duration)
            {
                temp.transform.position = Vector3.Lerp(sourceSlot.transform.position, endPos, t / duration);
                t += Time.deltaTime;
                yield return null;
            }

            temp.transform.position = endPos;

            // Destroy temporary block
            Destroy(temp);

            // Logic update
            fillLevel--;
            sourceSlot.color = Color.clear;
            sourceSlot.enabled = false;

            targetSlot.color = topColor;
            targetSlot.enabled = true;
            target.fillLevel++;

            yield return new WaitForSeconds(0.05f);
        }

        // === Reset tilt ===
        yield return RotateTube(transform, 0f, 0.15f);
    }


    private IEnumerator RotateTube(Transform tube, float toZAngle, float duration)
    {
        Quaternion from = tube.rotation;
        Quaternion to = Quaternion.Euler(0, 0, toZAngle);

        float t = 0f;
        while (t < duration)
        {
            tube.rotation = Quaternion.Slerp(from, to, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        tube.rotation = to;
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
