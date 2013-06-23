using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;

namespace IcucApp.Core
{
    public static class CacheStore
    {
        /// <summary>
        /// In-memory cache dictionary
        /// </summary>
        private static readonly Dictionary<string, object> Cache;
        private static readonly object Sync;

        /// <summary>
        /// Cache initializer
        /// </summary>
        static CacheStore()
        {
            Cache = new Dictionary<string, object>();
            Sync = new object();
        }


        /// <summary>
        /// Check if an object exists in cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Name of key in cache</param>
        /// <returns>True, if yes; False, otherwise</returns>
        public static bool Exists(string key)
        {
            lock (Sync)
            {
                return Cache.ContainsKey(key);
            }
        }

        /// <summary>
        /// Check if an object exists in cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <returns>True, if yes; False, otherwise</returns>
        public static bool Exists<T>() where T : class
        {
            Type type = typeof(T);
            lock (Sync)
            {
                return Cache.ContainsKey(type.Name);
            }
        }

        /// <summary>
        /// Get an object from cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <returns>Object from cache</returns>
        public static T Get<T>() where T : class
        {
            Type type = typeof(T);
            lock (Sync)
            {
                if (Cache.ContainsKey(type.Name) == false)
                    throw new ApplicationException("An object of the desired type does not exist: " + type.Name);

                lock (Sync)
                {
                    return (T)Cache[type.Name];
                }
            }
        }

		/// <summary>
		/// Load an object from cache
		/// Throws an exception when not found
		/// </summary>
		/// <typeparam name="T">Type of object</typeparam>
		/// <param name="key">Name of key in cache</param>
		/// <returns>Object from cache</returns>
		public static T Load<T>(string key) where T : class
		{
			Type type = typeof(T);
			lock (Sync)
			{
				if (Cache.ContainsKey(key) == false)
					throw new ApplicationException(String.Format("An object with key '{0}' does not exists", key));
				
				lock (Sync)
				{
					return (T)Cache[key];
				}
			}
		}

        /// <summary>
        /// Get an object from cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Name of key in cache</param>
        /// <returns>Object from cache</returns>
        public static T Get<T>(string key) where T : class
        {
            Type type = typeof(T);
            lock (Sync)
            {
                if (Cache.ContainsKey(key) == false) {
                    lock (Sync) {
    				    // try read from storage
                        var data = ReadFromStorage<T>(key);
                        if (data != null) {
                            Cache.Add(key, data);
                            return data;
                        }
                        return default(T);
                    }
                }

                lock (Sync)
                {
                    return (T)Cache[key];
                }
            }
        }

        /// <summary>
        /// Create default instance of the object and add it in cache
        /// </summary>
        /// <typeparam name="T">Class whose object is to be created</typeparam>
        /// <returns>Object of the class</returns>
        public static T Create<T>(string key, params object[] constructorParameters) where T : class
        {
            Type type = typeof(T);
            T value = (T)Activator.CreateInstance(type, constructorParameters);
            lock (Sync)
            {
                if (Cache.ContainsKey(key))
                    throw new ApplicationException(String.Format("An object with key '{0}' already exists", key));

                lock (Sync)
                {
                    Cache.Add(key, value);
                    SaveToStorage(type.Name, value);
                }
            }
            return value;
        }

        /// <summary>
        /// Create default instance of the object and add it in cache
        /// </summary>
        /// <typeparam name="T">Class whose object is to be created</typeparam>
        /// <returns>Object of the class</returns>
        public static T Create<T>(params object[] constructorParameters) where T : class
        {
            Type type = typeof(T);
            T value = (T)Activator.CreateInstance(type, constructorParameters);
            lock (Sync)
            {
                if (Cache.ContainsKey(type.Name))
                    throw new ApplicationException(String.Format("An object of type '{0}' already exists", type.Name));
                
                lock (Sync)
                {
                    Cache.Add(type.Name, value);
                    SaveToStorage(type.Name, value);
                }
            }
            return value;
        }


        public static void Add<T>(string key, T value)
        {
            lock (Sync)
            {
                if (Cache.ContainsKey(key))
                    throw new ApplicationException(String.Format("An object with key '{0}' already exists", key));

                lock (Sync)
                {
                    Cache.Add(key, value);
                    SaveToStorage(key, value);
                }
            }
        }

		public static void Set<T>(string key, T value, bool persist)
		{
			lock (Sync)
			{
				if (Cache.ContainsKey(key))
					Cache.Remove(key);

				Cache.Add(key, value);

                if (persist)
                    SaveToStorage(key, value);
			}
		}

        /// <summary>
        /// Remove an object type from cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        public static void Remove<T>()
        {
            Type type = typeof(T);
            lock (Sync)
            {
                if (Cache.ContainsKey(type.Name) == false)
                    return;

                lock (Sync)
                {
                    Cache.Remove(type.Name);
                }
            }
        }


        /// <summary>
        /// Remove an object stored with a key from cache
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key of the object</param>
        public static void Remove(string key)
        {
            lock (Sync)
            {
                if (Cache.ContainsKey(key) == false)
					return; // ignore when not exist

                lock (Sync)
                {
                    Cache.Remove(key);
                }
            }
        }

        private static T ReadFromStorage<T>(string name) {
            var fileName = name + ".txt";

            // save to isolated storage
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

            if (!isolatedStorage.FileExists(fileName))
                return default(T);

            using (StreamReader readFile = new StreamReader(new IsolatedStorageFileStream(fileName, FileMode.Open, FileAccess.Read, isolatedStorage)))
            {
                var jsonString = readFile.ReadToEnd();
                readFile.Close();
                return jsonString.FromJson<T>();
            }
        }

        private static void SaveToStorage(string name, object obj) {

            var fileName = name + ".txt";

            // concert to json
            var jsonString = obj.ToJson();

            // save to isolated storage
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write, isolatedStorage)))
            {
                writeFile.WriteLine(jsonString);
                writeFile.Close();
            }
        }

    }
}