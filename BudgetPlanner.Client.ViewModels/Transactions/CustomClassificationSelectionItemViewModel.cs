﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class CustomClassificationSelectionItemViewModel : ViewModelBase
    {

        [ObservableProperty]
        private Guid _classificationId;

        [ObservableProperty]
        private string _classificationName;

        [ObservableProperty]
        private bool _checked;

    }

}
