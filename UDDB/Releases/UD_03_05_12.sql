-- This query will display the current date

SELECT SYSDATE FROM DUAL;

alter table filterstate disable all triggers;
insert into filterstate (ID, NAME, DESCRIPTION, OWNER, BASEDONFILTERID) values (-6, '��� ��������', '��� ��������', 0, null);
alter table filterstate enable all triggers;

