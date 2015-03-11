
/*UPDATE TO 1.45*/

CREATE SEQUENCE seq_ContractorContractDoc START WITH 10000;
CREATE SEQUENCE seq_ContractDocDocumentImage START WITH 10000;
CREATE SEQUENCE seq_DocumentImage START WITH 10000;

ALTER TABLE CONTRACTDOC ADD DISBURSED_CACHE NUMERIC(18,2);
ALTER TABLE CONTRACTDOC ADD LEFT_CACHE NUMERIC(18,2);
ALTER TABLE CONTRACTDOC ADD DISBURSED_COWORKERS_CACHE NUMERIC(18,2);
ALTER TABLE CONTRACTDOC ADD LEFT_COWORKERS_CACHE NUMERIC(18,2);
ALTER TABLE CONTRACTDOC ADD STAGES_TOTAL_PRICE_CACHE NUMERIC(18,2);

CREATE TABLE ContractDocDocumentImage (
  ID              number(10, 0) NOT NULL, 
  ContractDocID   number(10) NOT NULL, 
  DocumentImageID number(10) NOT NULL, 
  PRIMARY KEY (ID), 
  CONSTRAINT U_CONTRACT_DOCUMENT 
    UNIQUE (ContractDocID, DocumentImageID));
CREATE TABLE DocumentImage (
  ID           number(10) NOT NULL, 
  Image        blob, 
  DocumentID   number(10) NOT NULL, 
  Name         nvarchar2(2000), 
  PhysicalName nvarchar2(255) NOT NULL, 
  Created      date NOT NULL, 
  Modified     date NOT NULL, 
  Description  nvarchar2(2000), 
  PRIMARY KEY (ID));
COMMENT ON TABLE DocumentImage IS 'Хранение файлов (сканированных копий) документов';
COMMENT ON COLUMN DocumentImage.ID IS 'Идентификатор файла документа';
COMMENT ON COLUMN DocumentImage.Image IS 'Содержимое документа';
COMMENT ON COLUMN DocumentImage.DocumentID IS 'Тип документа';
COMMENT ON COLUMN DocumentImage.Name IS 'Название документа';
COMMENT ON COLUMN DocumentImage.PhysicalName IS 'Имя файла документа';
COMMENT ON COLUMN DocumentImage.Created IS 'Дата создания записи';
COMMENT ON COLUMN DocumentImage.Modified IS 'Дата модификации записи';
COMMENT ON COLUMN DocumentImage.Description IS 'Описание документа';
ALTER TABLE DocumentImage ADD CONSTRAINT FKDocumentIm910905 FOREIGN KEY (DocumentID) REFERENCES Document (ID) ON DELETE CASCADE;
ALTER TABLE ContractDocDocumentImage ADD CONSTRAINT FKContractDo72937 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE CASCADE;
ALTER TABLE ContractDocDocumentImage ADD CONSTRAINT FKContractDo829218 FOREIGN KEY (DocumentImageID) REFERENCES DocumentImage (ID) ON DELETE CASCADE;


CREATE TABLE ContractorContractDoc (
  ID            number(10, 0) NOT NULL, 
  ContractorID  number(10) NOT NULL, 
  ContractDocID number(10) NOT NULL, 
  PRIMARY KEY (ID));
ALTER TABLE ContractorContractDoc ADD CONSTRAINT FKContractor814889 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE CASCADE;
ALTER TABLE ContractorContractDoc ADD CONSTRAINT FKContractor564632 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE CASCADE;

INSERT INTO Document
  (ID, Name) 
VALUES 
  (9, 'Архив документов');

INSERT INTO Contractortype
  (ID, Name) 
VALUES 
  (4, 'Физические лица');
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
INSERT INTO FilterState
  (ID, Name, Description, FilterData, Owner, BasedOnFilterID) 
VALUES 
  (-6, 'Первое полугодие', 'Первое полугодие', null, 0, null);
INSERT INTO FilterState
  (ID, Name, Description, FilterData, Owner, BasedOnFilterID) 
VALUES 
  (-7, 'Второе полугодие', 'Второе полугодие', null, 0, null);
INSERT INTO FilterState
  (ID, Name, Description, FilterData, Owner, BasedOnFilterID) 
VALUES 
  (-8, 'Все', 'Все договора', null, 0, null);

commit;

alter table CONTRACTOR modify Name NVARCHAR2(500);
 
alter table CONTRACTOR ADD  Insurance        nvarchar2(100);
alter table CONTRACTOR ADD  Birthplace       nvarchar2(2000); 
alter table CONTRACTOR ADD  MiddleName       nvarchar2(100);
alter table CONTRACTOR ADD  FirstName        nvarchar2(100); 
alter table CONTRACTOR ADD  Birthdate        date; 
alter table CONTRACTOR ADD  PasportNumber    nvarchar2(20);
alter table CONTRACTOR ADD  PasportSeries    nvarchar2(10); 
alter table CONTRACTOR ADD  PasportAuthority nvarchar2(2000); 
alter table CONTRACTOR ADD  FamilyName       nvarchar2(100); 
alter table CONTRACTOR ADD  PasportDate      date; 
alter table CONTRACTOR ADD  EducationID      number(10); 
alter table CONTRACTOR ADD  Sex      number(1); 

COMMENT ON COLUMN Contractor.Insurance IS 'Номер пенсионного удостоверения';
COMMENT ON COLUMN Contractor.Birthplace IS 'Место рождения';
COMMENT ON COLUMN Contractor.MiddleName IS 'Отчество';
COMMENT ON COLUMN Contractor.FirstName IS 'Имя';
COMMENT ON COLUMN Contractor.Birthdate IS 'Дата рождения';
COMMENT ON COLUMN Contractor.PasportNumber IS 'Номер паспорта';
COMMENT ON COLUMN Contractor.PasportSeries IS 'Серия паспорта';
COMMENT ON COLUMN Contractor.PasportAuthority IS 'Кем выдан паспорт';
COMMENT ON COLUMN Contractor.FamilyName IS 'Фамилия';
COMMENT ON COLUMN Contractor.PasportDate IS 'Дата выдачи паспорта';
COMMENT ON COLUMN Contractor.Sex IS 'Пол';

ALTER TABLE Contractor ADD CONSTRAINT FKContractor512206 FOREIGN KEY (EducationID) REFERENCES Education (ID);



/*Перенести данные контрагента из contractdoc в ContractorContractDoc*/
create or replace trigger ContractorContractDoc_insert before 
insert on ContractorContractDoc for each row 
begin
select seq_ContractorContractDoc.nextval
into :new.id
from dual;
end;

INSERT INTO ContractorContractDoc (ContractorID, ContractDocID) SELECT contractorid, id from contractdoc where not (contractorid is null);
commit;

drop trigger ContractorContractDoc_insert;
commit;

ALTER TABLE contractdoc
DROP COLUMN ContractorID

drop table INDIVIDUAL_CONTRACTDOC;
drop table individual;

/*Обновление представления для реестра*/

-- ****** Object: View UD.CONTRACTREPOSITORYVIEW Script Date: 29.11.2011 16:25:01 ******
CREATE OR REPLACE VIEW "CONTRACTREPOSITORYVIEW"
  AS SELECT c.ID, c.Price, n.Percents as "NdsPercents", cr.Culture, cm.Factor, c.NDSAlgorithmID, alg.Name as "NDSAlgName", n.ID as "NDSID", cr.ID as "CurrencyID", cm.ID as "CurrencyMeasureID", c.CurrencyRate, c.StartAt, 
  c.EndsAt, c.AppliedAt, c.ApprovedAt, c.InternalNum, c.Subject, c.BrokeAt, c.OutOfControlAt, c.ReallyFinishedAt, c.ContractStateID, C.DISBURSED_CACHE, C.Left_cache, c.Stages_total_price_cache,  c.DISBURSED_COWORKERS_CACHE,  c.LEFT_COWORKERS_CACHE, cs.Name as "StateName", c.Deleted, c.Origincontractid,
  ct.Name as "ContractTypeName", p.FamilyName as "PersonFamilyName",
  p.FirstName as "PersonFirstName", p.MiddleName as "PersonMiddleName", d.ID as "DegreeId", d.Name as "PersonDegree", rol.Name as "RoleName",
  e.FamilyName as "EmployeeFamilyName", e.FirstName as "EmployeeFirstName", e.MiddleName as "EmployeeMiddleName", pt.Name as "EmployeePost", pt.ID as "EmployeepostId",
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
cst.Parentid = s.Id and (cst.actid is null))) as "ClosedParentCount"
   FROM "UD".Contractdoc c 
   LEFT JOIN "UD".Contractstate cs ON cs.ID = c.ContractStateID
   LEFT JOIN "UD".Currencymeasure cm ON cm.ID = c.CurrencyMeasureId
   LEFT JOIN "UD".Currency cr ON cr.ID = c.CurrencyID
   LEFT JOIN "UD".NDS n ON n.ID = c.NDSID
   LEFT JOIN "UD".NDSAlgorithm alg ON alg.ID = c.NDSAlgorithmID
LEFT JOIN "UD".ContractType ct ON c.ContractTypeID = ct.ID
LEFT JOIN "UD".Person p ON p.ID = c.ContractorPersonID
LEFT OUTER JOIN "UD".Degree d ON d.ID = p.DegreeID
LEFT OUTER JOIN "UD".Responsible r ON r.ContractdocID = c.ID AND r.RoleID=1
LEFT JOIN "UD".Role rol ON rol.ID=r.RoleID
LEFT JOIN "UD".Employee e ON e.ID = r.EmployeeID
LEFT JOIN "UD".Post pt ON pt.ID = e.PostID 
ORDER BY c.ContractTypeID, c.InternalNum