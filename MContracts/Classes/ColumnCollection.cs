using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace MContracts.Classes
{
    public class ColumnCollection : ObservableCollection<DataGridColumn>
    {

    }

    public class TelerikColumnCollection: ObservableCollection<Telerik.Windows.Controls.GridViewColumn>
    {
        
    }
}
