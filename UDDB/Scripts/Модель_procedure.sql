/*
/*
Система учёта договоров 1.0
Скрипт сгенерирован 25/11/201206:03:20 PM
1.48
   1) Удалена таблица актов передачи актов к договорам
   2) Таблица  ContractTranActDoc используется и для договоров и для актов
        Удалено ограничение UNIQUE
   3) Переименовано ограничение первичного ключа между ApprovalProcess и ApprovalState
1.47
   1) Задействована таблица версионности схемы БД (1.47 - текущая схема)
   2) Исправлено представление репозитория актов
1.46
    1) Добавлены процедуры для поддержки работы распоряжений
    2) Изменено представление реестра договоров для группировки по основному договору
    3) Черновая (!!!) версия представления реестра актов
    4) Удален Individual. Учёт физ.лиц ведётся с использованием изменённой таблицы контрагентов. 
1.45
    1) Добавлены кэш-поля в contractdoc (disbursed_cache, left_cache)
1.44
    1) Добавлен Individual
    2) Добавлен Education
    3) Введены договоры и этапы с открытыми датами (поля Delta и Comment)
    4) Введено согласование этапов и результатов
    5) Введено назначение ответственного по этапу
	6) Введены группировки типов договоров для отчётов в соответствие с контрагентами (ReportGrouping)
	7) Введено хранение фильтров договоров
	8) Изменена политика удаления акта с cascade на set null
 
  1.43:
	1)  Добавлено поле Images:blob для Contractdoc
	2) Добавлены индексы на внешние ключи для таблиц
	    с интенсивными операциями соединения
	3) Изменены вставляемые строки в WorkType и CurrencyMeasure
*/

DROP PROCEDURE getContractManagers;
DROP PROCEDURE GetOriginContract;
DROP PROCEDURE GetGeneralContract;
DROP PROCEDURE GetMainContract;
DROP PROCEDURE GetActContractdocId;
DROP PROCEDURE GetActStageNumbers;
/
create or replace type ContractManagersTable as table of varchar(4000);
/
create or replace function getContractManagers(contractid in number)
return ContractManagersTable
PIPELINED
is
   CURSOR MANAGERSCURSOR IS 
   SELECT e.* 
   FROM responsible r, employee e 
   WHERE r.contractdocid = contractid and r.roleid = 1 
   and e.id = r.employeeid
   order by e.familyname, e.firstname, e.middlename; 
   
   managerrec employee%rowtype;
   s varchar(4000);
begin
  OPEN MANAGERSCURSOR; 

  s := '';

  LOOP 
    FETCH MANAGERSCURSOR INTO managerrec; 
    EXIT WHEN MANAGERSCURSOR%NOTFOUND;
    if (not(managerrec.familyname is null)) and (not(managerrec.id < 0)) then 
      s :=  s||managerrec.familyname;
      
      if (not(managerrec.firstname is null)) then 
        s :=  s||' '||SUBSTR(managerrec.firstname, 1, 1)||'.';
        if (not(managerrec.middlename is null)) then 
          s :=  s||' '||SUBSTR(managerrec.middlename, 1, 1)||'.';
        end if;
      end if;   
      s := s || '; ';      
    end if;
  end loop;
  
  s := trim(s);
  s := substr(s, 1, length(s)-1);
  pipe row(s);
  
  CLOSE MANAGERSCURSOR; 
  
  return;
end;
/
create or replace function GetOriginContract(contractid in number)
return integer
is
   CURSOR CONTRACTCURSOR IS
   SELECT origincontractid from
   contractdoc where id = contractid
   and origincontractid is not null;

   originid number;
begin
  OPEN CONTRACTCURSOR;
  originid := contractid;

  LOOP
    FETCH CONTRACTCURSOR INTO originid;
    EXIT WHEN CONTRACTCURSOR%NOTFOUND;

    originid := GetOriginContract(originid);
  end loop;

  
  CLOSE CONTRACTCURSOR;

  return originid;
end;
/
create or replace function GetGeneralContract(contractid in number)
return integer
is
   CURSOR CONTRACTCURSOR IS
   SELECT ch.generalcontractdocid
   from contracthierarchy ch
   where ch.subcontractdocid = contractid;

   originid number;
begin
  OPEN CONTRACTCURSOR;
  originid := getorigincontract(contractid);

  LOOP
    FETCH CONTRACTCURSOR INTO originid;
    EXIT WHEN CONTRACTCURSOR%NOTFOUND;

    originid := GetOriginContract(originid);
  end loop;


  CLOSE CONTRACTCURSOR;

  return originid;
end;
/
create or replace function GetMainContract(contractid in number)
return number
is
  originid number;
begin
  
  originid := getorigincontract(contractid);
  
  if originid <> contractid then
    return getgeneralcontract(originid);
  else 
    return getgeneralcontract(contractid);
  end if;
end;
/
create or replace function GetActContractdocId(currentactid in number)
return nvarchar2
is
   CURSOR stagecursor IS
   SELECT min(sc.contractdocid)
   from stage s, schedulecontract sc
   where s.Actid = currentactid
   and s.scheduleid = sc.scheduleid;

   Contractdocid number;
begin
  OPEN stagecursor;

  LOOP
    FETCH stagecursor INTO Contractdocid;
    EXIT WHEN stagecursor%NOTFOUND;


  end loop;


  CLOSE stagecursor;

  return Contractdocid;
end GetActContractdocId;
/
create or replace function GetActStageNumbers(currentactid in number)
return nvarchar2
is
   CURSOR stagecursor IS
   SELECT distinct s.num
   from stage s
   where s.actid = currentactid
   order by s.num ;

   s nvarchar2(25);
   sResult nvarchar2(200);
begin
  OPEN stagecursor;

  sResult := '';
  LOOP
    FETCH stagecursor INTO s;
    EXIT WHEN stagecursor%NOTFOUND;

    sResult := sResult || s || ';' ;
  end loop;


  CLOSE stagecursor;

  return sResult;
end GetActStageNumbers;
/