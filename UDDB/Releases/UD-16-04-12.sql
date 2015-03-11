-- This query will display the current date

SELECT SYSDATE FROM DUAL;

DROP TRIGGER trg_Schedulecontract_AD;
DROP SEQUENCE seq_ResponsibleAssignmentOrder;
DROP TABLE Individual CASCADE CONSTRAINTS;
DROP TABLE Education CASCADE CONSTRAINTS;
DROP TABLE Individual_ContractDoc CASCADE CONSTRAINTS;
DROP TABLE ReportGrouping CASCADE CONSTRAINTS;
DROP TABLE Report CASCADE CONSTRAINTS;
DROP TABLE FilterState CASCADE CONSTRAINTS;
DROP TABLE Report_FilterState CASCADE CONSTRAINTS;
DROP SEQUENCE seq_Education;
DROP SEQUENCE seq_Individual;
DROP SEQUENCE seq_ResponsibleAssignmentOrder;
DROP SEQUENCE seq_FilterState;
DROP SEQUENCE seq_Report;
CREATE SEQUENCE seq_Education START WITH 10000;
CREATE SEQUENCE seq_Individual START WITH 10000;
CREATE SEQUENCE seq_ResponsibleAssignmentOrder START WITH 10000;
CREATE SEQUENCE seq_FilterState START WITH 10000;
CREATE SEQUENCE seq_Report START WITH 10000;
alter table contractdoc add Images blob;
alter table contractdoc add Delta  number(8); 
alter table contractdoc add DeltaComment nvarchar2(2000);
COMMENT ON COLUMN ContractDoc.Images IS 'Образ пакета документов договора. Весь пакет документов сохраняется в виде одного архива формата ZIP';
COMMENT ON COLUMN ContractDoc.DeltaComment IS 'Примечание к договору с открытой датой';
alter table contractor modify  INN nvarchar2(20);
alter table contractor modify  BIK nvarchar2(12);
alter table CurrencyMeasure modify Name nvarchar2(20) UNIQUE;  
alter table Stage add Delta number(8); 
alter table Stage add ApprovalStateID  number(10); 
alter table Stage add Statedate date; 
alter table Stage add StateDescription nvarchar2(2000); 
alter table Stage add DeltaComment     nvarchar2(2000);

COMMENT ON COLUMN Stage.Delta IS 'Если дата окончания этапа задана открытой, то Delta хранит смещение относительно StartAt';
COMMENT ON COLUMN Stage.StateDescription IS 'Описание состояния';
COMMENT ON COLUMN Stage.DeltaComment IS 'Примечание к этапу с открытой датой';

alter table Responsible add StageID number(10);

alter TABLE StageResult add Statedate                date;
alter TABLE StageResult add StateDescription         nvarchar2(2000); 
alter TABLE StageResult add ResultDate               date; 
alter TABLE StageResult add Description              nvarchar2(2000);
alter TABLE StageResult add ActIntroductionNum       nvarchar2(100);
alter TABLE StageResult add ApprovalStateID          number(10);

COMMENT ON COLUMN StageResult.ResultDate IS 'Дата перехода в состояние';
COMMENT ON COLUMN StageResult.Description IS 'Описание состояния';
COMMENT ON COLUMN StageResult.ActIntroductionNum IS 'Номер акта внедрения';

alter TABLE ApprovalState add Color       number(10);
alter TABLE ApprovalState add StateDomain number(3);

COMMENT ON COLUMN ApprovalState.Color IS 'Цвет представления состояния';
COMMENT ON COLUMN ApprovalState.StateDomain IS 'NULL или 00000000  - ограничения на использование не заданы
000000000
        ^ - действие для договора
000000000
       ^ - действие для этапа
000000000
      ^  - действие для результата';


CREATE TABLE Individual (
  ID               number(10) NOT NULL, 
  FamilyName       nvarchar2(100) NOT NULL, 
  FirstName        nvarchar2(100) NOT NULL, 
  MiddleName       nvarchar2(100), 
  PasportSeries    nvarchar2(10), 
  PasportNumber    nvarchar2(20), 
  PasportAuthority nvarchar2(2000), 
  PasportDate      date, 
  Address          nvarchar2(2000), 
  Birthdate        date, 
  Birthplace       nvarchar2(2000), 
  INN              nvarchar2(20), 
  Insurance        nvarchar2(100), 
  EducationID      number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Individual IS 'Физическое лицо';
COMMENT ON COLUMN Individual.FamilyName IS 'Фамилия';
COMMENT ON COLUMN Individual.FirstName IS 'Имя';
COMMENT ON COLUMN Individual.MiddleName IS 'Отчество';
COMMENT ON COLUMN Individual.PasportSeries IS 'Серия паспорта';
COMMENT ON COLUMN Individual.PasportNumber IS 'Номер паспорта';
COMMENT ON COLUMN Individual.PasportAuthority IS 'Кем выдан паспорт';
COMMENT ON COLUMN Individual.PasportDate IS 'Дата выдачи паспорта';
COMMENT ON COLUMN Individual.Address IS 'Адрес';
COMMENT ON COLUMN Individual.Birthdate IS 'Дата рождения';
COMMENT ON COLUMN Individual.Birthplace IS 'Место рождения';
COMMENT ON COLUMN Individual.INN IS 'ИНН';
COMMENT ON COLUMN Individual.Insurance IS 'Номер пенсионного удостоверения';
COMMENT ON COLUMN Individual.EducationID IS 'Ссылка на образование';
CREATE TABLE Education (
  ID   number(10) NOT NULL, 
  Name nvarchar2(100) NOT NULL UNIQUE, 
  PRIMARY KEY (ID));
COMMENT ON TABLE Education IS 'Образование';
CREATE TABLE Individual_ContractDoc (
  IndividualID  number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  PRIMARY KEY (IndividualID, 
  ContractDocID));
COMMENT ON TABLE Individual_ContractDoc IS 'Закрепление физических лиц за договором';
CREATE TABLE ReportGrouping (
  ContractTypeID         number(10) NOT NULL, 
  ContractTypeSubgroupID number(10) NOT NULL, 
  ContractorID           number(10), 
  PRIMARY KEY (ContractTypeID, 
  ContractTypeSubgroupID));
CREATE TABLE Report (
  ID          number(10) NOT NULL, 
  Name        nvarchar2(100) NOT NULL UNIQUE, 
  Description nvarchar2(2000), 
  Template    nvarchar2(255), 
  PRIMARY KEY (ID));
COMMENT ON TABLE Report IS 'Отчёт';
COMMENT ON COLUMN Report.ID IS 'Идентификатор отчёта';
COMMENT ON COLUMN Report.Name IS 'Название отчёта';
COMMENT ON COLUMN Report.Description IS 'Описание отчёта';
COMMENT ON COLUMN Report.Template IS 'Имя шаблонв отчёта';
CREATE TABLE FilterState (
  ID              number(10) NOT NULL, 
  Name            nvarchar2(100) NOT NULL UNIQUE, 
  Description     nvarchar2(2000), 
  FilterData      blob, 
  Owner           number(10) NOT NULL, 
  BasedOnFilterID number(10), 
  PRIMARY KEY (ID));
COMMENT ON TABLE FilterState IS 'Пользовательские фильтры';
COMMENT ON COLUMN FilterState.ID IS 'Идентификатор фильтра';
COMMENT ON COLUMN FilterState.Name IS 'Наименование фильтра';
COMMENT ON COLUMN FilterState.Description IS 'Описание фильтра';
COMMENT ON COLUMN FilterState.FilterData IS 'Данные фильтра';
COMMENT ON COLUMN FilterState.Owner IS 'Владелец фильтра:
0 - система
-1 - публичный';
COMMENT ON COLUMN FilterState.BasedOnFilterID IS 'Определяет на каком системном фильтре основан пользовательский';
CREATE TABLE Report_FilterState (
  ReportID      number(10) NOT NULL, 
  FilterStateID number(10) NOT NULL, 
  PRIMARY KEY (ReportID, 
  FilterStateID));
COMMENT ON TABLE Report_FilterState IS 'Допустимые отчёты для пользовательского фильтра';
COMMENT ON COLUMN Report_FilterState.ReportID IS 'Ссылка на отчёт';
COMMENT ON COLUMN Report_FilterState.FilterStateID IS 'Ccылка на фильтр';


alter table Contractdoc modify PrepaymentPrecentType    number(2); 

delete from ResponsibleForOrder where ResponsibleAssignmentOrderID is null;
alter table ResponsibleForOrder modify  ResponsibleAssignmentOrderID number(10) NOT NULL;

update CurrencyMeasure set Name = '(По умолчанию)' where ID = -1; 

alter table FilterState disable all triggers;
INSERT INTO FilterState
  (ID, Name, Description, FilterData, Owner, BasedOnFilterID) 
VALUES 
  (-1, 'Первый квартал текущего года', 'Первый квартал текущего года', null, 0, null);
INSERT INTO FilterState
  (ID, Name, Description, FilterData, Owner, BasedOnFilterID) 
VALUES 
  (-2, 'Второй квартал текущего года', 'Второй квартал текущего года', null, 0, null);
INSERT INTO FilterState
  (ID, Name, Description, FilterData, Owner, BasedOnFilterID) 
VALUES 
  (-3, 'Третий квартал текущего года', 'Третий квартал текущего года', null, 0, null);
INSERT INTO FilterState
  (ID, Name, Description, FilterData, Owner, BasedOnFilterID) 
VALUES 
  (-4, 'Четвертый квартал текущего года', 'Четвертый квартал текущего года', null, 0, null);
INSERT INTO FilterState
  (ID, Name, Description, FilterData, Owner, BasedOnFilterID) 
VALUES 
  (-5, 'Текущий год', 'Текущий год', null, 0, null);
alter table FilterState enable all triggers;


CREATE OR REPLACE NOFORCE VIEW "CONTRACTREPOSITORYVIEW"
  AS SELECT c.ID, c.Price, n.Percents as "NdsPercents", cr.Culture, cm.Factor, c.NDSAlgorithmID, alg.Name as "NDSAlgName", n.ID as "NDSID", cr.ID as "CurrencyID", cm.ID as "CurrencyMeasureID", c.CurrencyRate, c.StartAt, 
  c.EndsAt, c.AppliedAt, c.ApprovedAt, c.InternalNum, c.Subject, c.BrokeAt, c.OutOfControlAt, c.ReallyFinishedAt, c.ContractStateID, cs.Name as "StateName", c.Deleted, c.Origincontractid,
  ct.Name as "ContractTypeName", ctr.ShortName as "ContractorShortName", ctt.Name as "ContractorType", p.FamilyName as "PersonFamilyName",
  p.FirstName as "PersonFirstName", p.MiddleName as "PersonMiddleName", d.ID as "DegreeId", d.Name as "PersonDegree", rol.Name as "RoleName",
  e.FamilyName as "EmployeeFamilyName", e.FirstName as "EmployeeFirstName", e.MiddleName as "EmployeeMiddleName", pt.Name as "EmployeePost", pt.ID as "EmployeepostId",
  (select count(*) from "UD".Contractdoc c2 where c2.origincontractid = c.id) as "AgreementReferenceCount",
   (select count(*) from ContractHierarchy ch where ch.subcontractdocid = c.id) as "GeneralReferenceCount",
   (select count(*) from ScheduleContract sc where sc.contractdocid = c.id) as ScheduleCount
   FROM "UD".Contractdoc c 
   LEFT JOIN "UD".Contractstate cs ON cs.ID = c.ContractStateID
   LEFT JOIN "UD".Currencymeasure cm ON cm.ID = c.CurrencyMeasureId
   LEFT JOIN "UD".Currency cr ON cr.ID = c.CurrencyID
   LEFT JOIN "UD".NDS n ON n.ID = c.NDSID
   LEFT JOIN "UD".NDSAlgorithm alg ON alg.ID = c.NDSAlgorithmID
LEFT JOIN "UD".ContractType ct ON c.ContractTypeID = ct.ID
LEFT JOIN "UD".Contractor ctr ON c.ContractorID = ctr.ID
LEFT JOIN "UD".ContractorType ctt ON ctr.ContractorTypeID = ctt.ID
LEFT JOIN "UD".Person p ON p.ID = c.ContractorPersonID
LEFT OUTER JOIN "UD".Degree d ON d.ID = p.DegreeID
LEFT OUTER JOIN "UD".Responsible r ON r.ContractdocID = c.ID AND r.RoleID=1
LEFT JOIN "UD".Role rol ON rol.ID=r.RoleID
LEFT JOIN "UD".Employee e ON e.ID = r.EmployeeID
LEFT JOIN "UD".Post pt ON pt.ID = e.PostID 
ORDER BY c.ContractTypeID, c.InternalNum;


alter view "CONTRACTREPOSITORYVIEW" add constraint vemp_pk primary key (ID) disable;



ALTER TABLE ContractPayment DROP CONSTRAINT FKContractPa282609;
ALTER TABLE ContractPayment DROP CONSTRAINT FKContractPa457777;
ALTER TABLE ContractPayment ADD CONSTRAINT FKContractPa282609 FOREIGN KEY (PaymentDocumentID) REFERENCES PaymentDocument (ID) ON DELETE Cascade;
ALTER TABLE ContractPayment ADD CONSTRAINT FKContractPa457777 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
alter table Stage drop constraint FKStage364615;
ALTER TABLE Stage ADD CONSTRAINT FKStage364615 FOREIGN KEY (ActID) REFERENCES Act (ID) ON DELETE Set null;
ALTER TABLE ApprovalProcess DROP CONSTRAINT FKApprovalPr530332;
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr530332 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
alter table ApprovalProcess drop constraint FKApprovalPr619985;
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr619985 FOREIGN KEY (ApprovalStateID) REFERENCES ApprovalState (ID) ON DELETE Cascade;
ALTER TABLE STAGE DROP CONSTRAINT AprovalStageState_FK;
ALTER TABLE Stage ADD CONSTRAINT AprovalStageState_FK FOREIGN KEY (ApprovalStateID) REFERENCES ApprovalState (ID) ON DELETE Cascade;

ALTER TABLE Individual ADD CONSTRAINT FKIndividual979987 FOREIGN KEY (EducationID) REFERENCES Education (ID);
ALTER TABLE Individual_ContractDoc ADD CONSTRAINT FKIndividual865334 FOREIGN KEY (IndividualID) REFERENCES Individual (ID);
ALTER TABLE Individual_ContractDoc ADD CONSTRAINT FKIndividual769769 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID);
ALTER TABLE ReportGrouping ADD CONSTRAINT ContractTypeGroup_FK FOREIGN KEY (ContractTypeID) REFERENCES ContractType (ID);
ALTER TABLE ReportGrouping ADD CONSTRAINT ContracttypeSubgroup_FK FOREIGN KEY (ContractTypeSubgroupID) REFERENCES ContractType (ID);
ALTER TABLE ReportGrouping ADD CONSTRAINT FKReportGrou286551 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE Report_FilterState ADD CONSTRAINT FKReport_Fil356408 FOREIGN KEY (ReportID) REFERENCES Report (ID);
ALTER TABLE Report_FilterState ADD CONSTRAINT FKReport_Fil878991 FOREIGN KEY (FilterStateID) REFERENCES FilterState (ID);
ALTER TABLE FilterState ADD CONSTRAINT FKFilterStat935122 FOREIGN KEY (BasedOnFilterID) REFERENCES FilterState (ID);
CREATE INDEX PrepNDS_Index 
  ON ContractDoc (PrepaymentNDSAlgorithmID);
CREATE INDEX ContractDoc_ContractTypeID 
  ON ContractDoc (ContractTypeID);
CREATE INDEX ContractDoc_CurrencyID 
  ON ContractDoc (CurrencyID);
CREATE INDEX ContractDoc_OriginContractID 
  ON ContractDoc (OriginContractID);
CREATE INDEX ContractDoc_NDSAlgorithmID 
  ON ContractDoc (NDSAlgorithmID);
CREATE INDEX ContractDoc_ContractStateID 
  ON ContractDoc (ContractStateID);
CREATE INDEX ContractDoc_CurrencyMeasureID 
  ON ContractDoc (CurrencyMeasureID);
CREATE INDEX ContractDoc_ContractorID 
  ON ContractDoc (ContractorID);
CREATE INDEX Contractor_ContractorTypeID 
  ON Contractor (ContractorTypeID);
CREATE INDEX Stage_NdsAlgorithmID 
  ON Stage (NdsAlgorithmID);
CREATE INDEX Stage_ParentID 
  ON Stage (ParentID);
CREATE INDEX Stage_ActID 
  ON Stage (ActID);
CREATE INDEX Stage_ScheduleID 
  ON Stage (ScheduleID);
CREATE INDEX Stage_NdsID 
  ON Stage (NdsID);
CREATE INDEX Act_ActTypeID 
  ON Act (ActTypeID);
CREATE INDEX Act_EnterpriceAuthorityID 
  ON Act (EnterpriceAuthorityID);
CREATE INDEX Act_NdsalgorithmID 
  ON Act (NdsalgorithmID);
CREATE INDEX Act_CurrencyID 
  ON Act (CurrencyID);
CREATE INDEX Act_CurrencymeasureID 
  ON Act (CurrencymeasureID);
CREATE INDEX StageResult_EFT_Index 
  ON StageResult (EconomicEfficiencyTypeID);
CREATE INDEX StageResult_NTPSubViewID 
  ON StageResult (NTPSubViewID);
CREATE INDEX NTPSubView_NTPViewID 
  ON NTPSubView (NTPViewID);
CREATE INDEX ReportGrouping_ContractorID 
  ON ReportGrouping (ContractorID);
CREATE OR REPLACE TRIGGER trg_Schedulecontract_AD
AFTER DELETE ON ScheduleContract
FOR EACH ROW
declare 
  depschedulecount number;
  CURSOR schedulecursor (Scheduleid NUMBER) IS
    select count(*) from schedulecontract s where s.scheduleid = scheduleid;
begin
  open schedulecursor(:old.scheduleid);
  fetch schedulecursor into depschedulecount;
  if (depschedulecount = 0) then 
    delete from schedule where schedule.id = :old.scheduleid;
  end if;  
  return;
end;





 
 

