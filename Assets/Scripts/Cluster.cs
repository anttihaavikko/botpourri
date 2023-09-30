using AnttiStarterKit.Extensions;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [SerializeField] private Enemy prefab;

    private bool triggered;
    private int level;

    public void Activate(Bug player, Enemies enemies, int pushAmount)
    {
        if (Vector3.Distance(transform.position, Vector3.zero) < 3.5f) return; 
        
        if (triggered) return;
        triggered = true;

        for (var i = 0; i < level * 0.5f + 1; i++)
        {
            var enemy = Instantiate(prefab, transform.position.RandomOffset(1f), Quaternion.identity);
            enemy.Setup(enemies, level);
            enemy.SetTarget(player);
            enemy.Push(player.transform.position, pushAmount);
        }
    }

    public void SetLevel(int lvl)
    {
        level = lvl;
    }
}