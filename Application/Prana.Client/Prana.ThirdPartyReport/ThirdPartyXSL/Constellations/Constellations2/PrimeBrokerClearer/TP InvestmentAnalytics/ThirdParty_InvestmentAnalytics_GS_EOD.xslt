<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
	<xsl:output method="xml" indent="yes" encoding="ISO-8859-1" />

	<xsl:template name="noofzeros">
		<xsl:param name="count"/>
		<xsl:if test="$count > 0">
			<xsl:value-of select ="'0'"/>
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="noofBlanks">
		<xsl:param name="count1"/>
		<xsl:if test="$count1 > 0">
			<xsl:value-of select ="' '"/>
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="$count1 - 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template match="/">
		<Groups>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->




			<Group
				OrderNumber ="OrderNumber" Cancelcorrectindicator ="Cancelcorrectindicator" Accountnumberoracronym="Accountnumberoracronym" SecurityIdentifier="SecurityIdentifier"
				Broker="Broker" Custodian="Custodian" TransactionType="TransactionType" CurrencyCode="CurrencyCode"
				TradeDate = "TradeDate" SettleDate = "SettleDate"
				Quantity="Quantity" Commission="Commission" Price="Price" AccruedInterest="AccruedInterest"
				TradeTax="TradeTax" MiscMoney="MiscMoney" NetAmount="NetAmount" Principal="Principal" Description="Description"
				SecurityType="SecurityType" CountrySettlementCode="CountrySettlementCode" ClearingAgent="ClearingAgent" SECFee="SECFee" RepoOpenSettleDate="RepoOpenSettleDate" RepoMaturityDate="RepoMaturityDate"
		  RepoRate="RepoRate" RepoInterest="RepoInterest" OptionUnderlyer="OptionUnderlyer" OptionExpiryDate="OptionExpiryDate" OptionCallPutIndicator="OptionCallPutIndicator"
		  OptionStrikePrice="OptionStrikePrice" Trailer="Trailer"		
		  EntityID="''" TaxLotState="''"  FileHeader="''" FileFooter="''"/>
				
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail">
				
				<xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
					<!-- ...buid a Group for this node_id -->
					<xsl:call-template name="TaxLotIDBuilder">
						<xsl:with-param name="I_GroupID">
							<xsl:value-of select="PBUniqueID" />
						</xsl:with-param>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</Groups>
	</xsl:template>


	<xsl:template name="TaxLotIDBuilder">
		<xsl:param name="I_GroupID" />

		<xsl:variable name="AllocatedQty" />
		
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
		</xsl:variable>

	
		<xsl:variable name="VarCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>
		<!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
		</xsl:variable>

	

		<xsl:variable name="Sidevar">
			<xsl:choose>
				<xsl:when test="Side='Buy'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="Side='Sell'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="Side='Sell short'">
					<xsl:value-of select="'SS'"/>
				</xsl:when>
				<xsl:when test="Side='Buy to Close'">
					<xsl:value-of select="'BuyToClose'"/>
				</xsl:when>
				<xsl:when test="Side='Sell to Open'">
					<xsl:value-of select="'SellToOpen'"/>
				</xsl:when>
				<xsl:when test="Side='Sell to Close'">
					<xsl:value-of select="'SellToClose'"/>
				</xsl:when>
				<xsl:when test="Side='Buy to Open'">
					<xsl:value-of select="'BuyToOpen'"/>
				</xsl:when>
				<xsl:otherwise> </xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<!--<xsl:variable name="tempTaxlotStateVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotStateID>1]/TaxLotState"/>
		</xsl:variable>-->

		<xsl:variable name="varTransactionType">
			<xsl:choose>
				<xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="Side = 'Sell' or Side='Sell to Close'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="Side = 'Buy to Close' or Side='Buy to Cover'">
					<xsl:value-of select="'BC'"/>
				</xsl:when>
				<xsl:when test="Side = 'Sell short' or Side='Sell to Open'">
					<xsl:value-of select="'SS'"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varInstruction">
			<xsl:choose>
				<xsl:when test="TaxLotState = 'Allocated'">
					<xsl:value-of select="'NEW'"/>
				</xsl:when>
				<xsl:when test="TaxLotState = 'Amended'">
					<xsl:value-of select="'MOD'"/>
				</xsl:when>
				<xsl:when test="TaxLotState = 'Deleted'">
					<xsl:value-of select="'CXL'"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varSecurityID">
			<xsl:choose>
				<xsl:when test="Asset = 'EquityOption'">
					<xsl:value-of select="OSIOptionSymbol"/>
				</xsl:when>
				<xsl:when test="Asset = 'FixedIncome'">
					<xsl:value-of select="CUSIP"/>
				</xsl:when>
				<xsl:when test="ISIN != ''">
					<xsl:value-of select="ISIN"/>
				</xsl:when>
				<xsl:when test="CUSIP != ''">
					<xsl:value-of select="CUSIP"/>
				</xsl:when>
				<xsl:when test="SEDOL != ''">
					<xsl:value-of select="SEDOL"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFeeType1">
			<xsl:choose>
				<xsl:when test="number(StampDuty)">
					<xsl:value-of select="'SEC'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFeeValue1">
			<xsl:choose>
				<xsl:when test="number(StampDuty)">
					<xsl:value-of select="StampDuty"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFeeType2">
			<xsl:choose>
				<xsl:when test="number(MiscFees)">
					<xsl:value-of select="'MSCF'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFeeValue2">
			<xsl:choose>
				<xsl:when test="number(MiscFees)">
					<xsl:value-of select="MiscFees"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:choose>
				<xsl:when test ="contains(Asset, 'Option')!= false">
					<xsl:value-of select ="UnderlyingSymbol"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePrice">
			<xsl:choose>
				<xsl:when test ="contains(Asset, 'Option')!= false">
					<xsl:value-of select ="StrikePrice"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable  name="PB_NAME">
			<xsl:value-of select="'GS'"/>
		</xsl:variable>

		<xsl:variable name = "PB_SYMBOL_NAME">
			<xsl:value-of select="Symbol"/>
		</xsl:variable>
		<xsl:variable name="PRANA_SYMBOL_NAME">
			<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
		</xsl:variable>

		<xsl:variable name="varSymbol">
			<xsl:choose>
				<xsl:when test ="$PRANA_SYMBOL_NAME!=''">
					<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
				</xsl:when>
				
				<xsl:when test="Asset='EquityOption'">
					<xsl:value-of select="OSIOptionSymbol"/>
				</xsl:when>

				<xsl:when test="Asset='FixedIncome'">
					<xsl:value-of select="Symbol"/>
				</xsl:when>
				
				<xsl:when test="SEDOL!=''">
					<xsl:value-of select="SEDOL"/>
				</xsl:when>
				<xsl:when test="BBCode!=''">
					<xsl:value-of select="BBCode"/>
				</xsl:when>
				<xsl:when test="CUSIP!=''">
					<xsl:value-of select="CUSIP"/>
				</xsl:when>
				
				
				<xsl:otherwise>
					<xsl:value-of select ="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="SwapCheck">
			<xsl:if test="Asset='Equity' and IsSwapped='True'">
				<xsl:value-of select="'SWAP'"/>
			</xsl:if>
		</xsl:variable>


		

		<xsl:variable name ="UDA_Country">
			<xsl:value-of select ="UDACountryName"/>
		</xsl:variable>
		
		

		<xsl:variable name ="varTradeTax">
			<xsl:choose>
				<xsl:when test ="CurrencySymbol != 'USD'">
					<xsl:value-of select ="StampDuty"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name ="varCommission">
			<xsl:choose>

				<xsl:when test ="IsSwapped='true' and number(FXRate)">
					<xsl:value-of  select="format-number(CommissionCharged div number(FXRate),'#.####')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="format-number(CommissionCharged,'#.#######')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varNetAmount">
			<xsl:choose>
				
		
				<xsl:when test ="IsSwapped='true' and number(FXRate)">
					<xsl:value-of select ="format-number(NetAmount div number(FXRate),'#.##')"/>
				</xsl:when>
				<xsl:otherwise>
			<xsl:value-of select ="format-number(NetAmount,'#.##')"/>
			</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="Cpty" select="CounterParty"/>

	
		<xsl:variable name="Curr">
			
					<xsl:value-of select="CurrencySymbol"/>
			
		</xsl:variable>

		

		<xsl:variable name ="PB_CounterParty">
			<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$Cpty]/@MLPBroker"/>
		</xsl:variable>

		<xsl:variable name="varCounterParty">
			<xsl:choose>
				<xsl:when test="$PB_CounterParty!=''">
					<xsl:value-of select="$PB_CounterParty"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="CounterParty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varAllocationState">
			<xsl:choose>
				<xsl:when test ="TaxLotState = 'Allocated'">
					<xsl:value-of  select="'N'"/>
				</xsl:when>
				<xsl:when test="TaxLotState='Amended'">
					<xsl:value-of select ="'A'"/>
				</xsl:when>
				<xsl:when test="TaxLotState='Deleted'">
					<xsl:value-of select ="'C'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name ="varSecurityType">
			<xsl:choose>
				<xsl:when test ="IsSwapped='true'">
					<xsl:value-of  select="'CFD'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varAccountID">
			<xsl:choose>
				<xsl:when test ="AccountName='Investment Analytics-GS'">
					<xsl:value-of  select="'002709194 '"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<Group
				OrderNumber ="{EntityID}" Cancelcorrectindicator ="{$varAllocationState}" Accountnumberoracronym="{$varAccountID}" SecurityIdentifier="{$varSymbol}"
				Broker="{CounterParty}" Custodian="GSCO" TransactionType="{$varTransactionType}" CurrencyCode="{$Curr}"
				TradeDate = "{TradeDate}" SettleDate = "{SettlementDate}"
				Quantity="{AllocatedQty}" Commission="{CommissionCharged}" Price="{AveragePrice}" AccruedInterest="{AccruedInterest}"
				TradeTax="{$varTradeTax}" MiscMoney="{MiscFees}" NetAmount="{NetAmount}" Principal="{GrossAmount}" Description="{CompanyName}"
				SecurityType="{$varSecurityType}" CountrySettlementCode="" ClearingAgent="" SECFee="" RepoOpenSettleDate="" RepoMaturityDate=""
		  RepoRate="" RepoInterest="" OptionUnderlyer="{$varUnderlyingSymbol}" OptionExpiryDate="" OptionCallPutIndicator="{substring(PutOrCall,1,1)}"
		  OptionStrikePrice="{$varStrikePrice}" Trailer=""
		
		  EntityID="{EntityID}" TaxLotState="{TaxLotState}" FileHeader="FALSE" FileFooter="FALSE">

		</Group>
	</xsl:template>
</xsl:stylesheet>