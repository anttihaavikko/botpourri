using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Bug bug;

    private Bug target;
    private Enemies container;
    private static readonly int Moving = Animator.StringToHash("moving");

    public void SetTarget(Bug player)
    {
        target = player;
    }

    private void Update()
    {
        anim.SetBool(Moving, false);
        
        if (!target) return;

        var diff = target.transform.position - transform.position;
        if (diff.magnitude > 0.7f)
        {
            anim.SetBool(Moving, true);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 3f);
        }
    }

    public void SetupContainer(Enemies enemies)
    {
        container = enemies;
        enemies.Add(this);
    }

    public void Die()
    {
        container.Remove(this);
        Destroy(gameObject);
    }

    public void Damage(int amount)
    {
        bug.Damage(amount);
    }
}