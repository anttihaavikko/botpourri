using System;
using System.Collections.Generic;
using AnttiStarterKit.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class Folder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bg;
    [SerializeField] private List<SpriteRenderer> icons;
    [SerializeField] private Color hilite;

    private SpriteRenderer icon;

    private bool activated;
    private Bonus bonus;

    public int Size => bonus?.size ?? 0;

    public void Setup(int level)
    {
        bonus = Bonus.Get(level);
        icon = icons[bonus.icon];
        icon.gameObject.SetActive(true);
    }

    public void Mark(bool state, Tooltip tooltip)
    {
        if (!state)
        {
            tooltip.Hide();
        }
        
        if (activated) return;
        bg.color = icon.color = state ? Color.white : new Color(1, 1, 1, 0.5f);

        if (state)
        {
            tooltip.Show(bonus, transform.position);
        }
    }

    public void Activate(Bug bug)
    {
        AudioManager.Instance.PlayEffectAt(0, transform.position, 0.5f);
        
        if (activated)
        {
            activated = false;
            bg.color = icon.color = Color.white;
            bug.AddSpace(bonus.size);
            bug.RemoveBonus(bonus);
            return;
        }
        
        activated = bug.FreeSpace >= bonus.size;
        if (activated)
        {
            bg.color = icon.color = hilite;
            bug.AddSpace(-bonus.size);
            bug.AddBonus(bonus);
        }
    }
}