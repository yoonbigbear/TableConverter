﻿using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

class DataConverter
{
    static ExcelReader excelReader = new ExcelReader();
    static SourceCodeGenerator generator;

	static string TableDataPath = string.Empty;

	static string ServerCsvOutputPath = string.Empty;
	static string ClientCsvOutputPath = string.Empty;

	static string ServerCSOutputPath = string.Empty;
	static string ClientCSOutputPath = string.Empty;

    public static Type BaseTypes(string type)
    {
        switch (type)
        {
            case "byte":
                return typeof(System.Byte);
            case "byte[]":
                return typeof(System.Byte[]);
            case "ubyte":
                return typeof(System.SByte);
            case "ubyte[]":
                return typeof(System.SByte[]);
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

    public static object ConvertTypes(string type, string value)
	{
        var splitValue = value.Split(',');
        if (splitValue.Length > 1)
        {

            switch (type)
            {
                case "byte[]":
                    {
                        byte[] arr = new byte[splitValue.Length];
                        for (int i = 0; i < arr.Length; i++)
                            arr[i] = Convert.ToByte(splitValue[i]);
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
        else
        {
            switch (type)
            {
                case "byte":
                    return Convert.ToByte(value);
                case "ubyte":
                    return Convert.ToSByte(value);
                case "short":
                    return Convert.ToInt16(value);
                case "ushort":
                    return Convert.ToUInt16(value);
                case "int":
                    return Convert.ToInt32(value);
                case "uint":
                    return Convert.ToUInt32(value);
                case "long":
                    return Convert.ToInt64(value);
                case "ulong":
                    return Convert.ToUInt64(value);
                case "float":
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

		//clean csv output directories before start
		if (serverCsvDir.Exists)
            serverCsvDir.Delete(true);
		if (clientCsvDir.Exists)
			clientCsvDir.Delete(true);

		if (serverCsDir.Exists)
			serverCsDir.Delete(true);
		if (clientCsDir.Exists)
			clientCsDir.Delete(true);

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
                generator = new SourceCodeGenerator(excelReader.dataT, excelReader.enumTs);
                generator.Generate($"{ServerCSOutputPath}", 0);
                generator.Generate($"{ClientCSOutputPath}", 1);
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