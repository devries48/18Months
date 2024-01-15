using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Months18.ViewModels
{
    public partial class TherapyPageViewModel:ObservableObject
    {
        public ObservableCollection<int> Numbers { get; } = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        [ObservableProperty]
        int span = 1;

        [RelayCommand]
        void ChangeSpan(int byAmount)
        {
            if (Span + byAmount <= 0)   //Prevent span from being <= 0.
            {
                Span = 1;
                return;
            }

            Span += byAmount;
        }
    }
}
