using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Plugin.Permissions;
using System;
using Droid = Android;

namespace FileFinderXF.Android
{
    [Activity(Label = "FileFinderXF", Icon = "@drawable/ic_launcher", Theme = "@style/SplashTheme", LaunchMode = LaunchMode.SingleInstance, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new string[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault }, DataMimeType = "application/pdf")]
    [IntentFilter(new string[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault }, DataMimeType = "application/x-pdf")]
    [IntentFilter(new string[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault }, DataMimeType = "text/*")]
    [IntentFilter(new string[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault }, DataMimeType = "image/*")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.SetTheme(Resource.Style.MainTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Bootstrapper_Droid.Initialize();

            Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Xamarin.Forms.FormsMaterial.Init(this, bundle);

            CheckForIntentData(Intent);

            LoadApplication(new App());
        }

        protected override void OnNewIntent(Intent intent)
        {
            CheckForIntentData(intent);
        }

        private void CheckForIntentData(Intent intent)
        {
            try
            {
                if (intent.Action.Equals(Intent.ActionSend) && intent.Type != null)
                {
                    Droid.Net.Uri uri = (Droid.Net.Uri)intent.GetParcelableExtra(Intent.ExtraStream);

                    // Set file path
                    string filePath = Plugin.FilePicker.IOUtil.GetPath(Application.Context, uri);

                    ComponentContainer.Current.Resolve<Core.IFileManager>().FilePath = filePath;
                    ComponentContainer.Current.Resolve<Core.IFileManager>().FileName = System.IO.Path.GetFileName(filePath);

                    App.SetMainPage(checkFile: true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("MainActivity: " + ex.Message);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

