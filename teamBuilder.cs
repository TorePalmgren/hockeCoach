using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace hockeyCoach
{
    public partial class teamBuilder : UserControl
    {

        //För att välja en spelare krävs att man har tillräckligt med pengar och draftstock.
        //Dessa har get och set metoder för att kunna tilldelas ett värde från startsidan när svårighetsgrad väljs.
        public int Money { get; set; }
        public int DraftStock { get; set; }

        //skapar ett lag som innehåller alla valda spelare.
        Team customTeam = new Team();


        public teamBuilder()
        {
            InitializeComponent();
        }

        private void teamBuilder_Load(object sender, EventArgs e)
        {
            Goalkeeper tempGK = new Goalkeeper();
            Defenseman tempD = new Defenseman();
            Winger tempR = new Winger();
            Winger tempL = new Winger();

            tempGK.salary = 0;
            tempGK.draftCost = 0;
            tempD.salary = 0;
            tempD.draftCost = 0;
            tempR.salary = 0;
            tempR.draftCost = 0;
            tempL.salary = 0;
            tempL.draftCost = 0;

            customTeam.gk = tempGK;
            customTeam.dMan = tempD;
            customTeam.rightWing = tempR;
            customTeam.leftWing = tempL;

        }

        //En metod som kan uppdatera värdena för controlls vid skapande.
        public void updValues()
        {

            label2.Text = "$" + FormatText(Money);
            label4.Text = "•" + Convert.ToString(DraftStock);


            //fyller meny-listan för försvarare med spelare från listan Defenders.
            Defenseman[] Defenders = fetchDMen("defensemen.txt");
            DManListEntry[] panelDManList = new DManListEntry[Defenders.Length];
            for (int i = 0; i < panelDManList.Length; i++)
            {
                panelDManList[i] = new DManListEntry(new PassData_CallTo(updateDManContent));
                panelDManList[i].name = Defenders[i].name;
                panelDManList[i].clubName = Defenders[i].club;
                panelDManList[i].blockedShots = Defenders[i].blockedShots;
                panelDManList[i].hits = Defenders[i].hits;
                panelDManList[i].salary = Defenders[i].salary;
                panelDManList[i].draftcost = Defenders[i].draftCost;

                panelDManList[i].upd_content();

                flowLayoutPanel1.Controls.Add(panelDManList[i]);

            }


            //fyller meny-listan för målvakter med spelare från listan goalkeepers.
            Goalkeeper[] Goalies = fetchGK("goalkeepers.txt");
            GKListEntry[] panelGKList = new GKListEntry[Goalies.Length];
            for (int i = 0; i < panelGKList.Length; i++)
            {
                panelGKList[i] = new GKListEntry(new PassGK_CallTo(updateGKContent));
                panelGKList[i].name = Goalies[i].name;
                panelGKList[i].clubName = Goalies[i].club;
                panelGKList[i].savePercent = Goalies[i].savePercent;
                panelGKList[i].salary = Goalies[i].salary;
                panelGKList[i].draftcost = Goalies[i].draftCost;

                panelGKList[i].upd_content();

                flowLayoutPanel2.Controls.Add(panelGKList[i]);

            }



            //fyller meny-listan för högervingar med spelare från listan wingers.
            Winger[] RWings = fetchWingers("wingers.txt");
            WingerListEntry[] panelWingerList = new WingerListEntry[RWings.Length];
            for (int i = 0; i < panelWingerList.Length; i++)
            {
                panelWingerList[i] = new WingerListEntry(new PassWinger_CallTo(updateRWingContent));
                panelWingerList[i].name = RWings[i].name;
                panelWingerList[i].clubName = RWings[i].club;
                panelWingerList[i].goals = RWings[i].goals;
                panelWingerList[i].assist = RWings[i].assists;
                panelWingerList[i].salary = RWings[i].salary;
                panelWingerList[i].draftcost = RWings[i].draftCost;

                panelWingerList[i].upd_content();

                flowLayoutPanel3.Controls.Add(panelWingerList[i]);

            }


            //fyller meny-listan för vänstervingar med spelare från listan wingers.
            Winger[] LWings = fetchWingers("wingers.txt");
            WingerListEntry[] panelWingerList2 = new WingerListEntry[LWings.Length];
            for (int i = 0; i < panelWingerList2.Length; i++)
            {
                panelWingerList2[i] = new WingerListEntry(new PassWinger_CallTo(updateLWingContent));
                panelWingerList2[i].name = LWings[i].name;
                panelWingerList2[i].clubName = LWings[i].club;
                panelWingerList2[i].goals = LWings[i].goals;
                panelWingerList2[i].assist = LWings[i].assists;
                panelWingerList2[i].salary = LWings[i].salary;
                panelWingerList2[i].draftcost = LWings[i].draftCost;

                panelWingerList2[i].upd_content();

                flowLayoutPanel4.Controls.Add(panelWingerList2[i]);

            }


        }


        //pick goalkeeper
        private void button1_Click(object sender, EventArgs e)
        {
            //visar menyn där man kan välja spelare.
            flowLayoutPanel2.Visible = true;
            flowLayoutPanel2.Enabled = true;

        }


        //pick defenseman
        private void button2_Click(object sender, EventArgs e)
        {


            //visar menyn där man kan välja spelare.
            flowLayoutPanel1.Visible = true;
            flowLayoutPanel1.Enabled = true;



        }


        //pick left wing
        private void button3_Click(object sender, EventArgs e)
        {
            flowLayoutPanel4.Visible = true;
            flowLayoutPanel4.Enabled = true;

        }


        //pick right wing
        private void button4_Click(object sender, EventArgs e)
        {
            flowLayoutPanel3.Visible = true;
            flowLayoutPanel3.Enabled = true;

        }

        
        //läser en textfil med spelarinfo för försvarare och ger tillbaka en lista med "Defenseman"
        public Defenseman[] fetchDMen(string filename)
        {

            Defenseman[] DefenderList = new Defenseman[20];

            int count = 0;
            string temp;
            StreamReader las = File.OpenText(filename);

            //kontrollerar att vi inte är sist i filen
            while (!las.EndOfStream)
            {            
                //läser en rad från filen
                temp = las.ReadLine();

                string[] plrAttributes = temp.Split(';');

                //skapar ett Defenseman objekt och ger det ett namn.
                DefenderList[count] = new Defenseman();
                DefenderList[count].name = plrAttributes[0];
                DefenderList[count].club = plrAttributes[1];
                DefenderList[count].blockedShots = Convert.ToInt32(plrAttributes[2]);
                DefenderList[count].hits = Convert.ToInt32(plrAttributes[3]);
                DefenderList[count].salary = Convert.ToInt32(plrAttributes[4]);
                DefenderList[count].draftCost = Convert.ToInt32(plrAttributes[5]);

                count++;
            }
            //stänger filen
            las.Close();

            return DefenderList;
        }


        //läser en textfil med spelarinfo för försvarare och ger tillbaka en lista med "goalkeeper"
        public Goalkeeper[] fetchGK(string filename)
        {

            Goalkeeper[] goalkeeperList = new Goalkeeper[20];

            int count = 0;
            string temp;
            StreamReader las = File.OpenText(filename);

            //kontrollerar att vi inte är sist i filen
            while (!las.EndOfStream)
            {
                //läser en rad från filen
                temp = las.ReadLine();

                string[] plrAttributes = temp.Split(';');

                //skapar ett goalkeeper objekt och ger det ett namn.
                goalkeeperList[count] = new Goalkeeper();
                goalkeeperList[count].name = plrAttributes[0];
                goalkeeperList[count].club = plrAttributes[1];
                goalkeeperList[count].savePercent = Convert.ToSingle(plrAttributes[2]);
                goalkeeperList[count].salary = Convert.ToInt32(plrAttributes[3]);
                goalkeeperList[count].draftCost = Convert.ToInt32(plrAttributes[4]);

                count++;
            }
            //stänger filen
            las.Close();

            return goalkeeperList;
        }


        //läser en textfil med spelarinfo för försvarare och ger tillbaka en lista med "Winger"
        public Winger[] fetchWingers(string filename)
        {

            Winger[] WingerList = new Winger[20];

            int count = 0;
            string temp;
            StreamReader las = File.OpenText(filename);

            //kontrollerar att vi inte är sist i filen
            while (!las.EndOfStream)
            {
                //läser en rad från filen
                temp = las.ReadLine();

                string[] plrAttributes = temp.Split(';');

                //skapar ett Defenseman objekt och ger det ett namn.
                WingerList[count] = new Winger();
                WingerList[count].name = plrAttributes[0];
                WingerList[count].club = plrAttributes[1];
                WingerList[count].goals = Convert.ToInt32(plrAttributes[2]);
                WingerList[count].assists = Convert.ToInt32(plrAttributes[3]);
                WingerList[count].salary = Convert.ToInt32(plrAttributes[4]);
                WingerList[count].draftCost = Convert.ToInt32(plrAttributes[5]);

                count++;
            }
            //stänger filen
            las.Close();

            return WingerList;
        }





        //när man valt en försvarare från menyn ska det som visas på skärmen uppdateras
        public void updateDManContent(Defenseman input)
        {
            int tempMoney = Money - customTeam.leftWing.salary - customTeam.rightWing.salary - customTeam.gk.salary - input.salary;
            int tempDraftStock = DraftStock - customTeam.leftWing.draftCost - customTeam.rightWing.draftCost - customTeam.gk.draftCost - input.draftCost;

            //kollar om man har tillräckligt med pengar och draftstock för att välja spelaren.
            if (tempMoney >= 0 && tempDraftStock >= 0)
            {

                //lägger till spelaren i customTeam samt subtraherar den spelarens lön och draftcost från totalen.
                customTeam.dMan = input;
                label6.Text = input.name;

                flowLayoutPanel1.Visible = false;
                flowLayoutPanel1.Enabled = false;

                label2.Text = "$" + FormatText(tempMoney);
                label4.Text = "•" + Convert.ToString(tempDraftStock);

            }

        }

        //när man valt en målvakt från menyn ska det som visas på skärmen uppdateras
        public void updateGKContent(Goalkeeper input)
        {
            int tempMoney = Money - customTeam.leftWing.salary - customTeam.rightWing.salary - customTeam.dMan.salary - input.salary;
            int tempDraftStock = DraftStock - customTeam.leftWing.draftCost - customTeam.rightWing.draftCost - customTeam.dMan.draftCost - input.draftCost;

            //kollar om man har tillräckligt med pengar och draftstock för att välja spelaren.
            if (tempMoney >= 0 && tempDraftStock >= 0)
            {

                //lägger till spelaren i customTeam samt subtraherar den spelarens lön och draftcost från totalen.
                customTeam.gk = input;
                label5.Text = input.name;

                flowLayoutPanel2.Visible = false;
                flowLayoutPanel2.Enabled = false;

                label2.Text = "$" + FormatText(tempMoney);
                label4.Text = "•" + Convert.ToString(tempDraftStock);

            }

        }

        //när man valt en winger från menyn ska det som visas på skärmen uppdateras
        public void updateRWingContent(Winger input)
        {
            int tempMoney = Money - customTeam.leftWing.salary - customTeam.dMan.salary - customTeam.gk.salary - input.salary;
            int tempDraftStock = DraftStock - customTeam.leftWing.draftCost - customTeam.dMan.draftCost - customTeam.gk.draftCost - input.draftCost;

            //kollar om man har tillräckligt med pengar och draftstock för att välja spelaren.
            if (tempMoney >= 0 && tempDraftStock >= 0)
            {

                //lägger till spelaren i customTeam samt subtraherar den spelarens lön och draftcost från totalen.
                customTeam.rightWing = input;
                label8.Text = input.name;

                flowLayoutPanel3.Visible = false;
                flowLayoutPanel3.Enabled = false;

                label2.Text = "$" + FormatText(tempMoney);
                label4.Text = "•" + Convert.ToString(tempDraftStock);

            }

        }

        //när man valt en winger från menyn ska det som visas på skärmen uppdateras
        public void updateLWingContent(Winger input)
        {
            int tempMoney = Money - customTeam.rightWing.salary - customTeam.dMan.salary - customTeam.gk.salary - input.salary;
            int tempDraftStock = DraftStock - customTeam.rightWing.draftCost - customTeam.dMan.draftCost - customTeam.gk.draftCost - input.draftCost;

            //kollar om man har tillräckligt med pengar och draftstock för att välja spelaren.
            if (tempMoney >= 0 && tempDraftStock >= 0)
            {

                //lägger till spelaren i customTeam samt subtraherar den spelarens lön och draftcost från totalen.
                customTeam.leftWing = input;
                label7.Text = input.name;

                flowLayoutPanel4.Visible = false;
                flowLayoutPanel4.Enabled = false;

                label2.Text = "$" + FormatText(tempMoney);
                label4.Text = "•" + Convert.ToString(tempDraftStock);

            }

        }




        //skapar simulation sidan.
        private void button9_Click(object sender, EventArgs e)
        {
            //kollar att det valts en spelare på varje position.
            if(customTeam.leftWing.name != null && customTeam.rightWing.name != null && customTeam.gk.name != null && customTeam.dMan.name != null)
            {
                //Stänger av och gör alla kontroller osynliga
                DisableControls();

                //skapar sidan
                simulateGame simPage = new simulateGame();
                simPage.Dock = DockStyle.Fill;
                this.Controls.Add(simPage);

                //skickar det byggda laget till simulationssidan.
                simPage.userTeam = customTeam;
                simPage.updContent();
            }  
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}
