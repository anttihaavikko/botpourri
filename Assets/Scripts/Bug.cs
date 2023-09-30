using System;
using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Animations;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Game;
using AnttiStarterKit.Managers;
using AnttiStarterKit.Visuals;
using Mono.Cecil;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private bool isPlayer;
    [SerializeField] private LineDrawer lineDrawer;
    [SerializeField] private Health health;
    [SerializeField] private Transform visionRange;
    [SerializeField] private GameObject shield;
    [SerializeField] private ScoreDisplay score;
    [SerializeField] private LayerMask withoutHints, withHints;
    
    public int FreeSpace => freeSpace;

    private readonly List<PathLine> paths = new ();
    private bool moving;
    private bool blocked;
    private int freeSpace;
    private Folder folder;
    private readonly List<Bonus> bonuses = new();
    private readonly Enemies enemies = new();
    private float shotDelay;
    private EffectCamera effectCam;
    private float attackDelay;
    private float attackRange;
    private float speed;
    private int damage;
    private int shieldLeft, shieldMax;
    private int chaining;

    private int steps;

    private static readonly int Moving = Animator.StringToHash("moving");

    private void Start()
    {
        effectCam = Camera.main.GetComponent<EffectCamera>();
        
        if (!isPlayer) return;

        Instantiate(circlePrefab, Vector3.zero, Quaternion.identity);
        StartPath(currentNode.transform.position);
        currentNode.ToggleHitBox(false);
        currentNode.Activate(this);
        currentNode.Setup(0);
        CalculateStats();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isPlayer) return;
        
        var node = col.GetComponent<Node>();
        if (node)
        {
            node.Show();
        }
        
        var cluster = col.GetComponent<Cluster>();
        if (cluster)
        {
            cluster.Activate(this, enemies);
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
        if (!isPlayer) return;
        
        UpdatePreviewLine();

        shotDelay = Mathf.MoveTowards(shotDelay, 0, Time.deltaTime);

        var target = enemies.Find(transform.position, attackRange);
        if (target && shotDelay <= 0)
        {
            lineDrawer.AddThunderLine(transform.position + Vector3.up, target.transform.position + Vector3.up * 0.3f, new Color(5, 5, 0), 0.6f, 0.5f);
            target.Damage(damage);
            shotDelay = attackDelay;
            
            for (var i = 0; i < chaining; i++)
            {
                var chainTarget = enemies.Find(target.transform.position, attackRange, target);
                if (chainTarget)
                {
                    lineDrawer.AddThunderLine(target.transform.position + Vector3.up * 0.3f, chainTarget.transform.position + Vector3.up * 0.3f, new Color(5, 5, 0), 0.6f, 0.5f);
                    target = chainTarget;
                    target.Damage(damage);
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && folder)
        {
            folder.Activate(this);
            return;
        }

        if (Input.GetMouseButtonDown(0) && !moving && !blocked && paths.Last().Count <= steps)
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
            var delay = MoveTo(pos, true);
            UpdateSteps();

            if (hit)
            {
                currentNode.Clear();
                currentNode.ToggleHitBox(true);
                currentNode = hit.collider.GetComponent<Node>();
                currentNode.Activate(this);
                this.StartCoroutine(() => currentNode.ToggleHitBox(false), delay);
                StartPath(pos);
            }
        }
    }

    private void AddNode(Vector3 pos)
    {
        Instantiate(circlePrefab, pos, Quaternion.identity);
        paths.Last().AddPoint(pos);
    }

    private float MoveTo(Vector3 pos, bool manual = false)
    {
        CancelInvoke(nameof(CheckReturn));
        
        anim.SetBool(Moving, true);
        anim.speed = 1 / speed * 0.2f;
        moving = true;
        var duration = Vector3.Distance(transform.position, pos) * speed;
        Tweener.MoveTo(transform, pos, duration, TweenEasings.QuadraticEaseInOut);
        this.StartCoroutine(() =>
        {
            moving = false;
            anim.SetBool(Moving, false);

            if (manual)
            {
                Invoke(nameof(CheckReturn), 3f * speed);
            }
            
        }, duration);

        return duration;
    }

    private void CheckReturn()
    {
        if (paths.Last().Count <= steps) return;
        ReturnTo(steps - 1);
        this.StartCoroutine(() => StartPath(paths.Last().GetPoint(0)), speed * paths.Last().GetTotalLength());
    }

    private void ReturnTo(int index)
    {
        var pos = paths.Last().GetPoint(index);
        var duration = MoveTo(pos);

        if (index > 0)
        {
            this.StartCoroutine(() => ReturnTo(index - 1), duration);
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
        
        if (moving || hit || paths.Last().Count > steps)
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

    private int SumOf(BonusId id)
    {
        return bonuses.Where(b => b.id == id).Sum(b => b.value);
    }

    private void CalculateStats()
    {
        steps = 3 + SumOf(BonusId.Steps);
        visionArea.radius = 3 + SumOf(BonusId.Vision) * 0.5f;
        if (visionRange)
        {
            visionRange.localScale = Vector3.one * (visionArea.radius * 0.5f);   
        }
        attackDelay = 0.5f * Mathf.Pow(0.85f, SumOf(BonusId.ShotRate));
        attackRange = 5 + SumOf(BonusId.Vision) * 1.5f;
        speed = Mathf.Pow(0.85f, SumOf(BonusId.Speed)) * 0.25f;
        damage = 1 + SumOf(BonusId.Damage);
        shieldMax = SumOf(BonusId.Shield);
        chaining = SumOf(BonusId.Chain);

        cam.cullingMask = SumOf(BonusId.Sensor) > 0 ? withHints : withoutHints;
        
        UpdateSteps();
        
        CancelInvoke(nameof(RegenerateShield));
        Invoke(nameof(RegenerateShield), 1f);
    }

    private void RegenerateShield()
    {
        shieldLeft = shieldMax;
        shield.SetActive(shieldLeft > 0);
    }
    
    public void Damage(int amount)
    {
        if (shieldLeft > 0)
        {
            amount--;
            shieldLeft--;
            shield.SetActive(shieldLeft > 0);
            CancelInvoke(nameof(RegenerateShield));
            Invoke(nameof(RegenerateShield), 3f);
        }

        if (amount > 0 && score)
        {
            score.ResetMulti();
        }
        
        health?.TakeDamage<Bug>(amount);
    }

    public void Shake(float amount)
    {
        effectCam.BaseEffect(amount);
    }

    public void SetHealth(int max)
    {
        health.Set(max, max);
    }

    public void GameOver()
    {
        visionArea.gameObject.SetActive(false);
    }

    public void Leech()
    {
        var chance = Mathf.Pow(0.75f, SumOf(BonusId.Leech));
        if (Random.value > chance)
        {
            health.Heal(1);
        }
    }

    public void AddScore(int level, Vector3 pos)
    {
        if (!score) return;
        var lvlMulti = Mathf.Max(1, level + 1);
        var amount = lvlMulti * lvlMulti * 10 * Mathf.Max(1, freeSpace) * Mathf.Min(score.Multi, 64);
        EffectManager.AddTextPopup(amount.ToString(), pos + Vector3.up);
        score.Add(amount, false);
        score.AddMulti();
    }
}