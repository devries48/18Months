namespace Months18.Models;

public class LevelModel
{
    private static readonly string[] headers = [
            "The Complete Journey",
            "Level 1, Introduction",
            "Level 2, Exploration",
            "Level 3, Obscurity",
            "Level 4, Madness",
        ];

    private static readonly string[] descriptions = [
        "Most listened albums through the months of therapy",
        "Level 1, A ",
        "Level 2, jazz, fusion, symphonic & progressive rock",
        "Level 3, ",
        "An acquired taste is necessary, be careful!",
    ];

    public int Level { get; private set; }

    public string Name => Level == 0 ? "No level filter" : $"LEVEL {Level}";

    public string Header => headers[Level];

    public string Description => descriptions[Level];

    public static LevelModel Create(int level) => new() { Level = level };
}
