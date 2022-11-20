//-----------------------------------------
// Auto Generated
//-----------------------------------------


using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Data
{

	public enum Usage : Byte
	{
		None = 0,
		weapon = 1,
		defence = 2,
		consume = 3,
		accessory = 4,
		misc = 5,
	}

	public enum Level : Byte
	{
		None = 0,
		normal = 1,
		magic = 2,
		rare = 3,
		unique = 4,
		legend = 5,
	}


	[StructLayout(LayoutKind.Sequential)]
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
	public struct Item
	{
		public readonly UInt32 id;
		public readonly Boolean stack;
		public readonly Byte level;
		public readonly Byte usage;
		public readonly String name;
		public readonly String sprite;
		public readonly String desc;
		public Item(object[] param)
		{
			this.id = (UInt32)param[0];
			this.stack = (Boolean)param[1];
			this.level = (Byte)param[2];
			this.usage = (Byte)param[3];
			this.name = (String)param[4];
			this.sprite = (String)param[5];
			this.desc = (String)param[6];
		}
	}


	public class ItemTable
	{
		protected Dictionary<System.UInt32, Item> Items = new ();
		protected Dictionary<string, Type> types = new ();

		public Item this[System.UInt32 id] => Items[id];
		public int Count {get; set; }

		public virtual void Load(string path)
		{
			TypeFunc();
			var table = ConvertCSVtoDataTable(path);
			var rows = table.Rows;
			Count = rows.Count;
			for (int i = 0; i < Count; ++i)
			{
				Item t = new Item(rows[i].ItemArray);
				Items.Add(t.id, t);
			}
		}

		void TypeFunc()
		{
			types["id"] = typeof(UInt32);
			types["stack"] = typeof(Boolean);
			types["level"] = typeof(Byte);
			types["usage"] = typeof(Byte);
			types["name"] = typeof(String);
			types["sprite"] = typeof(String);
			types["desc"] = typeof(String);
		}


		DataTable ConvertCSVtoDataTable(string strFilePath)
		{
			DataTable dt = new DataTable();
			using (StreamReader sr = new StreamReader(strFilePath))
			{
				string[] headers = sr.ReadLine().Split(',');
				for(int i = 0; i < headers.Length; ++i)
				{
#if UNITY_2021_3_OR_NEWER
					//client only
					dt.Columns.Add(headers[i].Split(':')[0], types[headers[i].Split(':')[0]]);
#else
					//exclude client only
					if (headers[i].Split(':').Length > 1)
						continue;
					dt.Columns.Add(headers[i], types[headers[i]]);
#endif
				}

				while(!sr.EndOfStream)
				{
					int typeIndex = 0;
					string[] rows = sr.ReadLine().Split(',');
					DataRow dr = dt.NewRow();
					for (int i = 0; i < headers.Length; i++)
					{
#if UNITY_2021_3_OR_NEWER
#else
						//exclude client only
						if (headers[i].Split(':').Length > 1)
							continue;
#endif
						var list = rows[i].Split(':');
						//if array
						if (list.Length > 1)
						{
#if UNITY_2021_3_OR_NEWER
							dr[typeIndex] = DataTableManager.ArrayConverter(types[headers[i]].Name, rows[i]);
#else
							dr[typeIndex] = Global.TableData.ArrayConverter(types[headers[i]].Name, rows[i]);
#endif
						}
						else
							dr[typeIndex] = list[0];
						typeIndex++;
					}
					dt.Rows.Add(dr);
				}
			}
			return dt;
		}
	}
}
