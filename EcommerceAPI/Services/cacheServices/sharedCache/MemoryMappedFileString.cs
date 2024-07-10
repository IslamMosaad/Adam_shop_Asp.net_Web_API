using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;

namespace EcommerceAPI.Services.cacheServices.sharedCache
{
    public class MemoryMappedFileString
    {
        private static readonly string MapName = "SharedMemoryMap";
        private static readonly long MapSize = 10 * 1024 * 1024; // 10 MB
        private static readonly object LockObject = new object();

        public static void Set(string key, string value, TimeSpan expiration)
        {
            var cacheItem = new CacheItem
            {
                Value = value,
                Expiration = DateTime.UtcNow.Add(expiration)
            };

            lock (LockObject)
            {
                using (var mmf = MemoryMappedFile.CreateOrOpen(MapName, MapSize))
                {
                    var data = ReadAllEntries(mmf);
                    data[key] = cacheItem;
                    WriteAllEntries(mmf, data);
                }
            }
        }

        public static string Get(string key)
        {
            lock (LockObject)
            {
                using (var mmf = MemoryMappedFile.CreateOrOpen(MapName, MapSize))
                {
                    var data = ReadAllEntries(mmf);
                    if (data.TryGetValue(key, out var cacheItem) && cacheItem.Expiration > DateTime.UtcNow)
                    {
                        return cacheItem.Value;
                    }
                    else
                    {
                        Remove(key); // Automatically remove expired item if found
                        return null;
                    }
                }
            }
        }

        public static bool Remove(string key)
        {
            lock (LockObject)
            {
                using (var mmf = MemoryMappedFile.CreateOrOpen(MapName, MapSize))
                {
                    var data = ReadAllEntries(mmf);
                    if (data.Remove(key))
                    {
                        WriteAllEntries(mmf, data);
                        return true;
                    }
                    return false;
                }
            }
        }

        public static void RemoveExpired()
        {
            lock (LockObject)
            {
                using (var mmf = MemoryMappedFile.CreateOrOpen(MapName, MapSize))
                {
                    var data = ReadAllEntries(mmf);
                    var expiredKeys = data.Where(kv => kv.Value.Expiration <= DateTime.UtcNow)
                                          .Select(kv => kv.Key)
                                          .ToList();

                    foreach (var key in expiredKeys)
                    {
                        data.Remove(key);
                    }

                    WriteAllEntries(mmf, data);
                }
            }
        }

        private static Dictionary<string, CacheItem> ReadAllEntries(MemoryMappedFile mmf)
        {
            var data = new Dictionary<string, CacheItem>();

            try
            {
                using (var accessor = mmf.CreateViewAccessor(0, MapSize, MemoryMappedFileAccess.Read))
                {
                    accessor.Read(0, out long length);
                    Console.WriteLine($"Length read: {length}");

                    if (length <= 0 || length > MapSize - 8)
                    {
                        Console.WriteLine("Invalid length read from memory-mapped file.");
                        return data;
                    }

                    var bytes = new byte[length];
                    accessor.ReadArray(8, bytes, 0, (int)length); // Read starting from offset 8 to skip the length value itself

                    var jsonString = Encoding.UTF8.GetString(bytes);
                    data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, CacheItem>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading cache entries from memory-mapped file: {ex.Message}");
                throw; // Rethrow or handle the exception as per your application's error handling strategy
            }

            return data;
        }

        private static void WriteAllEntries(MemoryMappedFile mmf, Dictionary<string, CacheItem> data)
        {
            try
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var bytes = Encoding.UTF8.GetBytes(jsonString);

                Console.WriteLine($"Bytes to write: {bytes.Length}");

                using (var accessor = mmf.CreateViewAccessor(0, MapSize, MemoryMappedFileAccess.Write))
                {
                    // Ensure the length is written at the beginning (offset 0)
                    accessor.Write(0, bytes.LongLength);
                    Console.WriteLine($"Length written: {bytes.LongLength}");

                    // Write starting from offset 8 to store the length of bytes
                    accessor.WriteArray(8, bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing cache entries to memory-mapped file: {ex.Message}");
                throw; // Rethrow or handle the exception as per your application's error handling strategy
            }
        }

        public class CacheItem
        {
            public string Value { get; set; }
            public DateTime Expiration { get; set; }
        }
    }
}
