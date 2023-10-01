using System;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Game;
using AnttiStarterKit.Managers;
using AnttiStarterKit.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Bug bug;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Transform center;
    [SerializeField] private SoundComposition biteSound;

    private Bug target;
    private Enemies container;
    private static readonly int Moving = Animator.StringToHash("moving");
    private float biteDelay;
    private Vector3 wanderDirection;
    private float speed;
    private int level;

    public Transform Center => center;

    private void Start()
    {
        EffectManager.AddEffect(5, transform.position);
    }

    public void SetTarget(Bug player)
    {
        bug.Look(player.transform);
        speed = anim.speed = Random.Range(0.9f, 1.1f);
        wanderDirection = Quaternion.Euler(0, 0, Random.value * 360f) * Vector3.up;
        target = player;
    }

    private void Update()
    {
        var pos = transform.position;
        anim.SetBool(Moving, false);

        if (!target)
        {
            anim.SetBool(Moving, true);
            transform.position = Vector3.MoveTowards(transform.position, pos + wanderDirection, Time.deltaTime * 1.5f * speed);
            return;
        }
        
        biteDelay = Mathf.MoveTowards(biteDelay, 0, Time.deltaTime);

        var tp = target.transform.position;
        var diff = tp - transform.position;
        if (diff.magnitude > 0.7f)
        {
            anim.SetBool(Moving, true);
            transform.position = Vector3.MoveTowards(transform.position, tp, Time.deltaTime * 1.5f * speed);
            return;
        }

        if (target && biteDelay <= 0)
        {
            target.Stutter(4);
            bug.OpenMouth();
            var p = transform.position;
            biteSound.Play(p);
            bug.Nudge((tp - pos).normalized * 0.3f);
            target.Damage(1);
            biteDelay = 0.75f;
        }
    }

    public void Setup(Enemies enemies, int lvl)
    {
        bug.SetHealth(3 + lvl);
        container = enemies;
        enemies.Add(this);

        transform.localScale *= Random.Range(0.9f, 1.2f);
    }

    public void Die()
    {
        if (target)
        {
            target.Stutter(6);
            target.AddScore(level, transform.position);
            target.Leech();
        }
        
        container.Remove(this);
        Destroy(gameObject);
    }

    public void Damage(int amount)
    {
        if (target)
        {
            target.Stutter(1);
        }
        
        bug.Damage(amount);
    }

    public void Push(Vector3 from, int amount)
    {
        var dir = transform.position - from;
        body.AddForce(dir.normalized * 15f * amount, ForceMode2D.Impulse);
    }
}