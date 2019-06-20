using Discord;
using Newtonsoft.Json;
using PlantKitty.Scripts.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlantKitty.Scripts.Data
{
    public class Player : Character
    {
        public ulong id;

        public PlayerTask task;
        public string field;
        public JobClass job;

        public int level;
        public float exp;
        public int pointsAvailable;
        public Attributes lastAttributes;
        public Attributes attributes;
        public Stats baseStats;

        public Equip[] equipment;
        public Inventory inventory;

        private float maxExp;
        private bool leveledUp;
        private List<Buff> buffs;

        public Player()
        {
            name = "";
            id = 0;

            task = null;
            field = "Trainee Island";
            job = new JobClass()
            {
                name = "Novice",
                description = "You are a nobody.",
                additive = null
            };

            level = 1;
            exp = 0f;
            pointsAvailable = 0;
            CalculateMaxExp();
            leveledUp = false;

            attributes = new Attributes(1);
            lastAttributes = attributes;

            equipment = new Equip[System.Enum.GetNames(typeof(EquipType)).Length];
            inventory = new Inventory(30);
            buffs = new List<Buff>();

            CalculateStats();
            CalculateMaxStats();
            currentStats = new Stats(maxStats);
        }

        private void CalculateMaxExp()
        {
            // x^2
            maxExp = level * level;
        }
        private void CalculateStats()
        {
            Attributes positive = new Attributes()
            {
                STR = CalculatePositiveBaseStat(attributes.STR),
                VIT = CalculatePositiveBaseStat(attributes.VIT),
                DEX = CalculatePositiveBaseStat(attributes.DEX),
                AGI = CalculatePositiveBaseStat(attributes.AGI),
                INT = CalculatePositiveBaseStat(attributes.INT),
                PSY = CalculatePositiveBaseStat(attributes.PSY)
            };
            Attributes negative = new Attributes()
            {
                STR = CalculateNegativeBaseStat(attributes.STR),
                VIT = CalculateNegativeBaseStat(attributes.VIT),
                DEX = CalculateNegativeBaseStat(attributes.DEX),
                AGI = CalculateNegativeBaseStat(attributes.AGI),
                INT = CalculateNegativeBaseStat(attributes.INT),
                PSY = CalculateNegativeBaseStat(attributes.PSY)
            };

            baseStats = new Stats()
            {
                HP = 30 + positive.VIT - negative.PSY,
                MP = 10 + positive.PSY,
                ATK = 1 + (positive.STR + positive.INT) / 2.00f - negative.DEX,
                DEF = 0 - negative.AGI - negative.INT,
                ACC = 70 + positive.DEX - negative.STR,
                EVA = 0 + positive.AGI - negative.VIT,
                SPD = 0 + positive.AGI - negative.STR
            };
        }
        private int CalculatePositiveBaseStat(int att)
        {
            // Positive = (100x)^(1/2)
            return (int) Math.Round( Math.Pow(100.00 * att, 1.00 / 2.00) );
        }
        private int CalculateNegativeBaseStat(int att)
        {
            // Negative = (100x-3000)^(1/2)
            if (att > 30)
                return (int)Math.Round(Math.Pow(100.00 * att - 3000.00, 1.00 / 2.00));
            else
                return 0;
        }
        private void ApplyEquipmentStats()
        {
            maxStats = baseStats;
            foreach (Equip e in equipment)
            {
                if (e != null)
                    maxStats += e.stats;
            }
        }
        private void ApplyBuffs()
        {
            foreach (Buff b in buffs)
            {
                b.Apply(this);
            }
        }
        private void IncreaseAttributes()
        {
            lastAttributes = new Attributes(attributes.ToString());

            if (job.additive == null)
            {
                List<string> att = attributes.GetType().GetFields().Select(a => a.Name).ToList();
                Random random = new Random();
                AddAttribute(att[random.Next(0, att.Count)], 1, false);
            } else
            {
                attributes += job.additive;

                CalculateStats();
                CalculateMaxStats();
            }
        }

        public void SetTask(PlayerTask task)
        {
            this.task = task;
            PlayerData.Instance.SaveData();
        }
        public void EndTask(string username, EmbedBuilder builder)
        {
            if (task == null) return;

            task.End(username, this, builder);
            task = null;

            PlayerData.Instance.SaveData();
        }
        public void SetJobClass(JobClass job)
        {
            this.job = job;

            if (this.job.additive != null)
            {
                attributes += this.job.additive;

                CalculateStats();
                CalculateMaxStats();
            }
        }

        public void LevelUp()
        {
            level++;
            CalculateMaxExp();

            IncreaseAttributes();
            CalculateStats();
            CalculateMaxStats();

            currentStats = new Stats(maxStats);
            pointsAvailable += 5;
            leveledUp = true;
        }
        public void CalculateMaxStats()
        {
            ApplyEquipmentStats();
            ApplyBuffs();
        }
        public void AddAttribute(string attribute, int amount, bool subtractPoint = true)
        {
            FieldInfo field = attributes.GetType().GetField(attribute);
            if (field == null) return;

            int amt = (int)field.GetValue(attributes) + amount;
            field.SetValue(attributes, amt);
            if (subtractPoint) pointsAvailable -= amount;

            CalculateStats();
            CalculateMaxStats();
        }
        public float GetMaxExp()
        {
            return maxExp;
        }
        public void AddExp(float xp)
        {
            exp += xp;
            if (exp < 0) exp = 0f;
            else if (exp >= maxExp) LevelUp();
        }
        public bool CheckIfLeveledUp(out Attributes changes)
        {
            if (leveledUp)
            {
                leveledUp = false;
                changes = attributes - lastAttributes;
                return true;
            }
            changes = new Attributes(0);
            return false;
        }

        public void Equip(Equip equip)
        {
            int index = (int)equip.equipType;
            if (equipment[index] != null)
            {
                inventory.AddItem(equipment[index], 1);
                equipment[index] = null;

                if (equip.equipType == EquipType.twoHand)
                {
                    // Remove primary and secondary when equipping two hand weapon
                    if (equipment[(int)EquipType.primary] != null)
                    {
                        inventory.AddItem(equipment[(int)EquipType.primary], 1);
                        equipment[(int)EquipType.primary] = null;
                    }
                    if (equipment[(int)EquipType.secondary] != null)
                    {
                        inventory.AddItem(equipment[(int)EquipType.secondary], 1);
                        equipment[(int)EquipType.secondary] = null;
                    }
                } else if (equip.equipType == EquipType.overall)
                {
                    // Remove chest and pants when equipping overall
                    if (equipment[(int)EquipType.chest] != null)
                    {
                        inventory.AddItem(equipment[(int)EquipType.chest], 1);
                        equipment[(int)EquipType.chest] = null;
                    }
                    if (equipment[(int)EquipType.pants] != null){
                        inventory.AddItem(equipment[(int)EquipType.pants], 1);
                        equipment[(int)EquipType.pants] = null;
                    }
                } else if (equip.equipType == EquipType.primary || equip.equipType == EquipType.secondary)
                {
                    // Remove two hand weapon when equipping a 1h weapon
                    if (equipment[(int)EquipType.twoHand] != null)
                    {
                        inventory.AddItem(equipment[(int)EquipType.twoHand], 1);
                        equipment[(int)EquipType.twoHand] = null;
                    }
                } else if (equip.equipType == EquipType.chest || equip.equipType == EquipType.pants)
                {
                    // Remove overall armor when equipping chest or pants
                    if (equipment[(int)EquipType.overall] != null)
                    {
                        inventory.AddItem(equipment[(int)EquipType.overall], 1);
                        equipment[(int)EquipType.overall] = null;
                    }
                }
            }

            equipment[index] = equip;
            CalculateStats();
        }
        public void RemoveEquip(Equip equip)
        {
            int index = (int)equip.equipType;
            if (equipment[index] != null)
            {
                inventory.AddItem(equipment[index], 1);
                equipment[index] = null;
                CalculateStats();
            }
        }

        public bool HasBuff(Buff buff)
        {
            foreach (Buff b in buffs)
            {
                if (b.GetType() == buff.GetType())
                    return true;
            }
            return false;
        }
        public bool AddBuff(Buff buff)
        {
            if (!HasBuff(buff))
            {
                buffs.Add(buff);
                CalculateStats();
                CalculateMaxStats();

                return true;
            }
            return false;
        }
        public void CheckBuffs()
        {
            if (buffs.Count > 0)
            {
                bool recalculateStats = false;
                for (int i = buffs.Count - 1; i >= 0; i--)
                {
                    if (buffs[i].IsDone)
                    {
                        buffs.RemoveAt(i);
                        recalculateStats = true;
                    }
                }

                if (recalculateStats)
                {
                    CalculateStats();
                    CalculateMaxStats();
                }
            }
        }
    }
}
