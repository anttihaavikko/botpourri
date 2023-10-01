using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AnttiStarterKit.Extensions;
using TMPro;

public class Flasher : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> sprites;
    [SerializeField] private List<LineRenderer> lines;
    [SerializeField] private List<SpriteRenderer> spritesNeedingShader;
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private List<TMP_Text> characters;
    [SerializeField] private List<GameObject> toHide;

    [SerializeField] private float duration = 0.1f;
    
    private List<Color> spriteColors, lineColors, characterColors, extraCharacterColors;
    private List<Material> defaultMaterials;

    public List<TMP_Text> extraCharacters = new();

    private void Start()
    {
        SaveColors();
    }

    public void AddExtras(IEnumerable<TMP_Text> extras)
    {
        extraCharacters.Clear();
        extraCharacters.AddRange(extras);
        extraCharacterColors = extraCharacters.Select(c => c.color).ToList();
    }

    public void SaveColors()
    {
        spriteColors = sprites.Select(s => s.color).ToList();
        lineColors = lines.Select(l => l.startColor).ToList();
        characterColors = characters.Select(c => c.color).ToList();
        if (spritesNeedingShader.Any())
        {
            defaultMaterials = spritesNeedingShader.Select(s => s.material).ToList();
        }
    }

    public void Flash()
    {
        Colorize(Color.white);
        ChangeMaterials(whiteMaterial);
        this.StartCoroutine(() =>
        {
            ResetColor();
            ChangeMaterials();
        }, duration);
    }
    
    private void ChangeMaterials()
    {
        spritesNeedingShader.ForEach(s => s.material = defaultMaterials[spritesNeedingShader.IndexOf(s)]);
    }

    private void ChangeMaterials(Material material)
    {
        spritesNeedingShader.ForEach(s => s.material = material);
    }

    private void Colorize(Color color)
    {
        sprites.ForEach(s => s.color = color);
        lines.ForEach(l => l.startColor = l.endColor = color);
        characters.ForEach(c => c.color = c.outlineColor = color);
        extraCharacters.Where(e => e).ToList().ForEach(c => c.color = c.outlineColor = color);
        toHide.ForEach(o => o.SetActive(false));
    }

    public void ResetColor()
    {
        sprites.ForEach(s => s.color = spriteColors[sprites.IndexOf(s)]);
        lines.ForEach(l => l.startColor = l.endColor = lineColors[lines.IndexOf(l)]);
        characters.ToList().ForEach(c =>
        {
            c.color = characterColors[characters.IndexOf(c)];
            c.outlineColor = Color.black;
        });

        extraCharacters.Where(e => e).ToList().ToList().ForEach(c =>
        {
            c.color = extraCharacterColors[extraCharacters.IndexOf(c)];
            c.outlineColor = Color.black;
        });
        
        toHide.ForEach(o => o.SetActive(true));
    }
}