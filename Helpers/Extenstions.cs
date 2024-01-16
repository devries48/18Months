namespace Months18.Helpers;

public static class Extenstions
{
    public static void DispatchIfRequired(this IDispatcher dispatcher, Action action)
    {
        if (dispatcher.IsDispatchRequired)
            dispatcher.Dispatch(action);
        else
            action();
    }
}
