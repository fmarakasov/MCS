update act set issigned = 1;
commit;

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