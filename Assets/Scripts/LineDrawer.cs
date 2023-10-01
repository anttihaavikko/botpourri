using System;
using System.Collections.Generic;
using AnttiStarterKit.Animations;
using AnttiStarterKit.Managers;
using UnityEngine;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.ScriptableObjects;
using AnttiStarterKit.Utils;

public class LineDrawer : ObjectPool<StickyLine>
{
    // public void AddLine(Vector3 from, Vector3 to, Color color, float width, float inDuration, float outDuration)
    // {
    //     var line = Get(true);
    //     var t = line.transform;
    //     t.position = from;
    //     t.localScale = Vector3.zero;
    //     line.positionCount = 20;
    //     var step = 1f / line.positionCount;
    //     for (var i = 0; i < line.positionCount; i++)
    //     {
    //         line.SetPosition(i, Vector3.Lerp(from, to, i * step));
    //     }
    //
    //     line.startColor = line.endColor = color;
    //     line.widthMultiplier = width;
    //     
    //     Tweener.ScaleToBounceOut(t, Vector3.one, inDuration);
    //     this.StartCoroutine(() => Tweener.ScaleToQuad(t, Vector3.zero, outDuration), inDuration);
    //     this.StartCoroutine(() => ReturnToPool(line), inDuration + outDuration);
    // }

    public void AddThunderLine(Transform from, Transform to, Color color, float width, float midOffset)
    {
        var line = Get(true);
        line.SetTargets(from, to, midOffset, color, width);
        this.StartCoroutine(() => ReturnToPool(line), 0.3f);
    }
}