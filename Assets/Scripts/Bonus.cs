using System.Collections.Generic;
using AnttiStarterKit.Extensions;
using UnityEngine;

public class Bonus
{
    public readonly BonusId id;
    public readonly string title;
    public readonly string description;
    public readonly int size;

    private Bonus(BonusBlueprint blueprint)
    {
        id = blueprint.id;
        title = blueprint.title;
        description = blueprint.description;
        size = Random.Range(blueprint.minSize, blueprint.maxSize + 1);
    }
    
    public static Bonus Get()
    {
        var blueprints = new List<BonusBlueprint>()
        {
            new(BonusId.Steps, "Battery", "Gain extra movement turn before need for charging.", 4, 8),
            new(BonusId.Vision, "Vision", "Enhances your vision radius.", 4, 16)
        };
        
        return new Bonus(blueprints.Random());
    }
}

public enum BonusId
{
    None,
    Steps,
    Vision
}

public class BonusBlueprint
{
    public readonly BonusId id;
    public readonly string title;
    public readonly string description;
    public readonly int maxSize, minSize;
    
    public BonusBlueprint(BonusId id, string title, string description, int minSize, int maxSize)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.minSize = minSize;
        this.maxSize = maxSize;
    }
}