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
    //i detta fall delegerar jag en "defenseman"
    public delegate void PassData_CallTo(Defenseman chosenDMan);


    public partial class DManListEntry : UserControl
    {
        
        public string name { get; set; }
        public string clubName { get; set; }

        public int blockedShots { get; set; }
        public int hits { get; set; }
        public int salary { get; set; }
        public int draftcost { get; set; }

        PassData_CallTo chosenDMan;


        public DManListEntry(PassData_CallTo chosenDMan)
        {
            InitializeComponent();

            this.chosenDMan = chosenDMan;
        }

        public void upd_content()
        {

            //uppdaterar alla värden i för alla spelare i listan.
            label1.Text = name;
            label2.Text = clubName;
            label3.Text = "Hits: " + Convert.ToString(hits);
            label4.Text = "Blocked shots: " + Convert.ToString(blockedShots);
            label5.Text = "Salary: $" + Convert.ToString(salary);
            label6.Text = "Draftcost: •" + Convert.ToString(draftcost);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Skapar ett defenseman objekt
            Defenseman DManPick = new Defenseman();
            DManPick.name = name;
            DManPick.club = clubName;
            DManPick.hits = hits;
            DManPick.blockedShots = blockedShots;
            DManPick.salary = salary;
            DManPick.draftCost = draftcost;

            //Skickar data till teambuilder
            chosenDMan(DManPick);
        }

        private void DManListEntry_Load(object sender, EventArgs e)
        {
            upd_content();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
