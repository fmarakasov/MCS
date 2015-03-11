#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace MCDomain.Model
{
    partial class Approvalprocess
    {

        partial void OnCreated()
        {

        }

        partial void OnMissivedateChanged()
        {
            if (Transferstateat == Enteringdate)
              Transferstateat = (Missivedate != null) ? Missivedate.Value : Enteringdate;
        }

        partial void OnTransferstateatChanged()
        {
            if (Missivedate == Enteringdate)
               Missivedate = Transferstateat;
            SendPropertyChanged("ConsiderationTime");
        }


        partial void OnEnterstateatChanged()
        {
            SendPropertyChanged("ConsiderationTime");
        }

        public Location ToLocation
        {
            get
            {
                return Location_Tolocationid;
            }

            set
            {
                Location_Tolocationid = value;
                SendPropertyChanged("ToLocation");
            }
        }

        public Location FromLocation
        {
            get
            {
                return Location_Fromlocationid;
            }
            set
            {
                Location_Fromlocationid = value;
                SendPropertyChanged("FromLocation");
            }
        }


        public TimeSpan ConsiderationTime
        {
            get { return Transferstateat - Enterstateat; }
        }

        /// <summary>
        ///   Получает разницку во времени между данным событием и предыдущим событием согласования
        ///   это поменялось, оставила код, используются другие свойства (Таня)
        /// </summary>
        public TimeSpan PrevStateTimespan
        {
            get { return CalculateTimespan(x => x.Enterstateat <= Enterstateat && x != this, true); }
        }

        public TimeSpan NextStateTimespan
        {
            get { return CalculateTimespan(x => x.Enterstateat >= Enterstateat && x != this, false); }
        }

        private TimeSpan CalculateTimespan(Func<Approvalprocess, bool> predicate, bool getMax)
        {
            if (Contractdoc == null) return new TimeSpan(0);

            Approvalprocess[] allContractAprovals = Contractdoc.Approvalprocesses.ToArray();

            try
            {
                IEnumerable<Approvalprocess> approvalEvents = allContractAprovals.Where(predicate);
                DateTime approvalEvent = getMax
                                             ? approvalEvents.Max(x => x.Enterstateat)
                                             : approvalEvents.Min(x => x.Enterstateat);
                DateTime hiBound = getMax ? Enterstateat : approvalEvent;
                DateTime lowBound = getMax ? approvalEvent : Enterstateat;

                return hiBound - lowBound;
            }
            catch (InvalidOperationException)
                // Если это первая запись в списке, то последовательность не будет содержать элементов
            {
                return new TimeSpan(0);
            }
        }

        public void InvalidateTimeSpans()
        {
            SendPropertyChanged("NextStateTimespan");
            SendPropertyChanged("PrevStateTimespan");
        }
    }
}