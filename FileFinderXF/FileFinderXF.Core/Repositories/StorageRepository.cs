﻿using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace FileFinderXF.Core
{
    public class StorageKey
    {

    }

    public class StorageRepository
    {
        static Lazy<StorageRepository> implementation = new Lazy<StorageRepository>(() => CreateStorage(), isThreadSafe: true);

        public static StorageRepository Current
        {
            get
            {
                var ret = implementation.Value;

                if (ret == null)
                {
                    throw new NotImplementedException();
                }

                return ret;
            }
        }

        private static StorageRepository CreateStorage()
        {
            BlobCache.ApplicationName = AppConfig.AppName;
            return new StorageRepository();
        }

        public async Task SaveAsync<T>(string key, T model)
        {
            try
            {
                await BlobCache.LocalMachine.InsertObject<T>(key, model);
            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        public async Task<T> LoadAsync<T>(string key)
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<T>(key);
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default(T);
        }

        public async Task<T> LoadAsync<T>(Func<T, bool> predExpr)
        {
            try
            {
                var list = await this.GetAllAsync<T>();

                return list.FirstOrDefault(predExpr);
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default(T);
        }

        public async Task DeleteAsync<T>(string key)
        {
            try
            {
                await BlobCache.LocalMachine.InvalidateObject<T>(key);
            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        public async Task DeleteAllAsync<T>()
        {
            try
            {
                await BlobCache.LocalMachine.InvalidateAllObjects<T>();
            }
            catch (Exception ex)
            {
                ex.Print();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            try
            {
                return await BlobCache.LocalMachine.GetAllObjects<T>();
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default(IEnumerable<T>);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Func<T, bool> predExpr)
        {
            try
            {
                var list = await GetAllAsync<T>();

                return list?.Where(predExpr);
            }
            catch (Exception ex)
            {
                ex.Print();
            }

            return default(IEnumerable<T>);
        }
    }
}
