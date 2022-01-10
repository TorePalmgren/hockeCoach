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
    public partial class simulateGame : UserControl
    {
        static Random r = new Random();
        public Team userTeam { get; set; }

        //motståndarlaget som man spelar mot i simulationen
        public Team opponentTeam = new Team();
 
        public int period = 1;
        public int minute = 20;
        public int homeTeamGoals = 0;
        public int awayTeamGoals = 0;


        //rTemp används för att slumpa tal
        //statComparison används för att hålla en stat som ska jämföras med
        //success threshold är vilken siffra som bestämmer oddsen för att en jämförelse ska lyckas
        //puckCarrier håller reda på vilken utespelare som har pucken. 1, hemmalag försvara. 2, bortalag försvarare. 3, hemmalag leftwing. 4, hemmalag rightwing. 5, bortalag lefwing. 6, bortalag rightwing.
        int rTemp, statComparison, successThreshold, puckCarrier, count;


        Defenseman[] defenders = new Defenseman[2];
        Winger[] attackers = new Winger[4];
        Goalkeeper[] goalies = new Goalkeeper[2];


        //temporära variabler för spelare.
        Winger tempWinger;
        Defenseman tempDMan;
        Goalkeeper tempGK;

        string tempString;



        public simulateGame()
        {
            InitializeComponent();
        }

        private void simulateGame_Load(object sender, EventArgs e)
        {

            opponentTeam.gk = new Goalkeeper();
            opponentTeam.dMan = new Defenseman();
            opponentTeam.leftWing = new Winger();
            opponentTeam.rightWing = new Winger();


            //lägger till spelare i motståndarlaget
            opponentTeam.gk.name = "Pekka Rinne";
            opponentTeam.gk.savePercent = 0.911f;
            opponentTeam.gk.teamIndex = 1;

            opponentTeam.dMan.name = "P.K Subban";
            opponentTeam.dMan.hits = 51;
            opponentTeam.dMan.blockedShots = 72;
            opponentTeam.dMan.teamIndex = 1;

            opponentTeam.rightWing.name = "Nicklas Bäckström";
            opponentTeam.rightWing.goals = 28;
            opponentTeam.rightWing.assists = 34;
            opponentTeam.rightWing.teamIndex = 1;

            opponentTeam.leftWing.name = "Jonathan Toews";
            opponentTeam.leftWing.goals = 28;
            opponentTeam.leftWing.assists = 34;
            opponentTeam.leftWing.teamIndex = 1;

        }

        public void updContent()
        {
            label2.Text = userTeam.dMan.name;
            label3.Text = Convert.ToString(userTeam.dMan.hits);


            defenders[0] = userTeam.dMan;
            defenders[1] = opponentTeam.dMan;
            attackers[0] = userTeam.leftWing;
            attackers[1] = userTeam.rightWing;
            attackers[2] = opponentTeam.leftWing;
            attackers[3] = opponentTeam.rightWing;
            goalies[0] = userTeam.gk;
            goalies[1] = opponentTeam.gk;

            tempWinger = attackers[0];
            tempDMan = defenders[0];
            tempGK = goalies[0];

            label8.Text = "";

            userTeam.gk.teamIndex = 0;
            userTeam.dMan.teamIndex = 0;
            userTeam.rightWing.teamIndex = 0;
            userTeam.leftWing.teamIndex = 0;


            label3.Text = userTeam.gk.name;
            label10.Text = userTeam.dMan.name;
            label2.Text = userTeam.rightWing.name;
            label12.Text = userTeam.leftWing.name;

            label14.Text = opponentTeam.gk.name;
            label16.Text = opponentTeam.dMan.name;
            label18.Text = opponentTeam.rightWing.name;
            label20.Text = opponentTeam.leftWing.name;

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }






        //Här simuleras en match mellan lagen. Varje tick sker en slumpad händelse. Spelarnas attribut påverkar oddsen
        //för att olika saker ska hända.
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Det som kan hända varje tick:

            //En dman som har pucken kan pass till en slumpad winger.

            //En winger som har pucken kan välja att passa till sin andra winger (Passningen kan då antingen gå fram eller så kan puckhållaren bli tacklad.
            //Detta avgörs av hur bra assiststatistik puckhållaren har samt hur bra tacklingsstatistick Försvararen har. Efter en tackling får försvararen pucken)

            //En winger som har pucken kan välja att skjuta. Skottet kan då blockeras av en försvarare eller räddas av målvakt.


            //(alla händelser skrivs ut på skärmen.)

            //varje period startar med en tekning. Som vinns av någon av de båda lagens anfallare
            if (minute == 20)
            {
                puckCarrier = r.Next(3, 7);

                //uppdaterar text på skärmen
                tempString = label8.Text;
                label8.Text = attackers[puckCarrier - 3].name + " Wins the faceoff";
                label8.Text += System.Environment.NewLine;
                label8.Text += System.Environment.NewLine;
                label8.Text += tempString;
            }
            //Ifall puckhållaren är en anfallre
            else if(puckCarrier >= 3)
            {
                rTemp = r.Next(0, 2);

                //om Rtemp är 0 så väljer anfallaren att passa
                if (rTemp == 0)
                {
                    //Hämtar antalet tacklingar som motståndarnas försvarare har
                    foreach (Defenseman def in defenders)
                    {
                        if(def.teamIndex != attackers[puckCarrier - 3].teamIndex)
                        {
                            statComparison = def.hits;
                            tempDMan = def;
                            break;

                        }
                    }

                    //För att avgöra om passningen lyckas eller inte så slumpas ett tal mellan 0 och 200
                    //Om detta tal är högre än succesThreshold så lyckas passningen.
                    //Detta innebär att om en spelare med låg assiststatistik möter en spelare med hög tacklingsstatistik
                    //så är chansen att passningen går fram låg.
                    successThreshold = 90 + statComparison - attackers[puckCarrier - 3].assists;

                    //om passningen går fram blir den andra anfallaren puckförare
                    if (r.Next(0, 201) > successThreshold)
                    {
                        tempWinger = attackers[puckCarrier - 3];

                        //detta stycke kollar vilken spelare som får passningen och därmed blir puckförare
                        for (int i = 0; i < attackers.Length; i++)
                        {
                            if (puckCarrier - 3 != i)
                            {
                                if (attackers[i].teamIndex == attackers[puckCarrier - 3].teamIndex)
                                {
                                    puckCarrier = i + 3;
                                    break;
                                }
                            }
                        }

                        //uppdaterar text på skärmen
                        tempString = label8.Text;
                        label8.Text = tempWinger.name + " passes the puck to " + attackers[puckCarrier - 3].name;
                        label8.Text += System.Environment.NewLine;
                        label8.Text += System.Environment.NewLine;
                        label8.Text += tempString;


                    }
                    //om passaren blir tacklad så får försvararen pucken
                    else
                    {
                        //uppdaterar text på skärmen
                        tempString = label8.Text;
                        label8.Text = attackers[puckCarrier - 3].name + " is hit by " + tempDMan.name;
                        label8.Text += System.Environment.NewLine;
                        label8.Text += System.Environment.NewLine;
                        label8.Text += tempString;


                        //gör att försvararen som utförde tacklinen får pucken
                        for (int i = 0; i < defenders.Length; i++)
                        {
                            if (tempDMan == defenders[i])
                            {
                                puckCarrier = i + 1;
                            }
                        }
                    }
                }
                //om rTemp är 1 så väljer anfallaren att skjuta
                else if(rTemp == 1)
                {
                    tempWinger = attackers[puckCarrier - 3];

                    //Hämtar antalet blockerade skott som motståndarnas försvarare har
                    foreach (Defenseman def in defenders)
                    {
                        if(def.teamIndex != tempWinger.teamIndex)
                        {
                            statComparison = def.blockedShots;
                            tempDMan = def;
                            break;
                        }
                    }

                    successThreshold = 15 + statComparison - tempWinger.goals;

                    //om skottet inte blev blockerat
                    if (r.Next(0, 201) > successThreshold)
                    {
                        foreach(Goalkeeper gk in goalies)
                        {
                            if(gk.teamIndex != tempWinger.teamIndex)
                            {
                                tempGK = gk;
                            }
                        }

                        //om skottet går i mål
                        if(r.Next(0, 101) > (tempGK.savePercent * 100) - 30)
                        {
                            //uppdaterar text på skärmen
                            tempString = label8.Text;
                            label8.Text = attackers[puckCarrier - 3].name + " shoots and scores!";
                            label8.Text += System.Environment.NewLine;
                            label8.Text += System.Environment.NewLine;
                            label8.Text += tempString;

                            //ändrar ställningen
                            if(tempWinger.teamIndex == 0)
                            {
                                homeTeamGoals++;
                            }
                            else
                            {
                                awayTeamGoals++;
                            }

                            //efter ett mål så blir det tekning
                            puckCarrier = r.Next(3, 7);

                            //uppdaterar text på skärmen
                            tempString = label8.Text;
                            label8.Text = attackers[puckCarrier - 3].name + " Wins the faceoff";
                            label8.Text += System.Environment.NewLine;
                            label8.Text += System.Environment.NewLine;
                            label8.Text += tempString;
                        }
                        //om skottet blev räddat får försvararen pucken
                        else
                        {
                            //uppdaterar text på skärmen
                            tempString = label8.Text;
                            label8.Text = attackers[puckCarrier - 3].name + " shoots but " + tempGK.name + " makes the save!";
                            label8.Text += System.Environment.NewLine;
                            label8.Text += System.Environment.NewLine;
                            label8.Text += tempString;

                            for (int i = 0; i < defenders.Length; i++)
                            {
                                if(defenders[i].teamIndex == tempGK.teamIndex)
                                {
                                    puckCarrier = i + 1;

                                    tempString = label8.Text;
                                    label8.Text = defenders[i].name + " takes the puck.";
                                    label8.Text += System.Environment.NewLine;
                                    label8.Text += System.Environment.NewLine;
                                    label8.Text += tempString;
                                    break;
                                }
                            }
                        }
                    }
                    //om skottet blev blockerat
                    else
                    {
                        tempString = label8.Text;
                        label8.Text = tempWinger.name + " shoots and it's blocked by " + tempDMan.name;
                        label8.Text += System.Environment.NewLine;
                        label8.Text += System.Environment.NewLine;
                        label8.Text += tempString;

                        //gör att försvararen som blockerade skottet får pucken
                        for (int i = 0; i < defenders.Length; i++)
                        {
                            if(tempDMan == defenders[i])
                            {
                                puckCarrier = i + 1;
                            }
                        }
                    }
                }
            }
            //ifall puckhållaren är en försvarare
            else if (puckCarrier <= 2)
            {
                count = 0;
                tempDMan = defenders[puckCarrier - 1];

                //slumpar vilken av anfallarna som försvararen kommer att passa till
                rTemp = r.Next(0, 2);

                for (int i = 0; i < attackers.Length; i++)
                {
                    //kollar om spelarna är i samma lag
                    if(attackers[i].teamIndex == tempDMan.teamIndex)
                    {
                        if(count == rTemp)
                        {
                            puckCarrier = i + 3;

                            tempString = label8.Text;
                            label8.Text = tempDMan.name + " passes the puck to " + attackers[puckCarrier - 3].name;
                            label8.Text += System.Environment.NewLine;
                            label8.Text += System.Environment.NewLine;
                            label8.Text += tempString;
                        }
                        else
                        {
                            count++;
                        }
                    }
                }
            }


            //varje tick är en minut.
            minute--;
            if(minute == 0)
            {
                if(period == 3)
                {
                    timer1.Enabled = false;
                }
                else
                {
                    minute = 20;
                    period++;

                }
            }

            //uppdaterar klocka och antal mål
            label4.Text = "Period: " + period + "  Time: " + minute + ":00";
            label5.Text = homeTeamGoals + " - " + awayTeamGoals;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;

            button1.Visible = false;
            button1.Enabled = false;

        }
    }
}
