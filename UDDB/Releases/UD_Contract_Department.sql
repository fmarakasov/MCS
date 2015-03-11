alter table Department disable all triggers;
insert into Department (Id, Managerid, Directedbyid, Name) values (0, -1, -1, 'Планово-экономический отдел ');
update employee e set e.departmentid = 0 where e.departmentid = 10023;
update responsiblefororder r set e.departmentid = 0 where e.departmentid = 10023;
delete from department d where d.id = 10023;
alter table Department enable all triggers;