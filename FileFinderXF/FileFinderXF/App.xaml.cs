using Xamarin.Forms;
using FileFinderXF.Core;
using Plugin.FilePicker.Abstractions;

namespace FileFinderXF
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Initialize();

            SetMainPage(checkFile: true);
        }

        public static void SetMainPage(bool checkFile = false)
        {
            if (checkFile)
            {
                var filePath = ComponentContainer.Current.Resolve<IFileManager>().FilePath;
                var fileName = ComponentContainer.Current.Resolve<IFileManager>().FileName;

                if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(fileName))
                {
                    try
                    {
                        FileData file = new FileData(filePath, fileName, (() =>
                        {
                            return ComponentContainer.Current.Resolve<ILocalFileSystemHelper>().GetFileStream(filePath);
                        }));

                        var viewModel = new FileInfoViewModel(file);
                        Current.MainPage = new NavigationPage(ViewContainer.Current.CreatePage(viewModel));

                        return;
                    }
                    catch (System.Exception ex)
                    {
                        ex.Print();
                    }
                }
            }

            Current.MainPage = new NavigationPage(ViewContainer.Current.CreatePage<FilePickViewModel>());
        }

        private void Initialize()
        {
            Bootstrapper.CreateTables();
            Bootstrapper.RegisterViews();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
