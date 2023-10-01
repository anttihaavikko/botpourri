using System;
using System.Collections.Generic;
using AnttiStarterKit.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

public class Blinker : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> lights;
    [SerializeField] private Color offColor;

    private void Start()
    {
        lights.ForEach(l => l.color = offColor);
        lights.Random().color = Color.white;
        Invoke(nameof(Start), Random.Range(0.2f, 1f));
    }
}