  /*
		UPDATE TO 1.50 
  */
  
  /*Проверка на завязку генерального договора на самого себя при создании СД*/
  ALTER TABLE ContractHierarchy ADD (Constraint  
    CHECK_LINK_TO_SELF CHECK (generalcontractdocid<>subcontractdocid));

  /* Установка алгоритма НДС для актов, в которых по каким-то обстоятельствам он на задан (по умолчанию - вт. ч. НСД)  */
  UPDATE Act SET NdsalgorithmID=1 where NdsalgorithmID IS NULL;

  /* TODO: Установка алгоритма НДС для актов, в которых по каким-то обстоятельствам он не совпадает с заданным в договоре */
  UPDATE Act a SET NdsalgorithmID=
   (select c.NdsalgorithmID from select  c.NdsalgorithmID from stage s, schedule sh, schedulecontract sc, contractdoc c where s.actid = a.id and s.scheduleid = sh.id and sc.scheduleid=sh.id and c.id = sc.contractdocid and rownum <=1) as ndsalg
   where a.NdsalgorithmID <> ndsalg and (ndsalog IS NOT NULL);
