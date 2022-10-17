# TableConverter
Convert excel table to csv file and automatically generate source code

## Requirements

[ExcelDataReader](https://github.com/ExcelDataReader/ExcelDataReader), [ExcelDataReader.DataSet](https://www.nuget.org/packages/ExcelDataReader.DataSet/)

## How to use

- Generated client source code has been tested with Unity 2021.3.1f1.
- The original project was not a general purpose. Therefore generator contains some uncommon namespaces and types such as "Global" or "SByte"
- Some tables need to extend. In example, you can find [sample extend class](example/TableEx.cs).
- Every table file **MUST HAVE** a sheet name of the **"Table"**. The **"Table"** sheet is the reserved name of the main table.
- First row of the table is for column(or variable) name. if name with **":c"** means use only client.
- Second row is for data type.
- Capital or small letter both works


## 
