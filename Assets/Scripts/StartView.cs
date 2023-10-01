using System;
using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartView : MonoBehaviour
{
    [SerializeField] private Bug bug;
    [SerializeField] private List<Transform> spots;

    private void Start()
    {
        ChangeSpot();
    }

    private void ChangeSpot()
    {
        bug.MoveTo(spots.Where(p => Vector3.Distance(p.position, bug.transform.position) > 1f).ToList().Random().position);
        Invoke(nameof(ChangeSpot), Random.Range(2f, 5f));
    }
}