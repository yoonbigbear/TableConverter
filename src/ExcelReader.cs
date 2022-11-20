using ExcelDataReader;
using System.Data;
using System.Text;

public class ExcelReader
{
    public DataT dataT = null;
    public List<EnumT> enumTs = null;

    public void Start(string path)
    {
        dataT = new DataT();
        enumTs = new List<EnumT>();

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            IExcelDataReader reader = null;
            if (path.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (path.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            if (reader == null)
                return;

            //parse to datatable
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false
                }
            });

            var sheets = ds.Tables;
            DataTable mainTable = new DataTable(); //keep
            // Create EnumTable for the file
            for (int i = 0; i < sheets.Count; i++)
			{
                //"table" is main table. will parst at the end
                if (sheets[i].TableName.ToLower() == "table")
				{
                    mainTable = sheets[i];
                    continue;
                }

                //enums
                ParseEnum(sheets[i]);
            }

            //main Table
            ParseMainTable(mainTable);
        }
    }

    void ParseMainTable(DataTable table)
    {
        // Key : Value
        //Header
        string key = string.Empty;
        Type value;
        string valuekey = string.Empty;
        for (int i = 0; i < table.Rows[0].ItemArray.Length; ++i)
        {
            key = table.Rows[0][i].ToString().ToLower();
            valuekey = table.Rows[1][i].ToString().ToLower();
            value = DataConverter.BaseTypes(valuekey);
            dataT.DataTable.Columns.Add(key, value);
            if (key.Equals("id"))
                dataT.keyType = value;
        };

        //Body
        for (int i = 2; i < table.Rows.Count; ++i)
        {
            var row = table.Rows[i];
            DataRow newRow = dataT.DataTable.NewRow();
            for (int j = 0; j < row.ItemArray.Length; ++j)
            {
                string rowValue = row[j].ToString().ToLower();
                var valueArray = rowValue.Split(',');
                //verify unique enum type
                var ret = enumTs.Find((e) => { 
                        return e.name.ToLower().Equals(dataT.DataTable.Columns[j].ColumnName); });
                if (ret != null)
                {
                    {
                        if (valueArray.Length > 1)
                        {
                            //array
                            for (int k = 0; k < valueArray.Length; ++k)
                            {
                                ret.body.TryGetValue(valueArray[k], out var v);
                                newRow[dataT.DataTable.Columns[j]] = v;
                            }
                        }
                        else
                        {
							ret.body.TryGetValue(rowValue, out var v);
                            newRow[dataT.DataTable.Columns[j]] = v;
                        }
                    }
                }
                else
                {
                    if (valueArray.Length > 1)
                    {
						//array
						newRow[dataT.DataTable.Columns[j]] = DataConverter.ConvertTypes(dataT.DataTable.Columns[j].DataType.Name, rowValue);
                    }
                    else
                    {
                        newRow[dataT.DataTable.Columns[j]] = row[j];
                    }
                }

            }
            dataT.DataTable.Rows.Add(newRow);
        };
    }

    void ParseEnum(DataTable table)
	{
        EnumT enumT = new EnumT();
        enumT.name = table.TableName;
        //First row is data type.
        enumT.type = DataConverter.BaseTypes(table.Rows[0][1].ToString().ToLower());

        string enumName;
        object value;

        //Second row - name:value
        for (int i = 1; i < table.Rows.Count; ++i)
        {
            enumName = table.Rows[i][0].ToString().ToLower();
            value = table.Rows[i][1];
            enumT.body.Add(enumName, value);
        };
        enumTs.Add(enumT);
    }
}

public class DataT
{
    public string name = string.Empty;
    public Type keyType;

    public DataTable DataTable = new DataTable();
}

public class EnumT
{
    public string name = string.Empty;
    public Type type;
    public Dictionary<string, object> body = new();

}
