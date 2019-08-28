using FileFinderXF.Core;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FileFinderXF
{
    public class FileInfoViewModel : BaseViewModel
    {
        public FileInfoViewModel(FileData fileData)
        {
            Files.Add(new FileInfoItemViewModel(fileData));

            FileTypeSource.Add("Default");
            FileTypeSource.Add("Contract");
            FileTypeSource.Add("Image");
            FileTypeSource.Add("Invoice");
        }

        private ICommand openFileCommand;
        public ICommand OpenFileCommand => openFileCommand ?? (openFileCommand = new Command(async (param) =>
        {
            if (IsBusy || !(param is FileInfoItemViewModel item))
            {
                return;
            }

            try
            {
                IsBusy = true;

                PermissionHelper permission = new PermissionHelper();

                var perm = await permission.CheckPermissionAsync(Plugin.Permissions.Abstractions.Permission.Storage);

                if (perm)
                {
                    CrossFilePicker.Current.OpenFile(item.File);
                }
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

        private ICommand removeFileCommand;
        public ICommand RemoveFileCommand => removeFileCommand ?? (removeFileCommand = new Command((param) =>
        {
            if (IsBusy || !(param is FileInfoItemViewModel item))
            {
                return;
            }

            try
            {
                IsBusy = true;

                Files.Remove(item);
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

        private ICommand editFileCommand;
        public ICommand EditFileCommand => editFileCommand ?? (editFileCommand = new Command((param) =>
        {
            if (IsBusy || !(param is FileInfoItemViewModel item))
            {
                return;
            }

            try
            {
                IsBusy = true;

                item.IsEditOn = true;
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

        private ICommand sendFilesCommand;
        public ICommand SendFilesCommand => sendFilesCommand ?? (sendFilesCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            var confirm = await this.ShowConfirmAsync("Vill du skicka alla filer", "Skicka filer");

            if (!confirm)
            {
                return;
            }

            try
            {
                IsBusy = true;
                IsSendingFile = true;

                await Task.Delay(TimeSpan.FromSeconds(3));

                FileIsSent = true;

                SuccessEvent?.Invoke(this, null);
            }
            catch (Exception ex)
            {
                ex.Print();
            }
            finally
            {
                IsBusy = false;
                IsSendingFile = false;
            }
        }));

        private ICommand homeCommand;
        public ICommand HomeCommand => homeCommand ?? (homeCommand = new Command(async () =>
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;

                App.SetMainPage();
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

                Files.Add(new FileInfoItemViewModel(fileData));
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

        public ObservableCollection<FileInfoItemViewModel> Files { get; set; } = new ObservableCollection<FileInfoItemViewModel>();
        public ObservableCollection<string> FileTypeSource { get; set; } = new ObservableCollection<string>();

        public bool FileIsSent { get; set; }
        public bool IsSendingFile { get; set; }

        public event EventHandler SuccessEvent;
    }

    public class FileInfoItemViewModel : INotifyPropertyChanged
    {
        public FileInfoItemViewModel(FileData file)
        {
            this.File = file;

            this.FileName = file.FileName;
            this.SelectedFileType = "Default";
        }

        public string SelectedFileType { get; set; }
        public string FileName { get; set; }
        public FileData File { get; set; }
        public bool IsEditOn { get; set; }

        private ICommand editDoneCommand;
        public ICommand EditDoneCommand => editDoneCommand ?? (editDoneCommand = new Command(() =>
        {
            IsEditOn = false;
        }));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
