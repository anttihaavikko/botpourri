using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ring;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Collider2D hitBox;
    [SerializeField] private TMP_Text screen;
    [SerializeField] private GameObject folders;
    [SerializeField] private Folder left, right;
    

    private bool booted;
    private int space;

    public void Activate(Bug bug)
    {
        if (!booted)
        {
            bug.AddSpace(space);
        }
        
        booted = true;
        UpdateScreen(bug);
        folders.SetActive(true);
    }

    public void UpdateScreen(Bug bug)
    {
        screen.text = $">FREE SPACE\n>{bug.FreeSpace} bytes";
    }

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

    public void Clear()
    {
        screen.text = ">";
        folders.SetActive(false);
    }

    public void Setup(int level)
    {
        space = (1 + level) * Random.Range(4, 16);
        left.Setup(level);
        right.Setup(level);
    }
}