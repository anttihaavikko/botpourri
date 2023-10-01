using AnttiStarterKit.Extensions;
using AnttiStarterKit.Managers;
using AnttiStarterKit.ScriptableObjects;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [SerializeField] private Enemy prefab;
    [SerializeField] private SoundComposition revealSound;

    private bool triggered;
    private int level;

    public void Activate(Bug player, Enemies enemies, int pushAmount)
    {
        if (Vector3.Distance(transform.position, Vector3.zero) < 3.5f) return; 
        
        if (triggered) return;
        triggered = true;
        
        revealSound.Play(transform.position, 0.5f);

        for (var i = 0; i < level * 0.5f + 1; i++)
        {
            var enemy = Instantiate(prefab, transform.position.RandomOffset(1f), Quaternion.identity);
            enemy.Setup(enemies, level);
            enemy.SetTarget(player);
            enemy.Push(player.transform.position, pushAmount);
        }
        
        EffectManager.AddEffect(4, transform.position);
    }

    public void SetLevel(int lvl)
    {
        level = lvl;
    }
}