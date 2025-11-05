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
			<xsl:for-each select="/NewDataSet/ThirdPartyFlatFileDetail">
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
				<!--As requested by Alex on May 7, 2014-->
				<xsl:when test="Side = 'Sell to Open' and Asset = 'EquityOption'">
					<xsl:value-of select="'SS'"/>
				</xsl:when>
				<xsl:when test="Side = 'Buy' or Side = 'Buy to Open'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="Side = 'Sell' or Side = 'Sell to Open' or Side = 'Sell to Close'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="Side = 'Buy to Close'">
					<xsl:value-of select="'BC'"/>
				</xsl:when>
				<xsl:when test="Side = 'Sell short'">
					<xsl:value-of select="'SS'"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClientAccount">
			<xsl:choose>
				<xsl:when test ="FundName = 'Newtyn Partners LP:522-91K57'">
					<xsl:value-of select ="'52291K57'"/>
				</xsl:when>
				<xsl:when test ="FundName = 'Newtyn TE Partners LP:522-91K58'">
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
				<xsl:when test="CurrencySymbol='USD'">
					<xsl:value-of select="Symbol"/>
				</xsl:when>
				
				<xsl:otherwise>
					<xsl:value-of select="SEDOLSymbol"/>
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



		<xsl:variable name="varSymbol">
			<xsl:choose>
				<xsl:when test="CurrencySymbol='USD'">
					<xsl:value-of select="Symbol"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="SEDOL"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name ="varLotID1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varLotID2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varLotID3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varLotID4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name ="varLotID5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/LotIDAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>

		</xsl:variable>

		<xsl:variable name="varQ2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varQ5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/ClosedQty"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="varClosedPrice1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedPrice5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/OpenPriceAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name="varClosedAgainstDate1">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=1]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate2">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=2]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate3">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=3]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate4">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=4]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClosedAgainstDate5">
			<xsl:choose>
				<xsl:when test ="ClosingAlgo = 'HIFO'  or ClosingAlgo = '*'">
					<xsl:value-of  select="''"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="/NewDataSet/ThirdPartyFlatFileDetail[TaxLotID = $I_TaxLotID][position()=5]/TradeDateAgainstClosing"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="UDA_Country">
			<xsl:value-of select ="UDACountryName"/>
		</xsl:variable>
		<xsl:variable name ="PB_SettleCurrency">
			<xsl:value-of select="document('../ReconMappingXml/SettlementCurrencyMapping.xml')/SettleCurrencyMapping/PB[@Name='ALL']/SymbolData[@UDACountry=$UDA_Country]/@SettleCurrency"/>
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
				<xsl:when test ="CounterParty = 'GWEP'">
					<xsl:value-of  select="format-number(AveragePrice,'#.0000')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select="AveragePrice"/>
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

		<Group 
				OrderNumber ="{EntityID}" Cancelcorrectindicator ="{$varAllocationState}" AccountNumber="{AccountNo}" SecurityIdentifier="{$varSecurityID}"
				Broker="{CounterParty}" Custodian="GSCO" TransactionType="{$varTransactionType}" CurrencyCode="{SettlementCurrency}"
				TradeDate = "{TradeDate}" SettleDate = "{SettlementDate}"
				Quantity="{AllocatedQty}" Commission="{CommissionCharged}" Price="{$varAvgPrice}" AccruedInterest="{AccruedInterest}" 
				TradeTax="{$varTradeTax}" MiscMoney="{MiscFees}" NetAmount="{NetAmount}" Principal="{GrossAmount}" Description="{Description}"
				SecurityType="Y" CountrySettlementCode="" ClearingAgent="" SECFee="" RepoOpenSettleDate="" RepoMaturityDate=""
		  RepoRate="" RepoInterest="" OptionUnderlyer="{$varUnderlyingSymbol}" OptionExpiryDate="" OptionCallPutIndicator="{substring(PutOrCall,1,1)}"
		  OptionStrikePrice="{$varStrikePrice}" Trailer=""
		  GenevaLotNumber1="{$varLotID1}" GainsKeeperLotNumber1="" LotDate1="{$varClosedAgainstDate1}" LotQty1="{$varQ1}" LotPrice1="{$varClosedPrice1}"
		  GenevaLotNumber2 ="{$varLotID2}" GainsKeeperLotNumber2="" LotDate2="{$varClosedAgainstDate2}" LotQty2="{$varQ2}" LotPrice2="{$varClosedPrice2}"
		  GenevaLotNumber3="{$varLotID3}" GainsKeeperLotNumber3="" LotDate3="{$varClosedAgainstDate3}" LotQty3="{$varQ3}" LotPrice3="{$varClosedPrice3}"
		  GenevaLotNumber4 ="{$varLotID4}" GainsKeeperLotNumber4="" LotDate4="{$varClosedAgainstDate4}" LotQty4="{$varQ4}" LotPrice4="{$varClosedPrice4}"
		  GenevaLotNumber5="{$varLotID5}" GainsKeeperLotNumber5="" LotDate5="{$varClosedAgainstDate5}" LotQty5="{$varQ5}" LotPrice5="{$varClosedPrice5}"
       
		  EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="" FileHeader="FALSE" FileFooter="FALSE">

		</Group>
	</xsl:template>
</xsl:stylesheet>
