-- This query will display the current date

-- Add/modify columns 
alter table CONTRACTDOC add acttypeid number(10);
-- Add comments to the columns 
comment on column CONTRACTDOC.acttypeid
  is '“ип акта';

alter table CONTRACTDOC
  add constraint CONTRACTACTTYPE_FK foreign key (ACTTYPEID)
  references acttype (ID);