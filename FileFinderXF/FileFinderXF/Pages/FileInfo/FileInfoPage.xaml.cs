using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileFinderXF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileInfoPage : ContentPage
    {
        public FileInfoPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SuccessLabel.Opacity = 0;

            if (!(BindingContext is FileInfoViewModel viewModel))
            {
                return;
            }

            viewModel.SuccessEvent += ViewModel_SuccessEvent;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (!(BindingContext is FileInfoViewModel viewModel))
            {
                return;
            }

            viewModel.SuccessEvent -= ViewModel_SuccessEvent;
        }

        private async void ViewModel_SuccessEvent(object sender, System.EventArgs e)
        {
            ToolbarItems.Clear();

            SuccessContent.TranslationY = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height / 2;

            await SuccessContent.TranslateTo(0, 0, 800, Easing.CubicIn);

            await SuccessLabel.FadeTo(1, 1000);
        }
    }
}