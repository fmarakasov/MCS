-- This query will display the current date

SELECT SYSDATE FROM DUAL;

insert into ECONOMEFFICIENCYPARAMETER  (ID, Name) values (10022, 'Период расчета (лет)');

insert into efficienceparametertype (ID, economefficiencyparameterid, economefficiencytypeid)
values (10023, 10022, 10001);
insert into efficienceparametertype (ID, economefficiencyparameterid, economefficiencytypeid)
values (10024, 10022, 10002);

alter table STAGERESULT add actintroductiondate date;
alter table STAGERESULT add efficiencycomment nvarchar2(2000);

commit;