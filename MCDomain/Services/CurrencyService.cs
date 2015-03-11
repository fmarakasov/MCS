using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MCDomain.CBR;

namespace MCDomain.Services
{
    public class RateOnDateEventArgs:EventArgs
    {
        /// <summary>
        /// Получает объект исключения
        /// </summary>
        public Exception Error { get; private set; }
        /// <summary>
        /// Получает оригинальный класс аргументов, возвращённый асинхронным методом
        /// </summary>
        public GetCursOnDateCompletedEventArgs OriginalCursOnDateEventArgs { get; private set; }
        /// <summary>
        /// Получает курс валюты
        /// </summary>
        public decimal Rate { get; private set; }

        public RateOnDateEventArgs(decimal rate, GetCursOnDateCompletedEventArgs args, Exception exception)
        {
            Rate = rate;
            OriginalCursOnDateEventArgs = args;
            Error = exception;
        }    
    }

    /// <summary>
    /// Обеспечивает доступ к сервисам валюты
    /// </summary>
    public class CurrencyService
    {
        public event EventHandler<RateOnDateEventArgs> CurrencyRateOnDateCompleted;
        private CBR.DailyInfoSoapClient _cbrClient;
        public CBR.DailyInfoSoapClient Cbr
        {
            get
            {
                if (_cbrClient == null)
                {
                    _cbrClient = new DailyInfoSoapClient();
                    _cbrClient.GetCursOnDateCompleted += new EventHandler<GetCursOnDateCompletedEventArgs>(_cbrClient_GetCursOnDateCompleted);
                }
                return _cbrClient;
            }
        }

        void _cbrClient_GetCursOnDateCompleted(object sender, GetCursOnDateCompletedEventArgs e)
        {
            Exception error = null;
            decimal rate = decimal.MinValue;

            try
            {
                rate = ExtractCurrencyRate(e.Result, (string)e.UserState);
            }
            catch (Exception err)
            {
                error = err;
            }
            
            if (CurrencyRateOnDateCompleted != null)
            {
                CurrencyRateOnDateCompleted(this, new RateOnDateEventArgs(rate, e, error));
            }
        }

        public void AsyncGetCurrencyRateOnDate(DateTime dateTime, string currencyCode)
        {
            Cbr.GetCursOnDateAsync(dateTime, currencyCode);                 
        }

        
        /// <summary>
        /// Получает курс валюты ЦБР на заданную дату
        /// </summary>
        /// <param name="dateTime">Дата</param>
        /// <param name="currencyCode">Условное обозначение валюты</param>
        /// <returns>Курс ЦБР</returns>
        public decimal GetCurrencyRateOnDate(DateTime dateTime, string currencyCode)
        {
            DataSet ds = Cbr.GetCursOnDate(dateTime.Date);
            return ExtractCurrencyRate(ds, currencyCode);
        }

        private static decimal ExtractCurrencyRate(DataSet ds, string currencyCode)
        {
            if (ds == null)
                throw new ArgumentNullException("ds", "Параметр ds не может быть null.");

            if (string.IsNullOrEmpty(currencyCode))
                throw new ArgumentNullException("currencyCode", "Параметр currencyCode не может быть null.");

            DataTable dt = ds.Tables["ValuteCursOnDate"];
            
            DataRow[] rows = dt.Select(string.Format("VchCode=\'{0}\'", currencyCode));

            if (rows.Length > 0)
            {
                decimal result;
                if (decimal.TryParse(rows[0]["Vcurs"].ToString(), out result))
                    return result;
                throw new InvalidCastException("От службы ожидалось значение курса валют.");

            }
            throw new KeyNotFoundException("Для заданной валюты не найден курс.");
        }
    }
}
