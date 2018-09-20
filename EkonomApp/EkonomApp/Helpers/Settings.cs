using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace EkonomApp.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants
        private const string SettingsKey = "First_Run";
        private const string SettingsKey1 = "Number";
        private const string SettingsKey2 = "Class";
        private const string SettingsKey3 = "ClassNumber";
        private static readonly string SettingsDefault = "yes";
        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }
        public static string Number
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey1, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey1, value);
            }
        }
        public static string ClassNumber
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey3, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey3, value);
            }
        }
        public static string Class
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey2, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey2, value);
            }

        }
    }
}