using System.Collections.Generic;
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

    private Bonus(BonusBlueprint blueprint)
    {
        id = blueprint.id;
        title = blueprint.title;
        description = blueprint.description;
        size = Random.Range(blueprint.minSize, blueprint.maxSize + 1);
        value = blueprint.value;
        icon = blueprint.icon;
    }
    
    public static Bonus Get()
    {
        var blueprints = new List<BonusBlueprint>
        {
            new(BonusId.Steps, "Battery", "Gain extra movement turn before need for charging.", 4, 8, 0),
            new(BonusId.Vision, "Vision", "Enhances your vision radius.", 4, 16, 1),
            new(BonusId.Speed, "Speed", "Increase your movement speed.", 8, 16, 2),
            new(BonusId.ShotRange, "Sniper", "Increase your maximum attack range.", 8, 16, 3),
            new(BonusId.ShotRate, "Dexterity", "Increase your attack speed.", 8, 16, 4),
            new(BonusId.Vision, "Mind's Eye", "Enhances your vision radius.", 16, 32, 5, 3)
        };
        
        return new Bonus(blueprints.Random());
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
    
    public BonusBlueprint(BonusId id, string title, string description, int minSize, int maxSize, int icon, int value = 1)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.minSize = minSize;
        this.maxSize = maxSize;
        this.value = value;
        this.icon = icon;
    }
}