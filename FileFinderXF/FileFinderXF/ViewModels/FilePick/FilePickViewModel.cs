using FileFinderXF.Core;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace FileFinderXF
{
    public class FilePickViewModel : BaseViewModel
    {
        private ICommand pickFileCommand;
        public ICommand PickFileCommand => pickFileCommand ?? (pickFileCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                FileData fileData = await CrossFilePicker.Current.PickFile();

                if (fileData == null)
                {
                    // User canceled the pick
                }

                await Navigation.PushAsync(ViewContainer.Current.CreatePage(new FileInfoViewModel(fileData)));
            }
            catch (Exception ex)
            {
                ex.Print();
            }
            finally
            {
                IsBusy = false;
            }
        }));
    }
}
