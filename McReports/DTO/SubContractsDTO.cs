using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace McReports.DTO
{
  public  class SubContractDto
  {
      /// <summary>
      /// Год
      /// </summary>
      public int Year;

      public Contractortype ContractorType;

      public Contracttype ContractType;

      public Stage StageGen;

      public Contractdoc SubContract;

      public Contractdoc GenContract;

      /// <summary>
      /// Заместитель директора, руководитель отдела, куратор от договорного отдела
      /// </summary>
      public string Directors;

      /// <summary>
      /// № Этапа генерального договора
      /// </summary>
      public string NumStageGen;

      /// <summary>
      /// Сроки выполнения по генеральному договору
      /// </summary>
      public string RunTimeGen;

      /// <summary>
      /// 
      /// </summary>
      public string SubOrganization;

      /// <summary>
      /// № субподрядного договора, Дата подписания субподрядного договора
      /// </summary>
      public string NumDateSub;

      /// <summary>
      /// Наименование работы (этапа) субподрядного договора
      /// </summary>
      public string Subject;

      /// <summary>
      /// Сроки выполнения по субподрядному договору
      /// </summary>
      public string RunTimeSub;

      /// <summary>
      /// Стоимость работ
      /// </summary>
      public decimal Price;
      
      /// <summary>
      /// Стоимость работ по акту
      /// </summary>
      public decimal PriceByAct;

      /// <summary>
      /// №/дата акта
      /// </summary>
      public string NumDateAct;
      
      /// <summary>
      /// 
      /// </summary>
      public string ContractorTypeName;
      public string ContractTypeName;
      public string NumDateGen;
  }
}
