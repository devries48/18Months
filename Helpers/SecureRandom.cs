namespace Months18.Helpers;

public class SecureRandom
{
    private int fromInc, toExcl;
    public SecureRandom(int toExclusive = 2) => Init(0, toExclusive);
    public SecureRandom(int fromInclusive, int toExclusive) => Init(fromInclusive, toExclusive);

    private void Init(int fromInclusive, int toExclusive)
    {
        fromInc = fromInclusive;
        toExcl = toExclusive;
    }

    public int Next() => RandomNumber(fromInc, toExcl);

    public static int RandomNumber(int fromInclusive, int toExclusive) => System.Security.Cryptography.RandomNumberGenerator
        .GetInt32(fromInclusive, toExclusive);
}