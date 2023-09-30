using System;
using AnttiStarterKit.Extensions;
using AnttiStarterKit.Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Bug bug;

    private Bug target;
    private Enemies container;
    private static readonly int Moving = Animator.StringToHash("moving");
    private float biteDelay;
    private Vector3 wanderDirection;
    private float speed;

    public void SetTarget(Bug player)
    {
        speed = anim.speed = Random.Range(0.9f, 1.1f);
        wanderDirection = Quaternion.Euler(0, 0, Random.value * 360f) * Vector3.up;
        target = player;
    }

    private void Update()
    {
        anim.SetBool(Moving, false);

        if (!target)
        {
            anim.SetBool(Moving, true);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + wanderDirection, Time.deltaTime * 1.5f * speed);
            return;
        }
        
        biteDelay = Mathf.MoveTowards(biteDelay, 0, Time.deltaTime);

        var diff = target.transform.position - transform.position;
        if (diff.magnitude > 0.7f)
        {
            anim.SetBool(Moving, true);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 1.5f * speed);
            return;
        }

        if (target && biteDelay <= 0)
        {
            target.Damage(1);
            biteDelay = 0.75f;
        }
    }

    public void Setup(Enemies enemies, int level)
    {
        bug.SetHealth(3 + level);
        container = enemies;
        enemies.Add(this);

        transform.localScale *= Random.Range(0.9f, 1.2f);
    }

    public void Die()
    {
        if (target)
        {
            target.Leech();
        }
        container.Remove(this);
        Destroy(gameObject);
    }

    public void Damage(int amount)
    {
        bug.Damage(amount);
    }
}