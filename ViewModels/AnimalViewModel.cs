namespace Months18.ViewModels;

public class AnimalViewModel : BaseViewModel
{
    string groupName;
    object selection;

    public string GroupName
    {
        get => groupName;
        set
        {
            groupName = value;
            OnPropertyChanged(nameof(GroupName));
        }
    }

    public object Selection
    {
        get => selection;
        set
        {
            selection = value;
            OnPropertyChanged(nameof(Selection));
        }
    }
}
