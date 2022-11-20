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


	[StructLayout(LayoutKind.Sequential)]
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
	public struct DropGroup
	{
		public readonly UInt32 id;
		public readonly UInt32 groupid;
		public readonly UInt32 item_tid;
		public readonly Int32 possibility;
		public DropGroup(object[] param)
		{
			this.id = (UInt32)param[0];
			this.groupid = (UInt32)param[1];
			this.item_tid = (UInt32)param[2];
			this.possibility = (Int32)param[3];
		}
	}


	public class DropGroupTable
	{
		protected Dictionary<System.UInt32, DropGroup> DropGroups = new ();
		protected Dictionary<string, Type> types = new ();

		public DropGroup this[System.UInt32 id] => DropGroups[id];
		public int Count {get; set; }

		public virtual void Load(string path)
		{
			TypeFunc();
			var table = ConvertCSVtoDataTable(path);
			var rows = table.Rows;
			Count = rows.Count;
			for (int i = 0; i < Count; ++i)
			{
				DropGroup t = new DropGroup(rows[i].ItemArray);
				DropGroups.Add(t.id, t);
			}
		}

		void TypeFunc()
		{
			types["id"] = typeof(UInt32);
			types["groupid"] = typeof(UInt32);
			types["item_tid"] = typeof(UInt32);
			types["possibility"] = typeof(Int32);
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
