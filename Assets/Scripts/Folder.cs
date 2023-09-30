using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Folder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bg;
    [SerializeField] private List<SpriteRenderer> icons;
    
    private SpriteRenderer icon;

    private bool activated;
    private Bonus bonus;

    private void Start()
    {
        bonus = Bonus.Get();
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
            bg.color = icon.color = Color.yellow;
            bug.AddSpace(-bonus.size);
            bug.AddBonus(bonus);
        }
    }
}