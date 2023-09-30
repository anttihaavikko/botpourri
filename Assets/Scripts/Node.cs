using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ring;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Collider2D hitBox;

    public void ToggleHitBox(bool state)
    {
        hitBox.enabled = state;
    }

    public void Hide()
    {
        ring.enabled = false;
        visuals.SetActive(false);
    }

    public void Show()
    {
        ring.enabled = true;
        visuals.SetActive(true);
    }
}