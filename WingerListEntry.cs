using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hockeyCoach
{
    //för att skicka data mellan två usercontrolls använder jag delegationer
    //i detta fall delegerar jag en "winger"
    public delegate void PassWinger_CallTo(Winger chosenWinger);

    public partial class WingerListEntry : UserControl
    {

        public string name { get; set; }
        public string clubName { get; set; }

        public int goals { get; set; }
        public int assist { get; set; }

        public int salary { get; set; }
        public int draftcost { get; set; }


        PassWinger_CallTo chosenWinger;

        public WingerListEntry(PassWinger_CallTo chosenWinger)
        {
            InitializeComponent();
            this.chosenWinger = chosenWinger;
        }


        public void upd_content()
        {

            //uppdaterar alla värden i för alla spelare i listan.
            label1.Text = name;
            label2.Text = clubName;
            label3.Text = "Goals: " + Convert.ToString(goals);
            label4.Text = "Assist: " + Convert.ToString(assist);
            label5.Text = "Salary: $" + FormatText(salary);
            label6.Text = "Draftcost: •" + Convert.ToString(draftcost);

        }





        private void WingerListEntry_Load(object sender, EventArgs e)
        {
            upd_content();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Skapar ett winger objekt
            Winger WingerPick = new Winger();
            WingerPick.name = name;
            WingerPick.club = clubName;
            WingerPick.goals = goals;
            WingerPick.assists = assist;
            WingerPick.salary = salary;
            WingerPick.draftCost = draftcost;


            //Skickar data till teambuilder
            chosenWinger(WingerPick);
        }


        //tar ett tal (mellan 100 000 och 99 999 999) och lägger till mellanrum för att göra det lättare att läsa
        public string FormatText(int tal)
        {
            string tempA = Convert.ToString(tal);

            if (tempA.Length == 6)
            {
                tempA = tempA.Insert(3, " ");
            }
            else if (tempA.Length == 7)
            {
                tempA = tempA.Insert(4, " ");
                tempA = tempA.Insert(1, " ");
            }
            else if (tempA.Length == 8)
            {
                tempA = tempA.Insert(5, " ");
                tempA = tempA.Insert(2, " ");
            }

            return tempA;
        }

    }
}
