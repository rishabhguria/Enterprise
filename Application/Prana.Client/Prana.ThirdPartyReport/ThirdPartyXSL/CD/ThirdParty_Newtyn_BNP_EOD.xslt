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
			<xsl:for-each select="/NewDataSet/ThirdPartyFlatFileDetail[contains(AccountName, 'Swap') = false]">
				<!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
				<xsl:if test="(1=position()) or(preceding-sibling::*[1]/TaxLotID != TaxLotID)">
					<!-- ...buid a Group for this node_id -->
					<xsl:call-template name="TaxLotIDBuilder">
						<xsl:with-param name="I_TaxLotID">
							<xsl:value-of select="TaxLotID" />
						</xsl:with-param>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</Groups>
	</xsl:template>


	<xsl:template name="TaxLotIDBuilder">
		<xsl:param name="I_TaxLotID" />

		<xsl:variable name="AllocatedQty" />
		<!-- Building a Group with the EntityID $I_TaxLotID... -->

		<!--Total Quantity-->
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/ClosedQty)"/>
		</xsl:variable>

		<xsl:variable name="QtySum1">
			<xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/ClosedQty)"/>
		</xsl:variable>

		<!--Total Commission-->
		<xsl:variable name="VarCommissionSum">
			<xsl:value-of  select="sum(/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>
		<!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID]/Symbol"/>
		</xsl:variable>-->
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID]/Side"/>
		</xsl:variable>

		<!--Side-->

		<xsl:variable name ="varAccountNo">
			<xsl:choose>
				<xsl:when test ="AccountName = 'BNP Newtyn Partners LP:491-00040'">
					<xsl:value-of select ="'49100040'"/>
				</xsl:when>
				<xsl:when test ="AccountName = 'BNP Newtyn TE Partners LP:491-00041'">
					<xsl:value-of select ="'49100041'"/>
				</xsl:when>
			</xsl:choose>
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
		<xsl:variable name="tempTaxlotStateVar">
			<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID=$I_TaxLotID][TaxLotStateID>1]/TaxLotState"/>
		</xsl:variable>

		<xsl:variable name="varTransactionType">
			<xsl:choose>
				<xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
					<xsl:value-of select="'BY'"/>
				</xsl:when>
				<xsl:when test="Side = 'Sell'">
					<xsl:value-of select="'SL'"/>
				</xsl:when>
				<xsl:when test="Side = 'Buy to Close'">
					<xsl:value-of select="'CS'"/>
				</xsl:when>
				<xsl:when test="Side = 'Sell short'">
					<xsl:value-of select="'SS'"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClientAccount">
			<xsl:choose>
				<xsl:when test ="AccountName = 'Newtyn Partners LP:522-91K57'">
					<xsl:value-of select ="'52291K57'"/>
				</xsl:when>
				<xsl:when test ="AccountName = 'Newtyn TE Partners LP:522-91K58'">
					<xsl:value-of select ="'52291K58'"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varInstruction">
			<xsl:choose>
				<xsl:when test="TaxLotState = 'Allocated'">
					<xsl:value-of select="'NEW'"/>
				</xsl:when>
				<xsl:when test="TaxLotState = 'Amemded'">
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



		<xsl:variable name="varSymbol">
			<xsl:choose>
				<xsl:when test="contains(Symbol, '-') != false and Asset = 'Equity' and CUSIP != ''">
					<xsl:value-of select="CUSIP"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varSecurity">
			<xsl:choose>
				<!--As Requested by  Korey on Jan 22-->
				<xsl:when test ="Symbol = 'OGS-W'">
					<xsl:value-of select ="'68235P108'"/>
				</xsl:when>
				<xsl:when test="Asset = 'EquityOption'">
					<xsl:value-of select="OSIOptionSymbol"/>
				</xsl:when>
				<xsl:when test="ISIN != '*'">
					<xsl:value-of select="ISIN"/>
				</xsl:when>
				<xsl:when test="CUSIP != '*'">
					<xsl:value-of select="CUSIP"/>
				</xsl:when>
				<xsl:when test="SEDOL != '*'">
					<xsl:value-of select="SEDOL"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="Symbol"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStatus">
			<xsl:choose>
				<xsl:when test="TaxLotState = 'Allocated'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="TaxLotState = 'Amemded'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="TaxLotState = 'Deleted'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varClosingMethod">
			<xsl:if test="substring(Side,1,1) = 'S' or Side ='Buy to Close'">
				<xsl:choose>
					<xsl:when test="ClosingAlgo = 'FIFO'">
						<xsl:value-of select="'FF'"/>
					</xsl:when>
					<xsl:when test="ClosingAlgo = 'LIFO'">
						<xsl:value-of select="'LF'"/>
					</xsl:when>
					<xsl:when test="ClosingAlgo = 'HIFO'">
						<xsl:value-of select="'HC'"/>
					</xsl:when>
					<xsl:when test="ClosingAlgo = 'LOWCOST'">
						<xsl:value-of select="'LC'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:variable>

		<xsl:variable name ="varCountry">
			<xsl:choose>
				<xsl:when test="CountryName = 'United States'">
					<xsl:value-of select="'US'"/>
				</xsl:when>
				<xsl:when test="CountryName = 'Japan'">
					<xsl:value-of select="'JP'"/>
				</xsl:when>
				<xsl:when test="CountryName = 'United Kingdom'">
					<xsl:value-of select="'UK'"/>
				</xsl:when>
				<xsl:when test="CountryName = 'Canada'">
					<xsl:value-of select="'CN'"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="UDA_Country">
			<xsl:value-of select ="UDACountryName"/>
		</xsl:variable>
		<xsl:variable name ="PB_SettleCurrency">
			<xsl:value-of select="document('../ReconMappingXml/SettlementCurrencyMapping.xml')/SettleCurrencyMapping/PB[@Name='ALL']/SymbolData[@UDACountry=$UDA_Country]/@SettleCurrency"/>
		</xsl:variable>

		<xsl:variable name="varQ1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = 'FIFO' or ClosingAlgo = 'LIFO' or ClosingAlgo = 'LOWCOST' or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>

		</xsl:variable>

		<xsl:variable name="varQ2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = 'FIFO' or ClosingAlgo = 'LIFO' or ClosingAlgo = 'LOWCOST' or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = 'FIFO' or ClosingAlgo = 'LIFO' or ClosingAlgo = 'LOWCOST' or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = 'FIFO' or ClosingAlgo = 'LIFO' or ClosingAlgo = 'LOWCOST' or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = 'FIFO' or ClosingAlgo = 'LIFO' or ClosingAlgo = 'LOWCOST' or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = 'FIFO' or ClosingAlgo = 'LIFO' or ClosingAlgo = 'LOWCOST' or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = 'FIFO' or ClosingAlgo = 'LIFO' or ClosingAlgo = 'LOWCOST' or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = 'FIFO' or ClosingAlgo = 'LIFO' or ClosingAlgo = 'LOWCOST' or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = 'FIFO' or ClosingAlgo = 'LIFO' or ClosingAlgo = 'LOWCOST' or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = 'FIFO' or ClosingAlgo = 'LIFO' or ClosingAlgo = 'LOWCOST' or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varAvgPrice">
			<xsl:choose>
				<xsl:when test ="CounterParty = 'GWEP'">
					<xsl:value-of  select="format-number(AveragePrice,'#.0000')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="AveragePrice"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<Group 
			trade_id ="{EntityID}" pb_trd_id ="" transaction_code="{$varTransactionType}" qty_filled="{AllocatedQty}" security="{$varSecurity}"
			price="{$varAvgPrice}" Account="{AccountNo}" executing_broker="{CounterParty}" trade_date = "{TradeDate}" security_id_type="{Asset}"
			commission_type ="T" ClientReference="" commission="{CommissionCharged}" settlement_date = "{SettlementDate}" cancel_correct="{$varStatus}"
			closing_method="{$varClosingMethod}" vp_date="" assigned_usr="" alloc_shares="{AllocatedQty}" target="" local_currency="{CurrencySymbol}"
			settlement_currency="{$PB_SettleCurrency}" settle_exchange_rate="{ForexRate}" exchange="{Exchange}" oth_fee="{OtherBrokerFee}" settlement_location=""
			trade_classification="" repo_interest_rate="" repo_end_date="" accrued_interest="{AccruedInterest}" country="{$varCountry}"
			use_control_account="" account_type_code="" custodian="" cancel_bond_interest="" deal_version="" order_limit="" order_type=""
			LotDate1="{$varClosedAgainstDate1}" LotQty1="{$varQ1}" 
			LotDate2="{$varClosedAgainstDate2}" LotQty2="{$varQ2}" 
			LotDate3="{$varClosedAgainstDate3}" LotQty3="{$varQ3}" 
			LotDate4="{$varClosedAgainstDate4}" LotQty4="{$varQ4}" 
			LotDate5="{$varClosedAgainstDate5}" LotQty5="{$varQ5}"
			
			EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="" FileHeader="FALSE" FileFooter="FALSE">

		</Group>
	</xsl:template>
</xsl:stylesheet>
