using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using MCDomain.Common;
using CommonBase;


namespace MCDomain.Model
{


    public partial class Yearreportcolor: IColor, IObjectId
    {
        public Quarters UserQuarter
        {
            get { return (Quarters) Quarter; }
            set
            {
                Quarter = (int)value;
                SendPropertyChanged("Quarter");
            }
        }


        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(Quarters));
            foreach (var ch in en)
            {
                if ((Quarters)Id == (Quarters)ch) return true;
            }
            return false;
        }

        public Color WinColor
        {
            get
            {
                int nval;
                if (int.TryParse(Color.ToString(), out nval))
                {
                    var bytes = BitConverter.GetBytes(nval);
                    var color = System.Windows.Media.Color.FromRgb(bytes[2], bytes[1], bytes[0]);
                    return color;
                }
                return new Color() {R = 255, G = 255, B = 255};
            }
        }

        public Color CoworkersWinColor
        {
            get
            {
                int nval;
                if (int.TryParse(Coworkerscolor.ToString(), out nval))
                {
                    var bytes = BitConverter.GetBytes(nval);
                    var color = System.Windows.Media.Color.FromRgb(bytes[2], bytes[1], bytes[0]);
                    return color;
                }
                return new Color() { R = 255, G = 255, B = 255 };
            }
        }
        

    }
}
