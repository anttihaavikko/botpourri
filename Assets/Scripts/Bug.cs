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
    [SerializeField] private PathLine pathPrefab;
    [SerializeField] private Animator anim;
    [SerializeField] private Node currentNode;
    [SerializeField] private LayerMask nodeMask;

    private readonly List<PathLine> paths = new ();
    private bool moving;
    private bool blocked;

    private static readonly int Moving = Animator.StringToHash("moving");

    private void Start()
    {
        StartPath(currentNode.transform.position);
        currentNode.ToggleHitBox(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var node = col.GetComponent<Node>();
        if (node)
        {
            node.Show();
        }
    }

    private void StartPath(Vector3 pos)
    {
        var path = Instantiate(pathPrefab, currentNode.transform.position, Quaternion.identity);
        path.SetStart(pos);
        paths.Add(path);
    }

    private void Update()
    {
        UpdatePreviewLine();

        if (Input.GetMouseButtonDown(0) && !moving && !blocked)
        {
            var pos = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);

            var p = transform.position;
            var dir = pos - p;
            var hit = Physics2D.Raycast(p, dir, dir.magnitude, nodeMask);

            if (hit)
            {
                pos = hit.point;
            }
            
            AddNode(pos);
            MoveTo(pos, true);

            if (hit)
            {
                currentNode.ToggleHitBox(true);
                currentNode = hit.collider.GetComponent<Node>();
                currentNode.Show();
                currentNode.ToggleHitBox(false);
                StartPath(pos);
            }
        }
    }

    private void AddNode(Vector3 pos)
    {
        paths.Last().AddPoint(pos);
    }

    private void MoveTo(Vector3 pos, bool manual = false)
    {
        anim.SetBool(Moving, true);
        moving = true;
        Tweener.MoveTo(transform, pos, 0.6f, TweenEasings.QuadraticEaseInOut);
        this.StartCoroutine(() =>
        {
            moving = false;
            anim.SetBool(Moving, false);

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
        this.StartCoroutine(() => StartPath(paths.Last().GetPoint(0)), 0.6f * 5);
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
        line.startColor = line.endColor = blocked ? new Color(1, 1, 1, 0.25f) : Color.white;
    }
    
}