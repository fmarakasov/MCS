-- This query will display the current date

SELECT SYSDATE FROM DUAL;

/*
    DROP OBJECTS OLDER VERSION 1.5
*/
DROP TABLE CONTRACT_TRANSFERACT_DOCUMENT CASCADE CONSTRAINTS;
DROP TABLE EFFICPARAMETER_STAGERESULT CASCADE CONSTRAINTS;
DROP TABLE TRANSFERACTTYPE_DOCUMENT CASCADE CONSTRAINTS;
DROP TABLE ADDITIONCONTRACTORPROPERTIY CASCADE CONSTRAINTS;
DROP TABLE Contract_TransferAct_Document CASCADE CONSTRAINTS;

DROP SEQUENCE SEQ_EFFICPARAMETER_STAGERESULT;

