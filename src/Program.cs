using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

class DataConverter
{
    static ExcelReader excelReader = new ExcelReader();
    static SourceCodeGeneratorCS generator;
	static SourceCodeGeneratorCpp generatorcpp;

	static string TableDataPath = string.Empty;

	static string ServerCsvOutputPath = string.Empty;
	static string ClientCsvOutputPath = string.Empty;

	static string ServerCSOutputPath = string.Empty;
	static string ClientCSOutputPath = string.Empty;

    static string ServerCPPOutputPath = string.Empty;
	static string ClientCPPOutputPath = string.Empty;

	public static Type BaseTypesCS(string type)
    {
        switch (type)
        {
			case "int8":
				return typeof(System.SByte);
			case "uint8":
				return typeof(System.Byte);
			case "byte":
                return typeof(System.SByte);
            case "byte[]":
                return typeof(System.SByte[]);
            case "ubyte":
                return typeof(System.Byte);
            case "ubyte[]":
                return typeof(System.Byte[]);
            case "short":
                return typeof(System.Int16);
            case "short[]":
                return typeof(System.Int16[]);
            case "ushort":
                return typeof(System.UInt16);
            case "ushort[]":
                return typeof(System.UInt16[]);
            case "int":
                return typeof(System.Int32);
            case "int[]":
                return typeof(System.Int32[]);
            case "uint":
                return typeof(System.UInt32);
            case "uint[]":
                return typeof(System.UInt32[]);
            case "long":
                return typeof(System.Int64);
            case "long[]":
                return typeof(System.Int64[]);
            case "ulong":
                return typeof(System.UInt64);
            case "ulong[]":
                return typeof(System.UInt64[]);
            case "float":
                return typeof(System.Single);
            case "float[]":
                return typeof(System.Single[]);
            case "double":
                return typeof(System.Double);
            case "double[]":
                return typeof(System.Double[]);
            case "string":
                return typeof(System.String);
            case "string[]":
                return typeof(System.String[]);
            case "bool":
                return typeof(System.Boolean);
            case "bool[]":
                return typeof(System.Boolean[]);
            default:
                return null;
        }
    }

    public static object ConvertTypesCS(string type, string value)
    {
        var splitValue = value.Split(',');
        switch (type.ToLower())
        {
			case "uint8[]":
				{
					byte[] arr = new byte[splitValue.Length];
					for (int i = 0; i < arr.Length; i++)
						arr[i] = Convert.ToByte(splitValue[i]);
					return arr;
				}
			case "int8[]":
				{
					sbyte[] arr = new sbyte[splitValue.Length];
					for (int i = 0; i < arr.Length; i++)
						arr[i] = Convert.ToSByte(splitValue[i]);
					return arr;
				}
			case "byte[]":
                {
                    byte[] arr = new byte[splitValue.Length];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = Convert.ToByte(splitValue[i]);
                    return arr;
                }
			case "sbyte[]":
				{
					sbyte[] arr = new sbyte[splitValue.Length];
					for (int i = 0; i < arr.Length; i++)
						arr[i] = Convert.ToSByte(splitValue[i]);
					return arr;
				}
			case "int16[]":
                {
                    Int16[] arr = new Int16[splitValue.Length];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = Convert.ToInt16(splitValue[i]);
                    return arr;
                }
            case "uint16[]":
                {
                    UInt16[] arr = new UInt16[splitValue.Length];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = Convert.ToUInt16(splitValue[i]);
                    return arr;
                }
            case "int32[]":
                {
                    Int32[] arr = new Int32[splitValue.Length];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = Convert.ToInt32(splitValue[i]);
                    return arr;
                }
            case "uint32[]":
                {
                    UInt32[] arr = new UInt32[splitValue.Length];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = Convert.ToUInt32(splitValue[i]);
                    return arr;
                }
            case "int64[]":
                {
                    Int64[] arr = new Int64[splitValue.Length];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = Convert.ToInt64(splitValue[i]);
                    return arr;
                }
            case "uint64[]":
                {
                    UInt64[] arr = new UInt64[splitValue.Length];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = Convert.ToUInt64(splitValue[i]);
                    return arr;
                }
            case "single[]":
                {
                    Single[] arr = new Single[splitValue.Length];
                    for (int i = 0; i < arr.Length; i++)
                        arr[i] = Convert.ToSingle(splitValue[i]);
                    return arr;
                }
            case "double[]":
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
			case "int8":
				return Convert.ToSByte(value);
			case "uint8":
				return Convert.ToByte(value);
			case "sbyte":
				return Convert.ToSByte(value);
			case "byte":
                return Convert.ToByte(value);
            case "ubyte":
                return Convert.ToByte(value);
            case "short":
                return Convert.ToInt16(value);
			case "int16":
				return Convert.ToInt16(value);
			case "ushort":
                return Convert.ToUInt16(value);
			case "uint16":
				return Convert.ToUInt16(value);
			case "int":
                return Convert.ToInt32(value);
			case "int32":
				return Convert.ToInt32(value);
			case "uint":
                return Convert.ToUInt32(value);
			case "uint32":
				return Convert.ToUInt32(value);
			case "long":
                return Convert.ToInt64(value);
			case "int64":
				return Convert.ToInt64(value);
			case "ulong":
                return Convert.ToUInt64(value);
			case "uint64":
				return Convert.ToUInt64(value);
			case "float":
                return Convert.ToSingle(value);
			case "single":
				return Convert.ToSingle(value);
			case "double":
                return Convert.ToDouble(value);
            case "string":
                return value;
            case "bool":
                return Convert.ToBoolean(value);
            default:
                return typeof(System.Enum);
        }
    }

    public static void Main()
    {
        if (!LoadConfig())
            return;

        System.IO.DirectoryInfo TableDir = new System.IO.DirectoryInfo(TableDataPath);
        System.IO.DirectoryInfo serverCsvDir =   new System.IO.DirectoryInfo(ServerCsvOutputPath);
		System.IO.DirectoryInfo clientCsvDir = new System.IO.DirectoryInfo(ClientCsvOutputPath);
		System.IO.DirectoryInfo serverCsDir = new System.IO.DirectoryInfo(ServerCSOutputPath);
		System.IO.DirectoryInfo clientCsDir = new System.IO.DirectoryInfo(ClientCSOutputPath);
		System.IO.DirectoryInfo serverCppDir = new System.IO.DirectoryInfo(ServerCPPOutputPath);
		System.IO.DirectoryInfo clientCppDir = new System.IO.DirectoryInfo(ClientCPPOutputPath);

		//clean csv output directories before start
		if (serverCsvDir.Exists)
            serverCsvDir.Delete(true);
		if (clientCsvDir.Exists)
			clientCsvDir.Delete(true);

		if (serverCsDir.Exists)
			serverCsDir.Delete(true);
		if (clientCsDir.Exists)
			clientCsDir.Delete(true);

		if (serverCppDir.Exists)
			serverCppDir.Delete(true);
		if (clientCppDir.Exists)
			clientCppDir.Delete(true);

		foreach (System.IO.FileInfo File in TableDir.GetFiles())
        {
            if (File.Extension.ToLower().CompareTo(".xlsx") == 0)
            {
                String FileNameOnly = File.Name.Substring(0, File.Name.Length - 5);
                String FullPathFileName = File.FullName;

                //string FileNameXLS = $"\\..\\..\\..\\..\\table\\{FileNameOnly}xlsx";
                string FileNameCSV = $"{FileNameOnly}.csv";

                //read excel file
                Console.WriteLine($"Start read datasheet....{FileNameOnly}");
                excelReader.Start(FullPathFileName);
                excelReader.dataT.name = FileNameOnly;

                //convert to csv
                Console.WriteLine($"Parse to CSV....{FileNameOnly}");
                CsvConverter.ToCSV(excelReader.dataT.DataTable, serverCsvDir, serverCsvDir + FileNameCSV);
                CsvConverter.ToCSV(excelReader.dataT.DataTable, clientCsvDir, clientCsvDir + FileNameCSV);

                //generate source code
                Console.WriteLine($"Generate source code....{FileNameOnly}");
                generator = new SourceCodeGeneratorCS(excelReader.dataT, excelReader.enumTs);
                generator.GenerateCs($"{ServerCSOutputPath}", 0);
                generator.GenerateCs($"{ClientCSOutputPath}", 1);
                generatorcpp = new SourceCodeGeneratorCpp(excelReader.dataT, excelReader.enumTs);
				generatorcpp.Generate($"{ServerCPPOutputPath}", 0);
				generatorcpp.Generate($"{ClientCPPOutputPath}", 1);
				Console.WriteLine("done" + "\n");
            }
        }
    }

	// config
	public static bool LoadConfig()
	{
        string filename = "PathConfig.json";
		try
		{
            if (File.Exists(filename))
            {
                using (var read = new StreamReader(filename))
                {
                    var jsonString = read.ReadToEnd();
                    JObject json = JObject.Parse(jsonString);

                    if (!json.ContainsKey("TableData"))
                        return false;
					TableDataPath = (string)json["TableData"];

					if (!json.ContainsKey("ServerCsvOutput"))
						return false;
					ServerCsvOutputPath = (string)json["ServerCsvOutput"];
					if (!json.ContainsKey("ClientCsvOutput"))
						return false;
					ClientCsvOutputPath = (string)json["ClientCsvOutput"];
					if (!json.ContainsKey("ServerCsOutput"))
						return false;
					ServerCSOutputPath = (string)json["ServerCsOutput"];
					if (!json.ContainsKey("ClientCsOutput"))
						return false;
					ClientCSOutputPath = (string)json["ClientCsOutput"];
					if (!json.ContainsKey("ServerCPPOutput"))
						return false;
					ServerCPPOutputPath = (string)json["ServerCPPOutput"];
					if (!json.ContainsKey("ClientCPPOutput"))
						return false;
					ClientCPPOutputPath = (string)json["ClientCPPOutput"];
				}
            }
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return false;
		}
		return true;
	}
};