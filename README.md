# TableConverter
Convert excel table to csv file and automatically generate source code for Unity and c# server  
엑셀 테이블을 읽어서 csv파일과 유니티 클라이언트, C# 서버 소스코드로 자동변환 합니다.
변환된 csv파일와 소스코드는 필요에 따라 확장하여 사용할 수 있습니다.
![image](https://user-images.githubusercontent.com/101116747/224537961-e9440e4e-01fe-44d0-97ae-847fd514b26b.png)
![image](https://user-images.githubusercontent.com/101116747/196241281-526c50df-1b64-4c1b-b094-8d0f921bc582.png)  
![image](https://user-images.githubusercontent.com/101116747/224537827-c577ecf3-17e8-4f14-a3db-19c650304902.png)  
![image](https://user-images.githubusercontent.com/101116747/224537858-5b969dff-693f-464d-98c6-ade8f278da51.png)  

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
