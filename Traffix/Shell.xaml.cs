using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Traffix
{

    public partial class Shell : PhoneApplicationPage
    {
        
        // Constructor
        public Shell()
        {     
            InitializeComponent();
        }


        #region private

        //no support in this version of windows phone for ICommand
        private void UpdateFirstRunButton_Click(object sender, RoutedEventArgs e)
        {

            HideFirstRun.Completed += (o, ev) => 
            {

                ViewModels.ShellViewModel vm = DependencyResolver.Resolve<ViewModels.ShellViewModel>();

                if (vm != null)
                {
                    vm.UpdateFirstRun.Execute(AllowLocationToggle.IsChecked.Value);
                }

            }; 

            HideFirstRun.Begin();

        }

        #endregion
        
    }

}