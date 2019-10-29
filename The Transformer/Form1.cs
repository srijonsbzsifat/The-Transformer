using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;


namespace The_Transformer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Thread t = new Thread(new ThreadStart(splashstart));
            t.Start();
            Thread.Sleep(7000);
            InitializeComponent();
            t.Abort();
            TopMost = true; 
        }

        public void splashstart()
        {
            Application.Run(new SplashScreen());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            ComboCore.SelectedIndex = 0;
            CombWind.SelectedIndex = 0;
            BtnCal.Enabled = false;
            BtnGraph.Hide();

            al.Clear();
            count = 0;
           // label13.Text = count.ToString();

            chart1.Series.Clear();
            chart1.Legends.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();
        }

        // function to enable Button
        void chkButton()
        {

            if (BoxN1.Text.Length > 0 && BoxRl.Text.Length > 0 && BoxN2.Text.Length>0 && BoxV1.Text.Length>0 && BoxThick.Text.Length >0 && BoxWidth.Text.Length>0)
            {
                BtnCal.Enabled = true;
            }
            else
                BtnCal.Enabled = false;
        }
        // array to save the values for graph
        ArrayList al = new ArrayList();
        int count = 0;
        /*                       */
        private void BtnCal_Click(object sender, EventArgs e)
        {
           double p, ri, r2, xi, x2, k, r0, r1, x1, x0, r2p, reqp, x2p, xeqp, zeqp, v2p, rl, n1, n2, v1, v2,T,W;
           double i1, i2,core,pr,px,winding,cu_loss,VR,K,m,n,density;
           double v20, i20, v22, i22,rl2,rl0;
            m = 0;
            n = 0;
            K = 0;
            density = 0;
            winding = 0;
            
            //input Section
            n1 = Convert.ToDouble (BoxN1.Text);
            n2 = Convert.ToDouble(BoxN2.Text);
            v1 = Convert.ToDouble(BoxV1.Text);
            rl = Convert.ToDouble(BoxRl.Text);
            T = Convert.ToDouble(BoxThick.Text);
            W = Convert.ToDouble(BoxWidth.Text);

            pr = 0;px = 0;
            T = T / 100;
            W = W / 100;

            //FOR CORE
            if(ComboCore.Text== "Silicon")
            {
                core = 5;
                K = 0.0593;
                m = 0.993;
                n = 1.740;
                density = 2328;
            }
            else if(ComboCore.Text == "Nickel/Iron")
            {
                core = 0;
                K = 0.0281;
                m = 1.21;
                n = 1.38;
                density = 8312;
            }
          //FOR WINDING
            if (CombWind.Text== "Copper (18 AWG)")
            {
                pr = 0.021;
                //pr = 0.042;
                px = (2 * pr / 100);
            }
            else if (CombWind.Text == "Copper (10 AWG)")
            {
                pr = 0.00326;
                px = (2 * pr / 100);
            }
            else if (CombWind.Text == "Aluminium (18 AWG)")
            {
                pr = 0.00328;
                px = (2 * pr / 100);
            }
            else if (CombWind.Text == "Aluminium (10 AWG)")
            {
                pr = 0.00536;
                px = (2 * pr / 100);
            }
            /*
             //perimeter
             p = 4 * T;
             //p = 2 * T + 2 * W;
             ri = p * pr * n1;
             r2 = p * pr * n2;
             xi = p * px * n1;

             x2 = p * px * n2;

              k = n1 / n2;

             r1 = k * k * r2;

             x1 = k * k * x2;

             r0 = ri - r1;
             x0 = xi - x1;
             r2p = k * k * r2;
             x2p = k * k * x2;

             reqp = r1 + r2p;
             xeqp = x1 + x2p;
             zeqp = Math.Sqrt((reqp * reqp) + (xeqp * xeqp));
             v2p = rl / (zeqp + rl) * v1;
             v2 = v2p / k;// secondary voltage
             i2 = v2 / rl;// secondary current
             i1 = v1 / (Math.Sqrt((ri * ri) + (xi * xi)));

             i1 = i2 / k;// primary current
             cu_loss = (i1 * i1 * r1) + (i2 * i2 * r2);// copper loss

             double c = v1 * i1 - v2 * i2;
             cu_loss = 0.9 * c;
             double Vol;
             Vol = T * T * W * 4;
             double B, f;
             B = 1.2;
             f = 50;
             double core_loss;
             double mass;
             core_loss = Math.Pow(B, n) * Math.Pow(f, m) * K;
             mass = density * Vol;
             core_loss = core_loss * Vol * 100;//core loss

             //core_loss = core_loss * mass;

             //GENERATING 3rd POINT FOR GRAPH

             v22 = v2;
             i22 = i2;
             rl2 = rl;

             ///outputs

             LblScyV.Text = v2.ToString("0.### V");
             LblPriC.Text = i1.ToString("0.### A");
             LblScyC.Text = i2.ToString("0.### A");
             LblCuLoss.Text = cu_loss.ToString("0.### W");
             LblCoreLoss.Text = core_loss.ToString("0.### W");
             //LblVoltageReg.Text = VR.ToString("");

             double rl1, rl3, rl4, v21, v23, v24, i21, i23, i24;
              //rl3 = 0;
              //rl4 = 0;
              int nnn = 1;
              //double reali2 = 0;
              double realLoad = 0;

              //GENERATING 1st POINT FOR GRAPH

              realLoad = rl - 100;
              p = 4 * T;
              ri = p * pr * n1;
              r2 = p * pr * n2;
              xi = p * px * n1;
              x2 = p * px * n2;
              k = n1 / n2;
              r1 = k * k * r2;
              x1 = k * k * x2;
              r0 = ri - r1;
              x0 = xi - x1;
              r2p = k * k * r2;
              x2p = k * k * x2;
              reqp = r1 + r2p;
              xeqp = x1 + x2p;
              zeqp = Math.Sqrt((reqp * reqp) + (xeqp * xeqp));
              v2p = realLoad / (zeqp + rl) * v1;
              v2 = v2p / k;
              i2 = v2 / realLoad;
              i1 = v1 / (Math.Sqrt((ri * ri) + (xi * xi)));

              i1 = i2 / k;
              v20 = v2;
              i20 = i2;
              rl0 = realLoad;

              //GENERATING 2nd POINT FOR GRAPH

              realLoad = rl - 50;
              p = 4 * T;
              ri = p * pr * n1;
              r2 = p * pr * n2;
              xi = p * px * n1;
              x2 = p * px * n2;
              k = n1 / n2;
              r1 = k * k * r2;
              x1 = k * k * x2;
              r0 = ri - r1;
              x0 = xi - x1;
              r2p = k * k * r2;
              x2p = k * k * x2;
              reqp = r1 + r2p;
              xeqp = x1 + x2p;
              zeqp = Math.Sqrt((reqp * reqp) + (xeqp * xeqp));
              v2p = realLoad / (zeqp + rl) * v1;
              v2 = v2p / k;
              i2 = v2 / realLoad;
              i1 = v1 / (Math.Sqrt((ri * ri) + (xi * xi)));

              i1 = i2 / k;
              i21 = i2;
              v21 = v2;
              rl1 = realLoad;

              //GENERATING 5th POINT FOR GRAPH

              realLoad = rl + 50;
              p = 4 * T;
              ri = p * pr * n1;
              r2 = p * pr * n2;
              xi = p * px * n1;
              x2 = p * px * n2;
              k = n1 / n2;
              r1 = k * k * r2;
              x1 = k * k * x2;
              r0 = ri - r1;
              x0 = xi - x1;
              r2p = k * k * r2;
              x2p = k * k * x2;
              reqp = r1 + r2p;
              xeqp = x1 + x2p;
              zeqp = Math.Sqrt((reqp * reqp) + (xeqp * xeqp));
              v2p = realLoad / (zeqp + rl) * v1;
              v2 = v2p / k;
              i2 = v2 / realLoad;
              i1 = v1 / (Math.Sqrt((ri * ri) + (xi * xi)));

              i1 = i2 / k;
              v23 = v2;
              i23 = i2;
              rl3 = realLoad;

              //GENERATING 5th POINT FOR GRAPH

              realLoad = rl + 100;
              p = 4 * T;
              ri = p * pr * n1;
              r2 = p * pr * n2;
              xi = p * px * n1;
              x2 = p * px * n2;
              k = n1 / n2;
              r1 = k * k * r2;
              x1 = k * k * x2;
              r0 = ri - r1;
              x0 = xi - x1;
              r2p = k * k * r2;
              x2p = k * k * x2;
              reqp = r1 + r2p;
              xeqp = x1 + x2p;
              zeqp = Math.Sqrt((reqp * reqp) + (xeqp * xeqp));
              v2p = realLoad / (zeqp + rl) * v1;
              v2 = v2p / k;
              i2 = v2 / realLoad;
              i1 = v1 / (Math.Sqrt((ri * ri) + (xi * xi)));

              i1 = i2 / k;
              v24 = v2;
              i24 = i2;
              rl4 = realLoad;

              this.chart2.Series["Cur"].Points.Clear();
              this.chart2.Series["Cur"].Points.AddXY(rl0, v20);
              this.chart2.Series["Cur"].Points.AddXY(rl1, v21);
              this.chart2.Series["Cur"].Points.AddXY(rl2, v22);
              this.chart2.Series["Cur"].Points.AddXY(rl3, v23);
              this.chart2.Series["Cur"].Points.AddXY(rl4, v24);

            /* label14.Text = "V";
             label15.Text = "A";
             label16.Text = "A";
             label18.Text = "W";
             label20.Text = "W";
             //label21.Text = "V";*/

            double c, Vol, B, f, core_loss;

            double vv2, ii2, ii1, ccu_loss, ccore_loss, rrl;
            int gx1, gy1, gi1;
            int gx2, gy2;
            int gx3, gy3;
            int gx4, gy4;
            int gx5, gy5;

            p = 4 * T;
            //p = 2 * T + 2 * W;
            ri = p * pr * n1;
            r2 = p * pr * n2;
            xi = p * px * n1;
            //xi = 5 * ri;
            x2 = p * px * n2;
            //x2 = 2.3 * r2;
            k = n1 / n2;
            //k = n2 / n1;
            r1 = k * k * r2;
            //r1 = 0.014 * (ri / 1.014);
            x1 = k * k * x2;
            //x1 = 0.0584 * (xi / 1.00584);
            r0 = ri - r1;
            x0 = xi - x1;
            r2p = k * k * r2;
            x2p = k * k * x2;
            //r2p = r2 / ( k * k );
            //x2p = x2 / ( k * k );
            reqp = r1 + r2p;
            xeqp = x1 + x2p;
            zeqp = Math.Sqrt((reqp * reqp) + (xeqp * xeqp));
            v2p = rl / (zeqp + rl) * v1;
            v2 = v2p / k;
            i2 = v2 / rl;
            //i1 = v1 / (Math.Sqrt((ri * ri) + (xi * xi)));

            i1 = i2 / k;
            cu_loss = (i1 * i1 * r1) + (i2 * i2 * r2);
            //cu_loss = (v1 * v1 / r1) + (v2 * v2 / r2);
            //double c;
            c = v1 * i1 - v2 * i2;
            cu_loss = 0.9 * c;
            //double Vol;
            Vol = T * T * W * 4;
            //double B, f;
            B = 1.2;
            f = 50;
            //double core_loss;
            core_loss = Math.Pow(B, n) * Math.Pow(f, m) * K;
            core_loss = core_loss * Vol * 100;


            ////////////////////////
            ///send original data to another form
            vv2 = v2;
            ii2 = i2;
            ii1 = i1;
            ccu_loss = cu_loss;
            ccore_loss = core_loss;
            rrl = rl;


            gx1 = Convert.ToInt32(v2);
            gy1 = Convert.ToInt32(i2);
            ///////////////////////
            ///generate multiple value 1
            rl = rl + 50;
            v2 = 0;
            i2 = 0;


            p = 4 * T;
            ri = p * pr * n1;
            r2 = p * pr * n2;
            xi = p * px * n1;
            x2 = p * px * n2;
            k = n1 / n2;
            r1 = k * k * r2;
            x1 = k * k * x2;
            r0 = ri - r1;
            x0 = xi - x1;
            r2p = k * k * r2;
            x2p = k * k * x2;
            reqp = r1 + r2p;
            xeqp = x1 + x2p;
            zeqp = Math.Sqrt((reqp * reqp) + (xeqp * xeqp));
            v2p = rl / (zeqp + rl) * v1;
            v2 = v2p / k;
            i2 = v2 / rl;
            i1 = i2 / k;
            cu_loss = (i1 * i1 * r1) + (i2 * i2 * r2);
            c = v1 * i1 - v2 * i2;
            cu_loss = 0.9 * c;
            Vol = T * T * W * 4;
            B = 1.2;
            f = 50;
            core_loss = Math.Pow(B, n) * Math.Pow(f, m) * K;
            core_loss = core_loss * Vol * 100;




            gx2 = Convert.ToInt32(v2);
            gy2 = Convert.ToInt32(i2);
            //////////////////////
            ///generate multiple value 2
            rl = rl + 50;
            v2 = 0;
            i2 = 0;

            p = 4 * T;
            ri = p * pr * n1;
            r2 = p * pr * n2;
            xi = p * px * n1;
            x2 = p * px * n2;
            k = n1 / n2;
            r1 = k * k * r2;
            x1 = k * k * x2;
            r0 = ri - r1;
            x0 = xi - x1;
            r2p = k * k * r2;
            x2p = k * k * x2;
            reqp = r1 + r2p;
            xeqp = x1 + x2p;
            zeqp = Math.Sqrt((reqp * reqp) + (xeqp * xeqp));
            v2p = rl / (zeqp + rl) * v1;
            v2 = v2p / k;
            i2 = v2 / rl;
            i1 = i2 / k;
            cu_loss = (i1 * i1 * r1) + (i2 * i2 * r2);
            c = v1 * i1 - v2 * i2;
            cu_loss = 0.9 * c;
            Vol = T * T * W * 4;
            B = 1.2;
            f = 50;
            core_loss = Math.Pow(B, n) * Math.Pow(f, m) * K;
            core_loss = core_loss * Vol * 100;



            gx3 = Convert.ToInt32(v2);
            gy3 = Convert.ToInt32(i2);
            //////////////////////

            ///generate multiple value 3
            rl = rl + 50;
            v2 = 0;
            i2 = 0;

            p = 4 * T;
            ri = p * pr * n1;
            r2 = p * pr * n2;
            xi = p * px * n1;
            x2 = p * px * n2;
            k = n1 / n2;
            r1 = k * k * r2;
            x1 = k * k * x2;
            r0 = ri - r1;
            x0 = xi - x1;
            r2p = k * k * r2;
            x2p = k * k * x2;
            reqp = r1 + r2p;
            xeqp = x1 + x2p;
            zeqp = Math.Sqrt((reqp * reqp) + (xeqp * xeqp));
            v2p = rl / (zeqp + rl) * v1;
            v2 = v2p / k;
            i2 = v2 / rl;
            i1 = i2 / k;
            cu_loss = (i1 * i1 * r1) + (i2 * i2 * r2);
            c = v1 * i1 - v2 * i2;
            cu_loss = 0.9 * c;
            Vol = T * T * W * 4;
            B = 1.2;
            f = 50;
            core_loss = Math.Pow(B, n) * Math.Pow(f, m) * K;
            core_loss = core_loss * Vol * 100;




            gx4 = Convert.ToInt32(v2);
            gy4 = Convert.ToInt32(i2);
            //////////////////////



            ///generate multiple value 3
            rl = rl + 50;
            v2 = 0;
            i2 = 0;

            p = 4 * T;
            ri = p * pr * n1;
            r2 = p * pr * n2;
            xi = p * px * n1;
            x2 = p * px * n2;
            k = n1 / n2;
            r1 = k * k * r2;
            x1 = k * k * x2;
            r0 = ri - r1;
            x0 = xi - x1;
            r2p = k * k * r2;
            x2p = k * k * x2;
            reqp = r1 + r2p;
            xeqp = x1 + x2p;
            zeqp = Math.Sqrt((reqp * reqp) + (xeqp * xeqp));
            v2p = rl / (zeqp + rl) * v1;
            v2 = v2p / k;
            i2 = v2 / rl;
            i1 = i2 / k;
            cu_loss = (i1 * i1 * r1) + (i2 * i2 * r2);
            c = v1 * i1 - v2 * i2;
            cu_loss = 0.9 * c;
            Vol = T * T * W * 4;
            B = 1.2;
            f = 50;
            core_loss = Math.Pow(B, n) * Math.Pow(f, m) * K;
            core_loss = core_loss * Vol * 100;


            gx5 = Convert.ToInt32(v2);
            gy5 = Convert.ToInt32(i2);
            //////////////////////////////////////


            int gr1, gr2, gr3, gr4, gr5;
            gr1 = Convert.ToInt32(rrl);
            gr2 = gr1 + 200;
            gr3 = gr2 + 200;
            gr4 = gr3 + 200;
            gr5 = gr4 + 200;
            //label22.Text = n1.ToString();
            //label23.Text = n2.ToString();
            //label24.Text = v1.ToString();
            //label25.Text = rl.ToString();

            LblScyV.Text = v2.ToString("0.### V");
            LblPriC.Text = i1.ToString("0.### A");
            LblScyC.Text = i2.ToString("0.### A");
            LblCuLoss.Text = cu_loss.ToString("0.### W");
            LblCoreLoss.Text = core_loss.ToString("0.### W");


            // label13.Text = v20.ToString();
            // label21.Text = v21.ToString();
            // label26.Text = v22.ToString();
            // label27.Text = v23.ToString();
            // label28.Text = v24.ToString();

            this.chart2.Series["Cur"].Points.Clear();

            //this.chart2.Series["Cur"].Points.AddXY(rl0, v20);
            //this.chart2.Series["Cur"].Points.AddXY(rl1, v21);
            //this.chart2.Series["Cur"].Points.AddXY(rl2, v22);
            //this.chart2.Series["Cur"].Points.AddXY(rl3, v23);
            //this.chart2.Series["Cur"].Points.AddXY(rl4, v24);
            this.chart2.Series["Cur"].Points.AddXY(0, 0);
            this.chart2.Series["Cur"].Points.AddXY(gr1, gx1);
            this.chart2.Series["Cur"].Points.AddXY(gr2, gx2);
            this.chart2.Series["Cur"].Points.AddXY(gr3, gx3);
            this.chart2.Series["Cur"].Points.AddXY(gr4, gx4);
            this.chart2.Series["Cur"].Points.AddXY(gr5, gx5);

            count += 1;
            if (count <6)
            {
                VData vd = new VData(rl, v2);
                al.Add(vd);
                //listBox1.Items.Add(vd);
                //label13.Text = count.ToString();
            }
        }

       
    

        private void BtnGraph_Click(object sender, EventArgs e)
        {
            
            
            //listBox1.Items.Clear();

            //init Graph
            chart1.Series.Clear();
            chart1.Legends.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();

            //System.Windows.Forms.DataVisualization.Charting ch = new System.Windows.Forms.DataVisualization.Charting();

            chart1.Titles.Add("Voltage Regulation Curve");
            ChartArea ca =new ChartArea();

           
            //init the limit of the graph
            double xmn = 0;
            double xmx = 0;

            double ymn = 0;
            double ymx = 0;

            //Getting limit of the graph depending on value
            foreach (VData vd in al)
            {
                if(vd.getrl() > xmx)
                    xmx = vd.getrl();

                if (vd.getrl() < xmn)
                    xmn = vd.getrl();

                if (vd.getV2()> ymx)
                   ymx = vd.getV2();

                if (vd.getV2()< ymn)
                    ymn = vd.getV2();
            }
            //setting limit of the graph depending on value
            ca.AxisX.Minimum = xmn;
            ca.AxisX.Maximum = xmx+100;

            ca.AxisY.Minimum = ymn;
            ca.AxisY.Maximum = ymx+100;

            chart1.ChartAreas.Add(ca);
     
            System.Windows.Forms.DataVisualization.Charting.Series sr = new Series();
            sr.ChartType = SeriesChartType.Line; //line type chart
            foreach (VData vd in al)
            {
                sr.Points.AddXY(vd.getrl(), vd.getV2());
            }
            chart1.Series.Add(sr);

            al.Clear();//clearing the stored value
            count = 0;//clearing count
            //label13.Text = count.ToString();
        }

        private void BoxN1_Validated(object sender, EventArgs e)
        {
            chkButton();
        }
       

        private void BoxN2_Validated(object sender, EventArgs e)
        {
            chkButton();
        }

        private void BtnClr_Click(object sender, EventArgs e)
        {   
            al.Clear();
            count = 0;
            //label13.Text = count.ToString();
            chart2.Series["Cur"].Points.Clear();
            chart1.Series.Clear();
            chart1.Legends.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();

            foreach (Control c in groupBox1.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = " ";
                }
            }

            foreach (Control cd in groupBox2.Controls)
            {
                if (cd is TextBox)
                {
                    cd.Text = " ";
                }
            }

            label14.Text = "";
            label15.Text = "";
            label16.Text = "";
            label18.Text = "";
            label20.Text = "";
           // label21.Text = "";
            LblScyV.Text = "";
            LblPriC.Text = "";
            LblScyC.Text = "";
            LblCoreLoss.Text = "";
            LblCuLoss.Text = "";
            //LblVoltageReg.Text = "";


        }

        private void BoxV1_Validated(object sender, EventArgs e)
        {
            chkButton();
        }

        private void BoxRl_Validated(object sender, EventArgs e)
        {
            chkButton();
        }

        private void BoxWidth_Validated(object sender, EventArgs e)
        {
            chkButton();
        }

        private void BoxThick_Validated(object sender, EventArgs e)
        {
            chkButton();
        }

        private void ComboCore_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BoxThick_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void CombWind_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void BoxN1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void BoxN2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BoxV1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BoxRl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BoxWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BoxThick_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
    //data class
    public class VData
    {
        double _rl;
        double _v2;
        
        public  VData(double rl,double v2) {
            _rl = Convert.ToDouble(rl.ToString("0.##"));
            _v2 = Convert.ToDouble(v2.ToString("0.##"));
            
        }
        public double getrl() { return _rl; }
        public double getV2() { return _v2; }
        

        override
        public string ToString() {
            return _rl + "        " + _v2;
        }

    }
}
