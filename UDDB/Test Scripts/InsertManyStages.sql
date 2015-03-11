-- This query will display the current date
declare 
  CurrID number(10);
begin
delete from stage where ID >=100 and ID < 10000;

CurrID:=100;
WHILE CurrID <= 500
LOOP
      INSERT INTO Stage
      (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID, Code, NdsID) 
      VALUES 
         (CurrID, '1', 'Это чрезвычайно длинное название этапа: Microsoft Visual Studio 2010 Version 10.0.40219.1 SP1Rel Microsoft .NET Framework Version 4.0.30319 SP1Rel Installed Version: Ultimate Microsoft Office Developer Tools   01019-532-2002102-70766 Microsoft Office Developer Tools Microsoft Visual Basic 2010   01019-532-2002102-70766 Microsoft Visual Basic 2010 Microsoft Visual C# 2010   01019-532-2002102-70766 Microsoft Visual C# 2010 Microsoft Visual C++ 2010   01019-532-2002102-70766 Microsoft Visual C++ 2010 Microsoft Visual F# 2010   01019-532-2002102-70766 Microsoft Visual F# 2010 Microsoft Visual Studio 2010 Architecture and Modeling Tools   01019-532-2002102-70766 Microsoft Visual Studio 2010 Architecture and Modeling Tools' 
          , '05.07.2010', '18.10.2010', 110, 1, null, null, 1, '2', 1);

	  CurrID := CurrID + 1;
END LOOP;
commit;
end;


