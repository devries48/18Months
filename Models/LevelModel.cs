namespace Months18.Models;

public class LevelModel
{
    private static readonly string[] headers = [
            "No filter, the complete chronological journey",
            "Level 1, Introduction",
            "Level 2, ",
            "Level 3, ",
            "Level 4, ",
        ];

    private static readonly string[] descriptions = [
        "Most listened albums through the months of therapy",
        "Level 1, 70's rock, psychedelic, cool jazz",
        "Level 2, ",
        "Level 3, ",
        "Level 4, ",
    ];

    public int Level { get; private set; }

    public string Name => Level == 0 ? "No level filter" : $"LEVEL {Level}";

    public string Header => headers[Level];

    public string Description => descriptions[Level];

    public static LevelModel Create(int level) => new() { Level = level };
}
