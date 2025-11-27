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

    public static ActivityCategory FromCode(string code)
    {
        return code.ToUpperInvariant() switch
        {
            "ENV" => Environment,
            "BODY" => Body,
            "NUTR" => Nutrition,
            "HYG" => Hygiene,
            "SOC" => Social,
            "ADM" => Admin,
            "LRN" => Learning,
            "PROJ" => Project,
            _ => throw new ArgumentException($"Invalid category code: {code}", nameof(code))
        };
    }

    public static readonly ActivityCategory Environment = new("ENV", "Environment");
    public static readonly ActivityCategory Body = new("BODY", "Body");
    public static readonly ActivityCategory Nutrition = new("NUTR", "Nutrition");
    public static readonly ActivityCategory Hygiene = new("HYG", "Hygiene");
    public static readonly ActivityCategory Social = new("SOC", "Social");
    public static readonly ActivityCategory Admin = new("ADM", "Administrative");
    public static readonly ActivityCategory Learning = new("LRN", "Learning");
    public static readonly ActivityCategory Project = new("PROJ", "Project");

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Code;
    }

    public override string ToString() => Name;
}