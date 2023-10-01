using System;
using AnttiStarterKit.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

public class StickyLine : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    
    private Transform from, to;
    private float midOffset;
    private Vector3 off1, off2;

    public void SetTargets(Transform t1, Transform t2, float mid, Color color, float width)
    {
        from = t1;
        to = t2;
        midOffset = mid;
        
        line.transform.localScale = Vector3.one;
        
        line.startColor = line.endColor = color;
        line.widthMultiplier = width;
        
        CalculateOffsets();
    }

    private void CalculateOffsets()
    {
        var offset = Mathf.Min(midOffset, Vector3.Distance(from.position, to.position) * 0.3f);
        off1 = Vector3.zero.RandomOffset(offset);
        off2 = Vector3.zero.RandomOffset(offset);
    }

    private void Update()
    {
        if (!from || !to) return;

        if (Random.value < 0.025f)
        {
            CalculateOffsets();
        }

        var fp = from.position;
        var tp = to.position;
        var mid = (fp + tp) * 0.5f;
        
        line.positionCount = 4;
        
        line.SetPositions(new []
        {
            fp,
            mid + off1,
            mid + off2,
            tp
        });
    }
}