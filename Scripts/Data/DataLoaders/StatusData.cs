using Newtonsoft.Json;
using PlantKitty.Scripts.Statuses;
using PlantKitty.Scripts.Statuses.StatusProperties;
using System;
using System.Collections.Generic;
using System.IO;

namespace PlantKitty.Scripts.Data.DataLoaders
{
    public class StatusData : DataLoader<Status>
    {
        public StatusData() : base()
        {
        }

        public override List<Status> GenerateData(string path)
        {
            List<Status> list = new List<Status>()
            {
                new Status()
                {
                    name = "Status 1",
                    continuous = false,
                    duration = 1,
                    properties = new List<StatusProperty>()
                    {
                        new DoT_StP()
                        {
                            percent = false,
                            inflict = 1f
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, json);
            Console.WriteLine("Created test StatusData.json!");
            return list;
        }
        public override void AddDataToDictionary(List<Status> dataList)
        {
            foreach (Status status in dataList)
            {
                data.Add(status.name, status);
            }
            Console.WriteLine("Status data loaded!");
        }
    }
}
