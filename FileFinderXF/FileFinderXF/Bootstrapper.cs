using FileFinderXF.Core;
using System;
using System.Collections.Generic;

namespace FileFinderXF
{
    public class Bootstrapper
    {
        public static void RegisterTypes()
        {
            // Repositories
            ComponentContainer.Current.Register<IDatabaseRepository, DatabaseRepository>(singelton: true);

            // Helpers
            ComponentContainer.Current.Register<ITranslateHelper, TranslateHelper>();
            ComponentContainer.Current.Register<INetworkStatusHelper, NetworkStatusHelper>(singelton: true);

            // Managers
            ComponentContainer.Current.Register<IFileManager, FileManager>(singelton: true);
        }

        public static void RegisterViews()
        {
            ViewContainer.Current.Register<FilePickViewModel, FilePickPage>();
            ViewContainer.Current.Register<FileInfoViewModel, FileInfoPage>();
        }

        public static void CreateTables()
        {
            ComponentContainer.Current.Resolve<IDatabaseRepository>().CreateTablesAsync(new List<Type>()
            {
            });
        }
    }
}
