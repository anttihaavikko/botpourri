using AnttiStarterKit.Extensions;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [SerializeField] private Enemy prefab;

    private bool triggered;
    private int level;

    public void Activate(Bug player, Enemies enemies)
    {
        if (Vector3.Distance(transform.position, Vector3.zero) < 2.5f) return; 
        
        if (triggered) return;
        triggered = true;

        for (var i = 0; i < level + 2; i++)
        {
            var enemy = Instantiate(prefab, transform.position.RandomOffset(1f), Quaternion.identity);
            enemy.Setup(enemies, level);
            enemy.SetTarget(player);
        }
    }

    public void SetLevel(int lvl)
    {
        level = lvl;
    }
}