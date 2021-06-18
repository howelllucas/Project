using System;

namespace Game
{
    public class CampSet_TableItem : TableItem
    {

        public int id;
        public int taskField;
        public int taskLevel;
        public int taskCount;
        public int taskDifficulty;
        public string campBuilding;
        public string autoLevel;
        public string fixedTask;
        public string sceneName;
        public string tid_name;
        public string name;
        public int reward;
        public int reward_type;
        public int reward_count;
        public int[] CampBuildingArr { get; private set; }
        public int[] AutoLevelArr { get; private set; }
        public int[] FixedTaskArr { get; private set; }
        public int[] TaskParamArr { get; private set; }

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "taskField", "taskField");
            fields.addField(this, "taskLevel", "taskLevel");
            fields.addField(this, "taskCount", "taskCount");
            fields.addField(this, "taskDifficulty", "taskDifficulty");
            fields.addField(this, "campBuilding", "campBuilding");
            fields.addField(this, "autoLevel", "autoLevel");
            fields.addField(this, "fixedTask", "fixedTask");
            fields.addField(this, "sceneName", "sceneName");
            fields.addField(this, "tid_name", "tid_name");
            fields.addField(this, "name", "name");
            fields.addField(this, "reward", "reward");
            fields.addField(this, "reward_type", "reward_type");
            fields.addField(this, "reward_count", "reward_count");

        }

        public override void Init()
        {
            base.Init();
            string[] parser = new string[] { "|" };
            CampBuildingArr = SpliteToInts(campBuilding, parser);
            AutoLevelArr = SpliteToInts(autoLevel, parser);
            //FixedTaskArr = SpliteToInts(fixedTask, parser);
            var taskArr = SplitToString(fixedTask);
            FixedTaskArr = new int[taskArr.Length];
            TaskParamArr = new int[taskArr.Length];
            for (int i = 0;i < taskArr.Length;++i)
            {
                var _values = taskArr[i].Split('#');
                if (_values.Length == 2)
                {
                    if (!int.TryParse(_values[0], out FixedTaskArr[i]))
                    {
                        FixedTaskArr[i] = 0;
                    }

                    if (!int.TryParse(_values[1], out TaskParamArr[i]))
                    {
                        TaskParamArr[i] = 0;
                    }
                }
                else
                {
                    if (!int.TryParse(taskArr[i], out FixedTaskArr[i]))
                    {
                        FixedTaskArr[i] = 0;
                    }

                    TaskParamArr[i] = -1;
                }
            }
        }
    }
}
