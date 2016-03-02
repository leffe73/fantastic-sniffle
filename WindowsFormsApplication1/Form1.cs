using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            List<string> myGroups =GetGroupNames(Environment.UserName);
            listBox1.DataSource = myGroups;
        }

        public List<string> GetGroupNamesOld(string userName)
        {
            var pc = new PrincipalContext(ContextType.Domain);
            var src = UserPrincipal.FindByIdentity(pc, userName).GetGroups(pc);
            var result = new List<string>();
            src.ToList().ForEach(sr => result.Add(sr.SamAccountName));
            return result;
        }
        private List<string> GetGroupNames(string userName)
        {
            List<string> result = new List<string>();

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "OKAB-AD"))
            {
                using (PrincipalSearchResult<Principal> src = UserPrincipal.FindByIdentity(pc, userName).GetGroups(pc))
                {
                    src.ToList().ForEach(sr => result.Add(sr.SamAccountName));
                }
            }

            return result;//.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text4 = "Leif";
            text4 = text4.Insert(text4.Length, '°'.ToString());
            text4 += '↓'.ToString();
            textBox1.Text = text4;
        }
    }
}
