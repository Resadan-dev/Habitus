using Valoron.BuildingBlocks;

namespace Valoron.Activities.Domain;

public class ActivityCategory : ValueObject
{
    public string Code { get; }
    public string Name { get; }

    private ActivityCategory(string code, string name)
    {
        Code = code;
        Name = name;
    }
    public static ActivityCategory Environment => new("ENV", "Environment");
    public static ActivityCategory Body => new("BODY", "Body");
    public static ActivityCategory Nutrition => new("NUTR", "Nutrition");
    public static ActivityCategory Hygiene => new("HYG", "Hygiene");
    public static ActivityCategory Social => new("SOC", "Social");
    public static ActivityCategory Admin => new("ADM", "Administrative");
    public static ActivityCategory Learning => new("LRN", "Learning");
    public static ActivityCategory Project => new("PROJ", "Project");

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Code;
    }

    public override string ToString() => Name;
}