// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PharmaTab
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

        private const string UsernameKey = "settings_key";
        private static readonly string UsernameKeyDefault = string.Empty;

        #endregion

        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault(UsernameKey, UsernameKeyDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UsernameKey, value);
            }
        }

        private const string AdminKey = "userpassword_key";
        private static readonly string AdminDefault = string.Empty;
        public static string Adminstate
        {
            get { return AppSettings.GetValueOrDefault(AdminKey, AdminDefault); }
            set { AppSettings.AddOrUpdateValue(AdminKey, value); }
        }


        private const string Filename = "userpassword_key";
        private static readonly string filedefault = string.Empty;
        public static string FilePath
        {
            get { return AppSettings.GetValueOrDefault(Filename, filedefault); }
            set { AppSettings.AddOrUpdateValue(Filename, value); }
        }
    }
}