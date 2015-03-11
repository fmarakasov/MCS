using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.ViewModel.Helpers;
using System.IO;
using Microsoft.Win32;
using System.Windows.Input;
using UIShared.Commands;
using McUIBase.ViewModel;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    public class OraExpViewModel:  RepositoryViewModel
    {

        private List<Encoding> encodings;
        public List<Encoding> Encodings
        {
            get
            {
                if (encodings == null)
                {
                    encodings = new List<Encoding>();
                    encodings.Add(Encoding.ASCII);
                    encodings.Add(Encoding.Unicode);
                    encodings.Add(Encoding.UTF8);
                    encodings.Add(Encoding.UTF32);
                }
                return encodings;
            }
        }

        private Encoding _fileencoding = Encoding.ASCII;
       public Encoding FileEncoding
       {
           get
           {
               return _fileencoding;
           }
           set
           {
               _fileencoding = value;
           }
       }
       private string _filename;

       /// <summary>
       /// в какой файл скидываем
       /// </summary>
       public string Filename
       {
         get
           {
              return _filename;
           }

         set
           {
              _filename = value;
           }
       }

       private int _querycount;
       /// <summary>
       /// общее количество запросов
       /// </summary>
       public int QueryCount
       {
           get
           {
               return _querycount;
           }
       }

       public string StringIndicator
       {
           get
           {
               return String.Format("{0} из {1}", CurrentQueryIndex, QueryCount);
           }
       }

       private int _currentqueryindex;
       public int CurrentQueryIndex
       {
           get
           {
               return _currentqueryindex;
           }
       }

       public void MakeQueries()
       {
           _currentqueryindex = 0;
           OnPropertyChanged("CurrentQueryIndex");
           _querycount = 74;
           OnPropertyChanged("QueryCount");
           OnPropertyChanged("StringIndicator");

           
           MakeQuery("select  \'insert into ACT (ID,NUM,SIGNDATE,ACTTYPEID,NDSID,REGIONID,TOTALSUM,SUMFORTRANSFER,STATUS," +
                     "ENTERPRICEAUTHORITYID,CURRENCYRATE,RATEDATE,NDSALGORITHMID,CURRENCYID,CURRENCYMEASUREID,ISSIGNED) " +
                     "values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NUM,Null,\'Null\',\'\'\'\'||NUM||\'\'\'\')||\',\'||decode(SIGNDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(SIGNDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(ACTTYPEID,Null,\'Null\',\'\'\'\'||ACTTYPEID||\'\'\'\')||\',\'||decode(NDSID,Null,\'Null\',\'\'\'\'||NDSID||\'\'\'\')||\',\'||decode(REGIONID,Null,\'Null\',\'\'\'\'||REGIONID||\'\'\'\')||\',\'||decode(TOTALSUM,Null,\'Null\',\'\'\'\'||TOTALSUM||\'\'\'\')||\',\'||decode(SUMFORTRANSFER,Null,\'Null\',\'\'\'\'||SUMFORTRANSFER||\'\'\'\')||\',\'||decode(STATUS,Null,\'Null\',\'\'\'\'||STATUS||\'\'\'\')||\',\'||decode(ENTERPRICEAUTHORITYID,Null,\'Null\',\'\'\'\'||ENTERPRICEAUTHORITYID||\'\'\'\')||\',\'||decode(CURRENCYRATE,Null,\'Null\',\'\'\'\'||CURRENCYRATE||\'\'\'\')||\',\'||decode(RATEDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(RATEDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(NDSALGORITHMID,Null,\'Null\',\'\'\'\'||NDSALGORITHMID||\'\'\'\')||\',\'||decode(CURRENCYID,Null,\'Null\',\'\'\'\'||CURRENCYID||\'\'\'\')||\',\'||decode(CURRENCYMEASUREID,Null,\'Null\',\'\'\'\'||CURRENCYMEASUREID||\'\'\'\')||\',\'||decode(ISSIGNED,Null,\'Null\',\'\'\'\'||ISSIGNED||\'\'\'\')||\');\' from ACT");


           MakeQuery("select  \'insert into ACTPAYMENTDOCUMENT (ID,ACTID,PAYMENTDOCUMENTID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(ACTID,Null,\'Null\',\'\'\'\'||ACTID||\'\'\'\')||\',\'||decode(PAYMENTDOCUMENTID,Null,\'Null\',\'\'\'\'||PAYMENTDOCUMENTID||\'\'\'\')||\');\' from ACTPAYMENTDOCUMENT");
           MakeQuery("select  \'insert into ACTTYPE (ID,CONTRACTORID,TYPENAME,ISACTIVE) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(CONTRACTORID,Null,\'Null\',\'\'\'\'||CONTRACTORID||\'\'\'\')||\',\'||decode(TYPENAME,Null,\'Null\',\'\'\'\'||TYPENAME||\'\'\'\')||\',\'||decode(ISACTIVE,Null,\'Null\',\'\'\'\'||ISACTIVE||\'\'\'\')||\');\' from ACTTYPE");
           MakeQuery("select  \'insert into APPROVALGOAL (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from APPROVALGOAL");
           MakeQuery("select  \'insert into APPROVALPROCESS (ID,CONTRACTDOCID,TOLOCATIONID,FROMLOCATIONID,APPROVALSTATEID,APPROVALGOALID,TRANSFERSTATEAT,ENTERSTATEAT,MISSIVEID,MISSIVEDATE,MISSIVETYPEID,DESCRIPTION,ENTERINGDATE) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(CONTRACTDOCID,Null,\'Null\',\'\'\'\'||CONTRACTDOCID||\'\'\'\')||\',\'||decode(TOLOCATIONID,Null,\'Null\',\'\'\'\'||TOLOCATIONID||\'\'\'\')||\',\'||decode(FROMLOCATIONID,Null,\'Null\',\'\'\'\'||FROMLOCATIONID||\'\'\'\')||\',\'||decode(APPROVALSTATEID,Null,\'Null\',\'\'\'\'||APPROVALSTATEID||\'\'\'\')||\',\'||decode(APPROVALGOALID,Null,\'Null\',\'\'\'\'||APPROVALGOALID||\'\'\'\')||\',\'||decode(TRANSFERSTATEAT,Null,\'Null\',\'to_date(\'\'\'||to_char(TRANSFERSTATEAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(ENTERSTATEAT,Null,\'Null\',\'to_date(\'\'\'||to_char(ENTERSTATEAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(MISSIVEID,Null,\'Null\',\'\'\'\'||MISSIVEID||\'\'\'\')||\',\'||decode(MISSIVEDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(MISSIVEDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(MISSIVETYPEID,Null,\'Null\',\'\'\'\'||MISSIVETYPEID||\'\'\'\')||\',\'||decode(DESCRIPTION,Null,\'Null\',\'\'\'\'||DESCRIPTION||\'\'\'\')||\',\'||decode(ENTERINGDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(ENTERINGDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\');\' from APPROVALPROCESS");
           MakeQuery("select  \'insert into APPROVALSTATE (ID,NAME, COLOR, STATEDOMAIN) values (\'" +
                     "||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'" +
                     "||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'" +
                     "||decode(COLOR,Null,\'Null\',\'\'\'\'||COLOR||\'\'\'\')||\',\'" +
                     "||decode(STATEDOMAIN,Null,\'Null\',\'\'\'\'||STATEDOMAIN||\'\'\'\')||\');\' from APPROVALSTATE");
           MakeQuery("select  \'insert into AUTHORITY (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from AUTHORITY");
           MakeQuery("select  \'insert into CLOSEDSTAGERELATION (ID,STAGEID,CLOSEDSTAGEID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(STAGEID,Null,\'Null\',\'\'\'\'||STAGEID||\'\'\'\')||\',\'||decode(CLOSEDSTAGEID,Null,\'Null\',\'\'\'\'||CLOSEDSTAGEID||\'\'\'\')||\');\' from CLOSEDSTAGERELATION");

           MakeQuery("select  \'insert into CONTRACTDOC (ID,PRICE,CONTRACTTYPEID,STARTAT,ENDSAT,APPLIEDAT,APPROVEDAT,INTERNALNUM,CONTRACTORNUM,CURRENCYID,ORIGINCONTRACTID,NDSALGORITHMID," +
                     "PREPAYMENTSUM,PREPAYMENTPERCENT,PREPAYMENTNDSALGORITHMID,NDSID,CONTRACTSTATEID,CURRENCYMEASUREID,ISPROTECTABILITY,SUBJECT,CURRENCYRATE,RATEDATE,DESCRIPTION," +
                     "CONTRACTORPERSONID,AUTHORITYID,PREPAYMENTPRECENTTYPE,DELETED,DEPARTMENTID,AGREEMENTNUM, OUTOFCONTROLAT, BROKEAT, REALLYFINISHEDAT, " +
                     "ISSUBGENERAL, ISGENERAL, DELTA, DELTACOMMENT, ACTTYPEID) " +
                     "values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(PRICE,Null,\'Null\',\'\'\'\'||PRICE||\'\'\'\')||\',\'" +
                     "||decode(CONTRACTTYPEID,Null,\'Null\',\'\'\'\'||CONTRACTTYPEID||\'\'\'\')||\',\'||decode(STARTAT,Null,\'Null\',\'to_date(\'\'\'||to_char(STARTAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(ENDSAT,Null,\'Null\',\'to_date(\'\'\'||to_char(ENDSAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(APPLIEDAT,Null,\'Null\',\'to_date(\'\'\'||to_char(APPLIEDAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(APPROVEDAT,Null,\'Null\',\'to_date(\'\'\'||to_char(APPROVEDAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(INTERNALNUM,Null,\'Null\',\'\'\'\'||INTERNALNUM||\'\'\'\')||\',\'||decode(CONTRACTORNUM,Null,\'Null\',\'\'\'\'||CONTRACTORNUM||\'\'\'\')||\',\'" +
                     "||decode(CURRENCYID,Null,\'Null\',\'\'\'\'||CURRENCYID||\'\'\'\')||\',\'||decode(ORIGINCONTRACTID,Null,\'Null\',\'\'\'\'||ORIGINCONTRACTID||\'\'\'\')||\',\'" +
                     "||decode(NDSALGORITHMID,Null,\'Null\',\'\'\'\'||NDSALGORITHMID||\'\'\'\')||\',\'||decode(PREPAYMENTSUM,Null,\'Null\',\'\'\'\'||PREPAYMENTSUM||\'\'\'\')||\',\'" +
                     "||decode(PREPAYMENTPERCENT,Null,\'Null\',PREPAYMENTPERCENT)||\',\'||decode(PREPAYMENTNDSALGORITHMID,Null,\'Null\',\'\'\'\'||PREPAYMENTNDSALGORITHMID||\'\'\'\')||\',\'" +
                     "||decode(NDSID,Null,\'Null\',\'\'\'\'||NDSID||\'\'\'\')||\',\'||decode(CONTRACTSTATEID,Null,\'Null\',\'\'\'\'||CONTRACTSTATEID||\'\'\'\')||\',\'" +
                     "||decode(CURRENCYMEASUREID,Null,\'Null\',\'\'\'\'||CURRENCYMEASUREID||\'\'\'\')||\',\'" +
                     "||decode(ISPROTECTABILITY,Null,\'Null\',\'\'\'\'||ISPROTECTABILITY||\'\'\'\')||\',\'||decode(SUBJECT,Null,\'Null\',\'\'\'\'||SUBJECT||\'\'\'\')||\',\'" +
                     "||decode(CURRENCYRATE,Null,\'Null\',\'\'\'\'||CURRENCYRATE||\'\'\'\')||\',\'||decode(RATEDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(RATEDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(DESCRIPTION,Null,\'Null\',\'\'\'\'||DESCRIPTION||\'\'\'\')||\',\'||decode(CONTRACTORPERSONID,Null,\'Null\',\'\'\'\'||CONTRACTORPERSONID||\'\'\'\')||\',\'" +
                     "||decode(AUTHORITYID,Null,\'Null\',\'\'\'\'||AUTHORITYID||\'\'\'\')||\',\'||decode(PREPAYMENTPRECENTTYPE,Null,\'Null\',\'\'\'\'||PREPAYMENTPRECENTTYPE||\'\'\'\')||\',\'" +
                     "||decode(DELETED,Null,\'Null\',\'\'\'\'||DELETED||\'\'\'\')||\',\'||decode(DEPARTMENTID,Null,\'Null\',\'\'\'\'||DEPARTMENTID||\'\'\'\')||\',\'" +
                     "||decode(AGREEMENTNUM,Null,\'Null\',\'\'\'\'||AGREEMENTNUM||\'\'\'\')||\',\'||decode(OUTOFCONTROLAT,Null,\'Null\',\'to_date(\'\'\'||to_char(OUTOFCONTROLAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(BROKEAT,Null,\'Null\',\'to_date(\'\'\'||to_char(BROKEAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(REALLYFINISHEDAT,Null,\'Null\',\'to_date(\'\'\'||to_char(REALLYFINISHEDAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(ISSUBGENERAL,Null,\'Null\',\'\'\'\'||ISSUBGENERAL||\'\'\'\')||\',\'||decode(ISGENERAL,Null,\'Null\',\'\'\'\'||ISGENERAL||\'\'\'\')||\',\'" +
                     "||decode(DELTA,Null,\'Null\',\'\'\'\'||DELTA||\'\'\'\')||\',\'||decode(DELTACOMMENT,Null,\'Null\',\'\'\'\'||DELTACOMMENT||\'\'\'\')||\',\'" +
                     "||decode(ACTTYPEID,Null,\'Null\',\'\'\'\'||ACTTYPEID||\'\'\'\')||\');\' from CONTRACTDOC");


           MakeQuery("select  \'insert into CONTRACTDOC_FUNDS_FACT (CONTRACTDOCID,TIME_DIMID,FUNDSDISBURSED,FUNDSLEFT,FUNDSTOTAL) values (\'||decode(CONTRACTDOCID,Null,\'Null\',\'\'\'\'||CONTRACTDOCID||\'\'\'\')||\',\'||decode(TIME_DIMID,Null,\'Null\',\'\'\'\'||TIME_DIMID||\'\'\'\')||\',\'||decode(FUNDSDISBURSED,Null,\'Null\',\'\'\'\'||FUNDSDISBURSED||\'\'\'\')||\',\'||decode(FUNDSLEFT,Null,\'Null\',\'\'\'\'||FUNDSLEFT||\'\'\'\')||\',\'||decode(FUNDSTOTAL,Null,\'Null\',\'\'\'\'||FUNDSTOTAL||\'\'\'\')||\');\' from CONTRACTDOC_FUNDS_FACT");
           MakeQuery("select  \'insert into CONTRACTHIERARCHY (ID,GENERALCONTRACTDOCID,SUBCONTRACTDOCID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(GENERALCONTRACTDOCID,Null,\'Null\',\'\'\'\'||GENERALCONTRACTDOCID||\'\'\'\')||\',\'||decode(SUBCONTRACTDOCID,Null,\'Null\',\'\'\'\'||SUBCONTRACTDOCID||\'\'\'\')||\');\' from CONTRACTHIERARCHY");
           

           MakeQuery("select  \'insert into CONTRACTOR (ID,NAME,SHORTNAME,ZIP,BANK,CONTRACTORTYPEID,INN,ACCOUNT,BIK,KPP,ADDRESS,CORRESPACCOUNT,OKPO,OKONH,OGRN,OKATO,OKVED, " +
                     "INSURANCE, BIRTHPLACE, MIDDLENAME, FIRSTNAME, BIRTHDATE, PASPORTNUMBER, PASPORTSERIES, PASPORTAUTHORITY, FAMILYNAME, PASPORTDATE, EDUCATIONID, SEX) " +
                     "values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'" +
                     "||decode(SHORTNAME,Null,\'Null\',\'\'\'\'||SHORTNAME||\'\'\'\')||\',\'||decode(ZIP,Null,\'Null\',\'\'\'\'||ZIP||\'\'\'\')||\',\'" +
                     "||decode(BANK,Null,\'Null\',\'\'\'\'||BANK||\'\'\'\')||\',\'||decode(CONTRACTORTYPEID,Null,\'Null\',\'\'\'\'||CONTRACTORTYPEID||\'\'\'\')||\',\'" +
                     "||decode(INN,Null,\'Null\',\'\'\'\'||INN||\'\'\'\')||\',\'||decode(ACCOUNT,Null,\'Null\',\'\'\'\'||ACCOUNT||\'\'\'\')||\',\'" +
                     "||decode(BIK,Null,\'Null\',\'\'\'\'||BIK||\'\'\'\')||\',\'||decode(KPP,Null,\'Null\',\'\'\'\'||KPP||\'\'\'\')||\',\'" +
                     "||decode(ADDRESS,Null,\'Null\',\'\'\'\'||ADDRESS||\'\'\'\')||\',\'||decode(CORRESPACCOUNT,Null,\'Null\',\'\'\'\'||CORRESPACCOUNT||\'\'\'\')||\',\'" +
                     "||decode(OKPO,Null,\'Null\',\'\'\'\'||OKPO||\'\'\'\')||\',\'||decode(OKONH,Null,\'Null\',\'\'\'\'||OKONH||\'\'\'\')||\',\'" +
                     "||decode(OGRN,Null,\'Null\',\'\'\'\'||OGRN||\'\'\'\')||\',\'||decode(OKATO,Null,\'Null\',\'\'\'\'||OKATO||\'\'\'\')||\',\'" +
                     "||decode(OKVED,Null,\'Null\',\'\'\'\'||OKVED||\'\'\'\')||\',\'" +
                     "||decode(INSURANCE,Null,\'Null\',\'\'\'\'||INSURANCE||\'\'\'\')||\',\'" +
                     "||decode(BIRTHPLACE,Null,\'Null\',\'\'\'\'||BIRTHPLACE||\'\'\'\')||\',\'" +
                     "||decode(MIDDLENAME,Null,\'Null\',\'\'\'\'||MIDDLENAME||\'\'\'\')||\',\'" +
                     "||decode(FIRSTNAME,Null,\'Null\',\'\'\'\'||FIRSTNAME||\'\'\'\')||\',\'" +
                     "||decode(BIRTHDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(BIRTHDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(PASPORTNUMBER,Null,\'Null\',\'\'\'\'||PASPORTNUMBER||\'\'\'\')||\',\'" +
                     "||decode(PASPORTSERIES,Null,\'Null\',\'\'\'\'||PASPORTSERIES||\'\'\'\')||\',\'" +
                     "||decode(PASPORTAUTHORITY,Null,\'Null\',\'\'\'\'||PASPORTAUTHORITY||\'\'\'\')||\',\'" +
                     "||decode(FAMILYNAME,Null,\'Null\',\'\'\'\'||FAMILYNAME||\'\'\'\')||\',\'" +
                     "||decode(PASPORTDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(PASPORTDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(EDUCATIONID,Null,\'Null\',\'\'\'\'||EDUCATIONID||\'\'\'\')||\',\'" +
                     "||decode(SEX,Null,\'Null\',\'\'\'\'||SEX||\'\'\'\')" +                     
                     "||\');\' from CONTRACTOR");

           MakeQuery("select  \'insert into CONTRACTORAUTHORITY (ID,AUTHORITYID,NUMDOCUMENT,VALIDFROM,VALIDTO,ISVALID,CONTRACTORID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(AUTHORITYID,Null,\'Null\',\'\'\'\'||AUTHORITYID||\'\'\'\')||\',\'||decode(NUMDOCUMENT,Null,\'Null\',\'\'\'\'||NUMDOCUMENT||\'\'\'\')||\',\'||decode(VALIDFROM,Null,\'Null\',\'to_date(\'\'\'||to_char(VALIDFROM,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(VALIDTO,Null,\'Null\',\'to_date(\'\'\'||to_char(VALIDTO,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(ISVALID,Null,\'Null\',\'\'\'\'||ISVALID||\'\'\'\')||\',\'||decode(CONTRACTORID,Null,\'Null\',\'\'\'\'||CONTRACTORID||\'\'\'\')||\');\' from CONTRACTORAUTHORITY");

           MakeQuery("select \'insert into CONTRACTORCONTRACTDOC(ID, CONTRACTORID, CONTRACTDOCID) " +
                     "values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'" +
                     "||decode(CONTRACTORID,Null,\'Null\',\'\'\'\'||CONTRACTORID||\'\'\'\')||\',\'" +
                     "||decode(CONTRACTDOCID,Null,\'Null\',\'\'\'\'||CONTRACTDOCID||\'\'\'\')||\');\' from CONTRACTORCONTRACTDOC");


           MakeQuery("select  \'insert into CONTRACTORPOSITION (ID,CONTRACTORID,POSITIONID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(CONTRACTORID,Null,\'Null\',\'\'\'\'||CONTRACTORID||\'\'\'\')||\',\'||decode(POSITIONID,Null,\'Null\',\'\'\'\'||POSITIONID||\'\'\'\')||\');\' from CONTRACTORPOSITION");
           MakeQuery("select  \'insert into CONTRACTORPROPERTIY (ID,PROPERTYID,CONTRACTORID,VALUE) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(PROPERTYID,Null,\'Null\',\'\'\'\'||PROPERTYID||\'\'\'\')||\',\'||decode(CONTRACTORID,Null,\'Null\',\'\'\'\'||CONTRACTORID||\'\'\'\')||\',\'||decode(VALUE,Null,\'Null\',\'\'\'\'||VALUE||\'\'\'\')||\');\' from CONTRACTORPROPERTIY");
           MakeQuery("select  \'insert into CONTRACTORTYPE (ID,NAME,REPORTORDER) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(REPORTORDER,Null,\'Null\',\'\'\'\'||REPORTORDER||\'\'\'\')||\');\' from CONTRACTORTYPE");
           MakeQuery("select  \'insert into CONTRACTPAYMENT (ID,PAYMENTDOCUMENTID,CONTRACTDOCID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(PAYMENTDOCUMENTID,Null,\'Null\',\'\'\'\'||PAYMENTDOCUMENTID||\'\'\'\')||\',\'||decode(CONTRACTDOCID,Null,\'Null\',\'\'\'\'||CONTRACTDOCID||\'\'\'\')||\');\' from CONTRACTPAYMENT");
           MakeQuery("select  \'insert into CONTRACTSTATE (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from CONTRACTSTATE");
           MakeQuery("select  \'insert into CONTRACTTRANACTDOC (ID,CONTRACTDOCID,TRANSFERACTID,DOCUMENTID,PAGESCOUNT) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(CONTRACTDOCID,Null,\'Null\',\'\'\'\'||CONTRACTDOCID||\'\'\'\')||\',\'||decode(TRANSFERACTID,Null,\'Null\',\'\'\'\'||TRANSFERACTID||\'\'\'\')||\',\'||decode(DOCUMENTID,Null,\'Null\',\'\'\'\'||DOCUMENTID||\'\'\'\')||\',\'||decode(PAGESCOUNT,Null,\'Null\',\'\'\'\'||PAGESCOUNT||\'\'\'\')||\');\' from CONTRACTTRANACTDOC");
           MakeQuery("select  \'insert into CONTRACTTROUBLE (ID,TROUBLEID,CONTRACTDOCID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(TROUBLEID,Null,\'Null\',\'\'\'\'||TROUBLEID||\'\'\'\')||\',\'||decode(CONTRACTDOCID,Null,\'Null\',\'\'\'\'||CONTRACTDOCID||\'\'\'\')||\');\' from CONTRACTTROUBLE");
           MakeQuery("select  \'insert into CONTRACTTYPE (ID,NAME,REPORTORDER) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(REPORTORDER,Null,\'Null\',\'\'\'\'||REPORTORDER||\'\'\'\')||\');\' from CONTRACTTYPE");
           MakeQuery("select  \'insert into CURRENCY (ID,NAME,CULTURE,CURRENCYI,CURRENCYR,CURRENCYM,SMALLI,SMALLR,SMALLM,HIGHSMALLNAME,LOWSMALLNAME,CODE) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(CULTURE,Null,\'Null\',\'\'\'\'||CULTURE||\'\'\'\')||\',\'||decode(CURRENCYI,Null,\'Null\',\'\'\'\'||CURRENCYI||\'\'\'\')||\',\'||decode(CURRENCYR,Null,\'Null\',\'\'\'\'||CURRENCYR||\'\'\'\')||\',\'||decode(CURRENCYM,Null,\'Null\',\'\'\'\'||CURRENCYM||\'\'\'\')||\',\'||decode(SMALLI,Null,\'Null\',\'\'\'\'||SMALLI||\'\'\'\')||\',\'||decode(SMALLR,Null,\'Null\',\'\'\'\'||SMALLR||\'\'\'\')||\',\'||decode(SMALLM,Null,\'Null\',\'\'\'\'||SMALLM||\'\'\'\')||\',\'||decode(HIGHSMALLNAME,Null,\'Null\',\'\'\'\'||HIGHSMALLNAME||\'\'\'\')||\',\'||decode(LOWSMALLNAME,Null,\'Null\',\'\'\'\'||LOWSMALLNAME||\'\'\'\')||\',\'||decode(CODE,Null,\'Null\',\'\'\'\'||CODE||\'\'\'\')||\');\' from CURRENCY");
           MakeQuery("select  \'insert into CURRENCYMEASURE (ID,NAME,FACTOR) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(FACTOR,Null,\'Null\',\'\'\'\'||FACTOR||\'\'\'\')||\');\' from CURRENCYMEASURE");
           MakeQuery("select  \'insert into DEGREE (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from DEGREE");
           MakeQuery("select  \'insert into DEPARTMENT (ID,PARENTID,MANAGERID,DIRECTEDBYID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(PARENTID,Null,\'Null\',\'\'\'\'||PARENTID||\'\'\'\')||\',\'||decode(MANAGERID,Null,\'Null\',\'\'\'\'||MANAGERID||\'\'\'\')||\',\'||decode(DIRECTEDBYID,Null,\'Null\',\'\'\'\'||DIRECTEDBYID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from DEPARTMENT");
           MakeQuery("select  \'insert into DISPOSAL (ID,NUM,APPROVEDDATE) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NUM,Null,\'Null\',\'\'\'\'||NUM||\'\'\'\')||\',\'||decode(APPROVEDDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(APPROVEDDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\');\' from DISPOSAL");
           MakeQuery("select  \'insert into DOCUMENT (ID,NAME, DOMAIN) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(DOMAIN,Null,\'Null\',\'\'\'\'||DOMAIN||\'\'\'\')||\');\' from DOCUMENT");
           MakeQuery("select  \'insert into ECONOMEFFICIENCYPARAMETER (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from ECONOMEFFICIENCYPARAMETER");
           MakeQuery("select  \'insert into ECONOMEFFICIENCYTYPE (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from ECONOMEFFICIENCYTYPE");
           MakeQuery("select  \'insert into EDUCATION (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from EDUCATION");
           MakeQuery("select  \'insert into EFFICIENCEPARAMETERTYPE (ID,ECONOMEFFICIENCYPARAMETERID,ECONOMEFFICIENCYTYPEID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(ECONOMEFFICIENCYPARAMETERID,Null,\'Null\',\'\'\'\'||ECONOMEFFICIENCYPARAMETERID||\'\'\'\')||\',\'||decode(ECONOMEFFICIENCYTYPEID,Null,\'Null\',\'\'\'\'||ECONOMEFFICIENCYTYPEID||\'\'\'\')||\');\' from EFFICIENCEPARAMETERTYPE");
           MakeQuery("select  \'insert into EFPARAMETERSTAGERESULT (ID,ECONOMEFFICIENCYPARAMETERID,STAGERESULTID,VALUE) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(ECONOMEFFICIENCYPARAMETERID,Null,\'Null\',\'\'\'\'||ECONOMEFFICIENCYPARAMETERID||\'\'\'\')||\',\'||decode(STAGERESULTID,Null,\'Null\',\'\'\'\'||STAGERESULTID||\'\'\'\')||\',\'||decode(VALUE,Null,\'Null\',\'\'\'\'||VALUE||\'\'\'\')||\');\' from EFPARAMETERSTAGERESULT");
           MakeQuery("select  \'insert into EMPLOYEE (ID,FAMILYNAME,FIRSTNAME,MIDDLENAME,SEX,POSTID,DEPARTMENTID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(FAMILYNAME,Null,\'Null\',\'\'\'\'||FAMILYNAME||\'\'\'\')||\',\'||decode(FIRSTNAME,Null,\'Null\',\'\'\'\'||FIRSTNAME||\'\'\'\')||\',\'||decode(MIDDLENAME,Null,\'Null\',\'\'\'\'||MIDDLENAME||\'\'\'\')||\',\'||decode(SEX,Null,\'Null\',\'\'\'\'||SEX||\'\'\'\')||\',\'||decode(POSTID,Null,\'Null\',\'\'\'\'||POSTID||\'\'\'\')||\',\'||decode(DEPARTMENTID,Null,\'Null\',\'\'\'\'||DEPARTMENTID||\'\'\'\')||\');\' from EMPLOYEE");
           MakeQuery("select  \'insert into ENTERPRISEAUTHORITY (ID,NUM,VALIDFROM,VALIDTO,ISVALID,EMPLOYEEID,AUTHORITYID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NUM,Null,\'Null\',\'\'\'\'||NUM||\'\'\'\')||\',\'||decode(VALIDFROM,Null,\'Null\',\'to_date(\'\'\'||to_char(VALIDFROM,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(VALIDTO,Null,\'Null\',\'to_date(\'\'\'||to_char(VALIDTO,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(ISVALID,Null,\'Null\',\'\'\'\'||ISVALID||\'\'\'\')||\',\'||decode(EMPLOYEEID,Null,\'Null\',\'\'\'\'||EMPLOYEEID||\'\'\'\')||\',\'||decode(AUTHORITYID,Null,\'Null\',\'\'\'\'||AUTHORITYID||\'\'\'\')||\');\' from ENTERPRISEAUTHORITY");
           MakeQuery("select  \'insert into FUNCCUSTOMERPERSON (ID,FUNCCUSTOMERID,PERSONID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(FUNCCUSTOMERID,Null,\'Null\',\'\'\'\'||FUNCCUSTOMERID||\'\'\'\')||\',\'||decode(PERSONID,Null,\'Null\',\'\'\'\'||PERSONID||\'\'\'\')||\');\' from FUNCCUSTOMERPERSON");
           MakeQuery("select  \'insert into FUNCTIONALCUSTOMER (ID,NAME,CONTRACTORID,PARENTFUNCTIONALCUSTOMERID,FUNCTIONALCUSTOMERTYPEID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(CONTRACTORID,Null,\'Null\',\'\'\'\'||CONTRACTORID||\'\'\'\')||\',\'||decode(PARENTFUNCTIONALCUSTOMERID,Null,\'Null\',\'\'\'\'||PARENTFUNCTIONALCUSTOMERID||\'\'\'\')||\',\'||decode(FUNCTIONALCUSTOMERTYPEID,Null,\'Null\',\'\'\'\'||FUNCTIONALCUSTOMERTYPEID||\'\'\'\')||\');\' from FUNCTIONALCUSTOMER");
           MakeQuery("select  \'insert into FUNCTIONALCUSTOMERCONTRACT (ID,FUNCTIONALCUSTOMERID,CONTRACTDOCID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(FUNCTIONALCUSTOMERID,Null,\'Null\',\'\'\'\'||FUNCTIONALCUSTOMERID||\'\'\'\')||\',\'||decode(CONTRACTDOCID,Null,\'Null\',\'\'\'\'||CONTRACTDOCID||\'\'\'\')||\');\' from FUNCTIONALCUSTOMERCONTRACT");
           MakeQuery("select  \'insert into FUNCTIONALCUSTOMERTYPE (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from FUNCTIONALCUSTOMERTYPE");
           MakeQuery("select  \'insert into IMPORTINGSCHEME (ID,SCHEMENAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(SCHEMENAME,Null,\'Null\',\'\'\'\'||SCHEMENAME||\'\'\'\')||\');\' from IMPORTINGSCHEME");
           MakeQuery("select  \'insert into IMPORTINGSCHEMEITEM (ID,COLUMNNAME,COLUMNSIGN,COLUMNINDEX,ISREQUIRED,SCHEMEID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(COLUMNNAME,Null,\'Null\',\'\'\'\'||COLUMNNAME||\'\'\'\')||\',\'||decode(COLUMNSIGN,Null,\'Null\',\'\'\'\'||COLUMNSIGN||\'\'\'\')||\',\'||decode(COLUMNINDEX,Null,\'Null\',\'\'\'\'||COLUMNINDEX||\'\'\'\')||\',\'||decode(ISREQUIRED,Null,\'Null\',\'\'\'\'||ISREQUIRED||\'\'\'\')||\',\'||decode(SCHEMEID,Null,\'Null\',\'\'\'\'||SCHEMEID||\'\'\'\')||\');\' from IMPORTINGSCHEMEITEM");
           MakeQuery("select  \'insert into LOCATION (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from LOCATION");
           MakeQuery("select  \'insert into MISSIVETYPE (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from MISSIVETYPE");
           MakeQuery("select  \'insert into NDS (ID,PERCENTS,YEAR) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(PERCENTS,Null,\'Null\',\'\'\'\'||PERCENTS||\'\'\'\')||\',\'||decode(YEAR,Null,\'Null\',\'\'\'\'||YEAR||\'\'\'\')||\');\' from NDS");
           MakeQuery("select  \'insert into NDSALGORITHM (ID,NAME,PRICETOOLTIP) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(PRICETOOLTIP,Null,\'Null\',\'\'\'\'||PRICETOOLTIP||\'\'\'\')||\');\' from NDSALGORITHM");
           MakeQuery("select  \'insert into NTPSUBVIEW (ID,NAME,NTPVIEWID,SHORTNAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(NTPVIEWID,Null,\'Null\',\'\'\'\'||NTPVIEWID||\'\'\'\')||\',\'||decode(SHORTNAME,Null,\'Null\',\'\'\'\'||SHORTNAME||\'\'\'\')||\');\' from NTPSUBVIEW");
           MakeQuery("select  \'insert into NTPVIEW (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from NTPVIEW");
           MakeQuery("select  \'insert into PAYMENTDOCUMENT (ID,NUM,PAYMENTDATE,CURRENCYMEASUREID,PREPAYMENTDOCUMENTTYPEID,PAYMENTSUM) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NUM,Null,\'Null\',\'\'\'\'||NUM||\'\'\'\')||\',\'||decode(PAYMENTDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(PAYMENTDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(CURRENCYMEASUREID,Null,\'Null\',\'\'\'\'||CURRENCYMEASUREID||\'\'\'\')||\',\'||decode(PREPAYMENTDOCUMENTTYPEID,Null,\'Null\',\'\'\'\'||PREPAYMENTDOCUMENTTYPEID||\'\'\'\')||\',\'||decode(PAYMENTSUM,Null,\'Null\',\'\'\'\'||PAYMENTSUM||\'\'\'\')||\');\' from PAYMENTDOCUMENT");
           MakeQuery("select  \'insert into PERSON (ID,DEGREEID,ISCONTRACTHEADAUTHORITY,ISACTSIGNAUTHORITY,ISVALID,FAMILYNAME,FIRSTNAME,MIDDLENAME,SEX,CONTRACTORPOSITIONID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(DEGREEID,Null,\'Null\',\'\'\'\'||DEGREEID||\'\'\'\')||\',\'||decode(ISCONTRACTHEADAUTHORITY,Null,\'Null\',\'\'\'\'||ISCONTRACTHEADAUTHORITY||\'\'\'\')||\',\'||decode(ISACTSIGNAUTHORITY,Null,\'Null\',\'\'\'\'||ISACTSIGNAUTHORITY||\'\'\'\')||\',\'||decode(ISVALID,Null,\'Null\',\'\'\'\'||ISVALID||\'\'\'\')||\',\'||decode(FAMILYNAME,Null,\'Null\',\'\'\'\'||FAMILYNAME||\'\'\'\')||\',\'||decode(FIRSTNAME,Null,\'Null\',\'\'\'\'||FIRSTNAME||\'\'\'\')||\',\'||decode(MIDDLENAME,Null,\'Null\',\'\'\'\'||MIDDLENAME||\'\'\'\')||\',\'||decode(SEX,Null,\'Null\',\'\'\'\'||SEX||\'\'\'\')||\',\'||decode(CONTRACTORPOSITIONID,Null,\'Null\',\'\'\'\'||CONTRACTORPOSITIONID||\'\'\'\')||\');\' from PERSON");
           MakeQuery("select  \'insert into POSITION (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from POSITION");
           MakeQuery("select  \'insert into POST (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from POST");
           MakeQuery("select  \'insert into PREPAYMENT (ID,CONTRACTDOCID,SUM,PercentValue,YEAR) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(CONTRACTDOCID,Null,\'Null\',\'\'\'\'||CONTRACTDOCID||\'\'\'\')||\',\'||decode(SUM,Null,\'Null\',\'\'\'\'||SUM||\'\'\'\')||\',\'||decode(PercentValue,Null,\'Null\',PercentValue)||\',\'||decode(YEAR,Null,\'Null\',\'\'\'\'||YEAR||\'\'\'\')||\');\' from PREPAYMENT");
           MakeQuery("select  \'insert into PREPAYMENTDOCUMENTTYPE (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from PREPAYMENTDOCUMENTTYPE");
           MakeQuery("select  \'insert into PROPERTY (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from PROPERTY");
           MakeQuery("select  \'insert into REGION (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from REGION");
           MakeQuery("select  \'insert into RESPONSIBLE (ID,DISPOSALID,EMPLOYEEID,ROLEID,CONTRACTDOCID, STAGEID) values " +
                     "(\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'" +
                     "||decode(DISPOSALID,Null,\'Null\',\'\'\'\'||DISPOSALID||\'\'\'\')||\',\'" +
                     "||decode(EMPLOYEEID,Null,\'Null\',\'\'\'\'||EMPLOYEEID||\'\'\'\')||\',\'" +
                     "||decode(ROLEID,Null,\'Null\',\'\'\'\'||ROLEID||\'\'\'\')||\',\'" +
                     "||decode(CONTRACTDOCID,Null,\'Null\',\'\'\'\'||CONTRACTDOCID||\'\'\'\')||\',\'" +
                     "||decode(STAGEID,Null,\'Null\',\'\'\'\'||STAGEID||\'\'\'\')||\');\' from RESPONSIBLE");

           MakeQuery("select  \'insert into RESPONSIBLEASSIGNMENTORDER (ID,ORDERNUM,ORDERDATE) values " +
                     "(\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'" +
                     "||decode(ORDERNUM,Null,\'Null\',\'\'\'\'||ORDERNUM||\'\'\'\')||\',\'" +
                     "||decode(ORDERDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(ORDERDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\');\' from RESPONSIBLEASSIGNMENTORDER");


           MakeQuery("select  \'insert into RESPONSIBLEFORORDER (ID,DEPARTMENTID,EMPLOYEEID, RESPONSIBLEASSIGNMENTORDERID, CONTRACTTYPEID) " +
                     "values (\'" +
                     "||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'" +
                     "||decode(DEPARTMENTID,Null,\'Null\',\'\'\'\'||DEPARTMENTID||\'\'\'\')||\',\'" +
                     "||decode(EMPLOYEEID,Null,\'Null\',\'\'\'\'||EMPLOYEEID||\'\'\'\')||\',\'" +
                     "||decode(RESPONSIBLEASSIGNMENTORDERID,Null,\'Null\',\'\'\'\'||RESPONSIBLEASSIGNMENTORDERID||\'\'\'\')||\',\'" +
                     "||decode(CONTRACTTYPEID,Null,\'Null\',\'\'\'\'||CONTRACTTYPEID||\'\'\'\')||\');\' from RESPONSIBLEFORORDER");


           MakeQuery("select  \'insert into ROLE (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from ROLE");
           MakeQuery("select  \'insert into SCHEDULE (ID,CURRENCYMEASUREID,WORKTYPEID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(CURRENCYMEASUREID,Null,\'Null\',\'\'\'\'||CURRENCYMEASUREID||\'\'\'\')||\',\'||decode(WORKTYPEID,Null,\'Null\',\'\'\'\'||WORKTYPEID||\'\'\'\')||\');\' from SCHEDULE");
           MakeQuery("select  \'insert into SCHEDULECONTRACT (ID,CONTRACTDOCID,APPNUM,SCHEDULEID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(CONTRACTDOCID,Null,\'Null\',\'\'\'\'||CONTRACTDOCID||\'\'\'\')||\',\'||decode(APPNUM,Null,\'Null\',\'\'\'\'||APPNUM||\'\'\'\')||\',\'||decode(SCHEDULEID,Null,\'Null\',\'\'\'\'||SCHEDULEID||\'\'\'\')||\');\' from SCHEDULECONTRACT");
           MakeQuery("select  \'insert into SIGHTFUNCPERSON (ID,NAME,SIGHTFUNCPERSONSCHID,ACTID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(SIGHTFUNCPERSONSCHID,Null,\'Null\',\'\'\'\'||SIGHTFUNCPERSONSCHID||\'\'\'\')||\',\'||decode(ACTID,Null,\'Null\',\'\'\'\'||ACTID||\'\'\'\')||\');\' from SIGHTFUNCPERSON");
           MakeQuery("select  \'insert into SIGHTFUNCPERSONSCHEME (ID,NUM,ISACTIVE,FUNCPERSONID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NUM,Null,\'Null\',\'\'\'\'||NUM||\'\'\'\')||\',\'||decode(ISACTIVE,Null,\'Null\',\'\'\'\'||ISACTIVE||\'\'\'\')||\',\'||decode(FUNCPERSONID,Null,\'Null\',\'\'\'\'||FUNCPERSONID||\'\'\'\')||\');\' from SIGHTFUNCPERSONSCHEME");
           
           MakeQuery("select  \'insert into STAGE (ID,NUM,SUBJECT,STARTSAT,ENDSAT,PRICE,NDSALGORITHMID,PARENTID,ACTID,SCHEDULEID,CODE,NDSID, DELTA, APPROVALSTATEID, STATEDATE, STATEDESCRIPTION, DELTACOMMENT) " +
                     "values (\'" +
                     "||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'" +
                     "||decode(NUM,Null,\'Null\',\'\'\'\'||NUM||\'\'\'\')||\',\'" +
                     "||decode(SUBJECT,Null,\'Null\',\'\'\'\'||SUBJECT||\'\'\'\')||\',\'" +
                     "||decode(STARTSAT,Null,\'Null\',\'to_date(\'\'\'||to_char(STARTSAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(ENDSAT,Null,\'Null\',\'to_date(\'\'\'||to_char(ENDSAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(PRICE,Null,\'Null\',\'\'\'\'||PRICE||\'\'\'\')||\',\'" +
                     "||decode(NDSALGORITHMID,Null,\'Null\',\'\'\'\'||NDSALGORITHMID||\'\'\'\')||\',\'" +
                     "||decode(PARENTID,Null,\'Null\',\'\'\'\'||PARENTID||\'\'\'\')||\',\'" +
                     "||decode(ACTID,Null,\'Null\',\'\'\'\'||ACTID||\'\'\'\')||\',\'" +
                     "||decode(SCHEDULEID,Null,\'Null\',\'\'\'\'||SCHEDULEID||\'\'\'\')||\',\'" +
                     "||decode(CODE,Null,\'Null\',\'\'\'\'||CODE||\'\'\'\')||\',\'" +
                     "||decode(NDSID,Null,\'Null\',\'\'\'\'||NDSID||\'\'\'\')||\',\'" +
                     "||decode(DELTA,Null,\'Null\',\'\'\'\'||DELTA||\'\'\'\')||\',\'" +
                     "||decode(APPROVALSTATEID,Null,\'Null\',\'\'\'\'||APPROVALSTATEID||\'\'\'\')||\',\'" +
                     "||decode(STATEDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(STATEDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(STATEDESCRIPTION,Null,\'Null\',\'\'\'\'||STATEDESCRIPTION||\'\'\'\')||\',\'" +
                     "||decode(DELTACOMMENT,Null,\'Null\',\'\'\'\'||DELTACOMMENT||\'\'\'\')||\');\' from STAGE");

           MakeQuery("select  \'insert into STAGERESULT (ID,NAME,ECONOMICEFFICIENCYTYPEID,STAGEID,NTPSUBVIEWID, STATEDATE, STATEDESCRIPTION, RESULTDATE, DESCRIPTION, ACTINTRODUCTIONNUM, APPROVALSTATEID) " +
                     "values (\'" +
                     "||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'" +
                     "||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'" +
                     "||decode(ECONOMICEFFICIENCYTYPEID,Null,\'Null\',\'\'\'\'||ECONOMICEFFICIENCYTYPEID||\'\'\'\')||\',\'" +
                     "||decode(STAGEID,Null,\'Null\',\'\'\'\'||STAGEID||\'\'\'\')||\',\'" +
                     "||decode(NTPSUBVIEWID,Null,\'Null\',\'\'\'\'||NTPSUBVIEWID||\'\'\'\')||\',\'" +
                     "||decode(STATEDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(STATEDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(STATEDESCRIPTION,Null,\'Null\',\'\'\'\'||STATEDESCRIPTION||\'\'\'\')||\',\'" +
                     "||decode(RESULTDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(RESULTDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'" +
                     "||decode(DESCRIPTION,Null,\'Null\',\'\'\'\'||DESCRIPTION||\'\'\'\')||\',\'" +
                     "||decode(ACTINTRODUCTIONNUM,Null,\'Null\',\'\'\'\'||ACTINTRODUCTIONNUM||\'\'\'\')||\',\'" +
                     "||decode(APPROVALSTATEID,Null,\'Null\',\'\'\'\'||APPROVALSTATEID||\'\'\'\')||\');\' from STAGERESULT");

           MakeQuery("select  \'insert into SUBGENERALHIERARCHI (ID,GENERALCONTRACTDOCSTAGEID,SUBCONTRACTDOCSTAGEID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(GENERALCONTRACTDOCSTAGEID,Null,\'Null\',\'\'\'\'||GENERALCONTRACTDOCSTAGEID||\'\'\'\')||\',\'||decode(SUBCONTRACTDOCSTAGEID,Null,\'Null\',\'\'\'\'||SUBCONTRACTDOCSTAGEID||\'\'\'\')||\');\' from SUBGENERALHIERARCHI");
           MakeQuery("select \'insert into TIME_DIM (ID,YEAR) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(YEAR,Null,\'Null\',\'\'\'\'||YEAR||\'\'\'\')||\');\' from TIME_DIM");
           MakeQuery("select  \'insert into TRANSFERACT (ID,NUM,SIGNDATE,TRANSFERACTTYPEID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NUM,Null,\'Null\',\'\'\'\'||NUM||\'\'\'\')||\',\'||decode(SIGNDATE,Null,\'Null\',\'to_date(\'\'\'||to_char(SIGNDATE,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(TRANSFERACTTYPEID,Null,\'Null\',\'\'\'\'||TRANSFERACTTYPEID||\'\'\'\')||\');\' from TRANSFERACT");
           MakeQuery("select  \'insert into TRANSFERACTTYPE (ID,NAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\');\' from TRANSFERACTTYPE");
           MakeQuery("select  \'insert into TRANSFERACTTYPEDOCUMENT (ID,TRANSFERACTTYPEID,DOCUMENTID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(TRANSFERACTTYPEID,Null,\'Null\',\'\'\'\'||TRANSFERACTTYPEID||\'\'\'\')||\',\'||decode(DOCUMENTID,Null,\'Null\',\'\'\'\'||DOCUMENTID||\'\'\'\')||\');\' from TRANSFERACTTYPEDOCUMENT");
           MakeQuery("select  \'insert into TROUBLE (ID,NAME,NUM,TOPTROUBLEID,TROUBLEREGISTRYID) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(NUM,Null,\'Null\',\'\'\'\'||NUM||\'\'\'\')||\',\'||decode(TOPTROUBLEID,Null,\'Null\',\'\'\'\'||TOPTROUBLEID||\'\'\'\')||\',\'||decode(TROUBLEREGISTRYID,Null,\'Null\',\'\'\'\'||TROUBLEREGISTRYID||\'\'\'\')||\');\' from TROUBLE");
           MakeQuery("select  \'insert into TROUBLESREGISTRY (ID,NAME,SHORTNAME,APPROVEDAT,ORDERNUM,VALIDFROM,VALIDTO) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(SHORTNAME,Null,\'Null\',\'\'\'\'||SHORTNAME||\'\'\'\')||\',\'||decode(APPROVEDAT,Null,\'Null\',\'to_date(\'\'\'||to_char(APPROVEDAT,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(ORDERNUM,Null,\'Null\',\'\'\'\'||ORDERNUM||\'\'\'\')||\',\'||decode(VALIDFROM,Null,\'Null\',\'to_date(\'\'\'||to_char(VALIDFROM,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\',\'||decode(VALIDTO,Null,\'Null\',\'to_date(\'\'\'||to_char(VALIDTO,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\');\' from TROUBLESREGISTRY");
           MakeQuery("select  \'insert into UDMETADATA (SCHEMERELEASE,SCHEMEBUILD,SCHEMETIMESTAMP) values (\'||decode(SCHEMERELEASE,Null,\'Null\',\'\'\'\'||SCHEMERELEASE||\'\'\'\')||\',\'||decode(SCHEMEBUILD,Null,\'Null\',\'\'\'\'||SCHEMEBUILD||\'\'\'\')||\',\'||decode(SCHEMETIMESTAMP,Null,\'Null\',\'to_date(\'\'\'||to_char(SCHEMETIMESTAMP,\'mm/dd/yyyy hh24:mi\')||\'\'\',\'\'mm/dd/yyyy hh24:mi\'\')\')||\');\' from UDMETADATA");
           MakeQuery("select  \'insert into WORKTYPE (ID,NAME,SHORTNAME) values (\'||decode(ID,Null,\'Null\',\'\'\'\'||ID||\'\'\'\')||\',\'||decode(NAME,Null,\'Null\',\'\'\'\'||NAME||\'\'\'\')||\',\'||decode(SHORTNAME,Null,\'Null\',\'\'\'\'||SHORTNAME||\'\'\'\')||\');\' from WORKTYPE");

       }

        public void MakeQuery(string query)
        {
            try
            {
                McDataContext ctx = Repository.TryGetContext();

                IQueryable<string> ss;
                if (ctx != null)
                {

                    ss = ctx.Query<string>(query);


                    if (ss.Count() > 0)
                    {
                        FileStream fs = new FileStream(Filename, FileMode.Append, FileAccess.Write, FileShare.Write);
                        fs.Close();

                        StreamWriter sw = new StreamWriter(Filename, true, FileEncoding);
                        foreach (string s in ss)
                        {
                            sw.WriteLine(s);
                        }

                        ss = ctx.Query<string>("select \'commit;\' from dual");
                        foreach (string s in ss)
                        {
                            sw.WriteLine(s);
                        }

                        sw.Close();
                    }
                }
            }
            catch
            {
            }
            
            _currentqueryindex++;
            OnPropertyChanged("CurrentQueryIndex");
            OnPropertyChanged("StringIndicator");
            
            
        }

        public event EventHandler<EventArgs> OnRefresh;

        public void SaveQueryresults()
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.AddExtension = true;
            sd.CreatePrompt = true;
            sd.Filter = "Лог-файлы|*.dfl";

            bool? sdr = sd.ShowDialog();
            if (sdr.HasValue ? sdr.Value : false)
            {
                Filename = sd.FileName;
                OnPropertyChanged("Filename");
                if (OnRefresh != null) OnRefresh(null, null);
                MakeQueries();
            }
        }

        private ICommand _savequeryresultscommand;
        /// <summary>
        /// открыть файл
        /// </summary>
        public ICommand SaveQueryresultsCommand
        {
            get
            {
                if (_savequeryresultscommand == null) _savequeryresultscommand = new RelayCommand(p => SaveQueryresults(), x => true);
                return _savequeryresultscommand;
            }
        }

/*
        private OraExpViewModel _viewmodel;
*/

        public OraExpViewModel(IContractRepository repository, ViewModelBase owner)
            : base(repository, owner)
        {
;
        }

        protected override void Save()
        {


        }

        protected override bool CanSave()
        {
            return true;
        }


    }
}
