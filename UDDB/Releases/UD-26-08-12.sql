-- This query will display the current date

SELECT SYSDATE FROM DUAL;


CREATE TABLE YearReportColor (
  Id			 number(10) not null,
  Quarter        number(2) NOT NULL, 
  Year           number(2) NOT NULL, 
  Color          number(10), 
  CoworkersColor number(10), 
  PRIMARY KEY (Id));

COMMENT ON TABLE YearReportColor IS 'Раскраска в годовом темплане';
COMMENT ON COLUMN YearReportColor.Quarter IS 'Квартал';
COMMENT ON COLUMN YearReportColor.Year IS 'Год: 0 - текущий, -1 - предыдущий, 1 - следующий';
COMMENT ON COLUMN YearReportColor.Color IS 'Цвет закрытого этапа для исполнителя';
COMMENT ON COLUMN YearReportColor.CoworkersColor IS 'Цвет закрытого этапа для соисполнителя';


create sequence SEQ_YEARREPORTCOLOR;
