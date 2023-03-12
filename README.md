# TableConverter
Convert excel table to csv file and automatically generate source code for Unity and c# server

![image](https://user-images.githubusercontent.com/101116747/196241281-526c50df-1b64-4c1b-b094-8d0f921bc582.png)

## Requirements

[ExcelDataReader](https://github.com/ExcelDataReader/ExcelDataReader), [ExcelDataReader.DataSet](https://www.nuget.org/packages/ExcelDataReader.DataSet/)

## How to use

- Generated client source code has been tested with Unity 2021.3.1f1. 
- Every table file **MUST HAVE** a sheet name of the **"Table"**. The **"Table"** sheet is the reserved name of the main table.
- The original project was not a general purpose. Therefore generator contains some uncommon namespaces and types such as "Global" or "SByte"
- Some tables need to extend. In example, you can find [sample extend class](example/TableEx.cs).
- First row of the table is for column(or variable) name. if name with **":c"** means use only client.
- Second row is for data type.
- Capital or small letter both works
- For personal use, it needs to fix file paths.


## Keep in mind

- Array types have not been tested yet :D
- Remind this project was not for the public. I'm pretty sure it needs lots of improvement.
