using System;
using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Animations;
using AnttiStarterKit.Extensions;
using Mono.Cecil;
using TMPro;
using UnityEngine;

public class Bug : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LineRenderer line;
    [SerializeField] private PathLine pathPrefab;
    [SerializeField] private Animator anim;
    [SerializeField] private Node currentNode;
    [SerializeField] private LayerMask nodeMask, folderMask;
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private TMP_Text stepDisplay;
    [SerializeField] private CircleCollider2D visionArea;
    
    public int FreeSpace => freeSpace;

    private readonly List<PathLine> paths = new ();
    private bool moving;
    private bool blocked;
    private int freeSpace;
    private Folder folder;
    private readonly List<Bonus> bonuses = new();

    private int steps;

    private static readonly int Moving = Animator.StringToHash("moving");

    private void Start()
    {
        StartPath(currentNode.transform.position);
        currentNode.ToggleHitBox(false);
        currentNode.Activate(this);
        CalculateStats();
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
        UpdateSteps();
    }

    private void Update()
    {
        UpdatePreviewLine();

        if (Input.GetMouseButtonDown(0) && folder)
        {
            folder.Activate(this);
            return;
        }

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
            UpdateSteps();

            if (hit)
            {
                currentNode.Clear();
                currentNode.ToggleHitBox(true);
                currentNode = hit.collider.GetComponent<Node>();
                currentNode.Activate(this);
                this.StartCoroutine(() => currentNode.ToggleHitBox(false), 0.6f);
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
        if (paths.Last().Count <= steps) return;
        ReturnTo(steps - 1);
        this.StartCoroutine(() => StartPath(paths.Last().GetPoint(0)), 0.6f * (paths.Last().Count - 1));
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
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition).WhereZ(0);

        var hit = Physics2D.OverlapPoint(mousePos, folderMask);
        if (hit)
        {
            folder = hit.GetComponent<Folder>();
            folder.Mark(true, tooltip);
        }
        
        if (moving || hit)
        {
            line.enabled = false;
            return;
        }

        if (folder)
        {
            folder.Mark(false, tooltip);
        }
        
        folder = null;

        line.enabled = true;

        var p = transform.position;
        line.SetPosition(0, p);
        line.SetPosition(1, p * 0.25f + mousePos * 0.75f);
        line.SetPosition(2, mousePos);

        blocked = paths.Any(path => path.Intersects(p, mousePos));
        line.startColor = line.endColor = blocked ? new Color(1, 1, 1, 0.25f) : Color.white;
    }

    public void AddSpace(int amount)
    {
        freeSpace += amount;
        currentNode.UpdateScreen(this);
    }

    private void UpdateSteps()
    {
        stepDisplay.text = $"{paths.Last().Count - 1}/{steps}";
    }

    public void AddBonus(Bonus bonus)
    {
        bonuses.Add(bonus);
        CalculateStats();
    }

    public void RemoveBonus(Bonus bonus)
    {
        bonuses.Remove(bonus);
        CalculateStats();
    }

    private void CalculateStats()
    {
        steps = 3 + bonuses.Count(b => b.id == BonusId.Steps);
        visionArea.radius = 3 + bonuses.Count(b => b.id == BonusId.Vision);
        UpdateSteps();
    }
}