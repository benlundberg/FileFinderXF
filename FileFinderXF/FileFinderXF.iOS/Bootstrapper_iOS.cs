﻿using FileFinderXF.Core;

namespace FileFinderXF.iOS
{
    public class Bootstrapper_iOS
    {
        public static void Initialize()
        {
            // Register common types
            Bootstrapper.RegisterTypes();

            // Register device specific types
            RegisterTypes();
        }

        private static void RegisterTypes()
        {
            // Helpers
            ComponentContainer.Current.Register<ILocalizeHelper, LocalizeHelper_iOS>();
            ComponentContainer.Current.Register<ILocalFileSystemHelper, LocalFileSystemHelper_iOS>(singelton: true);
        }
    }
}
