using Prana.Global;
using Prana.LogManager;
using System;

namespace Prana.TradingTicket
{
    /// <summary>
    /// Summary description for AlgoTicket.
    /// </summary>
    public class AlgoTicket : System.Windows.Forms.UserControl
    {

        private const string FORM_NAME = "AlgoTicket : ";
        private System.Windows.Forms.Label lblSide;
        private System.Windows.Forms.Label lblTactic;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblSymbol;
        private System.Windows.Forms.Label lblOrderType;
        private System.Windows.Forms.Label lblTIF;
        private System.Windows.Forms.Label lblExecInstr;
        private System.Windows.Forms.Label lblHandInstr;
        private System.Windows.Forms.Label lblAcct;
        private System.Windows.Forms.Label lblTradingAcct;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbSide;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbTactic;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbOrderType;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbTIF;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbExec;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbHand;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbTrading;
        private System.Windows.Forms.TextBox txtSymbol;
        private System.Windows.Forms.NumericUpDown nudQuantity;
        private System.Windows.Forms.GroupBox grpTime;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.DateTimePicker txtStart;
        private System.Windows.Forms.DateTimePicker txtEnd;
        private System.Windows.Forms.GroupBox grpParticipation;
        private System.Windows.Forms.Label lblMaximum;
        private System.Windows.Forms.Label lblDiscr;
        private System.Windows.Forms.Label lblSlices;
        private System.Windows.Forms.Label lblCompleteOrder;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbExecution;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox grpVolume;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblDisplay;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbMin;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbMax;
        private System.Windows.Forms.NumericUpDown nudDisplay;
        private System.Windows.Forms.PictureBox picBernstein;
        private System.Windows.Forms.PictureBox picCreditSus;
        private System.Windows.Forms.PictureBox picPiperJaffray;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTrade;
        private System.Windows.Forms.Button btnClose;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbmaximum;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAcct;
        private AlgoTicketTypes _ticketType = AlgoTicketTypes.NotSet;
        public AlgoTicket()
        {
            try
            {
                // This call is required by the Windows.Forms Form Designer.
                InitializeComponent();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {

            }
        }

        public void TicketType(AlgoTicketTypes ticketType)
        {
            try
            {
                _ticketType = ticketType;
                switch (_ticketType)
                {
                    case AlgoTicketTypes.Bernstein:
                        picBernstein.Visible = true;

                        grpParticipation.Visible = true;
                        break;

                    case AlgoTicketTypes.CreditSusie:
                        grpVolume.Visible = true;
                        nudDisplay.Visible = true;
                        lblDisplay.Visible = true;


                        picCreditSus.Visible = true;
                        break;

                    case AlgoTicketTypes.PiperJaffray:
                        grpVolume.Visible = true;

                        lblDisplay.Visible = false;
                        nudDisplay.Visible = false;

                        picPiperJaffray.Visible = true;
                        break;
                    default:
                        grpParticipation.Visible = false;
                        grpVolume.Visible = false;
                        nudDisplay.Visible = false;
                        lblDisplay.Visible = false;

                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        //		public AlgoTicket(AlgoTicketTypes ticketType)
        //		{
        //			// This call is required by the Windows.Forms Form Designer.
        //			InitializeComponent();
        //			_ticketType = ticketType;
        //			switch (_ticketType)
        //			{
        //				case AlgoTicketTypes.Bernstein:
        //					picCreditSus.Visible = false;
        //					picPiperJaffray.Visible = false;
        //					picBernstein.Visible = true;
        //					grpParticipation.Visible = true;
        //					grpVolume.Visible = false;
        //					break;
        //				
        //				case AlgoTicketTypes.CreditSusie:
        //					picBernstein.Visible = false;
        //					picPiperJaffray.Visible = false;
        //					grpParticipation.Visible = false;
        //					grpVolume.Visible = true;
        //					picCreditSus.Visible = true;
        //					break;
        //				
        //				case AlgoTicketTypes.PiperJaffray:
        //					picCreditSus.Visible = false;
        //					picBernstein.Visible = false;
        //					grpParticipation.Visible = false;
        //					grpVolume.Visible = true;
        //					lblDisplay.Visible = false;
        //					nudDisplay.Visible = false;
        //					picPiperJaffray.Visible = true;
        //					break;
        //				default:
        //					break;
        //			}
        //			// TODO: Add any initialization after the InitializeComponent call
        //
        //		}

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (lblSide != null)
                {
                    lblSide.Dispose();
                }
                if (lblTactic != null)
                {
                    lblTactic.Dispose();
                }
                if (lblQuantity != null)
                {
                    lblQuantity.Dispose();
                }
                if (lblSymbol != null)
                {
                    lblSymbol.Dispose();
                }
                if (lblOrderType != null)
                {
                    lblOrderType.Dispose();
                }
                if (lblTIF != null)
                {
                    lblTIF.Dispose();
                }
                if (lblExecInstr != null)
                {
                    lblExecInstr.Dispose();
                }
                if (lblHandInstr != null)
                {
                    lblHandInstr.Dispose();
                }
                if (lblAcct != null)
                {
                    lblAcct.Dispose();
                }
                if (lblTradingAcct != null)
                {
                    lblTradingAcct.Dispose();
                }
                if (cmbSide != null)
                {
                    cmbSide.Dispose();
                }
                if (cmbTactic != null)
                {
                    cmbTactic.Dispose();
                }
                if (cmbOrderType != null)
                {
                    cmbOrderType.Dispose();
                }
                if (cmbTIF != null)
                {
                    cmbTIF.Dispose();
                }
                if (cmbExec != null)
                {
                    cmbExec.Dispose();
                }
                if (cmbHand != null)
                {
                    cmbHand.Dispose();
                }
                if (cmbTrading != null)
                {
                    cmbTrading.Dispose();
                }
                if (txtSymbol != null)
                {
                    txtSymbol.Dispose();
                }
                if (nudQuantity != null)
                {
                    nudQuantity.Dispose();
                }
                if (cmbAcct != null)
                {
                    cmbAcct.Dispose();
                }
                if (cmbmaximum != null)
                {
                    cmbmaximum.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnTrade != null)
                {
                    btnTrade.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (picPiperJaffray != null)
                {
                    picPiperJaffray.Dispose();
                }
                if (picCreditSus != null)
                {
                    picCreditSus.Dispose();
                }
                if (picBernstein != null)
                {
                    picBernstein.Dispose();
                }
                if (nudDisplay != null)
                {
                    nudDisplay.Dispose();
                }
                if (cmbMax != null)
                {
                    cmbMax.Dispose();
                }
                if (cmbMin != null)
                {
                    cmbMin.Dispose();
                }
                if (lblDisplay != null)
                {
                    lblDisplay.Dispose();
                }
                if (lblMax != null)
                {
                    lblMax.Dispose();
                }
                if (lblMin != null)
                {
                    lblMin.Dispose();
                }
                if (grpVolume != null)
                {
                    grpVolume.Dispose();
                }
                if (checkBox1 != null)
                {
                    checkBox1.Dispose();
                }
                if (cmbExecution != null)
                {
                    cmbExecution.Dispose();
                }
                if (textBox2 != null)
                {
                    textBox2.Dispose();
                }
                if (textBox1 != null)
                {
                    textBox1.Dispose();
                }
                if (radioButton2 != null)
                {
                    radioButton2.Dispose();
                }
                if (radioButton1 != null)
                {
                    radioButton1.Dispose();
                }
                if (lblCompleteOrder != null)
                {
                    lblCompleteOrder.Dispose();
                }
                if (lblSlices != null)
                {
                    lblSlices.Dispose();
                }
                if (lblDiscr != null)
                {
                    lblDiscr.Dispose();
                }
                if (lblMaximum != null)
                {
                    lblMaximum.Dispose();
                }
                if (grpParticipation != null)
                {
                    grpParticipation.Dispose();
                }
                if (txtEnd != null)
                {
                    txtEnd.Dispose();
                }
                if (txtStart != null)
                {
                    txtStart.Dispose();
                }
                if (lblEnd != null)
                {
                    lblEnd.Dispose();
                }
                if (lblStart != null)
                {
                    lblStart.Dispose();
                }
                if (grpTime != null)
                {
                    grpTime.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand6 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand7 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand8 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand9 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance108 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance109 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance110 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance111 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance112 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance113 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance114 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance115 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance116 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance117 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance118 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance119 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance120 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance121 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance122 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance123 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance124 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance125 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance126 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance127 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance128 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance129 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance130 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance131 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance132 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlgoTicket));
            Infragistics.Win.Appearance appearance133 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand10 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance134 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance135 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance136 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance137 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance138 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance139 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance140 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance141 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance142 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance143 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance144 = new Infragistics.Win.Appearance();
            this.nudQuantity = new System.Windows.Forms.NumericUpDown();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.cmbTrading = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbHand = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbExec = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbTIF = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbOrderType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbTactic = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbSide = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblTradingAcct = new System.Windows.Forms.Label();
            this.lblAcct = new System.Windows.Forms.Label();
            this.lblHandInstr = new System.Windows.Forms.Label();
            this.lblExecInstr = new System.Windows.Forms.Label();
            this.lblTIF = new System.Windows.Forms.Label();
            this.lblOrderType = new System.Windows.Forms.Label();
            this.lblSymbol = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblTactic = new System.Windows.Forms.Label();
            this.lblSide = new System.Windows.Forms.Label();
            this.grpTime = new System.Windows.Forms.GroupBox();
            this.txtEnd = new System.Windows.Forms.DateTimePicker();
            this.txtStart = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.grpParticipation = new System.Windows.Forms.GroupBox();
            this.cmbmaximum = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.lblSlices = new System.Windows.Forms.Label();
            this.lblDiscr = new System.Windows.Forms.Label();
            this.lblMaximum = new System.Windows.Forms.Label();
            this.lblCompleteOrder = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cmbExecution = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.grpVolume = new System.Windows.Forms.GroupBox();
            this.cmbMax = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbMin = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblDisplay = new System.Windows.Forms.Label();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.nudDisplay = new System.Windows.Forms.NumericUpDown();
            this.picBernstein = new System.Windows.Forms.PictureBox();
            this.picCreditSus = new System.Windows.Forms.PictureBox();
            this.picPiperJaffray = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTrade = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmbAcct = new Infragistics.Win.UltraWinGrid.UltraCombo();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTactic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSide)).BeginInit();
            this.grpTime.SuspendLayout();
            this.grpParticipation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbmaximum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExecution)).BeginInit();
            this.grpVolume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBernstein)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCreditSus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPiperJaffray)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAcct)).BeginInit();
            this.SuspendLayout();
            // 
            // nudQuantity
            // 
            this.nudQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudQuantity.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudQuantity.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudQuantity.Location = new System.Drawing.Point(180, 16);
            this.nudQuantity.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudQuantity.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudQuantity.Name = "nudQuantity";
            this.nudQuantity.Size = new System.Drawing.Size(78, 21);
            this.nudQuantity.TabIndex = 19;
            this.nudQuantity.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // txtSymbol
            // 
            this.txtSymbol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtSymbol.Location = new System.Drawing.Point(98, 16);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(78, 21);
            this.txtSymbol.TabIndex = 17;
            this.txtSymbol.Text = "IBM";
            // 
            // cmbTrading
            // 
            this.cmbTrading.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTrading.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTrading.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbTrading.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbTrading.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTrading.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTrading.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTrading.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbTrading.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTrading.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbTrading.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTrading.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTrading.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTrading.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbTrading.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbTrading.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTrading.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTrading.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbTrading.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTrading.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTrading.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbTrading.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbTrading.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTrading.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbTrading.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbTrading.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTrading.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbTrading.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTrading.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTrading.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTrading.DisplayMember = "";
            this.cmbTrading.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTrading.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTrading.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbTrading.Location = new System.Drawing.Point(346, 52);
            this.cmbTrading.Name = "cmbTrading";
            this.cmbTrading.Size = new System.Drawing.Size(78, 21);
            this.cmbTrading.TabIndex = 16;
            this.cmbTrading.ValueMember = "";
            // 
            // cmbHand
            // 
            this.cmbHand.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbHand.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbHand.DisplayLayout.Appearance = appearance13;
            ultraGridBand2.ColHeadersVisible = false;
            this.cmbHand.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbHand.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbHand.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbHand.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbHand.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbHand.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbHand.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbHand.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbHand.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbHand.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbHand.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbHand.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbHand.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbHand.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbHand.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbHand.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbHand.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbHand.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbHand.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbHand.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbHand.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbHand.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbHand.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbHand.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbHand.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbHand.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbHand.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbHand.DisplayMember = "";
            this.cmbHand.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbHand.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbHand.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbHand.Location = new System.Drawing.Point(180, 52);
            this.cmbHand.Name = "cmbHand";
            this.cmbHand.Size = new System.Drawing.Size(78, 21);
            this.cmbHand.TabIndex = 15;
            this.cmbHand.ValueMember = "";
            // 
            // cmbExec
            // 
            this.cmbExec.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbExec.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbExec.DisplayLayout.Appearance = appearance25;
            ultraGridBand3.ColHeadersVisible = false;
            this.cmbExec.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbExec.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbExec.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbExec.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbExec.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmbExec.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbExec.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmbExec.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbExec.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbExec.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbExec.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmbExec.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbExec.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmbExec.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbExec.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmbExec.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbExec.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbExec.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbExec.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmbExec.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbExec.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmbExec.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmbExec.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbExec.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmbExec.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbExec.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbExec.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbExec.DisplayMember = "";
            this.cmbExec.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbExec.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbExec.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbExec.Location = new System.Drawing.Point(98, 52);
            this.cmbExec.Name = "cmbExec";
            this.cmbExec.Size = new System.Drawing.Size(78, 21);
            this.cmbExec.TabIndex = 14;
            this.cmbExec.ValueMember = "";
            // 
            // cmbTIF
            // 
            this.cmbTIF.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTIF.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTIF.DisplayLayout.Appearance = appearance37;
            ultraGridBand4.ColHeadersVisible = false;
            this.cmbTIF.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbTIF.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTIF.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTIF.DisplayLayout.GroupByBox.Appearance = appearance38;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTIF.DisplayLayout.GroupByBox.BandLabelAppearance = appearance39;
            this.cmbTIF.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance40.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTIF.DisplayLayout.GroupByBox.PromptAppearance = appearance40;
            this.cmbTIF.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTIF.DisplayLayout.MaxRowScrollRegions = 1;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTIF.DisplayLayout.Override.ActiveCellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.SystemColors.Highlight;
            appearance42.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTIF.DisplayLayout.Override.ActiveRowAppearance = appearance42;
            this.cmbTIF.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbTIF.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTIF.DisplayLayout.Override.CardAreaAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTIF.DisplayLayout.Override.CellAppearance = appearance44;
            this.cmbTIF.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTIF.DisplayLayout.Override.CellPadding = 0;
            appearance45.BackColor = System.Drawing.SystemColors.Control;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTIF.DisplayLayout.Override.GroupByRowAppearance = appearance45;
            appearance46.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbTIF.DisplayLayout.Override.HeaderAppearance = appearance46;
            this.cmbTIF.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTIF.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.BorderColor = System.Drawing.Color.Silver;
            this.cmbTIF.DisplayLayout.Override.RowAppearance = appearance47;
            this.cmbTIF.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance48.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTIF.DisplayLayout.Override.TemplateAddRowAppearance = appearance48;
            this.cmbTIF.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTIF.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTIF.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTIF.DisplayMember = "";
            this.cmbTIF.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTIF.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTIF.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbTIF.Location = new System.Drawing.Point(16, 52);
            this.cmbTIF.Name = "cmbTIF";
            this.cmbTIF.Size = new System.Drawing.Size(78, 21);
            this.cmbTIF.TabIndex = 13;
            this.cmbTIF.ValueMember = "";
            // 
            // cmbOrderType
            // 
            this.cmbOrderType.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOrderType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance49.BackColor = System.Drawing.SystemColors.Window;
            appearance49.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbOrderType.DisplayLayout.Appearance = appearance49;
            ultraGridBand5.ColHeadersVisible = false;
            this.cmbOrderType.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.cmbOrderType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOrderType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance50.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance50.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance50.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance50.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderType.DisplayLayout.GroupByBox.Appearance = appearance50;
            appearance51.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance51;
            this.cmbOrderType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance52.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance52.BackColor2 = System.Drawing.SystemColors.Control;
            appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance52.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderType.DisplayLayout.GroupByBox.PromptAppearance = appearance52;
            this.cmbOrderType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbOrderType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance53.BackColor = System.Drawing.SystemColors.Window;
            appearance53.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbOrderType.DisplayLayout.Override.ActiveCellAppearance = appearance53;
            appearance54.BackColor = System.Drawing.SystemColors.Highlight;
            appearance54.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbOrderType.DisplayLayout.Override.ActiveRowAppearance = appearance54;
            this.cmbOrderType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbOrderType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance55.BackColor = System.Drawing.SystemColors.Window;
            this.cmbOrderType.DisplayLayout.Override.CardAreaAppearance = appearance55;
            appearance56.BorderColor = System.Drawing.Color.Silver;
            appearance56.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbOrderType.DisplayLayout.Override.CellAppearance = appearance56;
            this.cmbOrderType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbOrderType.DisplayLayout.Override.CellPadding = 0;
            appearance57.BackColor = System.Drawing.SystemColors.Control;
            appearance57.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance57.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance57.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderType.DisplayLayout.Override.GroupByRowAppearance = appearance57;
            appearance58.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbOrderType.DisplayLayout.Override.HeaderAppearance = appearance58;
            this.cmbOrderType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbOrderType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance59.BackColor = System.Drawing.SystemColors.Window;
            appearance59.BorderColor = System.Drawing.Color.Silver;
            this.cmbOrderType.DisplayLayout.Override.RowAppearance = appearance59;
            this.cmbOrderType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance60.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbOrderType.DisplayLayout.Override.TemplateAddRowAppearance = appearance60;
            this.cmbOrderType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbOrderType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbOrderType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbOrderType.DisplayMember = "";
            this.cmbOrderType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbOrderType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbOrderType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbOrderType.Location = new System.Drawing.Point(346, 16);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Size = new System.Drawing.Size(78, 21);
            this.cmbOrderType.TabIndex = 12;
            this.cmbOrderType.ValueMember = "";
            // 
            // cmbTactic
            // 
            this.cmbTactic.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTactic.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance61.BackColor = System.Drawing.SystemColors.Window;
            appearance61.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTactic.DisplayLayout.Appearance = appearance61;
            ultraGridBand6.ColHeadersVisible = false;
            this.cmbTactic.DisplayLayout.BandsSerializer.Add(ultraGridBand6);
            this.cmbTactic.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTactic.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance62.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance62.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance62.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTactic.DisplayLayout.GroupByBox.Appearance = appearance62;
            appearance63.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTactic.DisplayLayout.GroupByBox.BandLabelAppearance = appearance63;
            this.cmbTactic.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance64.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance64.BackColor2 = System.Drawing.SystemColors.Control;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance64.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTactic.DisplayLayout.GroupByBox.PromptAppearance = appearance64;
            this.cmbTactic.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTactic.DisplayLayout.MaxRowScrollRegions = 1;
            appearance65.BackColor = System.Drawing.SystemColors.Window;
            appearance65.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTactic.DisplayLayout.Override.ActiveCellAppearance = appearance65;
            appearance66.BackColor = System.Drawing.SystemColors.Highlight;
            appearance66.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTactic.DisplayLayout.Override.ActiveRowAppearance = appearance66;
            this.cmbTactic.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbTactic.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance67.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTactic.DisplayLayout.Override.CardAreaAppearance = appearance67;
            appearance68.BorderColor = System.Drawing.Color.Silver;
            appearance68.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTactic.DisplayLayout.Override.CellAppearance = appearance68;
            this.cmbTactic.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTactic.DisplayLayout.Override.CellPadding = 0;
            appearance69.BackColor = System.Drawing.SystemColors.Control;
            appearance69.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance69.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance69.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance69.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTactic.DisplayLayout.Override.GroupByRowAppearance = appearance69;
            appearance70.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbTactic.DisplayLayout.Override.HeaderAppearance = appearance70;
            this.cmbTactic.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTactic.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance71.BackColor = System.Drawing.SystemColors.Window;
            appearance71.BorderColor = System.Drawing.Color.Silver;
            this.cmbTactic.DisplayLayout.Override.RowAppearance = appearance71;
            this.cmbTactic.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance72.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTactic.DisplayLayout.Override.TemplateAddRowAppearance = appearance72;
            this.cmbTactic.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTactic.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTactic.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTactic.DisplayMember = "";
            this.cmbTactic.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTactic.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTactic.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbTactic.Location = new System.Drawing.Point(264, 16);
            this.cmbTactic.Name = "cmbTactic";
            this.cmbTactic.Size = new System.Drawing.Size(78, 21);
            this.cmbTactic.TabIndex = 11;
            this.cmbTactic.ValueMember = "";
            // 
            // cmbSide
            // 
            this.cmbSide.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSide.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance73.BackColor = System.Drawing.SystemColors.Window;
            appearance73.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSide.DisplayLayout.Appearance = appearance73;
            ultraGridBand7.ColHeadersVisible = false;
            this.cmbSide.DisplayLayout.BandsSerializer.Add(ultraGridBand7);
            this.cmbSide.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSide.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance74.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance74.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance74.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance74.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.GroupByBox.Appearance = appearance74;
            appearance75.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSide.DisplayLayout.GroupByBox.BandLabelAppearance = appearance75;
            this.cmbSide.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance76.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance76.BackColor2 = System.Drawing.SystemColors.Control;
            appearance76.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance76.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSide.DisplayLayout.GroupByBox.PromptAppearance = appearance76;
            this.cmbSide.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSide.DisplayLayout.MaxRowScrollRegions = 1;
            appearance77.BackColor = System.Drawing.SystemColors.Window;
            appearance77.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSide.DisplayLayout.Override.ActiveCellAppearance = appearance77;
            appearance78.BackColor = System.Drawing.SystemColors.Highlight;
            appearance78.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSide.DisplayLayout.Override.ActiveRowAppearance = appearance78;
            this.cmbSide.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSide.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance79.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.Override.CardAreaAppearance = appearance79;
            appearance80.BorderColor = System.Drawing.Color.Silver;
            appearance80.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSide.DisplayLayout.Override.CellAppearance = appearance80;
            this.cmbSide.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSide.DisplayLayout.Override.CellPadding = 0;
            appearance81.BackColor = System.Drawing.SystemColors.Control;
            appearance81.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance81.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance81.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance81.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.Override.GroupByRowAppearance = appearance81;
            appearance82.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbSide.DisplayLayout.Override.HeaderAppearance = appearance82;
            this.cmbSide.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSide.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance83.BackColor = System.Drawing.SystemColors.Window;
            appearance83.BorderColor = System.Drawing.Color.Silver;
            this.cmbSide.DisplayLayout.Override.RowAppearance = appearance83;
            this.cmbSide.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance84.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSide.DisplayLayout.Override.TemplateAddRowAppearance = appearance84;
            this.cmbSide.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSide.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSide.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSide.DisplayMember = "";
            this.cmbSide.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSide.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbSide.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbSide.Location = new System.Drawing.Point(16, 16);
            this.cmbSide.Name = "cmbSide";
            this.cmbSide.Size = new System.Drawing.Size(78, 21);
            this.cmbSide.TabIndex = 10;
            this.cmbSide.ValueMember = "";
            // 
            // lblTradingAcct
            // 
            this.lblTradingAcct.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTradingAcct.Location = new System.Drawing.Point(346, 38);
            this.lblTradingAcct.Name = "lblTradingAcct";
            this.lblTradingAcct.Size = new System.Drawing.Size(70, 14);
            this.lblTradingAcct.TabIndex = 9;
            this.lblTradingAcct.Text = "Trading Acct";
            // 
            // lblAcct
            // 
            this.lblAcct.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblAcct.Location = new System.Drawing.Point(262, 38);
            this.lblAcct.Name = "lblAcct";
            this.lblAcct.Size = new System.Drawing.Size(64, 14);
            this.lblAcct.TabIndex = 8;
            this.lblAcct.Text = "Acct/BrKrID";
            // 
            // lblHandInstr
            // 
            this.lblHandInstr.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHandInstr.Location = new System.Drawing.Point(180, 38);
            this.lblHandInstr.Name = "lblHandInstr";
            this.lblHandInstr.Size = new System.Drawing.Size(60, 14);
            this.lblHandInstr.TabIndex = 7;
            this.lblHandInstr.Text = "Hand Instr";
            // 
            // lblExecInstr
            // 
            this.lblExecInstr.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblExecInstr.Location = new System.Drawing.Point(98, 38);
            this.lblExecInstr.Name = "lblExecInstr";
            this.lblExecInstr.Size = new System.Drawing.Size(54, 14);
            this.lblExecInstr.TabIndex = 6;
            this.lblExecInstr.Text = "Exec Instr";
            // 
            // lblTIF
            // 
            this.lblTIF.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTIF.Location = new System.Drawing.Point(16, 38);
            this.lblTIF.Name = "lblTIF";
            this.lblTIF.Size = new System.Drawing.Size(24, 14);
            this.lblTIF.TabIndex = 5;
            this.lblTIF.Text = "TIF";
            // 
            // lblOrderType
            // 
            this.lblOrderType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblOrderType.Location = new System.Drawing.Point(346, 4);
            this.lblOrderType.Name = "lblOrderType";
            this.lblOrderType.Size = new System.Drawing.Size(62, 14);
            this.lblOrderType.TabIndex = 4;
            this.lblOrderType.Text = "Order Type";
            // 
            // lblSymbol
            // 
            this.lblSymbol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSymbol.Location = new System.Drawing.Point(98, 2);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(42, 14);
            this.lblSymbol.TabIndex = 3;
            this.lblSymbol.Text = "Symbol";
            // 
            // lblQuantity
            // 
            this.lblQuantity.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblQuantity.Location = new System.Drawing.Point(180, 2);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(54, 14);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Quantity";
            // 
            // lblTactic
            // 
            this.lblTactic.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTactic.Location = new System.Drawing.Point(264, 2);
            this.lblTactic.Name = "lblTactic";
            this.lblTactic.Size = new System.Drawing.Size(36, 14);
            this.lblTactic.TabIndex = 1;
            this.lblTactic.Text = "Tactic";
            // 
            // lblSide
            // 
            this.lblSide.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSide.Location = new System.Drawing.Point(16, 2);
            this.lblSide.Name = "lblSide";
            this.lblSide.Size = new System.Drawing.Size(26, 16);
            this.lblSide.TabIndex = 0;
            this.lblSide.Text = "Side";
            // 
            // grpTime
            // 
            this.grpTime.Controls.Add(this.txtEnd);
            this.grpTime.Controls.Add(this.txtStart);
            this.grpTime.Controls.Add(this.lblEnd);
            this.grpTime.Controls.Add(this.lblStart);
            this.grpTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.grpTime.Location = new System.Drawing.Point(4, 74);
            this.grpTime.Name = "grpTime";
            this.grpTime.Size = new System.Drawing.Size(192, 66);
            this.grpTime.TabIndex = 1;
            this.grpTime.TabStop = false;
            this.grpTime.Text = "Time(EST)";
            // 
            // txtEnd
            // 
            this.txtEnd.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtEnd.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.txtEnd.Location = new System.Drawing.Point(72, 38);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.ShowUpDown = true;
            this.txtEnd.Size = new System.Drawing.Size(114, 21);
            this.txtEnd.TabIndex = 10;
            this.txtEnd.Value = new System.DateTime(2005, 7, 22, 23, 23, 20, 687);
            // 
            // txtStart
            // 
            this.txtStart.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.txtStart.Location = new System.Drawing.Point(72, 16);
            this.txtStart.Name = "txtStart";
            this.txtStart.ShowUpDown = true;
            this.txtStart.Size = new System.Drawing.Size(114, 21);
            this.txtStart.TabIndex = 9;
            this.txtStart.Value = new System.DateTime(2005, 7, 22, 23, 23, 20, 687);
            // 
            // lblEnd
            // 
            this.lblEnd.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblEnd.Location = new System.Drawing.Point(8, 40);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(52, 14);
            this.lblEnd.TabIndex = 1;
            this.lblEnd.Text = "End Time";
            // 
            // lblStart
            // 
            this.lblStart.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStart.Location = new System.Drawing.Point(8, 18);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(56, 16);
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "Start Time";
            // 
            // grpParticipation
            // 
            this.grpParticipation.Controls.Add(this.cmbmaximum);
            this.grpParticipation.Controls.Add(this.textBox2);
            this.grpParticipation.Controls.Add(this.textBox1);
            this.grpParticipation.Controls.Add(this.radioButton2);
            this.grpParticipation.Controls.Add(this.radioButton1);
            this.grpParticipation.Controls.Add(this.lblSlices);
            this.grpParticipation.Controls.Add(this.lblDiscr);
            this.grpParticipation.Controls.Add(this.lblMaximum);
            this.grpParticipation.Controls.Add(this.lblCompleteOrder);
            this.grpParticipation.Controls.Add(this.checkBox1);
            this.grpParticipation.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.grpParticipation.Location = new System.Drawing.Point(200, 74);
            this.grpParticipation.Name = "grpParticipation";
            this.grpParticipation.Size = new System.Drawing.Size(230, 94);
            this.grpParticipation.TabIndex = 2;
            this.grpParticipation.TabStop = false;
            this.grpParticipation.Text = "Paricipation Level";
            this.grpParticipation.Visible = false;
            // 
            // cmbmaximum
            // 
            this.cmbmaximum.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbmaximum.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance85.BackColor = System.Drawing.SystemColors.Window;
            appearance85.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbmaximum.DisplayLayout.Appearance = appearance85;
            ultraGridBand8.ColHeadersVisible = false;
            this.cmbmaximum.DisplayLayout.BandsSerializer.Add(ultraGridBand8);
            this.cmbmaximum.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbmaximum.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance86.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance86.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance86.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance86.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbmaximum.DisplayLayout.GroupByBox.Appearance = appearance86;
            appearance87.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbmaximum.DisplayLayout.GroupByBox.BandLabelAppearance = appearance87;
            this.cmbmaximum.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance88.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance88.BackColor2 = System.Drawing.SystemColors.Control;
            appearance88.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance88.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbmaximum.DisplayLayout.GroupByBox.PromptAppearance = appearance88;
            this.cmbmaximum.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbmaximum.DisplayLayout.MaxRowScrollRegions = 1;
            appearance89.BackColor = System.Drawing.SystemColors.Window;
            appearance89.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbmaximum.DisplayLayout.Override.ActiveCellAppearance = appearance89;
            appearance90.BackColor = System.Drawing.SystemColors.Highlight;
            appearance90.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbmaximum.DisplayLayout.Override.ActiveRowAppearance = appearance90;
            this.cmbmaximum.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbmaximum.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance91.BackColor = System.Drawing.SystemColors.Window;
            this.cmbmaximum.DisplayLayout.Override.CardAreaAppearance = appearance91;
            appearance92.BorderColor = System.Drawing.Color.Silver;
            appearance92.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbmaximum.DisplayLayout.Override.CellAppearance = appearance92;
            this.cmbmaximum.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbmaximum.DisplayLayout.Override.CellPadding = 0;
            appearance93.BackColor = System.Drawing.SystemColors.Control;
            appearance93.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance93.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance93.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance93.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbmaximum.DisplayLayout.Override.GroupByRowAppearance = appearance93;
            appearance94.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbmaximum.DisplayLayout.Override.HeaderAppearance = appearance94;
            this.cmbmaximum.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbmaximum.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance95.BackColor = System.Drawing.SystemColors.Window;
            appearance95.BorderColor = System.Drawing.Color.Silver;
            this.cmbmaximum.DisplayLayout.Override.RowAppearance = appearance95;
            this.cmbmaximum.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance96.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbmaximum.DisplayLayout.Override.TemplateAddRowAppearance = appearance96;
            this.cmbmaximum.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbmaximum.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbmaximum.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbmaximum.DisplayMember = "";
            this.cmbmaximum.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbmaximum.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbmaximum.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbmaximum.Location = new System.Drawing.Point(144, 12);
            this.cmbmaximum.Name = "cmbmaximum";
            this.cmbmaximum.Size = new System.Drawing.Size(70, 21);
            this.cmbmaximum.TabIndex = 20;
            this.cmbmaximum.ValueMember = "";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox2.Location = new System.Drawing.Point(144, 56);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(72, 21);
            this.textBox2.TabIndex = 19;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox1.Location = new System.Drawing.Point(144, 34);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(72, 21);
            this.textBox1.TabIndex = 18;
            // 
            // radioButton2
            // 
            this.radioButton2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.radioButton2.Location = new System.Drawing.Point(94, 36);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(44, 14);
            this.radioButton2.TabIndex = 7;
            this.radioButton2.Text = "BP";
            // 
            // radioButton1
            // 
            this.radioButton1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.radioButton1.Location = new System.Drawing.Point(44, 36);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(44, 14);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.Text = "CPS";
            // 
            // lblSlices
            // 
            this.lblSlices.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSlices.Location = new System.Drawing.Point(8, 58);
            this.lblSlices.Name = "lblSlices";
            this.lblSlices.Size = new System.Drawing.Size(34, 16);
            this.lblSlices.TabIndex = 4;
            this.lblSlices.Text = "Slices";
            // 
            // lblDiscr
            // 
            this.lblDiscr.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblDiscr.Location = new System.Drawing.Point(8, 38);
            this.lblDiscr.Name = "lblDiscr";
            this.lblDiscr.Size = new System.Drawing.Size(32, 12);
            this.lblDiscr.TabIndex = 1;
            this.lblDiscr.Text = "Discr";
            // 
            // lblMaximum
            // 
            this.lblMaximum.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMaximum.Location = new System.Drawing.Point(8, 14);
            this.lblMaximum.Name = "lblMaximum";
            this.lblMaximum.Size = new System.Drawing.Size(58, 16);
            this.lblMaximum.TabIndex = 0;
            this.lblMaximum.Text = "Maximum";
            // 
            // lblCompleteOrder
            // 
            this.lblCompleteOrder.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCompleteOrder.Location = new System.Drawing.Point(8, 78);
            this.lblCompleteOrder.Name = "lblCompleteOrder";
            this.lblCompleteOrder.Size = new System.Drawing.Size(86, 14);
            this.lblCompleteOrder.TabIndex = 5;
            this.lblCompleteOrder.Text = "Complete Order";
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(144, 78);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(16, 16);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.Text = "checkBox1";
            // 
            // cmbExecution
            // 
            this.cmbExecution.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbExecution.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance97.BackColor = System.Drawing.SystemColors.Window;
            appearance97.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbExecution.DisplayLayout.Appearance = appearance97;
            ultraGridBand9.ColHeadersVisible = false;
            this.cmbExecution.DisplayLayout.BandsSerializer.Add(ultraGridBand9);
            this.cmbExecution.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbExecution.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance98.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance98.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance98.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance98.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbExecution.DisplayLayout.GroupByBox.Appearance = appearance98;
            appearance99.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbExecution.DisplayLayout.GroupByBox.BandLabelAppearance = appearance99;
            this.cmbExecution.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance100.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance100.BackColor2 = System.Drawing.SystemColors.Control;
            appearance100.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance100.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbExecution.DisplayLayout.GroupByBox.PromptAppearance = appearance100;
            this.cmbExecution.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbExecution.DisplayLayout.MaxRowScrollRegions = 1;
            appearance101.BackColor = System.Drawing.SystemColors.Window;
            appearance101.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbExecution.DisplayLayout.Override.ActiveCellAppearance = appearance101;
            appearance102.BackColor = System.Drawing.SystemColors.Highlight;
            appearance102.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbExecution.DisplayLayout.Override.ActiveRowAppearance = appearance102;
            this.cmbExecution.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbExecution.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance103.BackColor = System.Drawing.SystemColors.Window;
            this.cmbExecution.DisplayLayout.Override.CardAreaAppearance = appearance103;
            appearance104.BorderColor = System.Drawing.Color.Silver;
            appearance104.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbExecution.DisplayLayout.Override.CellAppearance = appearance104;
            this.cmbExecution.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbExecution.DisplayLayout.Override.CellPadding = 0;
            appearance105.BackColor = System.Drawing.SystemColors.Control;
            appearance105.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance105.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance105.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance105.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbExecution.DisplayLayout.Override.GroupByRowAppearance = appearance105;
            appearance106.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbExecution.DisplayLayout.Override.HeaderAppearance = appearance106;
            this.cmbExecution.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbExecution.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance107.BackColor = System.Drawing.SystemColors.Window;
            appearance107.BorderColor = System.Drawing.Color.Silver;
            this.cmbExecution.DisplayLayout.Override.RowAppearance = appearance107;
            this.cmbExecution.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance108.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbExecution.DisplayLayout.Override.TemplateAddRowAppearance = appearance108;
            this.cmbExecution.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbExecution.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbExecution.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbExecution.DisplayMember = "";
            this.cmbExecution.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbExecution.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbExecution.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbExecution.Location = new System.Drawing.Point(110, 146);
            this.cmbExecution.Name = "cmbExecution";
            this.cmbExecution.Size = new System.Drawing.Size(78, 21);
            this.cmbExecution.TabIndex = 17;
            this.cmbExecution.ValueMember = "";
            // 
            // grpVolume
            // 
            this.grpVolume.Controls.Add(this.cmbMax);
            this.grpVolume.Controls.Add(this.cmbMin);
            this.grpVolume.Controls.Add(this.lblDisplay);
            this.grpVolume.Controls.Add(this.lblMax);
            this.grpVolume.Controls.Add(this.lblMin);
            this.grpVolume.Controls.Add(this.nudDisplay);
            this.grpVolume.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.grpVolume.Location = new System.Drawing.Point(200, 74);
            this.grpVolume.Name = "grpVolume";
            this.grpVolume.Size = new System.Drawing.Size(230, 94);
            this.grpVolume.TabIndex = 4;
            this.grpVolume.TabStop = false;
            this.grpVolume.Text = "Volume";
            this.grpVolume.Visible = false;
            // 
            // cmbMax
            // 
            this.cmbMax.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbMax.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance109.BackColor = System.Drawing.SystemColors.Window;
            appearance109.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbMax.DisplayLayout.Appearance = appearance109;
            this.cmbMax.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbMax.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance110.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance110.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance110.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance110.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbMax.DisplayLayout.GroupByBox.Appearance = appearance110;
            appearance111.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMax.DisplayLayout.GroupByBox.BandLabelAppearance = appearance111;
            this.cmbMax.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance112.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance112.BackColor2 = System.Drawing.SystemColors.Control;
            appearance112.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance112.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMax.DisplayLayout.GroupByBox.PromptAppearance = appearance112;
            this.cmbMax.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbMax.DisplayLayout.MaxRowScrollRegions = 1;
            appearance113.BackColor = System.Drawing.SystemColors.Window;
            appearance113.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbMax.DisplayLayout.Override.ActiveCellAppearance = appearance113;
            appearance114.BackColor = System.Drawing.SystemColors.Highlight;
            appearance114.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbMax.DisplayLayout.Override.ActiveRowAppearance = appearance114;
            this.cmbMax.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbMax.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance115.BackColor = System.Drawing.SystemColors.Window;
            this.cmbMax.DisplayLayout.Override.CardAreaAppearance = appearance115;
            appearance116.BorderColor = System.Drawing.Color.Silver;
            appearance116.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbMax.DisplayLayout.Override.CellAppearance = appearance116;
            this.cmbMax.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbMax.DisplayLayout.Override.CellPadding = 0;
            appearance117.BackColor = System.Drawing.SystemColors.Control;
            appearance117.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance117.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance117.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance117.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbMax.DisplayLayout.Override.GroupByRowAppearance = appearance117;
            appearance118.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbMax.DisplayLayout.Override.HeaderAppearance = appearance118;
            this.cmbMax.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbMax.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance119.BackColor = System.Drawing.SystemColors.Window;
            appearance119.BorderColor = System.Drawing.Color.Silver;
            this.cmbMax.DisplayLayout.Override.RowAppearance = appearance119;
            this.cmbMax.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance120.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbMax.DisplayLayout.Override.TemplateAddRowAppearance = appearance120;
            this.cmbMax.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbMax.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbMax.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbMax.DisplayMember = "";
            this.cmbMax.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbMax.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbMax.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMax.Location = new System.Drawing.Point(80, 37);
            this.cmbMax.Name = "cmbMax";
            this.cmbMax.Size = new System.Drawing.Size(120, 21);
            this.cmbMax.TabIndex = 22;
            this.cmbMax.ValueMember = "";
            // 
            // cmbMin
            // 
            this.cmbMin.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbMin.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance121.BackColor = System.Drawing.SystemColors.Window;
            appearance121.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbMin.DisplayLayout.Appearance = appearance121;
            this.cmbMin.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbMin.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance122.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance122.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance122.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance122.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbMin.DisplayLayout.GroupByBox.Appearance = appearance122;
            appearance123.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMin.DisplayLayout.GroupByBox.BandLabelAppearance = appearance123;
            this.cmbMin.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance124.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance124.BackColor2 = System.Drawing.SystemColors.Control;
            appearance124.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance124.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbMin.DisplayLayout.GroupByBox.PromptAppearance = appearance124;
            this.cmbMin.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbMin.DisplayLayout.MaxRowScrollRegions = 1;
            appearance125.BackColor = System.Drawing.SystemColors.Window;
            appearance125.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbMin.DisplayLayout.Override.ActiveCellAppearance = appearance125;
            appearance126.BackColor = System.Drawing.SystemColors.Highlight;
            appearance126.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbMin.DisplayLayout.Override.ActiveRowAppearance = appearance126;
            this.cmbMin.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbMin.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance127.BackColor = System.Drawing.SystemColors.Window;
            this.cmbMin.DisplayLayout.Override.CardAreaAppearance = appearance127;
            appearance128.BorderColor = System.Drawing.Color.Silver;
            appearance128.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbMin.DisplayLayout.Override.CellAppearance = appearance128;
            this.cmbMin.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbMin.DisplayLayout.Override.CellPadding = 0;
            appearance129.BackColor = System.Drawing.SystemColors.Control;
            appearance129.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance129.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance129.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance129.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbMin.DisplayLayout.Override.GroupByRowAppearance = appearance129;
            appearance130.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbMin.DisplayLayout.Override.HeaderAppearance = appearance130;
            this.cmbMin.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbMin.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance131.BackColor = System.Drawing.SystemColors.Window;
            appearance131.BorderColor = System.Drawing.Color.Silver;
            this.cmbMin.DisplayLayout.Override.RowAppearance = appearance131;
            this.cmbMin.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance132.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbMin.DisplayLayout.Override.TemplateAddRowAppearance = appearance132;
            this.cmbMin.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbMin.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbMin.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbMin.DisplayMember = "";
            this.cmbMin.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbMin.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbMin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMin.Location = new System.Drawing.Point(80, 15);
            this.cmbMin.Name = "cmbMin";
            this.cmbMin.Size = new System.Drawing.Size(120, 21);
            this.cmbMin.TabIndex = 21;
            this.cmbMin.ValueMember = "";
            // 
            // lblDisplay
            // 
            this.lblDisplay.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplay.Location = new System.Drawing.Point(10, 61);
            this.lblDisplay.Name = "lblDisplay";
            this.lblDisplay.Size = new System.Drawing.Size(56, 14);
            this.lblDisplay.TabIndex = 3;
            this.lblDisplay.Text = "Display%";
            // 
            // lblMax
            // 
            this.lblMax.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMax.Location = new System.Drawing.Point(10, 41);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(36, 12);
            this.lblMax.TabIndex = 2;
            this.lblMax.Text = "Max%";
            // 
            // lblMin
            // 
            this.lblMin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMin.Location = new System.Drawing.Point(10, 17);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(36, 14);
            this.lblMin.TabIndex = 1;
            this.lblMin.Text = "Min%";
            // 
            // nudDisplay
            // 
            this.nudDisplay.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.nudDisplay.Location = new System.Drawing.Point(80, 59);
            this.nudDisplay.Name = "nudDisplay";
            this.nudDisplay.Size = new System.Drawing.Size(122, 21);
            this.nudDisplay.TabIndex = 5;
            this.nudDisplay.Visible = false;
            // 
            // picBernstein
            // 
            this.picBernstein.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picBernstein.BackgroundImage")));
            this.picBernstein.Location = new System.Drawing.Point(24, 170);
            this.picBernstein.Name = "picBernstein";
            this.picBernstein.Size = new System.Drawing.Size(148, 34);
            this.picBernstein.TabIndex = 5;
            this.picBernstein.TabStop = false;
            this.picBernstein.Visible = false;
            // 
            // picCreditSus
            // 
            this.picCreditSus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picCreditSus.BackgroundImage")));
            this.picCreditSus.Location = new System.Drawing.Point(26, 170);
            this.picCreditSus.Name = "picCreditSus";
            this.picCreditSus.Size = new System.Drawing.Size(144, 32);
            this.picCreditSus.TabIndex = 6;
            this.picCreditSus.TabStop = false;
            this.picCreditSus.Visible = false;
            // 
            // picPiperJaffray
            // 
            this.picPiperJaffray.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picPiperJaffray.BackgroundImage")));
            this.picPiperJaffray.Location = new System.Drawing.Point(10, 172);
            this.picPiperJaffray.Name = "picPiperJaffray";
            this.picPiperJaffray.Size = new System.Drawing.Size(176, 24);
            this.picPiperJaffray.TabIndex = 7;
            this.picPiperJaffray.TabStop = false;
            this.picPiperJaffray.Visible = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 14);
            this.label1.TabIndex = 11;
            this.label1.Text = "Execution Style";
            // 
            // btnTrade
            // 
            this.btnTrade.BackColor = System.Drawing.Color.Transparent;
            this.btnTrade.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTrade.BackgroundImage")));
            this.btnTrade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrade.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnTrade.Location = new System.Drawing.Point(226, 174);
            this.btnTrade.Name = "btnTrade";
            this.btnTrade.Size = new System.Drawing.Size(75, 23);
            this.btnTrade.TabIndex = 43;
            this.btnTrade.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(310, 174);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 45;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmbAcct
            // 
            this.cmbAcct.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAcct.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance133.BackColor = System.Drawing.SystemColors.Window;
            appearance133.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAcct.DisplayLayout.Appearance = appearance133;
            ultraGridBand10.ColHeadersVisible = false;
            this.cmbAcct.DisplayLayout.BandsSerializer.Add(ultraGridBand10);
            this.cmbAcct.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAcct.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance134.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance134.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance134.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance134.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAcct.DisplayLayout.GroupByBox.Appearance = appearance134;
            appearance135.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAcct.DisplayLayout.GroupByBox.BandLabelAppearance = appearance135;
            this.cmbAcct.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance136.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance136.BackColor2 = System.Drawing.SystemColors.Control;
            appearance136.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance136.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAcct.DisplayLayout.GroupByBox.PromptAppearance = appearance136;
            this.cmbAcct.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAcct.DisplayLayout.MaxRowScrollRegions = 1;
            appearance137.BackColor = System.Drawing.SystemColors.Window;
            appearance137.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAcct.DisplayLayout.Override.ActiveCellAppearance = appearance137;
            appearance138.BackColor = System.Drawing.SystemColors.Highlight;
            appearance138.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAcct.DisplayLayout.Override.ActiveRowAppearance = appearance138;
            this.cmbAcct.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAcct.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance139.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAcct.DisplayLayout.Override.CardAreaAppearance = appearance139;
            appearance140.BorderColor = System.Drawing.Color.Silver;
            appearance140.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAcct.DisplayLayout.Override.CellAppearance = appearance140;
            this.cmbAcct.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAcct.DisplayLayout.Override.CellPadding = 0;
            appearance141.BackColor = System.Drawing.SystemColors.Control;
            appearance141.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance141.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance141.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance141.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAcct.DisplayLayout.Override.GroupByRowAppearance = appearance141;
            appearance142.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbAcct.DisplayLayout.Override.HeaderAppearance = appearance142;
            this.cmbAcct.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAcct.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance143.BackColor = System.Drawing.SystemColors.Window;
            appearance143.BorderColor = System.Drawing.Color.Silver;
            this.cmbAcct.DisplayLayout.Override.RowAppearance = appearance143;
            this.cmbAcct.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance144.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAcct.DisplayLayout.Override.TemplateAddRowAppearance = appearance144;
            this.cmbAcct.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAcct.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAcct.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAcct.DisplayMember = "";
            this.cmbAcct.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAcct.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAcct.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cmbAcct.Location = new System.Drawing.Point(264, 52);
            this.cmbAcct.Name = "cmbAcct";
            this.cmbAcct.Size = new System.Drawing.Size(78, 21);
            this.cmbAcct.TabIndex = 46;
            this.cmbAcct.ValueMember = "";
            // 
            // AlgoTicket
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.cmbAcct);
            this.Controls.Add(this.btnTrade);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cmbTrading);
            this.Controls.Add(this.cmbHand);
            this.Controls.Add(this.cmbExec);
            this.Controls.Add(this.cmbTIF);
            this.Controls.Add(this.picBernstein);
            this.Controls.Add(this.grpTime);
            this.Controls.Add(this.nudQuantity);
            this.Controls.Add(this.txtSymbol);
            this.Controls.Add(this.cmbOrderType);
            this.Controls.Add(this.cmbTactic);
            this.Controls.Add(this.cmbSide);
            this.Controls.Add(this.lblTradingAcct);
            this.Controls.Add(this.lblAcct);
            this.Controls.Add(this.lblHandInstr);
            this.Controls.Add(this.lblExecInstr);
            this.Controls.Add(this.lblTIF);
            this.Controls.Add(this.lblOrderType);
            this.Controls.Add(this.lblSymbol);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblTactic);
            this.Controls.Add(this.lblSide);
            this.Controls.Add(this.cmbExecution);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpParticipation);
            this.Controls.Add(this.grpVolume);
            this.Controls.Add(this.picCreditSus);
            this.Controls.Add(this.picPiperJaffray);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "AlgoTicket";
            this.Size = new System.Drawing.Size(434, 206);
            this.Load += new System.EventHandler(this.AlgoTicket_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTrading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTactic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSide)).EndInit();
            this.grpTime.ResumeLayout(false);
            this.grpParticipation.ResumeLayout(false);
            this.grpParticipation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbmaximum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExecution)).EndInit();
            this.grpVolume.ResumeLayout(false);
            this.grpVolume.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBernstein)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCreditSus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPiperJaffray)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAcct)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void AlgoTicket_Load(object sender, System.EventArgs e)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("OrderType");
                dt.Columns.Add("order_value");
                dt.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                dt.Rows.Add(new object[2] { "Market", 1 });
                dt.Rows.Add(new object[2] { "Limit", 2 });

                cmbOrderType.DataSource = null;
                cmbOrderType.DataSource = dt;
                cmbOrderType.DisplayMember = "OrderType";
                cmbOrderType.ValueMember = "order_value";
                cmbOrderType.Rows.Band.Columns["order_value"].Hidden = true;
                cmbOrderType.Value = 1;

                System.Data.DataTable dt1 = new System.Data.DataTable();
                dt1.Columns.Add("TIF_id");
                dt1.Columns.Add("TIF_value");
                dt1.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                dt1.Rows.Add(new object[2] { "DAY", 1 });
                dt1.Rows.Add(new object[2] { "FIKI", 2 });
                dt1.Rows.Add(new object[2] { "IOCA", 3 });
                dt1.Rows.Add(new object[2] { "OPEN", 4 });

                cmbTIF.DataSource = null;
                cmbTIF.DataSource = dt1;
                cmbTIF.DisplayMember = "TIF_id";
                cmbTIF.ValueMember = "TIF_value";
                cmbTIF.Value = 1;
                cmbTIF.Rows.Band.Columns["TIF_value"].Hidden = true;

                System.Data.DataTable dt2 = new System.Data.DataTable();
                dt2.Columns.Add("Tactic_id");
                dt2.Columns.Add("Tactic_value");
                dt2.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                dt2.Rows.Add(new object[2] { "VWAP", 1 });
                dt2.Rows.Add(new object[2] { "TWAP", 2 });
                dt2.Rows.Add(new object[2] { "GWAP", 3 });
                dt2.Rows.Add(new object[2] { "BEWAP", 4 });
                dt2.Rows.Add(new object[2] { "ESAP", 5 });
                dt2.Rows.Add(new object[2] { "LWAP", 6 });
                dt2.Rows.Add(new object[2] { "VTRACK", 7 });

                cmbTactic.DataSource = null;
                cmbTactic.DataSource = dt2;
                cmbTactic.DisplayMember = "Tactic_id";
                cmbTactic.ValueMember = "Tactic_value";
                cmbTactic.Value = 1;
                cmbTactic.Rows.Band.Columns["Tactic_value"].Hidden = true;


                System.Data.DataTable dt3 = new System.Data.DataTable();
                dt3.Columns.Add("Exec_id");
                dt3.Columns.Add("Exec_value");
                dt3.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                dt3.Rows.Add(new object[2] { "AON", 1 });
                dt3.Rows.Add(new object[2] { "CALL", 2 });
                dt3.Rows.Add(new object[2] { "CRST", 3 });
                dt3.Rows.Add(new object[2] { "HELD", 4 });
                dt3.Rows.Add(new object[2] { "NOHE", 5 });
                cmbExec.DataSource = null;
                cmbExec.DataSource = dt3;
                cmbExec.DisplayMember = "Exec_id";
                cmbExec.ValueMember = "Exec_value";
                cmbExec.Value = 4;
                cmbExec.Rows.Band.Columns["Exec_value"].Hidden = true;


                System.Data.DataTable dt4 = new System.Data.DataTable();
                dt4.Columns.Add("hand_id");
                dt4.Columns.Add("hand_value");
                dt4.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                dt4.Rows.Add(new object[2] { "AutoNoBroker", 1 });
                dt4.Rows.Add(new object[2] { "AutoAllowBroker", 2 });
                dt4.Rows.Add(new object[2] { "Manual", 3 });

                cmbHand.DataSource = null;
                cmbHand.DataSource = dt4;
                cmbHand.DisplayMember = "hand_id";
                cmbHand.ValueMember = "hand_value";
                cmbHand.Value = 1;
                cmbHand.Rows.Band.Columns["hand_value"].Hidden = true;



                System.Data.DataTable dt5 = new System.Data.DataTable();
                dt5.Columns.Add("Execution_id");
                dt5.Columns.Add("Execution_value");
                dt5.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                dt5.Rows.Add(new object[2] { "Patient", 1 });
                dt5.Rows.Add(new object[2] { "Normal", 2 });
                dt5.Rows.Add(new object[2] { "Aggressive", 3 });

                cmbExecution.DataSource = null;
                cmbExecution.DataSource = dt5;
                cmbExecution.DisplayMember = "Execution_id";
                cmbExecution.ValueMember = "Execution_value";
                cmbExecution.Value = 3;
                cmbExecution.Rows.Band.Columns["Execution_value"].Hidden = true;



                System.Data.DataTable dt6 = new System.Data.DataTable();
                dt6.Columns.Add("max_id");
                dt6.Columns.Add("max_value");
                dt6.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                dt6.Rows.Add(new object[2] { "5", 1 });
                dt6.Rows.Add(new object[2] { "10", 2 });
                dt6.Rows.Add(new object[2] { "15", 3 });
                dt6.Rows.Add(new object[2] { "20", 4 });

                cmbmaximum.DataSource = null;
                cmbmaximum.DataSource = dt6;
                cmbmaximum.DisplayMember = "max_id";
                cmbmaximum.ValueMember = "max_value";
                cmbmaximum.Value = 2;
                cmbmaximum.Rows.Band.Columns["max_value"].Hidden = true;

                System.Data.DataTable dt7 = new System.Data.DataTable();
                dt7.Columns.Add("acct_id");
                dt7.Columns.Add("acct_value");
                dt7.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                dt7.Rows.Add(new object[2] { "Broker 1", 1 });
                dt7.Rows.Add(new object[2] { "USB", 2 });
                dt7.Rows.Add(new object[2] { "Broker2", 3 });


                cmbAcct.DataSource = null;
                cmbAcct.DataSource = dt7;
                cmbAcct.DisplayMember = "acct_id";
                cmbAcct.ValueMember = "acct_value";
                cmbAcct.Value = 1;
                cmbAcct.Rows.Band.Columns["acct_value"].Hidden = true;

                System.Data.DataTable dt8 = new System.Data.DataTable();
                dt8.Columns.Add("side_id");
                dt8.Columns.Add("side_value");
                dt8.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                dt8.Rows.Add(new object[2] { "BUY", 1 });
                dt8.Rows.Add(new object[2] { "SELL", 2 });
                dt8.Rows.Add(new object[2] { "SHORTSELL", 3 });

                cmbSide.DataSource = null;
                cmbSide.DataSource = dt8;
                cmbSide.DisplayMember = "side_id";
                cmbSide.ValueMember = "side_value";
                cmbSide.Value = 1;
                cmbSide.Rows.Band.Columns["side_value"].Hidden = true;


                System.Data.DataTable dt9 = new System.Data.DataTable();
                dt9.Columns.Add("TradingAccountName");
                dt9.Columns.Add("TA_id");
                dt9.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
                dt9.Rows.Add(new object[2] { "TradinAccount 1", 1 });
                dt9.Rows.Add(new object[2] { "TradinAccount 2", 2 });
                dt9.Rows.Add(new object[2] { "TradinAccount 3", 3 });

                cmbTrading.DataSource = null;
                cmbTrading.DataSource = dt9;
                cmbTrading.DisplayMember = "TradingAccountName";
                cmbTrading.ValueMember = "TA_id";
                cmbTrading.Value = 1;
                cmbTrading.Rows.Band.Columns["TA_id"].Hidden = true;

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {

            }

        }
        public event EventHandler CloseClick = null;

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            if (CloseClick != null)
            {
                CloseClick(sender, e);
            }

        }


    }
}
