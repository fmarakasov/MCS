/*
/*
Система учёта договоров 1.0
Скрипт сгенерирован 25/11/201206:03:20 PM
1.48
   1) Удалена таблица актов передачи актов к договорам
   2) Таблица  ContractTranActDoc используется и для договоров и для актов
        Удалено ограничение UNIQUE
   3) Переименовано ограничение первичного ключа между ApprovalProcess и ApprovalState
1.47
   1) Задействована таблица версионности схемы БД (1.47 - текущая схема)
   2) Исправлено представление репозитория актов
1.46
    1) Добавлены процедуры для поддержки работы распоряжений
    2) Изменено представление реестра договоров для группировки по основному договору
    3) Черновая (!!!) версия представления реестра актов
    4) Удален Individual. Учёт физ.лиц ведётся с использованием изменённой таблицы контрагентов. 
1.45
    1) Добавлены кэш-поля в contractdoc (disbursed_cache, left_cache)
1.44
    1) Добавлен Individual
    2) Добавлен Education
    3) Введены договоры и этапы с открытыми датами (поля Delta и Comment)
    4) Введено согласование этапов и результатов
    5) Введено назначение ответственного по этапу
	6) Введены группировки типов договоров для отчётов в соответствие с контрагентами (ReportGrouping)
	7) Введено хранение фильтров договоров
	8) Изменена политика удаления акта с cascade на set null
 
  1.43:
	1)  Добавлено поле Images:blob для Contractdoc
	2) Добавлены индексы на внешние ключи для таблиц
	    с интенсивными операциями соединения
	3) Изменены вставляемые строки в WorkType и CurrencyMeasure
*/
*/
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo331897 FOREIGN KEY (ContractTypeID) REFERENCES ContractType (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo465214 FOREIGN KEY (CurrencyID) REFERENCES Currency (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo919377 FOREIGN KEY (OriginContractID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo12114 FOREIGN KEY (NDSAlgorithmID) REFERENCES NDSAlgorithm (ID) ON DELETE Cascade;
ALTER TABLE Prepayment ADD CONSTRAINT FKPrepayment822569 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo459574 FOREIGN KEY (PrepaymentNDSAlgorithmID) REFERENCES NDSAlgorithm (ID);
ALTER TABLE Person ADD CONSTRAINT FKPerson162299 FOREIGN KEY (DegreeID) REFERENCES Degree (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo371687 FOREIGN KEY (NDSID) REFERENCES NDS (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo917205 FOREIGN KEY (ContractStateID) REFERENCES ContractState (ID) ON DELETE Cascade;
ALTER TABLE ContractHierarchy ADD CONSTRAINT FKContractHi836133 FOREIGN KEY (GeneralContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractHierarchy ADD CONSTRAINT FKContractHi748403 FOREIGN KEY (SubContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo561570 FOREIGN KEY (CurrencyMeasureID) REFERENCES CurrencyMeasure (ID);
ALTER TABLE PaymentDocument ADD CONSTRAINT FKPaymentDoc430792 FOREIGN KEY (CurrencyMeasureID) REFERENCES CurrencyMeasure (ID);
ALTER TABLE PaymentDocument ADD CONSTRAINT FKPaymentDoc369604 FOREIGN KEY (PrepaymentDocumentTypeID) REFERENCES PrepaymentDocumentType (ID);
ALTER TABLE ContractPayment ADD CONSTRAINT FKContractPa457777 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ContractPayment ADD CONSTRAINT FKContractPa282609 FOREIGN KEY (PaymentDocumentID) REFERENCES PaymentDocument (ID) ON DELETE Cascade;
ALTER TABLE Trouble ADD CONSTRAINT FKTrouble646380 FOREIGN KEY (TopTroubleID) REFERENCES Trouble (ID);
ALTER TABLE ContractTrouble ADD CONSTRAINT FKContractTr714891 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE Trouble ADD CONSTRAINT FKTrouble472627 FOREIGN KEY (TroubleRegistryID) REFERENCES TroublesRegistry (ID);
ALTER TABLE ContractTrouble ADD CONSTRAINT FKContractTr217582 FOREIGN KEY (TroubleID) REFERENCES Trouble (ID) ON DELETE Cascade;
ALTER TABLE ContractorPropertiy ADD CONSTRAINT FKContractor416746 FOREIGN KEY (PropertyID) REFERENCES Property (ID) ON DELETE Cascade;
ALTER TABLE ContractorPropertiy ADD CONSTRAINT FKContractor152047 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE FunctionalCustomer ADD CONSTRAINT FKFunctional561442 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE FunctionalCustomer ADD CONSTRAINT FKFunctional131682 FOREIGN KEY (ParentFunctionalCustomerID) REFERENCES FunctionalCustomer (ID) ON DELETE Cascade;
ALTER TABLE FunctionalCustomerContract ADD CONSTRAINT FKFunctional603255 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE FunctionalCustomer ADD CONSTRAINT FKFunctional524086 FOREIGN KEY (FunctionalCustomerTypeID) REFERENCES FunctionalCustomerType (ID) ON DELETE Cascade;
ALTER TABLE FunctionalCustomerContract ADD CONSTRAINT FKFunctional451088 FOREIGN KEY (FunctionalCustomerID) REFERENCES FunctionalCustomer (ID) ON DELETE Cascade;
ALTER TABLE Contractor ADD CONSTRAINT FKContractor394676 FOREIGN KEY (ContractorTypeID) REFERENCES ContractorType (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo906594 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE ScheduleContract ADD CONSTRAINT FKScheduleCo823189 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE Stage ADD CONSTRAINT FKStage472417 FOREIGN KEY (NdsAlgorithmID) REFERENCES NDSAlgorithm (ID) ON DELETE Cascade;
ALTER TABLE Stage ADD CONSTRAINT FKStage219572 FOREIGN KEY (ParentID) REFERENCES Stage (ID) ON DELETE Cascade;
ALTER TABLE ClosedStageRelation ADD CONSTRAINT FKClosedStag717175 FOREIGN KEY (StageID) REFERENCES Stage (ID) ON DELETE Cascade;
ALTER TABLE ClosedStageRelation ADD CONSTRAINT FKClosedStag104726 FOREIGN KEY (ClosedStageID) REFERENCES Stage (ID) ON DELETE Cascade;
ALTER TABLE FuncCustomerPerson ADD CONSTRAINT FKFuncCustom534761 FOREIGN KEY (FuncCustomerID) REFERENCES FunctionalCustomer (ID) ON DELETE Cascade;
ALTER TABLE FuncCustomerPerson ADD CONSTRAINT FKFuncCustom258383 FOREIGN KEY (PersonID) REFERENCES Person (ID) ON DELETE Cascade;
ALTER TABLE Act ADD CONSTRAINT FKAct413802 FOREIGN KEY (RegionID) REFERENCES Region (ID) ON DELETE Cascade;
ALTER TABLE Act ADD CONSTRAINT FKAct287724 FOREIGN KEY (NDSID) REFERENCES NDS (ID) ON DELETE Cascade;
ALTER TABLE ActType ADD CONSTRAINT FKActType734662 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE Schedule ADD CONSTRAINT FKSchedule996822 FOREIGN KEY (WorkTypeID) REFERENCES WorkType (ID) ON DELETE Cascade;
ALTER TABLE Schedule ADD CONSTRAINT FKSchedule245187 FOREIGN KEY (CurrencyMeasureID) REFERENCES CurrencyMeasure (ID) ON DELETE Cascade;
ALTER TABLE Responsible ADD CONSTRAINT FKResponsibl163229 FOREIGN KEY (DisposalID) REFERENCES Disposal (ID) ON DELETE Cascade;
ALTER TABLE Responsible ADD CONSTRAINT EmployeeResponsible_FK FOREIGN KEY (EmployeeID) REFERENCES Employee (ID);
ALTER TABLE EnterpriseAuthority ADD CONSTRAINT FKEnterprise947996 FOREIGN KEY (EmployeeID) REFERENCES Employee (ID);
ALTER TABLE EnterpriseAuthority ADD CONSTRAINT FKEnterprise312223 FOREIGN KEY (AuthorityID) REFERENCES Authority (ID) ON DELETE Cascade;
ALTER TABLE SightFuncPersonScheme ADD CONSTRAINT FKSightFuncP349042 FOREIGN KEY (FuncPersonID) REFERENCES FuncCustomerPerson (ID) ON DELETE Cascade;
ALTER TABLE SightFuncPerson ADD CONSTRAINT FKSightFuncP777798 FOREIGN KEY (SightFuncPersonSchID) REFERENCES SightFuncPersonScheme (ID) ON DELETE Cascade;
ALTER TABLE SightFuncPerson ADD CONSTRAINT FKSightFuncP679819 FOREIGN KEY (ActID) REFERENCES Act (ID) ON DELETE Cascade;
ALTER TABLE ActPaymentDocument ADD CONSTRAINT FKActPayment756331 FOREIGN KEY (ActID) REFERENCES Act (ID) ON DELETE Cascade;
ALTER TABLE ActPaymentDocument ADD CONSTRAINT FKActPayment244981 FOREIGN KEY (PaymentDocumentID) REFERENCES PaymentDocument (ID);
ALTER TABLE TransferAct ADD CONSTRAINT FKTransferAc108083 FOREIGN KEY (TransferActTypeID) REFERENCES TransferActType (ID) ON DELETE Cascade;
ALTER TABLE TransferActActDocument ADD CONSTRAINT FKTransferAc168818 FOREIGN KEY (TransferActID) REFERENCES TransferAct (ID) ON DELETE Cascade;
ALTER TABLE TransferActActDocument ADD CONSTRAINT FKTransferAc97768 FOREIGN KEY (DocumentID) REFERENCES Document (ID) ON DELETE Cascade;
ALTER TABLE TransferActActDocument ADD CONSTRAINT FKTransferAc139859 FOREIGN KEY (ActID) REFERENCES Act (ID) ON DELETE Cascade;
ALTER TABLE ContractTranActDoc ADD CONSTRAINT FKContractTr395401 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE TransferActTypeDocument ADD CONSTRAINT FKTransferAc261459 FOREIGN KEY (TransferActTypeID) REFERENCES TransferActType (ID) ON DELETE Cascade;
ALTER TABLE TransferActTypeDocument ADD CONSTRAINT FKTransferAc666473 FOREIGN KEY (DocumentID) REFERENCES Document (ID) ON DELETE Cascade;
ALTER TABLE ContractTranActDoc ADD CONSTRAINT FKContractTr661538 FOREIGN KEY (TransferActID) REFERENCES TransferAct (ID) ON DELETE Cascade;
ALTER TABLE ContractTranActDoc ADD CONSTRAINT FKContractTr590488 FOREIGN KEY (DocumentID) REFERENCES Document (ID) ON DELETE Cascade;
ALTER TABLE StageResult ADD CONSTRAINT FKStageResul876631 FOREIGN KEY (StageID) REFERENCES Stage (ID) ON DELETE Cascade;
ALTER TABLE NTPSubView ADD CONSTRAINT FKNTPSubView475996 FOREIGN KEY (NTPViewID) REFERENCES NTPView (ID) ON DELETE Cascade;
ALTER TABLE StageResult ADD CONSTRAINT FKStageResul865700 FOREIGN KEY (NTPSubViewID) REFERENCES NTPSubView (ID) ON DELETE Cascade;
ALTER TABLE StageResult ADD CONSTRAINT FKStageResul506505 FOREIGN KEY (EconomicEfficiencyTypeID) REFERENCES EconomEfficiencyType (ID) ON DELETE Cascade;
ALTER TABLE EfficienceParameterType ADD CONSTRAINT FKEfficience269842 FOREIGN KEY (EconomEfficiencyParameterID) REFERENCES EconomEfficiencyParameter (ID) ON DELETE Cascade;
ALTER TABLE EfficienceParameterType ADD CONSTRAINT FKEfficience747914 FOREIGN KEY (EconomEfficiencyTypeID) REFERENCES EconomEfficiencyType (ID) ON DELETE Cascade;
ALTER TABLE EfParameterStageResult ADD CONSTRAINT FKEfParamete714975 FOREIGN KEY (EconomEfficiencyParameterID) REFERENCES EconomEfficiencyParameter (ID) ON DELETE Cascade;
ALTER TABLE EfParameterStageResult ADD CONSTRAINT FKEfParamete943607 FOREIGN KEY (StageResultID) REFERENCES StageResult (ID) ON DELETE Cascade;
ALTER TABLE ContractorPosition ADD CONSTRAINT FKContractor346407 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE Person ADD CONSTRAINT FKPerson487057 FOREIGN KEY (ContractorPositionID) REFERENCES ContractorPosition (ID) ON DELETE Cascade;
ALTER TABLE Stage ADD CONSTRAINT FKStage364615 FOREIGN KEY (ActID) REFERENCES Act (ID) ON DELETE Set null;
ALTER TABLE Act ADD CONSTRAINT FKAct228252 FOREIGN KEY (EnterpriceAuthorityID) REFERENCES EnterpriseAuthority (ID) ON DELETE Cascade;
ALTER TABLE ContractorAuthority ADD CONSTRAINT FKContractor597586 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE ContractorAuthority ADD CONSTRAINT FKContractor191604 FOREIGN KEY (AuthorityID) REFERENCES Authority (ID) ON DELETE Cascade;
ALTER TABLE ContractorPosition ADD CONSTRAINT FKContractor858767 FOREIGN KEY (PositionID) REFERENCES Position (ID);
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo959699 FOREIGN KEY (ContractorPersonID) REFERENCES Person (ID);
ALTER TABLE SubGeneralHierarchi ADD CONSTRAINT FKSubGeneral189443 FOREIGN KEY (GeneralContractDocStageID) REFERENCES Stage (ID) ON DELETE Cascade;
ALTER TABLE SubGeneralHierarchi ADD CONSTRAINT FKSubGeneral651218 FOREIGN KEY (SubContractDocStageID) REFERENCES Stage (ID) ON DELETE Cascade;
ALTER TABLE Act ADD CONSTRAINT FKAct654720 FOREIGN KEY (ActTypeID) REFERENCES ActType (ID);
ALTER TABLE ScheduleContract ADD CONSTRAINT FKScheduleCo47579 FOREIGN KEY (ScheduleID) REFERENCES Schedule (ID) ON DELETE Cascade;
ALTER TABLE Stage ADD CONSTRAINT FKStage337267 FOREIGN KEY (ScheduleID) REFERENCES Schedule (ID) ON DELETE Cascade;
ALTER TABLE ContractDoc ADD CONSTRAINT FKContractDo108170 FOREIGN KEY (AuthorityID) REFERENCES EnterpriseAuthority (ID);
ALTER TABLE Stage ADD CONSTRAINT FKStage410729 FOREIGN KEY (NdsID) REFERENCES NDS (ID);
ALTER TABLE Act ADD CONSTRAINT FKAct481665 FOREIGN KEY (NdsalgorithmID) REFERENCES NDSAlgorithm (ID);
ALTER TABLE Act ADD CONSTRAINT FKAct124626 FOREIGN KEY (CurrencyID) REFERENCES Currency (ID);
ALTER TABLE Act ADD CONSTRAINT FKAct99460 FOREIGN KEY (CurrencymeasureID) REFERENCES CurrencyMeasure (ID);
ALTER TABLE IMPORTINGSCHEMEITEM ADD CONSTRAINT FKIMPORTINGS434803 FOREIGN KEY (SchemeID) REFERENCES IMPORTINGSCHEME (ID);
ALTER TABLE Department ADD CONSTRAINT ParentDepartment_FK FOREIGN KEY (ParentID) REFERENCES Department (ID);
ALTER TABLE Employee ADD CONSTRAINT EmployeePost_FK FOREIGN KEY (PostID) REFERENCES Post (ID);
ALTER TABLE Employee ADD CONSTRAINT WorksAt_FK FOREIGN KEY (DepartmentID) REFERENCES Department (ID);
ALTER TABLE ResponsibleForOrder ADD CONSTRAINT DepartmentResponsibleOrder_FK FOREIGN KEY (DepartmentID) REFERENCES Department (ID);
ALTER TABLE ResponsibleForOrder ADD CONSTRAINT EmployeeResponsibleOrder_FK FOREIGN KEY (EmployeeID) REFERENCES Employee (ID);
ALTER TABLE Department ADD CONSTRAINT ManagedBy_FK FOREIGN KEY (ManagerID) REFERENCES Employee (ID);
ALTER TABLE Department ADD CONSTRAINT DirectedBy_FK FOREIGN KEY (DirectedByID) REFERENCES Employee (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr530332 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID) ON DELETE Cascade;
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr495868 FOREIGN KEY (ToLocationID) REFERENCES Location (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT APPROVALPROCESS_STATE_FK FOREIGN KEY (ApprovalStateID) REFERENCES ApprovalState (ID) ON DELETE Cascade;
ALTER TABLE Contractdoc_Funds_Fact ADD CONSTRAINT FKContractdo229014 FOREIGN KEY (ContractdocID) REFERENCES ContractDoc (ID);
ALTER TABLE Contractdoc_Funds_Fact ADD CONSTRAINT FKContractdo589247 FOREIGN KEY (Time_DimID) REFERENCES Time_Dim (ID);
ALTER TABLE Responsible ADD CONSTRAINT ContractdocResponsible_FK FOREIGN KEY (ContractdocID) REFERENCES ContractDoc (ID);
ALTER TABLE Responsible ADD CONSTRAINT ResponsibleRole_FK FOREIGN KEY (RoleID) REFERENCES Role (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr874000 FOREIGN KEY (FromLocationId) REFERENCES Location (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr803309 FOREIGN KEY (MissiveTypeID) REFERENCES MissiveType (ID);
ALTER TABLE ApprovalProcess ADD CONSTRAINT FKApprovalPr262517 FOREIGN KEY (ApprovalGoalID) REFERENCES ApprovalGoal (ID);
ALTER TABLE ResponsibleForOrder ADD CONSTRAINT ResponsibleForOrder_FK FOREIGN KEY (ResponsibleAssignmentOrderID) REFERENCES ResponsibleAssignmentOrder (ID) ON DELETE Cascade;
ALTER TABLE ResponsibleForOrder ADD CONSTRAINT ContractTypeRespForOrder_FK FOREIGN KEY (ContractTypeID) REFERENCES ContractType (ID);
ALTER TABLE Responsible ADD CONSTRAINT StageResponsible_FK FOREIGN KEY (StageID) REFERENCES Stage (ID);
ALTER TABLE StageResult ADD CONSTRAINT AprovalStageResultState_FK FOREIGN KEY (ApprovalStateID) REFERENCES ApprovalState (ID);
ALTER TABLE Stage ADD CONSTRAINT AprovalStageState_FK FOREIGN KEY (ApprovalStateID) REFERENCES ApprovalState (ID) ON DELETE Cascade;
ALTER TABLE ReportGrouping ADD CONSTRAINT ContractTypeGroup_FK FOREIGN KEY (ContractTypeID) REFERENCES ContractType (ID);
ALTER TABLE ReportGrouping ADD CONSTRAINT ContracttypeSubgroup_FK FOREIGN KEY (ContractTypeSubgroupID) REFERENCES ContractType (ID);
ALTER TABLE ReportGrouping ADD CONSTRAINT FKReportGrou286551 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID) ON DELETE Cascade;
ALTER TABLE Report_FilterState ADD CONSTRAINT FKReport_Fil356408 FOREIGN KEY (ReportID) REFERENCES Report (ID);
ALTER TABLE Report_FilterState ADD CONSTRAINT FKReport_Fil878991 FOREIGN KEY (FilterStateID) REFERENCES FilterState (ID);
ALTER TABLE FilterState ADD CONSTRAINT FKFilterStat935122 FOREIGN KEY (BasedOnFilterID) REFERENCES FilterState (ID);
ALTER TABLE Contractor ADD CONSTRAINT FKContractor512206 FOREIGN KEY (EducationID) REFERENCES Education (ID);
ALTER TABLE ContractDocContractor ADD CONSTRAINT FKContractDo516487 FOREIGN KEY (ContractDocID) REFERENCES ContractDoc (ID);
ALTER TABLE ContractDocContractor ADD CONSTRAINT FKContractDo863034 FOREIGN KEY (ContractorID) REFERENCES Contractor (ID);
ALTER TABLE ContractTranActDoc ADD CONSTRAINT FKContractTr647138 FOREIGN KEY (ActID) REFERENCES Act (ID);
