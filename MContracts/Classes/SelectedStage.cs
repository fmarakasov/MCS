using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.Classes
{
    public class SelectedStage
    {
        public bool IsSelected { get; set; }

        public Stage Stage { get; set; }

        public SelectedStage(Stage stage)
        {
            Stage = stage;
        }
    }
}
