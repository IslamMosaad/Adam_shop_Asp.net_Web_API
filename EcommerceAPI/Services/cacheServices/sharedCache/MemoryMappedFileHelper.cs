using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text.Json;
using System.Threading;

namespace EcommerceAPI.Services.cacheServices.sharedCache
{

    public static class MemoryMappedFileHelper<T> where T : class
    {
        private const string MapName = "SharedCacheMemory";
        private const int MapSize = 1024 * 1024 * 10; // 10 MB
        private static readonly string MutexName = "Global\\SharedCacheMemoryMutex";

        public static void WriteCacheData(T data)
        {
            var json = JsonSerializer.Serialize(data);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);

            using (var mutex = new Mutex(false, MutexName))
            {
                try
                {
                    mutex.WaitOne();

                    using (var mmf = MemoryMappedFile.CreateOrOpen(MapName, MapSize))
                    {
                        using (var accessor = mmf.CreateViewAccessor(0, bytes.Length, MemoryMappedFileAccess.Write))
                        {
                            accessor.WriteArray(0, bytes, 0, bytes.Length);
                        }
                    }
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        public static T ReadCacheData()
        {
            using (var mmf = MemoryMappedFile.OpenExisting(MapName))
            {
                using (var accessor = mmf.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read))
                {
                    byte[] bytes = new byte[MapSize]; // 1 MB buffer
                    accessor.ReadArray(0, bytes, 0, bytes.Length);

                    var json = System.Text.Encoding.UTF8.GetString(bytes).TrimEnd('\0');
                    return JsonSerializer.Deserialize<T>(json);
                }
            }
        }
    }

}

