
namespace Global
{
	public class TableData : Singleton<TableData>
	{
		string execPath = Path.GetDirectoryName(System.Reflection.Assembly.
				GetExecutingAssembly().GetName().CodeBase)?.Replace("file:\\", "");
		const string csvPath = "\\..\\..\\data\\";

		public Data.ItemTable itemTable { get; set; }
		public Data.DropGroupTableEx dropGroupTableEx { get; set; }


		public void Start()
		{
			itemTable = new Data.ItemTable();
			itemTable.Load($"{execPath}{csvPath}item.csv");

			dropGroupTableEx = new Data.DropGroupTableEx();
			dropGroupTableEx.Load($"{execPath}{csvPath}DropGroup.csv");

		}

		static public object ArrayConverter(string type, string value)
		{
			var splitValue = value.Split(':');
			switch (type)
			{
				case "byte[]":
					{
						byte[] arr = new byte[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToByte(splitValue[i]);
						return arr;
					}
				case "SByte[]":
					{
						SByte[] arr = new SByte[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToSByte(splitValue[i]);
						return arr;
					}
				case "Int16[]":
					{
						Int16[] arr = new Int16[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToInt16(splitValue[i]);
						return arr;
					}
				case "UInt16[]":
					{
						UInt16[] arr = new UInt16[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToUInt16(splitValue[i]);
						return arr;
					}
				case "Int32[]":
					{
						Int32[] arr = new Int32[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToInt32(splitValue[i]);
						return arr;
					}
				case "UInt32[]":
					{
						UInt32[] arr = new UInt32[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToUInt32(splitValue[i]);
						return arr;
					}
				case "Int64[]":
					{
						Int64[] arr = new Int64[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToInt64(splitValue[i]);
						return arr;
					}
				case "UInt64[]":
					{
						UInt64[] arr = new UInt64[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToUInt64(splitValue[i]);
						return arr;
					}
				case "Single[]":
					{
						Single[] arr = new Single[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToSingle(splitValue[i]);
						return arr;
					}
				case "Double[]":
					{
						Double[] arr = new Double[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToDouble(splitValue[i]);
						return arr;
					}
				case "string[]":
					return splitValue;
				case "bool[]":
					{
						Boolean[] arr = new Boolean[splitValue.Length];
						for (int i = 0; i < arr.Length; i++)
							arr[i] = Convert.ToBoolean(splitValue[i]);
						return arr;
					}
				default:
					{
						return typeof(System.Enum);
					}
			}
		}
	}

}
