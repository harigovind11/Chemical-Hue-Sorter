using UnityEngine;
using System.Collections.Generic;

public class TubeRandomFiller : MonoBehaviour
{
    public TestTube[] tubes;            // Assign in inspector
    public int numberOfColors;      // Number of unique colors
    public int tubesToFill;        // Number of non-empty tubes
    public Color[] availableColors;     // Optional: set custom colors

    void Start()
    {
        List<Color> colorPool = new List<Color>();

        // Generate color pool
        for (int i = 0; i < numberOfColors; i++)
        {
            Color color = (availableColors != null && i < availableColors.Length)
                ? availableColors[i]
                : GetDistinctColor(i);

            // Add 4 of each color
            for (int j = 0; j < 4; j++)
                colorPool.Add(color);
        }

        // Shuffle the pool
        for (int i = 0; i < colorPool.Count; i++)
        {
            int randomIndex = Random.Range(i, colorPool.Count);
            (colorPool[i], colorPool[randomIndex]) = (colorPool[randomIndex], colorPool[i]);
        }

        // Fill tubes
        int colorIndex = 0;
        for (int i = 0; i < tubesToFill; i++)
        {
            List<Color> tubeColors = new List<Color>();

            for (int j = 0; j < 4 && colorIndex < colorPool.Count; j++)
            {
                tubeColors.Add(colorPool[colorIndex]);
                colorIndex++;
            }

            tubes[i].SetInitialColors(tubeColors.ToArray());
        }

        // Set remaining tubes as empty
        for (int i = tubesToFill; i < tubes.Length; i++)
        {
            tubes[i].SetInitialColors(new Color[] { });
        }
    }

    // Fallback color generator if custom list isn't used
    private Color GetDistinctColor(int index)
    {
        Color[] defaults = new Color[] {
            Color.red, Color.blue, Color.green, Color.yellow,
            new Color(1f, 0.5f, 0f),  // orange
            Color.magenta, Color.cyan, Color.gray
        };

        if (index < defaults.Length) return defaults[index];

        // Random fallback if too many
        return Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.8f, 1f);
    }
}
