﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;

namespace OpenEvents.Admin.ViewModels
{
    public class DefaultViewModel : MasterPageViewModel
    {
		public string Title { get; set; }

		public DefaultViewModel()
		{
			Title = "Hello from DotVVM!";
		}

        public override string CurrentSection => "Default";
    }
}
