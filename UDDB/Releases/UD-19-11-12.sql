/*
UPDATE TO 1.47
Скрипт сгенерирован 04:25:42 PM 10/07/2012 
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


/*Определение типов*/
create or replace type ContractManagersTable as table of varchar(4000);
/
/*Хранимые процедуры*/
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
-----------------------------
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
--------------------------------------------------
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
-------------------------------------------------
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
---------------------------------------------------
create or replace function GetActContractdocId(currentactid in number)
return number
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
---------------------------------------------------
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
/*Представления*/
CREATE OR REPLACE VIEW CONTRACTREPOSITORYVIEW AS
SELECT c.ID, c.Price, n.Percents as "NdsPercents", cr.Culture, cm.Factor, c.NDSAlgorithmID, alg.Name as "NDSAlgName", n.ID as "NDSID", cr.ID as "CurrencyID", cm.ID as "CurrencyMeasureID", c.CurrencyRate, c.StartAt,
  c.EndsAt, c.AppliedAt, c.ApprovedAt, c.InternalNum, c.Subject, c.BrokeAt, c.OutOfControlAt, c.ReallyFinishedAt, c.ContractStateID, C.DISBURSED_CACHE, C.Left_cache, c.Stages_total_price_cache,  c.DISBURSED_COWORKERS_CACHE,  c.LEFT_COWORKERS_CACHE, cs.Name as "StateName", c.Deleted, c.Origincontractid,
  ct.Name as "ContractTypeName", p.FamilyName as "PersonFamilyName",
  p.FirstName as "PersonFirstName", p.MiddleName as "PersonMiddleName", d.ID as "DegreeId", d.Name as "PersonDegree",
  c.Agreementnum as "Agreementnum",
  (select * from TABLE(cast(getContractManagers(c.id) as ContractManagersTable))) as "ManagerNames",
  (select count(*) from "UD".Contractdoc c2 where c2.origincontractid = c.id) as "AgreementReferenceCount",
   (select count(*) from ContractHierarchy ch where ch.subcontractdocid = c.id) as "GeneralReferenceCount",
   (select count(*) from ScheduleContract sc where sc.contractdocid = c.id) as ScheduleCount,
   (select ctr.ShortName from ContractorContractDoc ccd , "UD".Contractor ctr where (ccd.ContractDocID=c.id AND ctr.Id=ccd.ContractorID and rownum = 1)) as "ContractorShortName",
   (select ctt.Name from ContractorContractDoc ccd , "UD".Contractor ctr, "UD".Contractortype ctt
  where (ccd.ContractDocID=c.id AND ctr.Id=ccd.ContractorID AND ctt.id = ctr.contractortypeid and rownum = 1)) as "ContractorType",
   (select count(*) from ContractorContractDoc ccd where ccd.ContractDocID=c.id) as "ContractorsCount",
   (select count(*) from stage s, schedulecontract schc
where (s.scheduleid = schc.scheduleid) and (schc.contractdocid =
c.Id) and
exists(select * from stage cst  where cst.Parentid = s.id)
and (s.actid is null) and (0=(select count(*) from stage cst where
cst.Parentid = s.Id and (cst.actid is null)))) as "OrphandParentCount",
(select count(*) from stage s, schedulecontract schc
where (s.scheduleid = schc.scheduleid) and (schc.contractdocid = c.Id) and
exists(select * from stage cst where cst.Parentid = s.id)
and (s.actid is not null) and exists(select * from stage cst where
cst.Parentid = s.Id and (cst.actid is null))) as "ClosedParentCount",
(GetMainContract(c.id)) as "MainContractID",
(select Internalnum from contractdoc where id = (GetMainContract(c.id))) as "MainContractSubject"
   FROM "UD".Contractdoc c
   LEFT JOIN "UD".Contractstate cs ON cs.ID = c.ContractStateID
   LEFT JOIN "UD".Currencymeasure cm ON cm.ID = c.CurrencyMeasureId
   LEFT JOIN "UD".Currency cr ON cr.ID = c.CurrencyID
   LEFT JOIN "UD".NDS n ON n.ID = c.NDSID
   LEFT JOIN "UD".NDSAlgorithm alg ON alg.ID = c.NDSAlgorithmID
LEFT JOIN "UD".ContractType ct ON c.ContractTypeID = ct.ID
LEFT JOIN "UD".Person p ON p.ID = c.ContractorPersonID
LEFT OUTER JOIN "UD".Degree d ON d.ID = p.DegreeID
ORDER BY c.ContractTypeID, c.InternalNum;
/
--------------------------------------------------------------------------------------
create or replace view actrepositoryview as
select
a.id,
a.acttypeid,
atp.typename "ActTypeName",
a.enterpriceauthorityid,
a.currencyid,
c.name "CurrencyName",
c.culture "CurrencyCulture",
a.currencymeasureid,
cm.name "CurrencyMeasureName",
cm.factor "CurrencyMeasureFactor",
a.ndsalgorithmid,
ndsa.name "NdsalgorithmName",
num,
signdate,
a.ndsid,
nds.percents "NdsPercents",
a.regionid,
r.name "RegionName",
totalsum,
sumfortransfer,
status,
currencyrate,
ratedate,
issigned,
(GetActStageNumbers(a.id)) as "ActStageNumbers",
(GetActContractdocId(a.Id)) as "Contractdocid",
(GetMainContract(GetActContractdocid(a.Id))) as "MainContractID",
(select Internalnum from contractdoc where id = (GetMainContract(GetActContractdocid(a.Id)))) as "MainContractSubject",
(select Internalnum from contractdoc where id = (GetActContractdocid(a.Id))) as "ContractSubject"
from act a
left join acttype atp on (a.acttypeid = a.id)
left join currency c on (c.id = a.currencyid)
left join currencymeasure cm on (cm.id = a.currencymeasureid)
left join ndsalgorithm ndsa on (ndsa.id = a.ndsalgorithmid)
left join nds on (nds.id = a.ndsid)
left join region r on (r.id = a.regionid);
/