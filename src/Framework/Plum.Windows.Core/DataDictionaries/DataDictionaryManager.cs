using System;
using System.Collections.Generic;
using System.Linq;

namespace Plum.Windows.DataDictionaries
{
    public class DataDictionaryManager
    {
        private readonly IPlumApi api;
        private Dictionary<string, Dictionary<string, string>> dataDictionaries;

        public static DataDictionaryManager Instance { get; private set; }

        public static void Initialize(IPlumApi api)
        {
            Instance = new DataDictionaryManager(api);
        }

        private DataDictionaryManager(IPlumApi api)
        {
            this.api = api;
            dataDictionaries = new Dictionary<string, Dictionary<string, string>>();
        }

        public Dictionary<string, string> GetDictionaryByCode(string code)
        {
            lock (dataDictionaries)
            {
                if (dataDictionaries.ContainsKey(code))
                    return dataDictionaries[code];

                var result = new Dictionary<string, string>();

                //try
                //{
                //    var dataDictionary = api.GetDataDictionaryByCodeAsync(code).Result;

                //    foreach (var item in dataDictionary.Items.OrderBy(x => x.Code))
                //    {
                //        result.Add(item.Code, item.DisplayText);
                //    }

                //    dataDictionaries.Add(code, result);
                //}
                //catch (Exception ex)
                //{
                //    Serilog.Log.Error(ex.ToDetailString());
                //}

                return result;
            }
        }
    }
}