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
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail">
				<!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
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
		<!-- Building a Group with the EntityID $I_GroupID... -->

		<!--Total Quantity-->
		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/AllocatedQty)"/>
		</xsl:variable>

		<!-- They need it blank -->
		<!--<xsl:variable name="QtySum">
			<xsl:value-of  select="''"/>
		</xsl:variable>-->
		<!--Total Commission-->
		<xsl:variable name="VarCommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>
		<!--<xsl:variable name="tempSymbol">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Symbol"/>
		</xsl:variable>-->
		<xsl:variable name="tempSideVar">
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID]/Side"/>
		</xsl:variable>

		<!--Side-->

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

		<xsl:variable name ="varAccountNo">
			<xsl:choose>
				<xsl:when test ="AccountName= 'GS Newtyn Partners LP:002486561'">
					<xsl:value-of select ="'002486561'"/>
				</xsl:when>
				<xsl:when test ="AccountName= 'GS Newtyn TE Partners LP:002486553'">
					<xsl:value-of select ="'002486553'"/>
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
				<xsl:when test="SEDOLSymbol != '*' and SEDOLSymbol != ''">
					<xsl:value-of select="SEDOLSymbol"/>
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


		<!--<xsl:variable name ="varLotID1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varLotID2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varLotID3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varLotID4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name ="varLotID5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>

		</xsl:variable>

		<xsl:variable name="varQ2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="varClosedPrice1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="varClosedAgainstDate1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>-->

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

		<xsl:variable name ="varAvgPrice">
			<xsl:choose>			
					
				<xsl:when test ="IsSwapped='true' and number(FXRate)">
					<xsl:value-of  select="format-number(AveragePrice div number(FXRate),'#.####')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="format-number(AveragePrice,'#.#######')"/>
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

		<!--<xsl:variable name="Curr" select="CurrencySymbol"/>-->

		<xsl:variable name="Curr">

			<!--<xsl:choose>
				<xsl:when test="IsSwapped='true'">
					<xsl:value-of select="'USD'"/>
				</xsl:when>
				<xsl:otherwise>-->
					<xsl:value-of select="CurrencySymbol"/>
				<!--</xsl:otherwise>
			</xsl:choose>-->


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

		<xsl:variable name="varProductCode">
			<xsl:choose>
				<xsl:when test="Asset='Equity' and IsSwapped='true'">
					<xsl:value-of select="'SWAP'"/>
				</xsl:when>
				<!--<xsl:when test="Asset = 'Equity' or Asset = 'EquityOption'" >
					<xsl:value-of select="'BS'"/>
				</xsl:when>
				<xsl:when test="Asset='FX' or Asset='FXForward'">
					<xsl:value-of select="'FX'"/>
				</xsl:when>-->
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFund">
		<xsl:choose>
			<xsl:when test="Asset='Equity' and IsSwapped='true'">
				<xsl:value-of select="'006365217'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'002587491'"/>
			</xsl:otherwise>
		</xsl:choose>
		</xsl:variable>

		<xsl:variable name="ExpiryDate">
			<xsl:choose>
				<xsl:when test="contains(Asset,'Option')">
					<xsl:value-of select="ExpirationDate"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varTaxlotState">
			<xsl:value-of select="'Allocated'"/>
		</xsl:variable>

		<xsl:variable name="Stamp">
			<xsl:choose>
				<xsl:when test="CurrencySymbol = 'HKD' or CurrencySymbol = 'JPY' or CurrencySymbol = 'GBP' or CurrencySymbol = 'EUR'">
					<xsl:value-of select="StampDuty"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name ="varAllocationState">
			<xsl:choose>
				<xsl:when test ="TaxLotState = 'Allocated'">
					<xsl:value-of  select="'N'"/>
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
		<Group
				OrderNumber ="{EntityID}" Cancelcorrectindicator ="{$varAllocationState}" Accountnumberoracronym="{AccountName}" SecurityIdentifier="{$varSymbol}"
				Broker="{CounterParty}" Custodian="GSCO" TransactionType="{$varTransactionType}" CurrencyCode="{SettlCurrency}"
				TradeDate = "{TradeDate}" SettleDate = "{SettlementDate}"
				Quantity="{AllocatedQty}" Commission="{CommissionCharged}" Price="{AveragePrice}" AccruedInterest="{AccruedInterest}"
				TradeTax="{$varTradeTax}" MiscMoney="{MiscFees}" NetAmount="{NetAmount}" Principal="{GrossAmount}" Description="{CompanyName}"
				SecurityType="{$varSecurityType}" CountrySettlementCode="" ClearingAgent="" SECFee="" RepoOpenSettleDate="" RepoMaturityDate=""
		  RepoRate="" RepoInterest="" OptionUnderlyer="{$varUnderlyingSymbol}" OptionExpiryDate="" OptionCallPutIndicator="{substring(PutOrCall,1,1)}"
		  OptionStrikePrice="{$varStrikePrice}" Trailer=""
		  
       
		  EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="" FileHeader="FALSE" FileFooter="FALSE">

		</Group>
	</xsl:template>
</xsl:stylesheet>