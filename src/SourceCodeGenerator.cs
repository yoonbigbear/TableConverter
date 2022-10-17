public class SourceCodeGenerator
{
	DataT mainTable = new DataT();
	List<EnumT> enumTs = new List<EnumT>();

	enum Target
	{
		Server = 0,
		Client = 1,
	}

	Target target = Target.Server;

	public SourceCodeGenerator(DataT mainTable, List<EnumT> enumTs)
	{
		this.mainTable = mainTable;
		this.enumTs = enumTs;
	}

	public void Generate(string outputpath, int target)
	{
		this.target = (Target)target;

		if (!Directory.Exists(outputpath))
		{
			Directory.CreateDirectory(outputpath);
		}

		if (File.Exists($"{outputpath}\\gen{mainTable.name}.cs"))
		{ 
			File.Delete($"{outputpath}\\gen{mainTable.name}.cs");
		}

		using (StreamWriter sw = new StreamWriter($"{outputpath}\\gen{mainTable.name}.cs"))
		{
			Header(sw);
			Using(sw);
			Namespace(sw);
		}
	}

	void Header(StreamWriter sw)
	{
		sw.WriteLine("//-----------------------------------------");
		sw.WriteLine("// Auto Generated");
		sw.WriteLine("//-----------------------------------------");
		sw.WriteLine("");
		sw.WriteLine("");
	}

	void Using(StreamWriter sw)
	{
		sw.WriteLine("using System;");
		sw.WriteLine("using System.Collections.Generic;");
		sw.WriteLine("using System.Data;");
		sw.WriteLine("using System.IO;");
		sw.WriteLine("using System.Runtime.CompilerServices;");
		sw.WriteLine("using System.Runtime.InteropServices;");
		sw.WriteLine("");
	}

	void Namespace(StreamWriter sw)
	{
		sw.WriteLine("namespace Data");
		sw.WriteLine("{");
		sw.WriteLine("");
		Enums(sw);
		sw.WriteLine("");
		DataStruct(sw);
		sw.WriteLine("");
		DataClass(sw);
		sw.WriteLine("}");
	}

	void Enums(StreamWriter sw)
	{
		foreach (EnumT t in enumTs)
		{
			sw.WriteLine($"	public enum {t.name} : {t.type.Name}");
			sw.WriteLine("	{");
			sw.WriteLine($"		None = 0,");
			foreach (var e in t.body)
			{
				sw.WriteLine($"		{e.Key} = {e.Value},");
			}

			sw.WriteLine("	}");
			sw.WriteLine("");
		}
	}

	void DataStruct(StreamWriter sw)
	{
		sw.WriteLine($"	[StructLayout(LayoutKind.Sequential)]");
		sw.WriteLine($"#if NET5_0_OR_GREATER");
		sw.WriteLine($"	[SkipLocalsInit]");
		sw.WriteLine($"#endif");
		sw.WriteLine($"	public struct {mainTable.name}");
		sw.WriteLine("	{");
		var rows = mainTable.DataTable.Rows;
		var columns = mainTable.DataTable.Columns;


		//writing data field
		for (int i = 0; i < columns.Count; ++i)
		{
			if (this.target == Target.Server)
			{
				//exclude client only type
				if (columns[i].ColumnName.Split(":").Length > 1)
				{
					continue;
				}
				sw.WriteLine($"		public readonly {columns[i].DataType.Name} {columns[i].ColumnName};");
			}
			else
			{
				sw.WriteLine($"		public readonly {columns[i].DataType.Name} {columns[i].ColumnName.Split(":")[0]};");
			}

		
		}

		sw.WriteLine($"		public {mainTable.name}(object[] param)");
		sw.WriteLine("		{");
		for (int i = 0; i < columns.Count; ++i)
		{
			if (this.target == Target.Server)
			{
				if (columns[i].ColumnName.Split(":").Length > 1)
				{
					continue;
				}
				sw.WriteLine($"			this.{columns[i].ColumnName} = ({columns[i].DataType.Name})param[{i}];");
			}
			else
			{
				sw.WriteLine($"			this.{columns[i].ColumnName.Split(":")[0]} = ({columns[i].DataType.Name})param[{i}];");
			}
		}
		sw.WriteLine("		}");

		sw.WriteLine("	}");
		sw.WriteLine($"");
	}

	void DataClass(StreamWriter sw)
	{
		sw.WriteLine($"	public class {mainTable.name}Table");
		sw.WriteLine("	{");

		sw.WriteLine($"		protected Dictionary<{mainTable.keyType}, {mainTable.name}> {mainTable.name}s = new ();");
		sw.WriteLine($"		protected Dictionary<string, Type> types = new ();");
		sw.WriteLine($"");
		sw.WriteLine($"		public {mainTable.name} this[{mainTable.keyType} id] => {mainTable.name}s[id];");
		sw.WriteLine($"		public int Count {{get; set; }}");
		sw.WriteLine($"");
		//StartFunc(sw);
		LoadFunc(sw);
		sw.WriteLine($"");
		TypeFunc(sw);
		sw.WriteLine($"");
		DataTableConvertFunc(sw);
		sw.WriteLine("	}");
	}

	void TypeFunc(StreamWriter sw)
	{
		sw.WriteLine($"		void TypeFunc()");
		sw.WriteLine("		{");
		var rows = mainTable.DataTable.Rows;
		var columns = mainTable.DataTable.Columns;

		for (int i = 0; i < columns.Count; ++i)
		{
			if (this.target == Target.Server)
			{
				//exclude for client type
				if (columns[i].ColumnName.Split(":").Length > 1)
				{
					continue;
				}
				sw.WriteLine($"			types[\"{columns[i].ColumnName}\"] = typeof({columns[i].DataType.Name});");
			}
			else
			{
				sw.WriteLine($"			types[\"{columns[i].ColumnName.Split(":")[0]}\"] = typeof({columns[i].DataType.Name});");
			}

			
		}

		sw.WriteLine("		}");
		sw.WriteLine($"");
	}

	void StartFunc(StreamWriter sw)
	{
		sw.WriteLine($"		public void Start()");
		sw.WriteLine("		{");

		sw.WriteLine("			TypeFunc();");

		sw.WriteLine("		}");
		sw.WriteLine($"");
	}

	void DataTableConvertFunc(StreamWriter sw)
	{
		sw.WriteLine("		DataTable ConvertCSVtoDataTable(string strFilePath)");
		sw.WriteLine("		{");
		sw.WriteLine("			DataTable dt = new DataTable();");
		sw.WriteLine("			using (StreamReader sr = new StreamReader(strFilePath))");
		sw.WriteLine("			{");
		sw.WriteLine("				string[] headers = sr.ReadLine().Split(',');");
		sw.WriteLine("				for(int i = 0; i < headers.Length; ++i)");
		sw.WriteLine("				{");
		sw.WriteLine("#if UNITY_2021_3_OR_NEWER");
		sw.WriteLine("					//client only");
		sw.WriteLine("					dt.Columns.Add(headers[i].Split(':')[0], types[headers[i].Split(':')[0]]);");
		sw.WriteLine("#else");
		sw.WriteLine("					//exclude client only");
		sw.WriteLine("					if (headers[i].Split(':').Length > 1)");
		sw.WriteLine("						continue;");
		sw.WriteLine("					dt.Columns.Add(headers[i], types[headers[i]]);");
		sw.WriteLine("#endif");
		sw.WriteLine("				}");
		sw.WriteLine($"");
		sw.WriteLine("				while(!sr.EndOfStream)");
		sw.WriteLine("				{");
		sw.WriteLine("					int typeIndex = 0;");
		sw.WriteLine("					string[] rows = sr.ReadLine().Split(',');");
		sw.WriteLine("					DataRow dr = dt.NewRow();");
		sw.WriteLine("					for (int i = 0; i < headers.Length; i++)");
		sw.WriteLine("					{");
		sw.WriteLine("#if UNITY_2021_3_OR_NEWER");
		sw.WriteLine("#else");
		sw.WriteLine("						//exclude client only");
		sw.WriteLine("						if (headers[i].Split(':').Length > 1)");
		sw.WriteLine("							continue;");
		sw.WriteLine("#endif");
		sw.WriteLine("						var list = rows[i].Split(':');");
		sw.WriteLine("						//if array");
		sw.WriteLine("						if (list.Length > 1)");
		sw.WriteLine("						{");
		sw.WriteLine("#if UNITY_2021_3_OR_NEWER");
		sw.WriteLine("							dr[typeIndex] = DataTableManager.ArrayConverter(types[headers[i]].Name, rows[i]);");
		sw.WriteLine("#else");
		sw.WriteLine("							dr[typeIndex] = Global.TableData.ArrayConverter(types[headers[i]].Name, rows[i]);");
		sw.WriteLine("#endif");
		sw.WriteLine("						}");
		sw.WriteLine("						else");
		sw.WriteLine("							dr[typeIndex] = list[0];");
		sw.WriteLine("						typeIndex++;");
		sw.WriteLine("					}");
		sw.WriteLine("					dt.Rows.Add(dr);");
		sw.WriteLine("				}");
		sw.WriteLine("			}");
		sw.WriteLine("			return dt;");
		sw.WriteLine("		}");
	}

	void LoadFunc(StreamWriter sw)
	{
		sw.WriteLine("		public virtual void Load(string path)");
		sw.WriteLine("		{");
		sw.WriteLine("			TypeFunc();");
		sw.WriteLine("			var table = ConvertCSVtoDataTable(path);");
		sw.WriteLine("			var rows = table.Rows;");
		sw.WriteLine("			Count = rows.Count;");
		sw.WriteLine("			for (int i = 0; i < Count; ++i)");
		sw.WriteLine("			{");
		sw.WriteLine($"				{mainTable.name} t = new {mainTable.name}(rows[i].ItemArray);");
		sw.WriteLine($"				{mainTable.name}s.Add(t.id, t);");
		sw.WriteLine("			}");
		sw.WriteLine("		}");
	}
}



