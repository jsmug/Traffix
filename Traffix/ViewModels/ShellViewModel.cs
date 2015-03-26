using Microsoft.Practices.Prism.Commands;
using System;
using Traffix.Services;

namespace Traffix.ViewModels
{
    
    public class ShellViewModel : ViewModel
    {

        private DelegateCommand<bool> _updateFirstCommand;


        public ShellViewModel()
        {
        }


        #region public

        public bool AllowLocation
        {

            get
            {
                return ApplicationSettingsService.GetSetting<bool>("AllowLocation");
            }

            set
            {

                ApplicationSettingsService.SetSetting("AllowLocation", value, false);
                base.OnPropertyChanged(x => this.AllowLocation);

            }

        }

        public int ChangeDistance
        {

            get
            {
                return ApplicationSettingsService.GetSetting<int>("ChangeDistance");
            }

            set
            {

                ApplicationSettingsService.SetSetting("ChangeDistance", Math.Max(value, 5), false);
                base.OnPropertyChanged(x => this.ChangeDistance);

            }

        }

        public bool IsFirstRun
        {

            get
            {
                return ApplicationSettingsService.GetSetting<bool>("FirstRun");
            }

        }

        public DelegateCommand<bool> UpdateFirstRun
        {

            get
            {

                if (this._updateFirstCommand == null)
                {
                    this._updateFirstCommand = new DelegateCommand<bool>(OnUpdateFirstRun);
                }

                return this._updateFirstCommand;

            }


        }

        public bool UpdateOnDistanceChange
        {

            get
            {
                return ApplicationSettingsService.GetSetting<bool>("UpdateOnDistanceChange");
            }

            set
            {

                ApplicationSettingsService.SetSetting("UpdateOnDistanceChange", value, false);
                base.OnPropertyChanged(x => this.UpdateOnDistanceChange);

            }

        }

        #endregion

        #region private

        public void OnUpdateFirstRun(bool allowLocation)
        {

            Services.ApplicationSettingsService.SetSetting("AllowLocation", allowLocation, false);
            Services.ApplicationSettingsService.SetSetting("FirstRun", false, false);

            base.OnPropertyChanged(x => this.IsFirstRun);

        }

        #endregion

    }

}
