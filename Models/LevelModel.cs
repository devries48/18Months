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
        "Echoes of my therapy journey: A chronicle of the albums that accompanied me through the highs and lows, reflecting the spectrum of emotions experienced during these transformative months.",
        "An introduction to diverse musical genres, featuring accessible jazz classics, early psychedelic rock anthems, heartfelt ballads, and the pioneering sounds of progressive rock beginnings.",
        "Explore extended compositions and fusion of genres and cultures, experience the grandeur of symphonic rock from Italy. Delve into intricately crafted arrangements as you traverse through the evolving landscapes of jazz and rock.",
        "More challenging and experimental, with avant-garde elements pushing the boundaries of conventional sound. Requiring multiple listens to unravel their intricate layers and fully appreciate their depth.",
        "Challenge conventional notions of music, inviting you to embrace the unconventional and discover the beauty in the unexpected.",
    ];

    public int Level { get; private set; }

    public string Name => Level == 0 ? "No level filter" : $"LEVEL {Level}";

    public string Header => headers[Level];

    public string Description => descriptions[Level];

    public static LevelModel Create(int level) => new() { Level = level };
}
