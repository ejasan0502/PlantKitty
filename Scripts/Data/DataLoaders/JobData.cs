using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace PlantKitty.Scripts.Data.DataLoaders
{
    public class JobData : DataLoader<JobClass>
    {
        public JobData() : base()
        {
        }

        public override List<JobClass> GenerateData(string path)
        {
            List<JobClass> list = new List<JobClass>()
            {
                new JobClass()
                {
                    name = "Novice",
                    description = "You are a nobody.",
                    attributes = new Attributes(0),
                    stats = new Stats(0),
                    skills = new List<string>()
                    {
                        "Firebolt"
                    }
                }
            };

            string json = JsonConvert.SerializeObject(list);
            File.WriteAllText(path, json);
            Console.WriteLine("Created test JobData.json!");
            return list;
        }
        public override void AddDataToDictionary(List<JobClass> dataList)
        {
            foreach (JobClass job in dataList)
            {
                data.Add(job.name, job);
            }
            Console.WriteLine("JobClass data loaded!");
        }
    }
}
