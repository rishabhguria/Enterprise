<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">


				<xsl:variable name="Cash">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL11"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Cash)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'SSC'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL15"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>




						<xsl:variable name="PB_FUND_NAME" select="COL4"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<xsl:variable name="Date" select="COL1"/>

						<Date>
							<xsl:value-of select="COL1"/>
						</Date>


						<CurrencyName>
							<xsl:value-of select="COL5"/>
						</CurrencyName>

						<CurrencyID>

							<xsl:value-of select="1"/>
						</CurrencyID>

						<xsl:variable name="AbsCash">
							<xsl:choose>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="$Cash"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Cash * -1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_ACRONYM_NAME">

							<xsl:choose>
								<xsl:when test="contains(COL15,'BTIG Short Rebate')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'REBATE')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Rebate')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'rebate')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Custody Fee')">
									<xsl:value-of select="'CUST FEE'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'CUSTODY FEE')">
									<xsl:value-of select="'CUST FEE'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'custody fee')">
									<xsl:value-of select="'CUST FEE'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Custody fee')">
									<xsl:value-of select="'CUST FEE'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Custody fee - btig')">
									<xsl:value-of select="'CUST FEE'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Misc btig cash')">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Misc Exp')">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Misc Income')">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Misc income')">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Reclass')">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'SHORT DEBIT FEE')">
									<xsl:value-of select="'Short Debit Fees'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'SHORT DEBIT FEE')">
									<xsl:value-of select="'Short Debit Fees'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Short Prm exp')">
									<xsl:value-of select="'Short Premium Expenses'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Short Prm.EXp')">
									<xsl:value-of select="'Short Premium Expenses'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Cash reclass')">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Short rebate')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Short Rebate Exp')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Short Rebate Expense')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Short Rebate')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Short Rebate Expenses')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Short rebate exp')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'short rebate exp')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Rebate Expense')">
									<xsl:value-of select="'Short_Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Short rebate premium')">
									<xsl:value-of select="'Short Rebate Premium'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Stock Loan fee')">
									<xsl:value-of select="'Stock Loan Fees'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Stock Loan Fee')">
									<xsl:value-of select="'Stock Loan Fees'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Stock loan fee')">
									<xsl:value-of select="'Stock Loan Fees'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Withdraw')">
									<xsl:value-of select="'CASH_WDL'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Admin Fee')">
									<xsl:value-of select="'Admin Fees Expense'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Bank fees from GS to JPM')">
									<xsl:value-of select="'Bank Charges'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Cash in Lieu')">
									<xsl:value-of select="'Cash In Lieu'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Cash in lieu - Homeaway')">
									<xsl:value-of select="'Cash In Lieu'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Contribution')">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Contribution - Paul Ross')">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Contribution - Rahul Kakar')">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'CRA - Cheque Number 5')">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'CONTRIBUTION')">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'CRA - Dilip Kachare')">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'CRA - Erin Minkwitz')">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'CRA - Mathew Hansen')">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Deposit')">
									<xsl:value-of select="'CASH_DEP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Int on Short Sale Proceed')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'INTEREST')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'JUL16 USD CREDIT INT')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Intereset Income')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'April USD Credit Int')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'AUG 16 USD Cr.Int')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'May 2016 Int on Sale Proceed')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Int on Sale Proceed')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Int On short Sale Proceed')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Int On Short Sale Proceed')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Int on short')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Interest')">
									<xsl:value-of select="'Interest Income'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Transfer')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'transfer')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'transfer')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'subs/reds')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'TRANSFER')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'TRANSFER')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Mgmt fee')">
									<xsl:value-of select="'Management Fee'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'management fee')">
									<xsl:value-of select="'Management Fee'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'MCGLADREY - AUDIT (GS to JPM)')">
									<xsl:value-of select="'AUDITEXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'TAX - POT')">
									<xsl:value-of select="'TAX_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'MCGLADREY - TAX RETURN (GS to')">
									<xsl:value-of select="'TAX_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Tax - POT paid by BTIG')">
									<xsl:value-of select="'TAX_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Mcgladrey tax return (GS to JP')">
									<xsl:value-of select="'TAX_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Misc. Income')">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Security Lending fees')">
									<xsl:value-of select="'Security Lending Fee'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'FX realized loss')">
									<xsl:value-of select="'FXRealizedPNL'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Realized FX gain')">
									<xsl:value-of select="'FXRealizedPNL'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'investor service')">
									<xsl:value-of select="'Investor service'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Realized FX loss')">
									<xsl:value-of select="'FXRealizedPNL'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Mgmt fee')">
									<xsl:value-of select="'Management Fee'"/>
								</xsl:when>
								
							
								<xsl:when test="contains(COL15,'Conversion')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>

								<xsl:when test="contains(COL15,'Transafer')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Investments')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>

								<xsl:when test="contains(COL15,'Reverse red in advance')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Management Fee')">
									<xsl:value-of select="'Management Fee'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Redemption')">
									<xsl:value-of select="'Redemptions'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Security Lending Fee')">
									<xsl:value-of select="'Security Lending Fee'"/>
								</xsl:when>
								
								<xsl:when test="contains(COL15,'Security Lending Rebate')">
									<xsl:value-of select="'Security Lending Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Security Lending Rebat')">
									<xsl:value-of select="'Security Lending Rebate'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'SGGH-PIPE cash')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Transfer')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'trfr')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Cash reclass')">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'BTIG SSR')">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Short Pre.Exp')">
									<xsl:value-of select="'Short Premium Expenses'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'PRM.EXP')">
									<xsl:value-of select="'Short Premium Expenses'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Prm.Exp')">
									<xsl:value-of select="'Short Premium Expenses'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'FX Charges')">
									<xsl:value-of select="'MISC_EXP'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Misc Revenue')">
									<xsl:value-of select="'MISC_INC'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'clearing')">
									<xsl:value-of select="'Clearing Fees'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Clearing')">
									<xsl:value-of select="'Clearing Fees'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'fee (GS to JPM)')">
									<xsl:value-of select="'Other_Fees'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Tsf')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'CRA - ALEXANDER')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'CRA - Alexander')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Trsf')">
									<xsl:value-of select="'CashTransferIn'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Various Invoice')">
									<xsl:value-of select="'SSC Various Invoice'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'SSNC')">
									<xsl:value-of select="'SSC Various Invoice'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'ticket')">
									<xsl:value-of select="'TicketFees'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'Ticket')">
									<xsl:value-of select="'TicketFees'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'WHT Adjustment')">
									<xsl:value-of select="'TAX'"/>
								</xsl:when>
								<xsl:when test="contains(COL15,'WHT PAID')">
									<xsl:value-of select="'TAX'"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>


						<JournalEntries>
							<xsl:choose>
								<xsl:when test="$Cash &lt; 0">
									<xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $AbsCash, '|Cash:',$AbsCash)"/>
								</xsl:when>
								<xsl:when test="$Cash &gt; 0">
									<xsl:value-of select="concat( 'Cash:',$AbsCash,'|', $PRANA_ACRONYM_NAME,':' , $AbsCash)"/>
								</xsl:when>
							</xsl:choose>
						</JournalEntries>

						<xsl:variable name="Description" select="COL15"/>

						<Description>
							<xsl:value-of select="$Description"/>
						</Description>

					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>