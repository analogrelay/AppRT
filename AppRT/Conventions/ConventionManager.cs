﻿using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRT.Conventions
{
    [Export]
    [Shared]
    public class ConventionManager
    {
        public ViewModelToViewConvention ViewModelToView { get; private set; }
        public ViewToViewModelConvention ViewToViewModel { get; private set; }
        public ViewModelBuilder ViewModelBuilder { get; private set; }

        public ConventionManager() : this(null, null, null)
        {
        }

        [ImportingConstructor]
        public ConventionManager(
            [Import(AllowDefault = true)] ViewModelToViewConvention viewModelToView,
            [Import(AllowDefault = true)] ViewToViewModelConvention viewToViewModel,
            [Import(AllowDefault = true)] ViewModelBuilder viewModelBuilder)
        {
            ViewModelToView = viewModelToView ?? new ViewModelToViewConvention();
            ViewToViewModel = viewToViewModel ?? new ViewToViewModelConvention();
            ViewModelBuilder = viewModelBuilder ?? new ViewModelBuilder();
        }
    }
}
