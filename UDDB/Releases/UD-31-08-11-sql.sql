alter table Responsible drop constraint U_DISPOSAL_EMPLOYEE;
alter table Responsible drop constraint FKRESPONSIBL163229;
alter table RESPONSIBLE
  add constraint FKRESPONSIBL163229 foreign key (DISPOSALID)
  references disposal (ID) on delete cascade;

alter table Responsible drop constraint EMPLOYEERESPONSIBLE_FK;
alter table RESPONSIBLE
  add constraint EMPLOYEERESPONSIBLE_FK foreign key (EMPLOYEEID)
  references employee (ID);