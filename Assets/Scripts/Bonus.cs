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
        size = blueprint.size;
        value = blueprint.value;
        icon = blueprint.icon;
        level = blueprint.level;
    }
    
    public static Bonus Get(int level)
    {
        var blueprints = new List<BonusBlueprint>
        {
            new(BonusId.Steps, "Battery", "Gain extra movement turn before need for charging.", 4, 0, 0),
            new(BonusId.Vision, "Vision", "Enhances your vision radius.", 4, 1, 0),
            new(BonusId.Speed, "Speed", "Increase your movement speed.", 8, 2, 0),
            new(BonusId.ShotRange, "Sniper", "Increase your maximum attack range.", 16, 3, 1),
            new(BonusId.ShotRate, "Fire Rate", "Increase your attack speed.", 16, 4, 1),
            new(BonusId.Vision, "Mind's Eye", "Enhances your vision radius.", 8, 5, 3, 2),
            new(BonusId.Damage, "Damage", "Increase your damage by one.", 24, 6, 2),
            new(BonusId.Shield, "Shield", "Gain a regenerating shield protecting you.", 32, 7, 3),
            new(BonusId.Chain, "Chain", "Your attacks chain to next closest enemy.", 32, 8, 3),
            new(BonusId.Leech, "Leech", "Chance to heal yourself for one when killing an enemy.", 128, 9, 3),
            new(BonusId.Sensor, "Sensor", "Gain ability to sense points of interest.", 128, 10, 2),
            new(BonusId.Speed, "Wings", "Dramatically increase your movement speed.", 8, 11, 2, 2)
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
    ShotRange,
    Damage,
    Shield,
    Chain,
    Leech,
    Sensor
}

public class BonusBlueprint
{
    public readonly BonusId id;
    public readonly string title;
    public readonly string description;
    public readonly int size;
    public readonly int value;
    public readonly int icon;
    public readonly int level;
    
    public BonusBlueprint(BonusId id, string title, string description, int size, int icon, int level, int value = 1)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.size = size;
        this.value = value;
        this.icon = icon;
        this.level = level;
    }
}