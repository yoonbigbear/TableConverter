using System.Data;
using System.Text;

public static class CsvConverter
{
    public static void ToCSV(this DataTable dtDataTable, System.IO.DirectoryInfo directory, string strFilePath)
    {
		if (!directory.Exists)
            directory.Create();

        using (StreamWriter sw = new StreamWriter(strFilePath, false))
        {
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);

            //body
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        if (dr[i] is Array)
                        {
                            var arr = dr[i] as Array;
                            //delemeter ':'
                            for(int j = 0; j < arr.Length; ++j)
                            {
								sw.Write(arr.GetValue(j));
                                if (j != arr.Length -1)
                                    sw.Write(":");
                            }
                        }
                        else
                        {
                            //value = String.Format("\"{0}\"", Encoding.UTF8.GetString(bytes));
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
        }
    }
}
