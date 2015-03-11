/*
	Скрипт для установки кодировки WIN1251. Для запуска требует входа с привилегией SYSDBA.
	Следует использовать только в случае проблем, выражающихся в появлении знаков ?? вместо строк на русском
	в строковых полях базы данных при операциях вставки и редактирования
*/

SHUTDOWN IMMEDIATE;
STARTUP MOUNT;
ALTER SYSTEM ENABLE RESTRICTED SESSION;
ALTER SYSTEM SET JOB_QUEUE_PROCESSES=0;
ALTER SYSTEM SET AQ_TM_PROCESSES=0;
ALTER DATABASE OPEN;
ALTER DATABASE CHARACTER SET INTERNAL_USE CL8MSWIN1251;
ALTER SYSTEM DISABLE RESTRICTED SESSION;
