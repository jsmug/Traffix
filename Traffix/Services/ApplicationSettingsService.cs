using System;
using System.IO.IsolatedStorage;

namespace Traffix.Services
{
    
    public static class ApplicationSettingsService
    {


        static ApplicationSettingsService()
        {

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("FirstRun"))
            {
                Reset();
            }

        }


        #region public

        public static T GetSetting<T>(string key)
        {

            T setting = default(T);

            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {

                try
                {
                    setting = (T)IsolatedStorageSettings.ApplicationSettings[key];
                }
                catch
                {
                }

            }

            return setting;

        }

        public static void SetSetting(string key, object value, bool autoSave)
        {

            if (!IsolatedStorageSettings.ApplicationSettings.Contains(key))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(key, value);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[key] = value;
            }

            if (autoSave)
            {
                IsolatedStorageSettings.ApplicationSettings.Save();
            }

        }

        public static void Save()
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static void Reset()
        {

            IsolatedStorageSettings.ApplicationSettings.Add("AllowLocation", false);
            IsolatedStorageSettings.ApplicationSettings.Add("ChangeDistance", 5);
            IsolatedStorageSettings.ApplicationSettings.Add("FirstRun", true);
            IsolatedStorageSettings.ApplicationSettings.Add("UpdateOnDistanceChange", true);
            IsolatedStorageSettings.ApplicationSettings.Save();

        }

        #endregion

    }

}
