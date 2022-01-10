using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace hockeyCoach
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //skapar sidan där man bygger sitt lag samt ger värden för mängd pengar och draftstock
            createTeamBuilder(300, 15);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //skapar sidan där man bygger sitt lag samt ger värden för mängd pengar och draftstock
            createTeamBuilder(200, 10);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //skapar sidan där man bygger sitt lag samt ger värden för mängd pengar och draftstock
            createTeamBuilder(100, 5);

        }


        //denna metod skapar en teamBuilder och tilldelar värden till money och draftstock.
        public void createTeamBuilder(int money, int draftstock)
        {
            //Stänger av och gör alla kontroller osynliga
            DisableControls();

            //skapar sidan
            teamBuilder builderPage = new teamBuilder();
            builderPage.Dock = DockStyle.Fill;
            this.Controls.Add(builderPage);
            builderPage.Money = money;
            builderPage.DraftStock = draftstock;
            builderPage.updValues();

        }

        //Varje ny sida skapas ovanpå den nuvarande. Därför används denna metod för att stänga av och göra kontroller osynliga.
        public void DisableControls()
        {
            foreach (Control c in this.Controls)
            {
                c.Enabled = false;
                c.Visible = false;
            }
        }

        
    }


    //Detta är spelarklasserna som har alla attribut för spelarna. Eftersom varje position har olika attribut
    //så finns det en klass för varje position. (Båda wingers har samma attribut och är därför samma klass)
    public class Winger
    {
        public int goals, assists, salary, draftCost;
        public string name, club;

    }

    public class Defenseman
    {
        public int hits, blockedShots, salary, draftCost;
        public string name, club;

    }

    public class Goalkeeper
    {
        public int savePercent;
        public string name, club;

    }


    //Ett team innehåller en spelare varje position
    public class Team
    {
        public Winger rightWing;
        public Winger leftWing;
        public Defenseman dMan;
        public Goalkeeper gk;
    }

}
