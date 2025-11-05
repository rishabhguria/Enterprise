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
			<xsl:for-each select="/NewDataSet/ThirdPartyFlatFileDetail[contains(AccountName, 'Swap') = false and TaxLotState = 'Allocated']">
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

		<!--Side-->

		<xsl:variable name="varTransactionCode">
			<xsl:choose>
				<xsl:when test="Side='Buy' or Side='Buy to Open'">
					<xsl:value-of select="'by'"/>
				</xsl:when>
				<xsl:when test="Side='Buy to Close' or Side='Buy to Cover'">
					<xsl:value-of select="'cs'"/>
				</xsl:when>
				<xsl:when test="Side='Sell' or Side='Sell to Close'">
					<xsl:value-of select="'sl'"/>
				</xsl:when>
				<xsl:when test="Side='Sell short' or Side='Sell to Open'">
					<xsl:value-of select="'ss'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varClientAccount">
			<xsl:choose>
				<xsl:when test ="AccountName = 'BNP NP 491-00040'">
					<xsl:value-of select ="'newbnp'"/>
				</xsl:when>
				<xsl:when test ="AccountName = 'BNP NTE 491-00041'">
					<xsl:value-of select ="'ntebnp'"/>
				</xsl:when>
				<xsl:when test ="AccountName = 'GS NP 002486561'">
					<xsl:value-of select ="'newgs'"/>
				</xsl:when>
				<xsl:when test ="AccountName = 'GS NTE 002486553'">
					<xsl:value-of select ="'ntegs'"/>
				</xsl:when>
				<xsl:when test ="AccountName = 'Citi NP 522-91K57'">
					<xsl:value-of select ="'newciti'"/>
				</xsl:when>
				<xsl:when test ="AccountName = 'Citi NTE 522-91K58'">
					<xsl:value-of select ="'nteciti'"/>
				</xsl:when>
				<xsl:when test ="AccountName = 'Fidelity NP 015479'">
					<xsl:value-of select ="'newfid'"/>
				</xsl:when>
				<xsl:when test ="AccountName = 'Fidelity NTE 015468'">
					<xsl:value-of select ="'ntefid'"/>
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

		<xsl:variable name="varSecType">
			<xsl:choose>

				<xsl:when test="Asset = 'PrivateEquity'">
					<xsl:value-of select ="'cbus'"/>
				</xsl:when >
				<xsl:when test="Asset = 'Equity'">
					<xsl:value-of select ="'csus'"/>
				</xsl:when >
				<xsl:when test="Asset='EquityOption' and PutOrCall = 'CALL'">
					<xsl:value-of select ="'clus'"/>
				</xsl:when >
				<xsl:when test="Asset='EquityOption' and PutOrCall = 'PUT'">
					<xsl:value-of select ="'ptus'"/>
				</xsl:when >
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name ="varCheckSymbolUnderlying">
			<xsl:value-of select ="substring-before(Symbol,'-')"/>
		</xsl:variable>

		<xsl:variable name ="varEqtSymbol">
			<xsl:choose>
				<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/W') != ''">
					<xsl:value-of select ="concat(translate(Symbol,'.','/'),'S')"/>
				</xsl:when>
				<!--
							Change	Date :23-12-2011 , 
							For  DTE/PA and  DTE/PC
                            Will Display dte a, dte c
							-->
				<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/P') != ''">
					<xsl:value-of select ="concat(substring-before(Symbol,'/P'),' ',substring-after(Symbol,'/P'))"/>
				</xsl:when>
				<!--End Change-->
				<xsl:when test ="Asset='Equity' and substring-before(Symbol,'/W') = ''">
					<xsl:value-of select ="Symbol"/>
				</xsl:when >
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name ="varSecurity">
			<xsl:value-of select="translate(Symbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
		</xsl:variable>

		<xsl:variable name ="varStatus">
			<xsl:choose>
				<xsl:when test="TaxLotState = 'Allocated'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="TaxLotState = 'Amended'">
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
					<xsl:when test ="ClosingAlgo = 'LIFO'">
						<xsl:value-of select ="'l'"/>
					</xsl:when>
					<xsl:when test ="ClosingAlgo = 'FIFO'">
						<xsl:value-of select ="'f'"/>
					</xsl:when>
					<xsl:when test ="ClosingAlgo = 'HIFO'">
						<xsl:value-of select ="'h'"/>
					</xsl:when>
					<xsl:when test ="ClosingAlgo != '*'">
						<xsl:value-of select ="'s'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:variable>

		<xsl:variable name ="varM2M">
			<xsl:choose>
				<xsl:when test ="substring(UDASecurityTypeName,3,2) = 'us'">
					<xsl:value-of select ="'n'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="'y'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varBroker">
			<xsl:choose>
				<xsl:when test="CounterParty='DEAN'">
					<xsl:value-of select="'msco'"/>
				</xsl:when>
				<xsl:when test="CounterParty='GOLD'">
					<xsl:value-of select="'gskr'"/>
				</xsl:when>
				<xsl:when test="CounterParty ='JONO'">
					<xsl:value-of select="'JONE'"/>
				</xsl:when>
				<xsl:when test ="string-length(CounterParty) &lt; 4">
					<xsl:value-of select="concat(translate(CounterParty,$vUppercaseChars_CONST,$vLowercaseChars_CONST),'kr')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test ="CounterParty = 'CITI'">
							<xsl:value-of select ="'cits'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="translate(CounterParty,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varOthFee">
			<xsl:choose>
				<xsl:when test ="Asset = 'PrivateEquity'">
					<xsl:value-of select = "0"/>
				</xsl:when >
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test ="TransactionLevy != 0">
							<xsl:value-of select = 'format-number(TransactionLevy+OtherBrokerFee, "###.0000000")'/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose >
		</xsl:variable>

		<xsl:variable name ="PRANA_FUND_NAME">
			<xsl:value-of select ="AccountName"/>
		</xsl:variable>

		<xsl:variable name ="Prana_SIDE">
			<xsl:value-of select ="Side"/>
		</xsl:variable>
		
		<xsl:variable name ="varSourceSymbol">
			<xsl:value-of select="document('../ReconMappingXml/ThirdParty_SourceSymbolMapping.xml')/SourceSymbolMapping/PB[@Name='AXYS']/SymbolData[@PranaFund=$PRANA_FUND_NAME and @PranaSide = $Prana_SIDE]/@PranaSymbol"/>
		</xsl:variable>

		<xsl:variable name ="varSourceSymbol1">
			<xsl:value-of select="translate($varSourceSymbol,$vUppercaseChars_CONST,$vLowercaseChars_CONST)"/>
		</xsl:variable>

		<xsl:variable name ="varNetAmount2">
			<xsl:choose>
				<!--Ankit 07022014: Modified as requested by Tom-->
				<xsl:when test ="CounterParty = 'ENSK' and number(ForexRate)">
					<xsl:value-of select ="format-number(NetAmount,'0.####')*ForexRate"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="format-number(NetAmount,'0.####')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<Group 
			PortfolioCode="{$varClientAccount}" TranCode="{$varTransactionCode}" Comment="" SecType="{UDASecurityTypeName}"
			SecuritySymbol="{$varSecurity}" TradeDate="{translate(TradeDate,'/','')}" SettleDate="{translate(SettlementDate,'/','')}" OriginalCostDate="" Quantity="{AllocatedQty}"
			CloseMeth="{$varClosingMethod}" VersusDate="" SourceType="{UDACountryName}" SourceSymbol="{$varSourceSymbol1}" TradeDateFXRate="" SettleDateFXRate=""
			OriginalFXRate="" MarkToMarket="{$varM2M}" TradeAmount="{$varNetAmount2}" OriginalCost="" Comment1="" WithholdingTax="" Exchange=""
			ExchangeFee="{format-number(StampDuty+SecFee+ClearingFee+TaxOnCommissions+MiscFees, '###.00')}" commission="{format-number(CommissionCharged, '#.00')}" Broker="{$varBroker}" ImpliedComm="n"
			OtherFees="{format-number($varOthFee,'#.00')}" CommPurpose="" Pledge="n" LotLocation="253" DestPledge="" DestLotLocation="" OriginalFace="" YieldOnCost=""
			DurationOnCost="" UserDef1="" UserDef2="" UserDef3="" TranID="" IPCounter="" Repl="" Source="" Comment2="" OmniAcct="" Recon=""
			Post="y" LabelName="" LabelDefinition="" LabelDefinition_Date="" LabelDefinition_String="" Comment3="" RecordDate="" ReclaimAmount=""
			Strategy="" Comment4="" IncomeAccount="" AccrualAccount="" DivAccrualMethod="" PerfContributionOrWithdrawal=""
			 
			EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="" RowHeader ="FALSE" FileHeader="FALSE" FileFooter="FALSE">

		</Group>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
