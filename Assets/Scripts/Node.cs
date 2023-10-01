using AnttiStarterKit.Managers;
using TMPro;
using UnityEngine;
using AnttiStarterKit.Extensions;

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

    public void Activate(Bug bug, GameObject installHelp)
    {
        if (!booted)
        {
            EffectManager.AddTextPopup($"+{space} <size=7>bytes</size>", transform.position + Vector3.up * 3);
            bug.AddSpace(space);

            var p = transform.position;
            var notOverlapping = Mathf.Abs(p.x) >= 20 || Mathf.Abs(p.y) >= 6;

            this.StartCoroutine(() =>
            {
                if (notOverlapping && bug.HasNoBonuses && (bug.FreeSpace > left.Size || bug.FreeSpace > right.Size))
                {
                    installHelp.SetActive(true);
                    installHelp.transform.position = transform.position;
                    installHelp.SetActive(true);
                    
                    bug.TryHeal();
                }
            }, 0.3f);
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
        if (!ring.enabled)
        {
            EffectManager.AddEffect(4, transform.position);   
        }

        ring.enabled = true;
        visuals.SetActive(true);
    }

    public void Clear()
    {
        screen.text = ">_";
        folders.SetActive(false);
    }

    public void Setup(int level)
    {
        space = (1 + level) * Random.Range(4, 16);
        left.Setup(level);
        right.Setup(level);
    }
}