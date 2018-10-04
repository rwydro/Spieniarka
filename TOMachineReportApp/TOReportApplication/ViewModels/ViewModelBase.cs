﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TOReportApplication.Annotations;
using Unity;

namespace TOReportApplication.ViewModels
{
    public class ViewModelBase: INotifyPropertyChanged
    {
      public IUnityContainer container;

        public ViewModelBase(IUnityContainer container)
        {
            this.container = container;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnActionExecute<T>(Action<T> action, T param)
        {
            var handler = action;
            if (handler != null)
                handler(param);
        }
    }
}
