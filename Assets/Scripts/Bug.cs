using System;
using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Animations;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class Bug : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Path pathPrefab;

    private readonly List<Path> paths = new ();
    private bool moving;
    private bool blocked;

    private void Start()
    {
        StartPath();
    }

    private void StartPath()
    {
        var path = Instantiate(pathPrefab, Vector3.zero, Quaternion.identity);
        paths.Add(path);
    }

    private void Update()
    {
        UpdatePreviewLine();

        if (Input.GetMouseButtonDown(0) && !moving && !blocked)
        {
            var mousePos = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);
            AddNode(mousePos);
            MoveTo(mousePos, true);
        }
    }

    private void AddNode(Vector3 pos)
    {
        paths.Last().AddPoint(pos);
    }

    private void MoveTo(Vector3 pos, bool manual = false)
    {
        moving = true;
        Tweener.MoveToBounceOut(transform, pos, 0.6f);
        this.StartCoroutine(() =>
        {
            moving = false;

            if (manual)
            {
                CheckReturn();
            }
            
        }, 0.6f);
    }

    private void CheckReturn()
    {
        if (paths.Last().Count <= 5) return;
        ReturnTo(4);
        this.StartCoroutine(StartPath, 0.6f * 5);
    }

    private void ReturnTo(int index)
    {
        var pos = paths.Last().GetPoint(index);
        MoveTo(pos);

        if (index > 0)
        {
            this.StartCoroutine(() => ReturnTo(index - 1), 0.6f);
        }
    }

    private void UpdatePreviewLine()
    {
        if (moving)
        {
            line.enabled = false;
            return;
        }

        line.enabled = true;
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);

        var p = transform.position;
        line.SetPosition(0, p);
        line.SetPosition(1, p * 0.25f + mousePos * 0.75f);
        line.SetPosition(2, mousePos);

        blocked = paths.Any(path => path.Intersects(p, mousePos));
        line.startColor = line.endColor = blocked ? new Color(1, 1, 1, 0.5f) : Color.white;
    }
    
}