using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public abstract class DataLoader<T>
    {
        public Dictionary<string, T> data;

        public DataLoader()
        {
            data = new Dictionary<string, T>();
        }

        public abstract List<T> GenerateData(string path);
        public abstract void AddDataToDictionary(List<T> dataList);

        public async Task Load(string path)
        {
            List<T> dataList = new List<T>();
            if (!File.Exists(path))
            {
                dataList = GenerateData(path);
            } 
            else
                dataList = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));

            AddDataToDictionary(dataList);
            await Task.Delay(1000);
        }
    }
}
