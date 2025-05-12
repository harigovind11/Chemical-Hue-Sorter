using UnityEngine;

public class GameManager : MonoBehaviour
{
    private TestTube selectedTube;
    private SpriteRenderer selectedRenderer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit && hit.TryGetComponent(out TestTube tube))
            {
                SpriteRenderer targetRenderer = tube.GetComponent<SpriteRenderer>();

                if (selectedTube == null)
                {
                    // Select this tube
                    selectedTube = tube;
                    selectedRenderer = targetRenderer;
                    selectedRenderer.color = Color.yellow;
                    Debug.Log($"Selected: {tube.name}");
                }
                else
                {
                    // Attempt pour into a different tube
                    if (tube != selectedTube)
                    {
                        bool success = selectedTube.CanPourInto(tube);
                        //selectedTube.PourInto(tube);
                       StartCoroutine(selectedTube.AnimatePourTo(tube));

                        if (success)
                        {
                            targetRenderer.color = Color.green;
                        }
                        else
                        {
                            targetRenderer.color = Color.red;
                        }

                        // Reset colors after a short delay
                        StartCoroutine(ResetColorsAfterDelay(selectedRenderer, targetRenderer));
                    }

                    selectedTube = null;
                    selectedRenderer = null;
                }
            }
        }
    }

    private System.Collections.IEnumerator ResetColorsAfterDelay(SpriteRenderer from, SpriteRenderer to)
    {
        yield return new WaitForSeconds(0.4f); // Wait a bit to show color feedback
        if (from != null) from.color = Color.white;
        if (to != null) to.color = Color.white;
    }
}
