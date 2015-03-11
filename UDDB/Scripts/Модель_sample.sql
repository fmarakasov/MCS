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
INSERT INTO ContractType
  (ID, Name, ReportOrder) 
VALUES 
  (1, 'НИОКР', 1);
INSERT INTO ContractType
  (ID, Name, ReportOrder) 
VALUES 
  (2, 'Газификация и газоснабжение регионов', 2);
INSERT INTO ContractType
  (ID, Name, ReportOrder) 
VALUES 
  (3, 'ПИР', 3);
INSERT INTO ContractType
  (ID, Name, ReportOrder) 
VALUES 
  (4, 'Экспертиза сметной документации', 4);
INSERT INTO ContractType
  (ID, Name, ReportOrder) 
VALUES 
  (5, 'Производство', 5);
INSERT INTO ContractType
  (ID, Name, ReportOrder) 
VALUES 
  (6, 'Прочие', 6);
INSERT INTO ContractType
  (ID, Name, ReportOrder) 
VALUES 
  (-1, '(Не определён)', null);
INSERT INTO ContractType
  (ID, Name, ReportOrder) 
VALUES 
  (7, 'Оказание услуг', null);
INSERT INTO Currency
  (ID, Name, Culture, CurrencyI, CurrencyR, CurrencyM, SmallI, SmallR, SmallM, HighSmallName, LowSmallName, Code) 
VALUES 
  (1, 'Рубль', 'ru-ru', 'рубль', 'рубля', 'рублей', 'копейка', 'копейки', 'копеек', 'руб.', 'коп.', 'RUB');
INSERT INTO Currency
  (ID, Name, Culture, CurrencyI, CurrencyR, CurrencyM, SmallI, SmallR, SmallM, HighSmallName, LowSmallName, Code) 
VALUES 
  (2, 'Доллар', 'en-us', 'доллар', 'доллара', 'долларов', 'цент', 'цента', 'центов', 'дол.', 'цент.', 'USD');
INSERT INTO Currency
  (ID, Name, Culture, CurrencyI, CurrencyR, CurrencyM, SmallI, SmallR, SmallM, HighSmallName, LowSmallName, Code) 
VALUES 
  (-1, '(Не определён)', 'ru-ru', null, null, null, null, null, null, null, null, null);
INSERT INTO NDSAlgorithm
  (ID, Name, PriceTooltip) 
VALUES 
  (1, 'в том числе НДС', 'Включая НДС');
INSERT INTO NDSAlgorithm
  (ID, Name, PriceTooltip) 
VALUES 
  (2, 'кроме того НДС', 'Без НДС');
INSERT INTO NDSAlgorithm
  (ID, Name, PriceTooltip) 
VALUES 
  (3, 'без НДС', '');
INSERT INTO NDSAlgorithm
  (ID, Name, PriceTooltip) 
VALUES 
  (-1, '(Не определён)', '');
INSERT INTO ContractorType
  (ID, Name, ReportOrder) 
VALUES 
  (1, 'Другие организации', 3);
INSERT INTO ContractorType
  (ID, Name, ReportOrder) 
VALUES 
  (2, 'Дочерние организации', 2);
INSERT INTO ContractorType
  (ID, Name, ReportOrder) 
VALUES 
  (3, 'ОАО "Газпром"', 1);
INSERT INTO ContractorType
  (ID, Name, ReportOrder) 
VALUES 
  (-1, '(Не определён)', 4);
INSERT INTO Degree
  (ID, Name) 
VALUES 
  (1, 'кандидат наук');
INSERT INTO Degree
  (ID, Name) 
VALUES 
  (2, 'доктор наук');
INSERT INTO Degree
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO NDS
  (ID, Percents, Year) 
VALUES 
  (1, 18, 2004);
INSERT INTO ContractState
  (ID, Name) 
VALUES 
  (1, 'Не подписан');
INSERT INTO ContractState
  (ID, Name) 
VALUES 
  (2, 'Подписан');
INSERT INTO ContractState
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO CurrencyMeasure
  (ID, Name, Factor) 
VALUES 
  (1, 'ед.', 1);
INSERT INTO CurrencyMeasure
  (ID, Name, Factor) 
VALUES 
  (2, 'тыс.', 1000);
INSERT INTO CurrencyMeasure
  (ID, Name, Factor) 
VALUES 
  (3, 'млн.', 1000000);
INSERT INTO CurrencyMeasure
  (ID, Name, Factor) 
VALUES 
  (-1, '(По умолчанию)', 1);
INSERT INTO Position
  (ID, Name) 
VALUES 
  (1, 'Директор');
INSERT INTO Position
  (ID, Name) 
VALUES 
  (2, 'Начальник отдела поставок');
INSERT INTO Position
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO Post
  (ID, Name) 
VALUES 
  (-1, '(Не задана)');
INSERT INTO Post
  (ID, Name) 
VALUES 
  (1, 'Заместитель директора');
INSERT INTO Authority
  (ID, Name) 
VALUES 
  (1, 'Основание для Промгаза 1');
INSERT INTO Authority
  (ID, Name) 
VALUES 
  (2, 'Основание для исполнителя 1');
INSERT INTO Authority
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (1, 'Счет');
INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (2, 'Счет-фактура');
INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (3, 'Накладная');
INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO PrepaymentDocumentType
  (ID, Name) 
VALUES 
  (4, 'Платёжное поручение');
INSERT INTO PaymentDocument
  (ID, Num, PaymentDate, CurrencyMeasureID, PrepaymentDocumentTypeID, PaymentSum) 
VALUES 
  (1, '1', '1/1/2011', 1, 1, 100);
INSERT INTO Department
  (ID, ParentID, ManagerID, DirectedByID, Name) 
VALUES 
  (-1, null, -1, -1, '(Не задан)');
INSERT INTO TroublesRegistry
  (ID, Name, ShortName, ApprovedAt, OrderNum, ValidFrom, ValidTo) 
VALUES 
  (1, 'Перечень приоритетных научно-технических проблем ОАО "Газпром» на 2006-2010 годы', 'Перечень проблем 2006-2010г', '05.01.2006', '01-106', '01.01.2006', '01.01.2010');
INSERT INTO TroublesRegistry
  (ID, Name, ShortName, ApprovedAt, OrderNum, ValidFrom, ValidTo) 
VALUES 
  (2, 'Перечень приоритетных научно-технических проблем ОАО "Газпром» на 2002-2006 годы', 'Перечень проблем 2002-2006г', '10.01.2002', '01-342', '01.01.2002', '31.12.2005');
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (1, '1', '1', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (2, 'Совершенствование методов и моделей формирования перспективных планов и программ газовой промышленности России для устойчивого развития топливно-энергетического комплекса страны.', '1.1', 1, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (3, '2', '2', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (4, '3', '3', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (5, 'Создание методов и технологий для повышения эффективности разработки и безопасной эксплуатации месторождений.', '3.2', 4, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (6, 'Разработка методов, технических средств  и технологий освоения трудноизвлекаемых и нетрадиционных ресурсов газа в низконапорных коллекторах, газогидратных залежах и метана угольных бассейнов.', '3.4', 4, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (7, '4', '4', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (8, 'Создание современных методов и средств диспетчерского управления ЕСГ.', '4.4', 7, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (9, 'Создание технологий и технических средств для строительства, реконструкции и эксплуатации трубопроводных систем с оптимальными параметрами транспорта газа и устойчивостью к воздействию естественных факторов и технологических нагрузок', '4.1', 7, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (10, ' Развитие технологий и совершествование оборудования для обеспечения надежного функционирования ЕСГ, включая методы и средства диагностики и ремонта.', '4.2', 7, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (11, '5', '5', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (12, 'Совершенствование существующих и создание новых технологических процессов и технических средств глубокой переработки углеводородного сырья с целью производства и вывода на рынок новых видов продукции и услуг.', '5.2', 11, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (13, 'Развитие системы обеспечения эффективного использования Обществом топливно-энергетических ресурсов и стимулирования газо- и энергосбережения потребителям ОАО "Газпром"', '5.3', 11, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (14, '6', '6', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (15, '7', '7', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (16, 'Совершенствование методов и моделей формирования инвестиционных программ и управления проектами.', '7.4', 15, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (17, 'Разработка эффективной информационной, ценовой и налоговой политики, а также механизмов ее реализации в целях повышения рыночной капитализации и финансовой устойчивости Общества.', '7.5', 15, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (18, '8', '8', null, 1);
INSERT INTO Trouble
  (ID, Name, Num, TopTroubleID, TroubleRegistryID) 
VALUES 
  (19, ' Развитие системы управления здравоохранением ОАО «Газпром»', '8.3', 18, 1);
INSERT INTO Contractor
  (ID, Name, FirstName, MiddleName, PasportSeries, PasportNumber, PasportAuthority, PasportDate, Birthdate, Birthplace, Insurance, FamilyName, ShortName, Zip, Bank, ContractorTypeID, INN, Account, BIK, KPP, Address, CorrespAccount, OKPO, OKONH, OGRN, OKATO, OKVED, EducationID, Sex) 
VALUES 
  (1, 'НИЦ «Цеосит» ОИК СО РАН ', null, null, null, null, null, null, null, null, null, null, 'НИЦ «Цеосит» ОИК СО РАН ', null, null, 1, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, FirstName, MiddleName, PasportSeries, PasportNumber, PasportAuthority, PasportDate, Birthdate, Birthplace, Insurance, FamilyName, ShortName, Zip, Bank, ContractorTypeID, INN, Account, BIK, KPP, Address, CorrespAccount, OKPO, OKONH, OGRN, OKATO, OKVED, EducationID, Sex) 
VALUES 
  (2, 'ЗАО фирма "НОЭМИ"', null, null, null, null, null, null, null, null, null, null, 'ЗАО фирма "НОЭМИ', null, null, 1, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, FirstName, MiddleName, PasportSeries, PasportNumber, PasportAuthority, PasportDate, Birthdate, Birthplace, Insurance, FamilyName, ShortName, Zip, Bank, ContractorTypeID, INN, Account, BIK, KPP, Address, CorrespAccount, OKPO, OKONH, OGRN, OKATO, OKVED, EducationID, Sex) 
VALUES 
  (3, 'ООО КЗГО', null, null, null, null, null, null, null, null, null, null, 'ООО КЗГО', null, null, 1, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, FirstName, MiddleName, PasportSeries, PasportNumber, PasportAuthority, PasportDate, Birthdate, Birthplace, Insurance, FamilyName, ShortName, Zip, Bank, ContractorTypeID, INN, Account, BIK, KPP, Address, CorrespAccount, OKPO, OKONH, OGRN, OKATO, OKVED, EducationID, Sex) 
VALUES 
  (4, 'Государственное учреждение НПО «Тайфун»', null, null, null, null, null, null, null, null, null, null, 'Государственное учреждение НПО «Тайфун»', '116832', null, 1, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, FirstName, MiddleName, PasportSeries, PasportNumber, PasportAuthority, PasportDate, Birthdate, Birthplace, Insurance, FamilyName, ShortName, Zip, Bank, ContractorTypeID, INN, Account, BIK, KPP, Address, CorrespAccount, OKPO, OKONH, OGRN, OKATO, OKVED, EducationID, Sex) 
VALUES 
  (5, 'ОАО   «ИнфоТеКС»', null, null, null, null, null, null, null, null, null, null, 'ОАО   «ИнфоТеКС»', '113623', null, 1, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, FirstName, MiddleName, PasportSeries, PasportNumber, PasportAuthority, PasportDate, Birthdate, Birthplace, Insurance, FamilyName, ShortName, Zip, Bank, ContractorTypeID, INN, Account, BIK, KPP, Address, CorrespAccount, OKPO, OKONH, OGRN, OKATO, OKVED, EducationID, Sex) 
VALUES 
  (6, 'ОАО «Медицина для Вас»', null, null, null, null, null, null, null, null, null, null, 'ОАО «Медицина для Вас»', '169900', null, 1, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, FirstName, MiddleName, PasportSeries, PasportNumber, PasportAuthority, PasportDate, Birthdate, Birthplace, Insurance, FamilyName, ShortName, Zip, Bank, ContractorTypeID, INN, Account, BIK, KPP, Address, CorrespAccount, OKPO, OKONH, OGRN, OKATO, OKVED, EducationID, Sex) 
VALUES 
  (7, 'ОАО "Газпром"', null, null, null, null, null, null, null, null, null, null, 'ОАО "Газпром"', null, null, 3, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, FirstName, MiddleName, PasportSeries, PasportNumber, PasportAuthority, PasportDate, Birthdate, Birthplace, Insurance, FamilyName, ShortName, Zip, Bank, ContractorTypeID, INN, Account, BIK, KPP, Address, CorrespAccount, OKPO, OKONH, OGRN, OKATO, OKVED, EducationID, Sex) 
VALUES 
  (8, 'ГОУ ВПО Ухтинский Государственный Технический Университет', null, null, null, null, null, null, null, null, null, null, 'ГОУ ВПО УГТУ', '169300', null, 1, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO Contractor
  (ID, Name, FirstName, MiddleName, PasportSeries, PasportNumber, PasportAuthority, PasportDate, Birthdate, Birthplace, Insurance, FamilyName, ShortName, Zip, Bank, ContractorTypeID, INN, Account, BIK, KPP, Address, CorrespAccount, OKPO, OKONH, OGRN, OKATO, OKVED, EducationID, Sex) 
VALUES 
  (-1, 'Не определён', null, null, null, null, null, null, null, null, null, null, null, null, null, -1, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO Property
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO FunctionalCustomerType
  (ID, Name) 
VALUES 
  (1, 'Тип функционального заказчика 1');
INSERT INTO FunctionalCustomerType
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (1, 'Управление инновационного развития', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (2, 'Департамент по управлению персоналом', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (3, 'Департамент по добыче газа, газового конденсата, нефти', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (4, 'Департамент по транспортировке, подземному хранению и использованию газа', 7, null, 1);
INSERT INTO FunctionalCustomer
  (ID, Name, ContractorID, ParentFunctionalCustomerID, FunctionalCustomerTypeID) 
VALUES 
  (5, 'Департамент стратегического развития', 7, null, 1);
INSERT INTO ContractorPosition
  (ID, ContractorID, PositionID) 
VALUES 
  (1, 1, 1);
INSERT INTO ContractorPosition
  (ID, ContractorID, PositionID) 
VALUES 
  (2, 2, 1);
INSERT INTO ContractorPosition
  (ID, ContractorID, PositionID) 
VALUES 
  (3, 3, 2);
INSERT INTO ContractorPosition
  (ID, ContractorID, PositionID) 
VALUES 
  (4, 4, 1);
INSERT INTO ContractorPosition
  (ID, ContractorID, PositionID) 
VALUES 
  (-1, -1, -1);
INSERT INTO Employee
  (ID, Familyname, FirstName, Middlename, Sex, PostID, DepartmentID) 
VALUES 
  (1, 'Лосев', 'Леонид', 'Львович', 1, -1, -1);
INSERT INTO Employee
  (ID, Familyname, FirstName, Middlename, Sex, PostID, DepartmentID) 
VALUES 
  (-1, 'Нет данных', 'Нет данных', 'Нет данных', 1, -1, -1);
INSERT INTO EnterpriseAuthority
  (ID, Num, ValidFrom, ValidTo, IsValid, EmployeeID, AuthorityID) 
VALUES 
  (1, '12-23', '01.01.2010', '01.01.2011', 1, 1, 1);
INSERT INTO EnterpriseAuthority
  (ID, Num, ValidFrom, ValidTo, IsValid, EmployeeID, AuthorityID) 
VALUES 
  (-1, '(Не определён)', null, null, null, -1, -1);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (2, null, 0, 0, 1, 'Рожков', 'Сергей', 'Андреевич', 1, 1);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (3, 1, 0, 0, 1, 'Воробьева', 'Ирина', 'Львовна', 0, null);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (4, null, 0, 0, 1, 'Козлов', 'Петр', 'Макарович', 1, null);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (5, 2, 0, 0, 1, 'Урумян', 'Николай', 'Валерьевич', 1, null);
INSERT INTO Person
  (ID, DegreeID, IsContractHeadAuthority, IsActSignAuthority, IsValid, FamilyName, FirstName, MiddleName, Sex, ContractorPositionID) 
VALUES 
  (-1, -1, 0, 0, 0, '(Нет данных)', '(Нет данных)', '(Нет данных)', 1, null);
INSERT INTO WorkType
  (ID, Name, ShortName) 
VALUES 
  (1, 'Разработка документации', 'РД');
INSERT INTO WorkType
  (ID, Name, ShortName) 
VALUES 
  (2, 'Разработка программного комплекса', 'РПК');
INSERT INTO WorkType
  (ID, Name, ShortName) 
VALUES 
  (3, 'Оказание услуг по организации проведения государственной экологической экспертизы ', 'ГЭЭ');
INSERT INTO WorkType
  (ID, Name, ShortName) 
VALUES 
  (4, 'Оказание услуг по организации проведения государственной экспертизы ', 'ГЭ');
INSERT INTO WorkType
  (ID, Name, ShortName) 
VALUES 
  (-1, '(Не определён)', 'Н/А');
INSERT INTO Schedule
  (ID, CurrencyMeasureID, WorkTypeID) 
VALUES 
  (1, 1, 2);
INSERT INTO Schedule
  (ID, CurrencyMeasureID, WorkTypeID) 
VALUES 
  (2, 1, 1);
INSERT INTO Region
  (ID, Name) 
VALUES 
  (1, 'Москва и Московская облать');
INSERT INTO Region
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO ActType
  (ID, ContractorID, TypeName, IsActive) 
VALUES 
  (1, 7, 'Акт закрытия', 1);
INSERT INTO ActType
  (ID, ContractorID, TypeName, IsActive) 
VALUES 
  (-1, -1, '(Не определён)', null);
INSERT INTO Act
  (ID, Num, SignDate, ActTypeID, NDSID, RegionID, TotalSum, SumForTransfer, Status, EnterpriceAuthorityID, CurrencyRate, RateDate, NdsalgorithmID, CurrencyID, CurrencymeasureID, IsSigned) 
VALUES 
  (1, '143-23', '01.01.2011', 1, 1, 1, 200, 200, 1, 1, null, null, 1, 1, 1, null);
INSERT INTO ApprovalState
  (ID, Name, Color, StateDomain) 
VALUES 
  (-1, '(Не задано)', null, null);
INSERT INTO ApprovalState
  (ID, Name, Color, StateDomain) 
VALUES 
  (1, 'Согласование', null, null);
INSERT INTO ApprovalState
  (ID, Name, Color, StateDomain) 
VALUES 
  (2, 'Подписано', null, null);
INSERT INTO ApprovalState
  (ID, Name, Color, StateDomain) 
VALUES 
  (3, 'Исправление', null, null);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID, Code, NdsID, Delta, ApprovalStateID, Statedate, StateDescription, DeltaComment) 
VALUES 
  (1, '1', 'Анализ CEN/TS 15399:2007 и CEN/TS 15173:2006 и действующих нормативных документов РФ в области газораспределения. Определение структуры и разделов проекта стандарта. Обсуждение итогов работы  рабочей группой ', '05.07.2010 ', '18.10.2010', 200, 1, null, 1, 1, '23', 1, null, null, null, null, null);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID, Code, NdsID, Delta, ApprovalStateID, Statedate, StateDescription, DeltaComment) 
VALUES 
  (2, '2', 'Разработка первой редакции проекта стандарта «Газораспределительные системы. Система управления сетями газораспределения» Обсуждение первой редакции проекта стандарта рабочей группой. ', '18.10.2010', '27.12.2010', 600, 1, null, 1, 1, '4', 1, null, null, null, null, null);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID, Code, NdsID, Delta, ApprovalStateID, Statedate, StateDescription, DeltaComment) 
VALUES 
  (3, '3', 'Оформление уведомления о разработке стандарта и его публикация. Публикация проекта стандарта в информационной системе общего пользования в электронно-цифровой форме для проведения публичного обсуждения. Сбор замечаний и предложений, полученных при публичном обсуждении проекта стандарта. ', '10.01.2011', '15.05.2011', 200, 1, null, null, 1, '5', 1, null, null, null, null, null);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID, Code, NdsID, Delta, ApprovalStateID, Statedate, StateDescription, DeltaComment) 
VALUES 
  (4, '4', 'Внесение изменений и дополнений в проект и подготовка окончательной редакции стандарта. Обсуждение окончательной редакции проекта стандарта рабочей группой. ', '01.04.2011', '30.09.2011', 600, 1, null, null, 1, '6', 1, null, null, null, null, null);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID, Code, NdsID, Delta, ApprovalStateID, Statedate, StateDescription, DeltaComment) 
VALUES 
  (5, '5', 'Передача в ПК 4 проекта стандарта «Газораспределительные системы. Система управления сетями газораспределения» для организации экспертизы секретариатом ТК 23.', '01.10.2011', '25.12.2011', 400, 1, null, null, 1, '2', 1, null, null, null, null, null);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID, Code, NdsID, Delta, ApprovalStateID, Statedate, StateDescription, DeltaComment) 
VALUES 
  (6, '1.1', 'Анализ CEN/TS 15399:2007 и CEN/TS 15173:2006 и действующих нормативных документов РФ в области газораспределения. ', '05.07.2010', '01.09.2010', 90, 1, 9, null, 2, '4', 1, null, null, null, null, null);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID, Code, NdsID, Delta, ApprovalStateID, Statedate, StateDescription, DeltaComment) 
VALUES 
  (7, '1.2', 'Определение структуры и разделов проекта стандарта.', '01.09.2010', '01.10.2010', 10, 1, 9, null, 2, '5', 1, null, null, null, null, null);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID, Code, NdsID, Delta, ApprovalStateID, Statedate, StateDescription, DeltaComment) 
VALUES 
  (8, '1.3', 'Обсуждение итогов работы  рабочей группой.', '01.10.2010', '18.10.2010', 10, 1, null, null, 2, '6', 1, null, null, null, null, null);
INSERT INTO Stage
  (ID, Num, Subject, StartsAt, EndsAt, Price, NdsAlgorithmID, ParentID, ActID, ScheduleID, Code, NdsID, Delta, ApprovalStateID, Statedate, StateDescription, DeltaComment) 
VALUES 
  (9, '1', 'Анализ CEN/TS 15399:2007 и CEN/TS 15173:2006 и действующих нормативных документов РФ ', '05.07.2010', '18.10.2010', 110, 1, null, null, 2, '2', 1, null, null, null, null, null);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, AuthorityID, PrepaymentPrecentType, Deleted, AgreementNum, IsSubGeneral, IsGeneral, BrokeAt, OutOfControlAt, ReallyFinishedAt, Images, Departmentid, Delta, DeltaComment, Disbursed_cache, Left_cache, Stages_total_price_cache, Disbursed_coworkers_cache, Left_coworkers_cache) 
VALUES 
  (1, 2000, 1, '01.01.2010', '31.12.2011', '27.01.2009', '27.01.2010', '1001-10-1', null, 1, null, 1, 600, 30, 1, 1, 1, 1, 7, null, 'Разработка и внедрение новых технологических регламентов «Газораспределительные системы. Система управления сетями газораспределения»', null, null, 'Договор имеет 3 субподрядных договора ID = 2,3,4 и 1 доп. соглашение ID=5', 5, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, AuthorityID, PrepaymentPrecentType, Deleted, AgreementNum, IsSubGeneral, IsGeneral, BrokeAt, OutOfControlAt, ReallyFinishedAt, Images, Departmentid, Delta, DeltaComment, Disbursed_cache, Left_cache, Stages_total_price_cache, Disbursed_coworkers_cache, Left_coworkers_cache) 
VALUES 
  (2, 323, 1, '01.10.2010', '18.10.2010', '20.05.2009', '20.05.2009', '124-04-01', null, 1, null, 1, null, null, -1, 1, 1, 1, 1, null, 'Разработка технологического регламента', null, null, 'Субподрядное соглашение к ID=1', 2, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, AuthorityID, PrepaymentPrecentType, Deleted, AgreementNum, IsSubGeneral, IsGeneral, BrokeAt, OutOfControlAt, ReallyFinishedAt, Images, Departmentid, Delta, DeltaComment, Disbursed_cache, Left_cache, Stages_total_price_cache, Disbursed_coworkers_cache, Left_coworkers_cache) 
VALUES 
  (3, 500, 1, '10.01.2010', '27.12.2011', '15.03.2009', '15.03.2009', '300-06-1', null, 1, null, 1, null, null, -1, 1, 1, 1, 2, null, 'Разработка нормативной документации для информационного сопровождения разведки и разработки газоконденсатных и нефтегазоконденсатных месторождений в области изучения газоконденсатной характеристики скважин и месторождений, планирования и мониторинга добычи полезных ископаемых', null, null, 'Субподрядное соглашение к ID=1', 3, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, AuthorityID, PrepaymentPrecentType, Deleted, AgreementNum, IsSubGeneral, IsGeneral, BrokeAt, OutOfControlAt, ReallyFinishedAt, Images, Departmentid, Delta, DeltaComment, Disbursed_cache, Left_cache, Stages_total_price_cache, Disbursed_coworkers_cache, Left_coworkers_cache) 
VALUES 
  (4, 100, 1, '01.04.2011', '15.05.2011', '28.11.2009', '01.01.2010', '53-06-1', null, 1, null, 1, null, null, -1, 1, 1, 1, 3, null, 'Проведение подготовительных работ', null, null, 'Субподрядное соглашение к ID=1', 4, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, AuthorityID, PrepaymentPrecentType, Deleted, AgreementNum, IsSubGeneral, IsGeneral, BrokeAt, OutOfControlAt, ReallyFinishedAt, Images, Departmentid, Delta, DeltaComment, Disbursed_cache, Left_cache, Stages_total_price_cache, Disbursed_coworkers_cache, Left_coworkers_cache) 
VALUES 
  (5, 2000, 1, '01.01.2010', '31.12.2011', '06.05.2009', '20.05.2009', '1001-10-1/1', null, 1, 1, 1, 600, 30, -1, 1, 1, 1, 7, null, 'Разработка и внедрение новых технологических регламентов «Газораспределительные системы. Система управления сетями газораспределения»', null, null, 'Доп. соглашение к ID=1', 5, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO ContractDoc
  (ID, Price, ContractTypeID, StartAt, EndsAt, AppliedAt, ApprovedAt, InternalNum, ContractorNum, CurrencyID, OriginContractID, NDSAlgorithmID, PrepaymentSum, PrepaymentPercent, PrepaymentNDSAlgorithmID, NDSID, ContractStateID, CurrencyMeasureID, ContractorID, IsProtectability, Subject, CurrencyRate, RateDate, Description, ContractorPersonID, AuthorityID, PrepaymentPrecentType, Deleted, AgreementNum, IsSubGeneral, IsGeneral, BrokeAt, OutOfControlAt, ReallyFinishedAt, Images, Departmentid, Delta, DeltaComment, Disbursed_cache, Left_cache, Stages_total_price_cache, Disbursed_coworkers_cache, Left_coworkers_cache) 
VALUES 
  (6, 100000, 1, '01.01.2005', '31.12.2005', '01.01.2005', '10.12.2004', '10-20-30-40', null, 1, null, 1, null, null, -1, 1, 1, 1, 2, null, 'Удалённый договор', null, null, 'Удалённый договор', 3, null, null, 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
INSERT INTO Disposal
  (ID, Num, ApprovedDate) 
VALUES 
  (1, '103-15', '01.03.2009');
INSERT INTO Disposal
  (ID, Num, ApprovedDate) 
VALUES 
  (2, '124-16', '15.05.2009');
INSERT INTO Disposal
  (ID, Num, ApprovedDate) 
VALUES 
  (3, '135-9', '15.03.2009');
INSERT INTO Disposal
  (ID, Num, ApprovedDate) 
VALUES 
  (4, '165-65-9', '05.01.2010');
INSERT INTO Disposal
  (ID, Num, ApprovedDate) 
VALUES 
  (5, '32-43', '01.01.2010');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (1, 'Руководитель');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (2, 'Руководитель направления');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (3, 'Заместитель директора');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (-1, '(Не определена)');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (4, 'Ответственный по договорам');
INSERT INTO Role
  (ID, Name) 
VALUES 
  (5, 'Куратор от договорного отдела');
INSERT INTO Responsible
  (DisposalID, EmployeeID, ID, RoleID, ContractdocID, StageID) 
VALUES 
  (1, 1, 1, 1, 1, null);
INSERT INTO Responsible
  (DisposalID, EmployeeID, ID, RoleID, ContractdocID, StageID) 
VALUES 
  (1, 1, 2, 2, 1, null);
INSERT INTO Responsible
  (DisposalID, EmployeeID, ID, RoleID, ContractdocID, StageID) 
VALUES 
  (2, 1, 3, 1, 2, null);
INSERT INTO Responsible
  (DisposalID, EmployeeID, ID, RoleID, ContractdocID, StageID) 
VALUES 
  (3, 1, 4, 3, 2, null);
INSERT INTO ContractTrouble
  (TroubleID, ContractDocID, ID) 
VALUES 
  (6, 1, 1);
INSERT INTO ContractTrouble
  (TroubleID, ContractDocID, ID) 
VALUES 
  (16, 1, 2);
INSERT INTO ActPaymentDocument
  (ActID, PaymentDocumentID, ID) 
VALUES 
  (1, 1, 1);
INSERT INTO TransferActType
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO TransferActType
  (ID, Name) 
VALUES 
  (1, 'Акт передачи договоров');
INSERT INTO TransferActType
  (ID, Name) 
VALUES 
  (2, 'кт передачи актов к договорам');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (1, 'Договор');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (3, 'Календарный план');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (4, 'Протокол цены');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (5, 'Лист согласования');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (6, 'Протокол разногласий');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (7, 'Протокол согласования разногласий');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (8, 'Письма');
INSERT INTO Document
  (ID, Name) 
VALUES 
  (-1, '(Не определён)');
INSERT INTO NTPView
  (ID, Name) 
VALUES 
  (1, 'Концепция');
INSERT INTO NTPView
  (ID, Name) 
VALUES 
  (2, 'Нормативно-методическая документация');
INSERT INTO NTPView
  (ID, Name) 
VALUES 
  (-1, '(Не определена)');
INSERT INTO NTPSubView
  (ID, Name, NTPViewID, ShortName) 
VALUES 
  (1, 'Концепция', 1, null);
INSERT INTO NTPSubView
  (ID, Name, NTPViewID, ShortName) 
VALUES 
  (2, 'СТО', 2, null);
INSERT INTO NTPSubView
  (ID, Name, NTPViewID, ShortName) 
VALUES 
  (3, 'Методические рекомендации', 2, null);
INSERT INTO NTPSubView
  (ID, Name, NTPViewID, ShortName) 
VALUES 
  (-1, '(Не определено)', -1, null);
INSERT INTO EconomEfficiencyType
  (ID, Name) 
VALUES 
  (1, 'Тип 1');
INSERT INTO EconomEfficiencyType
  (ID, Name) 
VALUES 
  (2, 'Тип 2');
INSERT INTO EconomEfficiencyType
  (ID, Name) 
VALUES 
  (3, 'Тип 3');
INSERT INTO EconomEfficiencyType
  (ID, Name) 
VALUES 
  (-1, '(Не определено)');
INSERT INTO EconomEfficiencyParameter
  (ID, Name) 
VALUES 
  (-1, '(Не определено)');
INSERT INTO ContractorAuthority
  (ID, AuthorityID, NumDocument, ValidFrom, ValidTo, IsValid, ContractorID) 
VALUES 
  (1, 2, '13-65', '01.01.2010', '01.01.2011', 1, 7);
INSERT INTO ContractHierarchy
  (GeneralContractDocID, SubContractDocID, ID) 
VALUES 
  (1, 2, 1);
INSERT INTO ContractHierarchy
  (GeneralContractDocID, SubContractDocID, ID) 
VALUES 
  (1, 3, 2);
INSERT INTO ContractHierarchy
  (GeneralContractDocID, SubContractDocID, ID) 
VALUES 
  (1, 4, 3);
INSERT INTO UDMetadata
  (SchemeRelease, SchemeBuild, SchemeTimestamp) 
VALUES 
  (1, 7, '');
INSERT INTO Prepayment
  (ContractDocID, Sum, PercentValue, Year, ID) 
VALUES 
  (1, 300, 15, 2009, 1);
INSERT INTO Prepayment
  (ContractDocID, Sum, PercentValue, Year, ID) 
VALUES 
  (1, 300, 15, 2010, 2);
INSERT INTO Location
  (ID, Name) 
VALUES 
  (-1, '(Не задано)');
INSERT INTO Location
  (ID, Name) 
VALUES 
  (1, 'УИР');
INSERT INTO Location
  (ID, Name) 
VALUES 
  (2, 'Исполнитель');
INSERT INTO MissiveType
  (ID, Name) 
VALUES 
  (1, 'Исх.');
INSERT INTO MissiveType
  (ID, Name) 
VALUES 
  (2, 'Вх.');
INSERT INTO MissiveType
  (ID, Name) 
VALUES 
  (-1, '(не задано)');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (1, 'Исправление замечаний ФЗ');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (2, 'Исправление замечаний УИР');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (3, 'Исправление устных замечаний');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (4, 'Исправление официально переданных замечаний');
INSERT INTO ApprovalGoal
  (ID, Name) 
VALUES 
  (-1, '(не задано)');
INSERT INTO ScheduleContract
  (ContractDocID, AppNum, ScheduleID, ID) 
VALUES 
  (1, 3, 1, 1);
INSERT INTO ReportGrouping
  (ContractTypeID, ContractTypeSubgroupID, ContractorID) 
VALUES 
  (3, 2, 7);
INSERT INTO ReportGrouping
  (ContractTypeID, ContractTypeSubgroupID, ContractorID) 
VALUES 
  (3, 4, 7);
INSERT INTO ReportGrouping
  (ContractTypeID, ContractTypeSubgroupID, ContractorID) 
VALUES 
  (3, 5, 7);
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
