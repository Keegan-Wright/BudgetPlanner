﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Reflection.Emit;

namespace BudgetPlanner.Client.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private object? _navigationData;

        public async Task RunOnBackgroundThreadAsync(Action action)
        {
            await Task.Run(action.Invoke);
        }
        public async Task RunOnBackgroundThreadAsync(Task taskToRun)
        {
            await Task.Run(async () =>
            {
                await taskToRun;
            });
        }
    }
}
