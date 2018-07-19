using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight;
using CommonServiceLocator;

namespace PssHighLowTemperature.ViewModel
{
    public class LocatorViewModel:ViewModelBase
    {
        public LocatorViewModel()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
        }
        public MainViewModel Main
        {
            get
            {
                return new MainViewModel();
            }
        }
    }
}
