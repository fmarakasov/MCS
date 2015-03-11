using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MCDomain.CBR;
using MCDomain.Common;
using MCDomain.Model;
using MCDomain.Services;
using MContracts.Classes;
using Oracle.DataAccess.Client;

namespace ConceptsChamber
{   
 

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        MCDomain.Model.McDataContext ctx = new McDataContext("User Id=UD;Password=sys;Server=XE;Connect Mode=Default;Home=oraclient11g_home2;Persist Security Info=True");

        private void Form1_Load(object sender, EventArgs e)
        {
            functionalcustomerBindingSource.DataSource = ctx.Functionalcustomers.GetNewBindingList();
            functionalcustomercontractBindingSource.DataSource =
                ctx.Contractdocs.First().Functionalcustomercontracts.GetNewBindingList();
                //new EntitySetProxy<MCDomain.Model.Functionalcustomercontract>(
                //    ctx.Contractdocs.First().Functionalcustomercontracts);
      
        }
        

     
    }
}
