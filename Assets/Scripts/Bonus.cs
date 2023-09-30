using System.Collections.Generic;
using System.Linq;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class Bonus
{
    public readonly BonusId id;
    public readonly string title;
    public readonly string description;
    public readonly int size;
    public readonly int value;
    public readonly int icon;
    public readonly int level;

    private Bonus(BonusBlueprint blueprint)
    {
        id = blueprint.id;
        title = blueprint.title;
        description = blueprint.description;
        size = Random.Range(blueprint.minSize, blueprint.maxSize + 1);
        value = blueprint.value;
        icon = blueprint.icon;
        level = blueprint.level;
    }
    
    public static Bonus Get(int level)
    {
        var blueprints = new List<BonusBlueprint>
        {
            new(BonusId.Steps, "Battery", "Gain extra movement turn before need for charging.", 4, 8, 0, 0),
            new(BonusId.Vision, "Vision", "Enhances your vision radius.", 4, 16, 1, 0),
            new(BonusId.Speed, "Speed", "Increase your movement speed.", 8, 16, 2, 0),
            new(BonusId.ShotRange, "Sniper", "Increase your maximum attack range.", 8, 16, 3, 1),
            new(BonusId.ShotRate, "Fire Rate", "Increase your attack speed.", 8, 16, 4, 1),
            new(BonusId.Vision, "Mind's Eye", "Enhances your vision radius.", 16, 32, 5, 3, 2)
        };
        
        return new Bonus(blueprints.Where(b => b.level <= level).ToList().Random());
    }
}

public enum BonusId
{
    None,
    Steps,
    Vision,
    Speed,
    ShotRate,
    ShotRange
}

public class BonusBlueprint
{
    public readonly BonusId id;
    public readonly string title;
    public readonly string description;
    public readonly int maxSize, minSize;
    public readonly int value;
    public readonly int icon;
    public readonly int level;
    
    public BonusBlueprint(BonusId id, string title, string description, int minSize, int maxSize, int icon, int level, int value = 1)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.minSize = minSize;
        this.maxSize = maxSize;
        this.value = value;
        this.icon = icon;
        this.level = level;
    }
}