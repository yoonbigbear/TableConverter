using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class SourceCodeGeneratorCpp
{
	DataT mainTable = new DataT();
	List<EnumT> enumTs = new List<EnumT>();

	enum Target
	{
		Server = 0,
		Client = 1,
	}

	Target target = Target.Server;

	public static string ConvertTypesCpp(string type)
	{
		switch (type.ToLower())
		{
			case "byte[]":
				{
					return "Vector<byte>";
				}
			case "int16[]":
				{
					return "Vector<int16_t>";
				}
			case "uint16[]":
				{
					return "Vector<uint16_t>";
				}
			case "int32[]":
				{
					return "Vector<int32_t>";
				}
			case "uint32[]":
				{
					return "Vector<uint32_t>";
				}
			case "int64[]":
				{
					return "Vector<int64_t>";
				}
			case "uint64[]":
				{
					return "Vector<uint64_t>";
				}
			case "single[]":
				{
					return "Vector<float>";
				}
			case "double[]":
				{
					return "Vector<double>";
				}
			case "string[]":
				return "Vector<String>";
			case "bool[]":
				{
					return "Vector<bool>";
				}
			case "string":
				{
					return "String";
				}
			case "byte":
			case "ubyte":
			case "short":
			case "Int16":
			case "ushort":
			case "uint16":
			case "int":
			case "int32":
			case "uint":
			case "uint32":
			case "long":
			case "int64":
			case "ulong":
			case "uint64":
			case "float":
			case "single":
			case "double":
			case "bool":
			default:
				return type;
		}
	}

	public static string LoadTypesCpp(string type)
	{
		switch (type.ToLower())
		{
			case "byte[]":
				{
					return "byte";
				}
			case "int16[]":
				{
					return "int16_t";
				}
			case "uint16[]":
				{
					return "uint16_t";
				}
			case "int32[]":
				{
					return "int32_t";
				}
			case "uint32[]":
				{
					return "uint32_t";
				}
			case "int64[]":
				{
					return "int64_t";
				}
			case "uint64[]":
				{
					return "uint64_t";
				}
			case "single[]":
				{
					return "float";
				}
			case "double[]":
				{
					return "double";
				}
			case "string[]":
				return "String";
			case "bool[]":
				{
					return "bool";
				}
		}
		return type;
	}

	public SourceCodeGeneratorCpp(DataT mainTable, List<EnumT> enumTs)
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

		if (File.Exists($"{outputpath}\\gen{mainTable.name}.h"))
		{
			File.Delete($"{outputpath}\\gen{mainTable.name}.h");
		}

		using (StreamWriter sw = new StreamWriter($"{outputpath}\\gen{mainTable.name}.h"))
		{
			Header(sw);
			Pragma(sw);
			NamespaceCpp(sw);
			
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

	void Pragma(StreamWriter sw)
	{
		sw.WriteLine("#pragma once");
		sw.WriteLine("#include \"types.h\"");
		sw.WriteLine("#include \"csv_parser.h\"");
	}

	void NamespaceCpp(StreamWriter sw)
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
			sw.WriteLine($"	enum {t.name} : {t.type.Name}");
			sw.WriteLine("	{");
			sw.WriteLine($"		None = 0,");
			foreach (var e in t.body)
			{
				sw.WriteLine($"		{e.Key} = {e.Value},");
			}

			sw.WriteLine("	};");
			sw.WriteLine("");
		}
	}

	void DataStruct(StreamWriter sw)
	{
		sw.WriteLine($"	struct {mainTable.name}");
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
				sw.WriteLine($"		{ConvertTypesCpp(columns[i].DataType.Name)} {columns[i].ColumnName};");
			}
			else
			{
				sw.WriteLine($"		{ConvertTypesCpp(columns[i].DataType.Name)} {columns[i].ColumnName.Split(":")[0]};");
			}


		}

		sw.WriteLine("	};");
		sw.WriteLine($"");
	}

	void DataClass(StreamWriter sw)
	{
		sw.WriteLine($"	class {mainTable.name}Table");
		sw.WriteLine("	{");
		sw.WriteLine("	public:");

		sw.WriteLine($"		UnorderedMap<{mainTable.keyType.Name}, {mainTable.name}> {mainTable.name}s;");
		sw.WriteLine($"");
		sw.WriteLine($"		Vector<{mainTable.name}> {mainTable.name}List;");
		sw.WriteLine($"		bool HasKey({mainTable.keyType.Name} id)");
		sw.WriteLine("	{");
		sw.WriteLine($"		return {mainTable.name}s.contains(id);");
		sw.WriteLine("	}");
		sw.WriteLine($"		{mainTable.name}& operator[]({mainTable.keyType.Name} id)");
		sw.WriteLine("	{");
		sw.WriteLine($"		return {mainTable.name}s[id];");
		sw.WriteLine("	}");
		sw.WriteLine($"");
		LoadFunc(sw);
		sw.WriteLine($"");
		sw.WriteLine("	};");
	}

	void LoadFunc(StreamWriter sw)
	{
		sw.WriteLine("		virtual void Load(String path)");
		sw.WriteLine("		{");
		sw.WriteLine("			CSVReader reader(path);");
		sw.WriteLine("			for (CSVRow& row : reader)");
		sw.WriteLine("			{");
		sw.WriteLine($"				{mainTable.name} t;");
		
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
				if (columns[i].DataType.IsArray)
				{
					sw.WriteLine("				{");
					sw.WriteLine($"					String str = row[\"{columns[i].ColumnName}\"].get<String>();");
					sw.WriteLine($"					std::stringstream ss(str);");
					sw.WriteLine($"					String token;");
					sw.WriteLine($"					while (std::getline(ss, token, ':'))");
					sw.WriteLine($"					{{");
					if (columns[i].DataType.Name == "string[]")
					{
						sw.WriteLine($"						t.{columns[i].ColumnName}.emplace_back(token);");
					}
					else
					{
						sw.WriteLine($"						t.{columns[i].ColumnName}.emplace_back(std::stod(token));");
					}
					sw.WriteLine($"					}}");
					sw.WriteLine("				}");
				}
				else
				{ 
					sw.WriteLine($"				t.{columns[i].ColumnName} = row[\"{columns[i].ColumnName}\"].get<{ConvertTypesCpp(columns[i].DataType.Name)}>();");
				}
			}
			else
			{
				var split = columns[i].ColumnName.Split(":");
				if (split.Length > 1)
				{
					sw.WriteLine($"				t.{split[0]} = row[\"{split[0]}\"].get<{ConvertTypesCpp(columns[i].DataType.Name)}>();");
				}
				else
				{
					sw.WriteLine($"				t.{columns[i].ColumnName} = row[\"{columns[i].ColumnName}\"].get<{ConvertTypesCpp(columns[i].DataType.Name)}>();");
				}
			}


		}

		sw.WriteLine("			}");
		sw.WriteLine("		}");
	}
}
