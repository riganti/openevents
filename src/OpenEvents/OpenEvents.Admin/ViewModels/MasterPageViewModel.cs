using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotVVM.Framework.ViewModel;

namespace OpenEvents.Admin.ViewModels
{
    public abstract class MasterPageViewModel : DotvvmViewModelBase
    {

        public abstract string CurrentSection { get; }

    }
}
