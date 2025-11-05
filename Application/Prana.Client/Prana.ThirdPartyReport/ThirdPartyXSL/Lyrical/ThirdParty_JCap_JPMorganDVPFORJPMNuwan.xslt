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
				<xsl:variable name = "PB_FUND_NAME">
					<xsl:value-of select="AccountName"/>
				</xsl:variable>

				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../ReconMappingXml/FundList.xml')/FundMapping/PB[@Name='JPM']/FundData[@PranaFund=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:if test="$PRANA_FUND_NAME!=''">

					<!-- ...and, if it is the first node OR this EntityID is != from the previous... -->
					<xsl:if test="(1=position()) or(preceding-sibling::*[1]/PBUniqueID != PBUniqueID)">
						<!-- ...buid a Group for this node_id -->
						<xsl:call-template name="TaxLotIDBuilder">
							<xsl:with-param name="I_GroupID">
								<xsl:value-of select="PBUniqueID" />
							</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
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
				<xsl:when test="(Side='Buy to Cover' or Side='Buy to Close') and Asset = 'Equity'">
					<xsl:value-of select="'BC'"/>
				</xsl:when>
				<xsl:when test="Side='Buy to Close' and Asset = 'EquityOption'">
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
			<xsl:value-of  select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID=$I_GroupID][TaxLotStateID>1]/TaxLotState"/>
		</xsl:variable>

		<xsl:variable name="varTaxlotStateGrp">
			<xsl:choose>
				<xsl:when test="$tempTaxlotStateVar != ''">COR</xsl:when>
				<xsl:otherwise>NEW</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>


		<!--Creation of OSISymbol, in case it doesn't exist in Database-->

		<xsl:variable name="underlyingBlanks">
			<xsl:call-template name="noofBlanks">
				<xsl:with-param name="count1" select="(6-string-length(UnderlyingSymbol))"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="varUnderlying">
			<xsl:value-of select="concat(UnderlyingSymbol,$underlyingBlanks)"/>
		</xsl:variable>

		<xsl:variable name="expirationDate">
			<xsl:value-of select="concat(substring(ExpirationDate,9,2),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2))"/>
		</xsl:variable>

		<xsl:variable name="putCall">
			<xsl:value-of select="substring(PutOrCall,1,1)"/>
		</xsl:variable>

		<xsl:variable name="intStrike">
			<xsl:value-of select="substring-before(StrikePrice,'.')"/>
		</xsl:variable>

		<xsl:variable name="decimalStrike">
			<xsl:value-of select="concat(substring-after(Symbol,'.'),'0')"/>
		</xsl:variable>

		<xsl:variable name="intStrikeZeros">
			<xsl:call-template name="noofzeros">
				<xsl:with-param name="count" select="(5-string-length($intStrike))"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="varStrikePrice">
			<xsl:value-of select="concat($intStrikeZeros, $intStrike, $decimalStrike)"/>
		</xsl:variable>

		<!--Security Name-->

		<xsl:variable name="varSymbol">
			<xsl:choose>
				<xsl:when test="contains(Symbol, '-') != false and Asset = 'Equity' and SEDOL != ''">
					<xsl:value-of select="SEDOL"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="Asset = 'EquityOption' and OSIOptionSymbol != ''">
							<xsl:value-of select="OSIOptionSymbol"/>
						</xsl:when>
						<xsl:when test="Asset = 'EquityOption' and OSIOptionSymbol = ''">
							<xsl:value-of select="concat($varUnderlying, $expirationDate, $putCall, $varStrikePrice)"/>
						</xsl:when>
						<!--<xsl:when test="ISIN != ''">
              <xsl:value-of select="ISIN"/>
            </xsl:when>-->
						<xsl:otherwise>
							<xsl:value-of select ="Symbol"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<!--Quantity shown for Non-US Equities on Summary Level-->
		<xsl:variable name ="varQuantity">
			<xsl:choose>
			<xsl:when test="contains(Symbol,'-') != true and Asset = 'Equity'">
				<xsl:value-of select="$QtySum"/>
			</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<!--Method-->

		<!--<xsl:variable name="varMethod">
      <xsl:choose>
        <xsl:when test="contains(Symbol,'-') = true and Asset = 'Equity'">
          <xsl:value-of select="'DTC PRIME BROKER'"/>
        </xsl:when>
        <xsl:when test="contains(Symbol,'-') = true and Asset = 'EquityOption'">
          <xsl:value-of select="'CMTA'"/>
        </xsl:when>
        <xsl:when test="contains(Symbol,'-') != true and Asset = 'Equity'">
          <xsl:value-of select="'NON_US_LOCAL'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>-->
		<xsl:variable name="varMethod">
			<xsl:choose>
				<xsl:when test="contains(Symbol,'-') != true and Asset = 'Equity'">
					<!--<xsl:value-of select ="'PRIME'"/>-->
					<xsl:value-of select ="'INTL'"/>
				</xsl:when>
				<xsl:when test="Asset = 'Equity'">
					<xsl:value-of select="'DTC PRIME BROKER'"/>
				</xsl:when>
				<xsl:when test="Asset = 'EquityOption'">
					<xsl:value-of select="'CMTA'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<!--Impact of Commission on Settlement Money-->

		<xsl:variable name="varImpactSettlement">
			<xsl:choose>
				<xsl:when test="contains(Symbol,'-') != true and Asset = 'Equity'">
					<xsl:value-of select="'Y'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'N'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<!--Commission Amount-->

		<xsl:variable name="varCommissionAmount">
			<xsl:choose>
				<xsl:when test="Asset = 'Equity' or Asset = 'EquityOption'">
					<xsl:value-of select="$VarCommissionSum"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<!--Settlement Currency Name-->

		<xsl:variable name="varSettleCurrency">
			<xsl:choose>
				<xsl:when test="contains(Symbol,'-') != false">
					<xsl:value-of select="CurrencySymbol"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<!--CouunterParty Mapping-->

		<xsl:variable name="PRANA_COUNTERPARTY">
			<xsl:value-of select="CounterParty"/>
		</xsl:variable>

		<xsl:variable name="PB_COUNTERPARTY">
			<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name = 'GS']/BrokerData[@PranaBroker = $PRANA_COUNTERPARTY]/@MLPBroker"/>
		</xsl:variable>

		<xsl:variable name="varCounterParty">
			<xsl:choose>
				<xsl:when test="$PB_COUNTERPARTY = ''">
					<xsl:value-of select="$PRANA_COUNTERPARTY"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$PB_COUNTERPARTY"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<!--Exchange Mapping-->

		<xsl:variable name="PRANA_EXCHANGE">
			<xsl:value-of select="Exchange"/>
		</xsl:variable>

		<!--<xsl:variable name="PB_EXCHANGE">
			<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExchangeMapping.xml')/ExchangeMapping/PB[@Name = 'GS']/ExchangeData[@PranaExchange = $PRANA_EXCHANGE]/@PBExchangeName"/>
		</xsl:variable>

		<xsl:variable name="varExchangeName">
			<xsl:if test="$varMethod = 'FLOOR' or $varMethod = 'BROKER TO BROKER'">
				<xsl:choose>
					<xsl:when test="$PB_EXCHANGE = ''">
						<xsl:value-of select="''"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$PB_EXCHANGE"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:variable>-->

		<!--Account number hard coded for Non-US Equity On Summary Level-->
		<xsl:variable name ="varAccountNo">
			<xsl:choose>
				<xsl:when test="contains(Symbol,'-') != true and Asset = 'Equity'">
					<xsl:value-of select ="'10241906'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>






		<Group 
			  TRADE_ID ="" ACTION ="" SECURITY ="{$varSymbol}" WI ="" OPTION_TYPE ="" UNDERLYER ="" STRIKE = "" EXPIRY_DATE = "" 
		METHOD = "{$varMethod}" MARKET = "{$PRANA_EXCHANGE}" EXECUTING_BROKER = "{$varCounterParty}" CLEARING_BROKER = "" TRADE_DATE = "{TradeDate}" SETTLEMENT_DATE = "{SettlementDate}"
		SIDE = "{$Sidevar}" QUANTITY = "{$varQuantity}" PRICE = "{format-number(AveragePrice,'00000000.00000000')}"
		COMMISSION_TYPE = "TOTAL" COMMISSION_AMOUNT = "{format-number($varCommissionAmount,'00000000.00')}" IMPACT_SETTLEMENT_MONEY = "{$varImpactSettlement}" SETTLEMENT_CURRENCY = "{$varSettleCurrency}" PREFIGURED_PRINCIPAL = ""
		INTEREST = "" SERVICE_FEES = "" POSTAGE ="" STAMP_TAX = "" LEVY_TAX = "" ACCOUNT ="{$varAccountNo}"
		ACCOUNT_TYPE = "" IS_ALLOCATION = "" TRAILER_CODE_1 = "" TRAILER_DESCRIPTION_1 = "" VERSUS_PURCHASE_DATE_1 = "" VERSUS_PURCHASE_QUANTITY_1 = "" RR= "" 
			  EntityID="{EntityID}" TaxLotState="{TaxLotState}" TaxLotState1="" FileHeader="TRUE" FileFooter="TRUE">

			<!-- ...selecting all the records for this Group... -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[PBUniqueID = $I_GroupID]">

				<xsl:variable name = "PB_FUND_NAME">
					<xsl:value-of select="AccountName"/>
				</xsl:variable>

				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../ReconMappingXml/FundList.xml')/FundMapping/PB[@Name='JPM']/FundData[@PranaFund=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:if test="$PRANA_FUND_NAME!=''">
					<!-- ...and building a ThirdPartyFlatFileDetail for each -->
					<xsl:variable name="taxLotIDVar" select="EntityID"/>

					<xsl:variable name="var1">
						<xsl:value-of select="position()"/>
					</xsl:variable>

					<xsl:variable name ="FundAccountNol">
						<xsl:choose>

							<!--Fund for the Aged, Inc.-->
							<xsl:when test="AccountName ='Fund for the Aged: 1045216'">
								<xsl:value-of select ="'420-20986-97'"/>
							</xsl:when>
							<!--Altman 2009 Investment Consolidation LLC-->
							<xsl:when test="AccountName ='Altman 2009 Investment Consolidation LLC'">
								<xsl:value-of select ="'420-20996-95'"/>
							</xsl:when>
							<!--Alaska Permanent Fund Corporation-->
							<xsl:when test="AccountName ='Alaska Permanent Fund Corporation'">
								<xsl:value-of select ="'420-11705-96'"/>
							</xsl:when>
							<!--The Jay H. Baker Children's Trust FBO Stephanie E. Baker-->
							<xsl:when test="AccountName ='The Jay H Baker Children Trust: 2650388'">
								<xsl:value-of select ="'420-20870'"/>
							</xsl:when>
							<!--Big River Group Fund SPC Limited-Equity Segregated Portfolio-->
							<xsl:when test="AccountName ='Big River Group Fund SPC Limited-Equity Segregated'">
								<xsl:value-of select ="'420-20876'"/>
							</xsl:when>
							<!--Birch Capital Fund SPC Limited-Equity Segregated Portfolio-->
							<xsl:when test="AccountName ='Birch Capital Fund SPC Limited-Equity Segregated P'">
								<xsl:value-of select ="'420-20877'"/>
							</xsl:when>
							<!--Bristol County Retirement System-->
							<xsl:when test="AccountName ='Bristol CountyRetirement System'">
								<xsl:value-of select ="'420-20726-92'"/>
							</xsl:when>
							<!--Cedars-Sinai Medical Center-->
							<xsl:when test="AccountName ='SJXF10011602'">
								<xsl:value-of select ="'420-25788'"/>
							</xsl:when>
							<!--Christopher D. Heinz Trust-->
							<xsl:when test="AccountName ='Christopher D. Heinz Trust'">
								<xsl:value-of select ="'420-22577-98'"/>
							</xsl:when>
							<!--Cherir Assets Limited-->
							<xsl:when test="AccountName ='Cherir Assets Limited :002538734'">
								<xsl:value-of select ="'420-25824-92'"/>
							</xsl:when>
							<!--Eastwood Capital Fund SPC-->
							<xsl:when test="AccountName ='Eastwood Capital Fund SPC'">
								<xsl:value-of select ="'420-20878'"/>
							</xsl:when>
							<!--Evolve Bank and Trust Agent for Structured Assignments Inc.-->
							<xsl:when test="AccountName ='Evolve Bank: 1040005331'">
								<xsl:value-of select ="'420-25684-91'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Aspen Partners LLC: RZE003003'">
								<xsl:value-of select ="'420-26626'"/>
							</xsl:when>

							

							<xsl:when test="AccountName ='Conor Bastable: RZF001030'">
								<xsl:value-of select ="'420-28943'"/>
							</xsl:when>
							
							
							
							<!--Harbrook Limited-->
							<xsl:when test="AccountName ='Harbrook Limited'">
								<xsl:value-of select ="'420-20985-98'"/>
							</xsl:when>
							<!--Heinz II Charitable & Family Trust-->
							<xsl:when test="AccountName ='Heinz II Charitable &amp; Family Trust'">
								<xsl:value-of select ="'420-20740'"/>
							</xsl:when>
							<!--HJ Heinz III Descendants Trust-->
							<xsl:when test="AccountName ='HJ Heinz III Descendants Trust'">
								<xsl:value-of select ="'420-20746'"/>
							</xsl:when>
							<!--HJ Heinz II Family Trust-->
							<xsl:when test="AccountName ='HJ Heinz II Family Trust'">
								<xsl:value-of select ="'420-20741'"/>
							</xsl:when>
							<!--HJ Heinz III Granchildrens Trust-->
							<xsl:when test="AccountName ='HJ Heinz III Granchildrens Trust'">
								<xsl:value-of select ="'420-20749'"/>
							</xsl:when>
							<!--Teresa F Heinz Marital Trust-->
							<xsl:when test="AccountName ='Teresa F Heinz Marital Trust'">
								<xsl:value-of select ="'420-20742'"/>
							</xsl:when>
							<!--Heinz Family Foundation Awars Endowment Fund-->
							<xsl:when test="AccountName ='Heinz Awars Endowment Fund: 10260741947'">
								<xsl:value-of select ="'420-26090'"/>
							</xsl:when>
							<!--Heinz Family Foundation T&J Endowment Fund-->
							<xsl:when test="AccountName ='Heinz T&amp;J Endowment Fund: 10260743410'">
								<xsl:value-of select ="'420-26091'"/>
							</xsl:when>
							<!--The John Herma 1987 Trust-->
							<xsl:when test="AccountName ='The John Herma 1987 Trust: 2650390'">
								<xsl:value-of select ="'420-20868'"/>
							</xsl:when>
							<!--Jewish Community Foundation LA-->
							<xsl:when test="AccountName ='Jewish Community Foundation: 2650400'">
								<xsl:value-of select ="'420-20875-91'"/>
							</xsl:when>
							<!--Jay H Baker Living Trust-->
							<xsl:when test="AccountName ='Jay H Baker Living Trust: 2614745'">
								<xsl:value-of select ="'420-25419'"/>
							</xsl:when>
							<!--Jewish Home & Hospital Retirement Plan-->
							<xsl:when test="AccountName ='Jewish Home &amp; Hospital: 1045217'">
								<xsl:value-of select ="'420-20987-96'"/>
							</xsl:when>
							<!--The William S. Kellogg Irrevocable Trust-->
							<xsl:when test="AccountName ='The William S Kellogg: 2650385'">
								<xsl:value-of select ="'420-20871'"/>
							</xsl:when>
							<!--Kentucky Center-->
							<xsl:when test="AccountName ='Kentucky Center: 750804794669'">
								<xsl:value-of select ="'420-20881'"/>
							</xsl:when>
							<!--Landair Holding Inc.-->
							<xsl:when test="AccountName ='10571177700'">
								<xsl:value-of select ="'420-25893-98'"/>
							</xsl:when>
							<!--LYRIX-->
							<xsl:when test="AccountName ='LYRIX-000000000000940'">
								<xsl:value-of select ="'420-10302-95'"/>
							</xsl:when>
							<!--Memorial Hermann Health System-->
							<xsl:when test="AccountName ='Memorial Hermann Health System: 20011002'">
								<xsl:value-of select ="'420-20883-91'"/>
							</xsl:when>
							<!--Memorial Hermann Benefit Fund-->
							<xsl:when test="AccountName ='Memorial Hermann Benefit Fund'">
								<xsl:value-of select ="'420-25476-93'"/>
							</xsl:when>
							<!--The Molly Trust-->
							<xsl:when test="AccountName ='The Molly Trust'">
								<xsl:value-of select ="'420-20863-95'"/>
							</xsl:when>
							<!--Okabena U.S. Satellite Equity Fund, LLC-->
							<xsl:when test="AccountName ='Okabena U.S. Satellite Equity Fund LLC'">
								<xsl:value-of select ="'420-07163-99'"/>
							</xsl:when>
							<!--The Purpleville Foundation-->
							<xsl:when test="AccountName ='The Purpleville Foundation: 14462208'">
								<xsl:value-of select ="'420-26056'"/>
							</xsl:when>
							<!--PVF-KW, LP-->
							<xsl:when test="AccountName ='PVF-KW LP'">
								<xsl:value-of select ="'420-25522-97'"/>
							</xsl:when>
							<!--Quad/Graphics, Inc.-->
							<xsl:when test="AccountName ='Quad/Graphics Inc.'">
								<xsl:value-of select ="'420-20991-90'"/>
							</xsl:when>
							<!--Salisbury Asset Management LLC-->
							<xsl:when test="AccountName ='Salisbury Asset Management: 03-00311'">
								<xsl:value-of select ="'420-25757-93'"/>
							</xsl:when>
							<!--Salisbury Family Foundation-->
							<xsl:when test="AccountName ='Salisbury Family Foundation: 03-00312'">
								<xsl:value-of select ="'420-25758-92'"/>
							</xsl:when>
							<!--Jay H Baker Children's Trust FBO Stephen M. Baker-->
							<xsl:when test="AccountName ='Jay H Baker Children&quot;s Trust: 2650389'">
								<xsl:value-of select ="'420-25423-97'"/>
							</xsl:when>
							<!--Socatean Partners-->
							<xsl:when test="AccountName ='Socatean Partners'">
								<xsl:value-of select ="'420-24117-71'"/>
							</xsl:when>
							<!--Michael Stern-->
							<xsl:when test="AccountName ='Michael Stern'">
								<xsl:value-of select ="'420-25449-97'"/>
							</xsl:when>
							<!--Stetson University-->
							<xsl:when test="AccountName ='Stetson University: 3040000762'">
								<xsl:value-of select ="'420-25699-94'"/>
							</xsl:when>
							<!--Telesis II-->
							<xsl:when test="AccountName ='Telesis II'">
								<xsl:value-of select ="'420-20733-93'"/>
							</xsl:when>
							<!--UCITS-->
							<xsl:when test="AccountName ='UCITS: 018419'">
								<xsl:value-of select ="'420-11795-97'"/>
							</xsl:when>
							<!--UCITS - Ireland-->




							<!--ARM LLC-->
							<xsl:when test="AccountName ='ARM LLC: M54121005'">
								<xsl:value-of select ="'420-09723-98'"/>
							</xsl:when>

							<!--Cap 1 LLC-->
							<xsl:when test="AccountName ='Cap 1 LLC: W51843006'">
								<xsl:value-of select ="'420-25861-96'"/>
							</xsl:when>

							<!--CKMJ-->
							<xsl:when test="AccountName ='CKMJ LP'">
								<xsl:value-of select ="'420-20739'"/>
							</xsl:when>

							<!--Deminil Collections-->
								<xsl:when test="AccountName ='DeMenil Collections LLC'">
								<xsl:value-of select ="'420-11738-14'"/>
							</xsl:when>

							<!--Arther W. Collins IRA-->
							<xsl:when test="AccountName ='Arther W. Collins IRA: M54005000'">
								<xsl:value-of select ="'420-25756-94'"/>
							</xsl:when>

							<!--Demvest Evergreen-->
							<xsl:when test="AccountName ='Demvest Evergreen'">
								<xsl:value-of select ="'420-04734-96'"/>
							</xsl:when>

							<!--DM Foundation-->
							<xsl:when test="AccountName ='DM Foundation'">
								<xsl:value-of select ="'420-09698-99'"/>
							</xsl:when>

							<!--Dogwood Enterprises-->
							<xsl:when test="AccountName ='Dogwood Enterprises'">
								<xsl:value-of select ="'420-20980'"/>
							</xsl:when>

							<!--Sanford B Ehrenkans-->
							<xsl:when test="AccountName ='Sanford'">
								<xsl:value-of select ="'420-20723'"/>
							</xsl:when>

							<!--Endicott Group-->
							<xsl:when test="AccountName ='Endicott Group PS Plan'">
								<xsl:value-of select ="'420-07843-14'"/>
							</xsl:when>

							<!--Gary Facenda-->
							<xsl:when test="AccountName ='Gary Faccenda'">
								<xsl:value-of select ="'422-04300'"/>
							</xsl:when>

							<!--Edward Gage-->
							<xsl:when test="AccountName ='Edward Gage'">
								<xsl:value-of select ="'420-07831-18'"/>
							</xsl:when>

							<!--Claude Ghez-->
							<xsl:when test="AccountName ='Claude Ghez'">
								<xsl:value-of select ="'420-10473-15'"/>
							</xsl:when>

							<!--Thomas Johnson-->
							<xsl:when test="AccountName ='Thomas S. Johnson'">
								<xsl:value-of select ="'420-20928'"/>
							</xsl:when>

							<!--Jeff Kesswin-->
							<xsl:when test="AccountName ='Jeff Keswin'">
								<xsl:value-of select ="'420-04649'"/>
							</xsl:when>

							<!--James and Barbera Korein-->
							<xsl:when test="AccountName ='James &amp; Barbara Korein: M13179003'">
								<xsl:value-of select ="'420-26084'"/>
							</xsl:when>

							<!--Lama Partners-->
							<xsl:when test="AccountName ='LAMA Partners'">
								<xsl:value-of select ="'420-25417'"/>
							</xsl:when>

							<!--Zorbra Leiberman-->
							<xsl:when test="AccountName ='Zorba Lieberman'">
								<xsl:value-of select ="'420-08288-14'"/>
							</xsl:when>

							<!--Lyrical Long Only LP-->
							<xsl:when test="AccountName ='Lyrical Long Only LP'">
								<xsl:value-of select ="'420-25505'"/>
							</xsl:when>

							<!--Ian S. Madover-->
							<xsl:when test="AccountName ='Ian S. Madover &amp; Arielle T. Madover TIC'">
								<xsl:value-of select ="'420-20905'"/>
							</xsl:when>

							<!--Marsh Capital-->
							<xsl:when test="AccountName ='Marsh Capital Investors LLC'">
								<xsl:value-of select ="'420-07521-13'"/>
							</xsl:when>

							<!--Menil Associates-->
							<xsl:when test="AccountName ='Menil Associates'">
								<xsl:value-of select ="'420-10261-11'"/>
							</xsl:when>

							<!--James Patterson-->
							<xsl:when test="AccountName ='James Patterson'">
								<xsl:value-of select ="'420-06380-15'"/>
							</xsl:when>

							<!--James Patterson 2011-->
							<xsl:when test="AccountName ='James D. Patterson 2011: 420-25890'">
								<xsl:value-of select ="'420-25890'"/>
							</xsl:when>

							<!--James PPatterson 2012-->
							<xsl:when test="AccountName ='James D. Patterson 2012: 420-25888'">
								<xsl:value-of select ="'420-25888'"/>
							</xsl:when>

							<!--Caroline Ritter-->
							<xsl:when test="AccountName ='Caroline Ritter'">
								<xsl:value-of select ="'420-10409-14'"/>
							</xsl:when>

							<!--Rock Ridge Holding-->
							<xsl:when test="AccountName ='Rock Ridge Holdings LP'">
								<xsl:value-of select ="'420-20971'"/>
							</xsl:when>

							<!--Sidgemore-->
							<xsl:when test="AccountName ='Randi Lu Sidgmore Revocable Trust'">
								<xsl:value-of select ="'420-25666'"/>
							</xsl:when>

							<!--Matthew Sirovich-->
							<xsl:when test="AccountName ='Matthew Sirovich'">
							<xsl:value-of select ="'420-20495'"/>
							</xsl:when>

							<!--Sterling Stamos-->
							<xsl:when test="AccountName ='Sterling Stamos'">
								<xsl:value-of select ="'420-25518'"/>
							</xsl:when>

							<!--Swieca Family Foundation-->
							<xsl:when test="AccountName ='Swieca Family Foundation'">
								<xsl:value-of select ="'420-09304-12'"/>
							</xsl:when>

							<!--United States-Japan Foundation-->
							<xsl:when test="AccountName ='United States -Japan Foundation: M49007004'">
								<xsl:value-of select ="'420-24116-92'"/>
							</xsl:when>

							<!--Andrew Wellington-->
							<xsl:when test="AccountName ='Andrew Wellington'">
								<xsl:value-of select ="'420-06408-13'"/>
							</xsl:when>

							<!--Robert Wimers-->
							<xsl:when test="AccountName ='Robert G. Wilmers'">
								<xsl:value-of select ="'420-08209-10'"/>
							</xsl:when>

							<!--City of Philadelphia-->
							<xsl:when test="AccountName ='City of Philadelphia: P95719'">
								<xsl:value-of select ="'420-26383'"/>
							</xsl:when>


							<!--Sidney E. Frank Foundation: 2870533-->
							<xsl:when test="AccountName ='Sidney E. Frank Foundation: 2870533'">
								<xsl:value-of select ="''"/>
							</xsl:when>

							<!--Willo Funds: 2818698-->
							<xsl:when test="AccountName ='Willo Funds: 2818698'">
								<xsl:value-of select ="''"/>
							</xsl:when>


							<!--William S. Kellogg-->
							<xsl:when test="AccountName ='William S Kellogg: 2614746'">
								<xsl:value-of select ="'420-25420'"/>
							</xsl:when>
							<!--The William S. Kellogg Children's Trust FBO Jeff Kellogg-->
							<xsl:when test="AccountName ='The William S Kellogg Children &quot; s Trust: 2650386'">
								<xsl:value-of select ="'420-20865'"/>
							</xsl:when>
							<!--The William S. Kellogg Children's Trust FBO Kurt Kellogg-->
							<xsl:when test="AccountName ='The William S Kellogg Children &quot;s Trust: 2650387'">
								<xsl:value-of select ="'420-20866'"/>
							</xsl:when>

							<!--ARM LLC: M54121005-->
							<xsl:when test="AccountName ='ARM LLC: M54121005'">
								<xsl:value-of select ="'420-09723-98'"/>
							</xsl:when>

							<!--Cap 1 LLC: W51843006-->
							<xsl:when test="AccountName ='Cap 1 LLC: W51843006'">
								<xsl:value-of select ="'420-25861-96'"/>
							</xsl:when>


							<!--Cap 1 LLC: W51843006-->
							<xsl:when test="AccountName ='TF Cornerstone Portfolio LLC: M58632007'">
								<xsl:value-of select ="'420-29058-91'"/>
							</xsl:when>
							
							<!--Cap 1 LLC: W51843006-->
							<xsl:when test="AccountName ='Elaine Blumberg: M18910006'">
								<xsl:value-of select ="'420-26766'"/>
							</xsl:when>
							
							<!--Cap 1 LLC: W51843006-->
							<xsl:when test="AccountName ='Thomas Blumberg: M18905006'">
								<xsl:value-of select ="'420-26767'"/>
							</xsl:when>



							<!--Chapin School: RZF001238-->
							<xsl:when test="AccountName ='Chapin School: RZF001238'">
								<xsl:value-of select ="'420-29069'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Dogwood Enterprises: QXV001384'">
								<xsl:value-of select ="'420-29070'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Johnson Family GST Trust: QXV003166'">
								<xsl:value-of select ="'420-29017'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Richard T. Silver: QXV002937'">
								<xsl:value-of select ="'420-29017'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Richard Axilrod: QXV003174'">
								<xsl:value-of select ="'420-29009'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Jeffrey Katz: QXV001723'">
								<xsl:value-of select ="'420-28149'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Socatean Partners: QXV003000'">
								<xsl:value-of select ="'420-29004'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Joel S. Ehrenkranz: QXV003091'">
								<xsl:value-of select ="'420-28997'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Marianne Schaprio Roth IRA: QXV002911'">
								<xsl:value-of select ="'420-29006'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Daniel Schapiro IRA Rollover: QXV002895'">
								<xsl:value-of select ="'420-28267'"/>
							</xsl:when>

							<!--<xsl:when test="AccountName ='Aspen Partners LLC: RZE003003'">
								<xsl:value-of select ="'420-26626'"/>
							</xsl:when>-->

							<xsl:when test="AccountName ='Jeffrey Katz 2010 Family Trust: QXV0024323'">
								<xsl:value-of select ="'420-29007'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Ana Meier: QXV002804'">
								<xsl:value-of select ="'420-29005'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Taylor 1998 Irrevocable Trust: QXV002861'">
								<xsl:value-of select ="'420-28182'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Westwood 1998 Irrevocable Trust: QXV002853'">
								<xsl:value-of select ="'420-28270'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Axilrod Children&quot;s Trust: QXV002788'">
								<xsl:value-of select ="'420-28908'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Paul Edelman: QXV002812'">
								<xsl:value-of select ="'420-28470'"/>
							</xsl:when>

							<xsl:when test="AccountName ='The John Katz Investment Trust 1: QXV002713'">
								<xsl:value-of select ="'420-29014'"/>
							</xsl:when>

							<xsl:when test="AccountName ='The John Katz Lifetime Trust: QXV002721'">
								<xsl:value-of select ="'420-29012'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Clara Y. Bingham: QXV002549'">
								<xsl:value-of select ="'420-28741'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Joseph L. Gitterman III Revocable Trust: QXV002705'">
								<xsl:value-of select ="'420-29001'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Roger C. Altman: QXV002556'">
								<xsl:value-of select ="'420-29008'"/>
							</xsl:when>
							
							<xsl:when test="AccountName ='Roger C. Altman: QXV002556'">
								<xsl:value-of select ="'420-29008'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Carrol Saunders Roberson: RZE001676'">
								<xsl:value-of select ="'420-28998'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Silverman Family Investors LLC: QXV002564'">
								<xsl:value-of select ="'420-29000'"/>
							</xsl:when>

							<!--<xsl:when test="AccountName ='Conor Bastable: RZF001030'">
								<xsl:value-of select ="'420-28943'"/>
							</xsl:when>-->

							<xsl:when test="AccountName ='Christian Education Foundation: RZF001071'">
								<xsl:value-of select ="'420-28269'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Four Winds Capital LP: RZE002765'">
								<xsl:value-of select ="'420-29003'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Hugh and Charlotte MacLellan: RZF001063'">
								<xsl:value-of select ="'420-28999'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Lamar Legacy LP: RZE002435'">
								<xsl:value-of select ="'420-29011'"/>
							</xsl:when>

							<xsl:when test="AccountName ='The MacLellan Foundation Inc: RZF001048'">
								<xsl:value-of select ="'420-29013'"/>
							</xsl:when>

							<xsl:when test="AccountName ='RDB Family Trust: RZE002203'">
								<xsl:value-of select ="'420-28996'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Smith 1996 Family Trust No. 1: RZF001089'">
								<xsl:value-of select ="'420-29002'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Joseph and Cheryl JTIC: RZF001113'">
								<xsl:value-of select ="'420-28268'"/>
							</xsl:when>

							<xsl:when test="AccountName ='O. Nemirovsky Associates LLC: M58731007'">
								<xsl:value-of select ="'420-29076'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Ofer Nemirovsky: M21194002'">
								<xsl:value-of select ="'420-29077'"/>
							</xsl:when>


							<xsl:when test="AccountName ='James Bullock Trust: 7957175'">
								<xsl:value-of select ="'420-29252'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Susan Carter 2009: 7957065'">
								<xsl:value-of select ="'420-29206-92'"/>
							</xsl:when>

							<xsl:when test="AccountName ='PCR Children&quot;s Trust FBO Cole: 7956908'">
								<xsl:value-of select ="'420-29114'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Mark Dyne Trust: 7957444'">
								<xsl:value-of select ="'420-29251'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Sandra Ecker: 7957281'">
								<xsl:value-of select ="'420-29207-91'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Ecker Family Holdings LLC: 7957278'">
								<xsl:value-of select ="'420-29208-90'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Bernard Foundation: 7956870'">
								<xsl:value-of select ="'420-29104'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Galloway Partnership: 7957050'">
								<xsl:value-of select ="'420-29148'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Floyd Gottwald  Jr: 7957046'">
								<xsl:value-of select ="'420-29132-91'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Grandstand Associates LLC: 7957148'">
								<xsl:value-of select ="'420-29181'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Cecil Trus: 7957594'">
								<xsl:value-of select ="'420-29304'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Hickey Investments LLLP: 7957057'">
								<xsl:value-of select ="'420-29138'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Modyl LP: 7957445'">
								<xsl:value-of select ="'420-29250'"/>
							</xsl:when>

							<xsl:when test="AccountName ='PCR FBO Susan Ryzewic:7956672'">
								<xsl:value-of select ="'420-29100'"/>
							</xsl:when>

							<xsl:when test="AccountName ='William Shaw Rev Trust: 7957185'">
								<xsl:value-of select ="'420-29188'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Pamela Whitman Rev Trust: 7957059'">
								<xsl:value-of select ="'420-29146'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Winship Investment LLLP: 7956881'">
								<xsl:value-of select ="'420-29106'"/>
							</xsl:when>

							<xsl:when test="AccountName ='Caren Zivony: 7957320'">
								<xsl:value-of select ="'420-29228-96'"/>
							</xsl:when>

							<xsl:when test="AccountName ='SSCSIL SIG LYRICAL FUND'">
								<xsl:value-of select ="'420-26001-95'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="AccountName"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<ThirdPartyFlatFileDetail
						  Group_Id="" TRADE_ID ="" ACTION ="" SECURITY ="" WI ="" OPTION_TYPE ="" UNDERLYER ="" STRIKE = "" EXPIRY_DATE = "" METHOD = "" MARKET = "" EXECUTING_BROKER = ""
						  CLEARING_BROKER = "" TRADE_DATE = "" SETTLEMENT_DATE = "" SIDE = "" QUANTITY = "{AllocatedQty}" PRICE = "" COMMISSION_TYPE = "" 
						  COMMISSION_AMOUNT = "" IMPACT_SETTLEMENT_MONEY = "" SETTLEMENT_CURRENCY = "" PREFIGURED_PRINCIPAL = "" INTEREST = "" 
						  SERVICE_FEES = "" POSTAGE ="" STAMP_TAX = "" LEVY_TAX = "" ACCOUNT ="{$FundAccountNol}" ACCOUNT_TYPE = "" IS_ALLOCATION = "Y" 
						  TRAILER_CODE_1 = "" TRAILER_DESCRIPTION_1 = "" VERSUS_PURCHASE_DATE_1 = "" VERSUS_PURCHASE_QUANTITY_1 = "" RR= "" 
									EntityID="{EntityID}" TaxLotState="{TaxLotState}"  FileHeader="TRUE" FileFooter="TRUE"/>
				</xsl:if>
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>
