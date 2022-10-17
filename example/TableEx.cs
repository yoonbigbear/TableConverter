namespace Data
{
	public class DropGroupTableEx : Data.DropGroupTable
	{
		public Dictionary<uint, List<DropGroupEx>> OverrideTable = new();
		public Dictionary<uint, int> TotalPossibility = new();

		System.Random random = new();

		public struct DropGroupEx
		{
			public DropGroup data;
			public int possibility;
		}
		public override void Load(string path)
		{
			base.Load(path);

			int value = 0;
			foreach (var e in DropGroups)
			{
				if (!OverrideTable.ContainsKey(e.Value.groupid))
				{
					OverrideTable[e.Value.groupid] = new List<DropGroupEx>();
					value = 0;
				}
				value += e.Value.possibility;

				var dropGroupEx = new DropGroupEx();
				dropGroupEx.data = this[e.Key];
				dropGroupEx.possibility = value;
				OverrideTable[e.Value.groupid].Add(dropGroupEx);
				TotalPossibility[e.Value.groupid] = value;
			}
		}

		public uint Reward(uint groupId)
		{
			var total = TotalPossibility[groupId];

			var rand = random.Next(0, total);

			foreach (var e in OverrideTable[groupId])
			{
				if (e.possibility > rand)
					return e.data.item_tid;
			}
			return 0;
		}
	}
}
