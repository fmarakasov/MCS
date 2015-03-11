using System;

namespace CommonBase
{
    public struct Money : IComparable
    {
// Внутреннее представление - количество копеек
        private long value;
// Конструкторы
        public Money(decimal value)
        {
            this.value = (long) Math.Round(100*value + 0.00000000001M, 0);
        }

        public Money(long high, byte low)
        {
            if (low < 0 || low > 99)
                throw new ArgumentException();
            if (high >= 0)
                value = 100*high + low;
            else
                value = 100*high - low;
        }

// Вспомогательный конструктор
        private Money(long copecks)
        {
            value = copecks;
        }

        public static Money Zerro
        {
            get { return new Money(0); }
        }

        public decimal DecimalAmount
        {
            get { return ((decimal) value)/100; }
            set { this.value = (long) value; }
        }

// Количество рублей
        public long High
        {
            get { return value/100; }
        }

// Количество копеек
        public byte Low
        {
            get { return (byte) (Math.Abs(value)%100); }
        }

        public static Money Null
        {
            get { return ToMoney(-1); }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return DecimalAmount.CompareTo(((Money) obj).DecimalAmount);
        }

        #endregion

        public static Money ToMoney(object val)
        {
            if (val is Money) return (Money) val;
            Money total = Zerro;
            if (val != DBNull.Value && val != null)
            {
                var dVal = (decimal) val;
                total = (Money) (decimal) dVal;
            }
            return total;
        }

        public static Money ToMoney(decimal val)
        {
            Money total;
            total = (Money) (decimal) val;
            return total;
        }

// Абсолютная величина
        public Money Abs()
        {
            return new Money(Math.Abs(value));
        }

// Сложение - функциональная форма
        public Money Add(Money r)
        {
            return new Money(value + r.value);
        }

// Вычитание - функциональная форма 
        public Money Subtract(Money r)
        {
            return new Money(value - r.value);
        }

// Умножение - функциональная форма
        public Money Multiply(double value)
        {
            double round = Math.Round(this.value*value + 0.000000001, 0);
            return new Money((long) round);
        }

// Деление - функциональная форма
        public Money Divide(double value)
        {
            return new Money((long) Math.Round(this.value/value + 0.000000001, 0));
        }

// Остаток от деления нацело - функциональная форма
        public long GetRemainder(uint n)
        {
            return value%n;
        }

// Сравнение - функциональная форма
        public int CompareTo(Money r)
        {
            if (value < r.value)
                return -1;
            else if (value == r.value)
                return 0;
            else
                return 1;
        }

// Деление на одинаковые части
// Количество частей должно быть не меньше 2
        public Money[] Share(uint n)
        {
            if (n < 2)
                throw new ArgumentException();
            var lowResult = new Money(value/n);
            Money highResult =
                lowResult.value >= 0 ? new Money(lowResult.value + 1) : new Money(lowResult.value - 1);
            var results = new Money[n];
            long remainder = Math.Abs(value%n);
            for (long i = 0; i < remainder; i++)
                results[i] = highResult;
            for (long i = remainder; i < n; i++)
                results[i] = lowResult;
            return results;
        }

// Деление пропорционально коэффициентам
// Количество коэффициентов должно быть не меньше 2
        public Money[] Allocate(params uint[] ratios)
        {
            if (ratios.Length < 2)
                throw new ArgumentException();
            long total = 0;
            for (int i = 0; i < ratios.Length; i++)
                total += ratios[i];
            long remainder = value;
            var results = new Money[ratios.Length];
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = new Money(value*ratios[i]/total);
                remainder -= results[i].value;
            }
            if (remainder > 0)
            {
                for (int i = 0; i < remainder; i++)
                    results[i].value++;
            }
            else
            {
                for (int i = 0; i > remainder; i--)
                    results[i].value--;
            }
            return results;
        }

// Перекрытые методы Object
        public override bool Equals(object value)
        {
            try
            {
                return this == (Money) value;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return High + "," + (Low > 9 ? Low.ToString() : "0" + Low);
        }

// Преобразования в строку аналогично double
        public string ToString(IFormatProvider provider)
        {
            if (provider is IMoneyToStringProvider)
// здесь - формирование числа прописью
                return ((IMoneyToStringProvider) provider).MoneyToString(this);
            else
// а здесь - обычный double с учетом стандартного провайдера
                return ((double) this).ToString(provider);
        }

        public string ToString(string format)
        {
            return ((double) this).ToString(format);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return ((double) this).ToString(format, provider);
        }

// Унарные операторы
        public static Money operator +(Money r)
        {
            return r;
        }

        public static Money operator -(Money r)
        {
            return new Money(-r.value);
        }

        public static Money operator ++(Money r)
        {
            return new Money(r.value++);
        }

        public static Money operator --(Money r)
        {
            return new Money(r.value--);
        }

// Бинарные операторы
        public static Money operator +(Money a, Money b)
        {
            return new Money(a.value + b.value);
        }

        public static Money operator -(Money a, Money b)
        {
            return new Money(a.value - b.value);
        }

        public static Money operator *(double a, Money b)
        {
            return new Money((long) Math.Round(a*b.value + 0.0000000000, 0));
        }

        public static Money operator *(Money a, double b)
        {
            return new Money((long) Math.Round(a.value*b + 0.0000000000, 0));
        }

        public static Money operator /(Money a, double b)
        {
            return new Money((long) Math.Round(a.value/b + 0.0000000000, 0));
        }

        public static Money operator %(Money a, uint b)
        {
            return new Money((a.value%b));
        }

        public static bool operator ==(Money a, Money b)
        {
            return a.value == b.value;
        }

        public static bool operator !=(Money a, Money b)
        {
            return a.value != b.value;
        }

        public static bool operator >(Money a, Money b)
        {
            return a.value > b.value;
        }

        public static bool operator <(Money a, Money b)
        {
            return a.value < b.value;
        }

        public static bool operator >=(Money a, Money b)
        {
            return a.value >= b.value;
        }

        public static bool operator <=(Money a, Money b)
        {
            return a.value <= b.value;
        }

// Операторы преобразования
        public static implicit operator double(Money r)
        {
            return (double) r.value/100;
        }

        public static explicit operator Money(decimal d)
        {
            return new Money(d);
        }

        public static Money Parse(string amount)
        {
            return ToMoney(decimal.Parse(amount));
        }
    }

// Интерфейс специализированного провайдера преобразования денег в строковое представление
    public interface IMoneyToStringProvider : IFormatProvider
    {
        string MoneyToString(Money m);
    }

// Преобразование числа в строку = число прописью
    public class NumberToRussianString
    {
// Род единицы измерения

        #region WordGender enum

        public enum WordGender
        {
            Masculine, // мужской
            Feminine, // женский
            Neuter // средний
        } ;

        #endregion

        #region WordMode enum

        public enum WordMode
        {
            Mode1, // рубль
            Mode2_4, // рубля
            Mode0_5 // рублей
        } ;

        #endregion

// Строковые представления чисел
        private const string number0 = "ноль";

        private static readonly string[] number1 =
            {"один", "одна", "одно"};

        private static readonly string[] number2 =
            {"два", "две", "два"};

        private static readonly string[] number3_9 =
            {"три", "четыре", "пять", "шесть", "семь", "восемь", "девять"};

        private static readonly string[] number10_19 =
            {
                "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать",
                "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"
            };

        private static readonly string[] number20_90 =
            {"двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто"};

        private static readonly string[] number100_900 =
            {"сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот"};

        private static readonly string[,] ternaries =
            {
                {"тысяча", "тысячи", "тысяч"},
                {"миллион", "миллиона", "миллионов"},
                {"миллиард", "миллиарда", "миллиардов"},
                {"триллион", "триллиона", "триллионов"},
                {"биллион", "биллиона", "биллионов"}
            };

        private static readonly WordGender[] TernaryGenders =
            {
                WordGender.Feminine, // тысяча - женский
                WordGender.Masculine, // миллион - мужской
                WordGender.Masculine, // миллиард - мужской
                WordGender.Masculine, // триллион - мужской
                WordGender.Masculine // биллион - мужской
            };

// Функция преобразования 3-значного числа, заданного в виде строки,
// с учетом рода (мужской, женский или средний).
// Род учитывается для корректного формирования концовки:
// "один" (рубль) или "одна" (тысяча)
// version 2
// 15.11.02 - updated
        private static string TernaryToString(long ternary, WordGender gender)
// private static string TernaryToString(int ternary, WordGender gender)
// (end) 15.11.02 - updated
        {
            string s = "";
// version 2
// 15.11.02 - updated
// учитываются только последние 3 разряда, т.е. 0..999 
//long t = ternary % 1000;
//int digit2 = (int) (t / 100);
//int digit1 = (int) ((t % 100) / 10);
//int digit0 = (int) (t % 10);
            long digit2 = ternary/100;
            long digit1 = (ternary%100)/10;
            long digit0 = ternary%10;
// (end) 15.11.02 - updated
// сотни
            while (digit2 >= 10) digit2 %= 10;
            if (digit2 > 0)
                s = number100_900[digit2 - 1] + " ";
            if (digit1 > 1)
            {
                s += number20_90[digit1 - 2] + " ";
                if (digit0 >= 3)
                    s += number3_9[digit0 - 3] + " ";
                else
                {
                    if (digit0 == 1) s += number1[(int) gender] + " ";
                    if (digit0 == 2) s += number2[(int) gender] + " ";
                }
            }
            else if (digit1 == 1)
                s += number10_19[digit0] + " ";
            else
            {
                if (digit0 >= 3)
                    s += number3_9[digit0 - 3] + " ";
                else if (digit0 > 0)
                {
                    if (digit0 == 1) s += number1[(int)gender] + " ";
                    if (digit0 == 2) s += number2[(int)gender] + " ";
                }
                else { }
// version 2
// 15.11.02 - updated
//s += number0 + " ";

// (end) 15.11.02 - updated
            }
            return s.TrimEnd();
        }

//
        private static string TernaryToString(long value, byte ternaryIndex)
        {
// version 2
// 15.11.02 - updated
//long ternary = value;
//for (byte i = 0; i < ternaryIndex; i++) 
// ternary /= 1000;
            for (byte i = 0; i < ternaryIndex; i++)
                value /= 1000;
// учитываются только последние 3 разряда, т.е. 0..999 
            var ternary = (int) (value%1000);
// (end) 15.11.02 - updated
            if (ternary == 0)
                return "";
            else
            {
                ternaryIndex--;
                return TernaryToString(ternary, TernaryGenders[ternaryIndex]) + " " +
                       ternaries[ternaryIndex, (int) GetWordMode(ternary)] + " ";
            }
        }

// Функция возвращает число прописью с учетом рода единицы измерения
        public static string NumberToString(long value, WordGender gender)
        {
// version 2 
// 15.11.02 - updated
// if (value <= 0) 
            if (value < 0)
// (end) 15.11.02 - updated
                return "";
// version 2 
// 15.11.02 - added
            else if (value == 0)
                return number0;
// (end) 15.11.02 - added
            else 
// version 2 
// 15.11.02 - updated
//return TernaryToString(value, 5) +
// TernaryToString(value, 4) +
// TernaryToString(value, 3) +
// TernaryToString(value, 2) +
// TernaryToString(value, 1) +
// TernaryToString(value, gender);
                return TernaryToString(value, 5) +
                       TernaryToString(value, 4) +
                       TernaryToString(value, 3) +
                       TernaryToString(value, 2) +
                       TernaryToString(value, 1) +
                       TernaryToString(value, gender);
// (end) 15.11.02 - updated
        }

// Варианты написания единицы измерения 

// Определение варианта написания единицы измерения по 3-х значному числу
        public static WordMode GetWordMode(long number)
        {
// достаточно проверять только последние 2 цифры,
// т.к. разные падежи единицы измерения раскладываются
// 0 рублей, 1 рубль, 2-4 рубля, 5-20 рублей, 
// дальше - аналогично первому десятку 
            int digit1 = (int) (number%100)/10;
            var digit0 = (int) (number%10);
            if (digit1 == 1)
                return WordMode.Mode0_5;
            else if (digit0 == 1)
                return WordMode.Mode1;
            else if (2 <= digit0 && digit0 <= 4)
                return WordMode.Mode2_4;
            else
                return WordMode.Mode0_5;
        }
    }

// Преобразование денег в сумму прописью
    public abstract class MoneyToStringProviderBase : IMoneyToStringProvider
    {
// сокращенное написание рублей ? - рублей/руб.
        private readonly bool digitLow = true;
        private readonly bool shortHigh;
// сокращенное написание копеек ? - копеек/коп.
        private readonly bool shortLow;
// отображение копеек в виде цифр ? - 00
// Конструктор
        protected MoneyToStringProviderBase(bool shortHigh, bool shortLow, bool digitLow)
        {
            this.shortHigh = shortHigh;
            this.shortLow = shortLow;
            this.digitLow = digitLow;
        }

// Реализация интерфейса IMoneyToStringProvider
// Метод родительского интерфейса IFormatProvider

        #region IMoneyToStringProvider Members

        public object GetFormat(Type formatType)
        {
            if (formatType != typeof (RoubleToStringProvider))
                return null;
            else
                return this;
        }

// Функция возвращает число рублей и копеек прописью
        public string MoneyToString(Money m)
        {
            long r = m.High;
            long c = m.Low;
            return string.Format("{0} {1} {2} {3}",
                                 NumberToRussianString.NumberToString(r, GetGender(true)),
                                 shortHigh
                                     ? GetShortName(true)
                                     : GetName(NumberToRussianString.GetWordMode(r), true),
                                 digitLow
                                     ? string.Format("{0:d2}", c)
                                     : NumberToRussianString.NumberToString(c, GetGender(false)),
                                 shortLow
                                     ? GetShortName(false)
                                     : GetName(NumberToRussianString.GetWordMode(c), false));
        }

        #endregion

        protected abstract NumberToRussianString.WordGender GetGender(bool high);
// Функция возвращает наименование денежной единицы в соответствующей форме 
// (1) рубль / (2) рубля / (5) рублей
        protected abstract string GetName(NumberToRussianString.WordMode wordMode, bool high);
// Функция возвращает сокращенное наименование денежной единицы 
        protected abstract string GetShortName(bool high);
    }

// Преобразование русских денег (рубли + копейки) в сумму прописью
    public class RoubleToStringProvider : MoneyToStringProviderBase
    {
// варианты написания рублей
        private static readonly string[] roubles =
            {"рубль", "рубля", "рублей"};

// варианты написания копеек
        private static readonly string[] copecks =
            {"копейка", "копейки", "копеек"};

        public RoubleToStringProvider(bool shortRoubles, bool shortCopecks, bool digitCopecks) :
            base(shortRoubles, shortCopecks, digitCopecks)
        {
        }

        protected override NumberToRussianString.WordGender GetGender(bool high)
        {
            return high ? NumberToRussianString.WordGender.Masculine : NumberToRussianString.WordGender.Feminine;
        }

        protected override string GetName(NumberToRussianString.WordMode wordMode, bool high)
        {
            return high ? roubles[(int) wordMode] : copecks[(int) wordMode];
        }

        protected override string GetShortName(bool high)
        {
            return high ? "руб." : "коп.";
        }
    }

// Преобразование американских денег (доллары + центы) в сумму прописью
    public class DollarToStringProvider : MoneyToStringProviderBase
    {
// варианты написания долларов
        private static readonly string[] dollars =
            {"доллар", "доллара", "долларов"};

// варианты написания центов
        private static readonly string[] cents =
            {"цент", "цента", "центов"};

        public DollarToStringProvider(bool shortDollar, bool shortCent, bool digitCent) :
            base(shortDollar, shortCent, digitCent)
        {
        }

        protected override NumberToRussianString.WordGender GetGender(bool high)
        {
            return NumberToRussianString.WordGender.Masculine;
        }

        protected override string GetName(NumberToRussianString.WordMode wordMode, bool high)
        {
            return high ? dollars[(int) wordMode] : cents[(int) wordMode];
        }

        protected override string GetShortName(bool high)
        {
            return high ? "дол." : "ц.";
        }
    }

    /// <summary>
    /// Падежи русского языка
    /// </summary>
    public enum WordCase
    {
        CaseI,
        CaseR,
        CaseM
    } ;
    /// <summary>
    /// Обеспецивает настраиваемый перевод числа строковое валютное представление
    /// </summary>
    public class CustomizableCurrencyToStringProvider : MoneyToStringProviderBase
    {
        private string[] _currency = new string[3] {String.Empty, String.Empty, String.Empty};
        private string[] _smallcurrency = new string[3] { String.Empty, String.Empty, String.Empty };
        private string _highSmallName = string.Empty;
        private string _lowSmallName = string.Empty;

        public string LowSmallName
        {
            get { return _lowSmallName; }
            set { _lowSmallName = value; }
        }

        public string HighSmallName
        {
            get { return _highSmallName; }
            set { _highSmallName = value; }
        }

        public void SetCurrency(WordCase index, string value)
        {
            _currency[(int) index] = value;
        }

        public string GetCurrency(WordCase index)
        {
            return _currency[(int) index];
        }

        public void SetSmallCurrency(WordCase index, string value)
        {
            _smallcurrency[(int)index] = value;
        }

        public string GetSmallCurrency(WordCase index)
        {
            return _smallcurrency[(int)index];
        }


        public CustomizableCurrencyToStringProvider(bool shortCurency, bool shortSmall, bool digitSmall) :
            base(shortCurency, shortSmall, digitSmall)
        {
            
        }

        protected override NumberToRussianString.WordGender GetGender(bool high)
        {
            return NumberToRussianString.WordGender.Masculine;
        }

        protected override string GetName(NumberToRussianString.WordMode wordMode, bool high)
        {
            return high ? _currency[(int)wordMode] : _smallcurrency[(int)wordMode];
        }

        protected override string GetShortName(bool high)
        {
            return high ? HighSmallName : LowSmallName;
        }
    }

}