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


			<xsl:variable name ="FundAccountNol">
				<xsl:choose>

					<!--Fund for the Aged, Inc.-->
					<xsl:when test="AccountNo ='1045216'">
						<xsl:value-of select ="'420-20986-97'"/>
					</xsl:when>
					<!--Altman 2009 Investment Consolidation LLC-->
					<xsl:when test="AccountNo ='441051301'">
						<xsl:value-of select ="'420-20996-95'"/>
					</xsl:when>
					<!--Alaska Permanent Fund Corporation-->
					<xsl:when test="AccountNo ='AKPFCD269002'">
						<xsl:value-of select ="'420-11705-96'"/>
					</xsl:when>
					<!--The Jay H. Baker Children's Trust FBO Stephanie E. Baker-->
					<xsl:when test="AccountNo ='2650388'">
						<xsl:value-of select ="'420-20870'"/>
					</xsl:when>
					<!--Big River Group Fund SPC Limited-Equity Segregated Portfolio-->
					<xsl:when test="AccountNo ='002523702'">
						<xsl:value-of select ="'420-20876'"/>
					</xsl:when>
					<!--Birch Capital Fund SPC Limited-Equity Segregated Portfolio-->
					<xsl:when test="AccountNo ='002523686'">
						<xsl:value-of select ="'420-20877'"/>
					</xsl:when>
					<!--Bristol County Retirement System-->
					<xsl:when test="AccountNo ='ERM1'">
						<xsl:value-of select ="'420-20726-92'"/>
					</xsl:when>
					<!--Cedars-Sinai Medical Center-->
					<xsl:when test="AccountNo ='SJXF10011602'">
						<xsl:value-of select ="'420-25788'"/>
					</xsl:when>
					<!--Christopher D. Heinz Trust-->
					<xsl:when test="AccountNo ='010F260223850'">
						<xsl:value-of select ="'420-22577-98'"/>
					</xsl:when>
					<!--Cherir Assets Limited-->
					<xsl:when test="AccountNo ='002538734'">
						<xsl:value-of select ="'420-25824-92'"/>
					</xsl:when>
					<!--Eastwood Capital Fund SPC-->
					<xsl:when test="AccountNo ='002523405'">
						<xsl:value-of select ="'420-20878'"/>
					</xsl:when>
					<!--Evolve Bank and Trust Agent for Structured Assignments Inc.-->
					<xsl:when test="AccountNo ='1040005331'">
						<xsl:value-of select ="'420-25684-91'"/>
					</xsl:when>
					<!--Harbrook Limited-->
					<xsl:when test="AccountNo ='SS1840'">
						<xsl:value-of select ="'420-20985-98'"/>
					</xsl:when>
					<!--Heinz II Charitable & Family Trust-->
					<xsl:when test="AccountNo ='010F260223120'">
						<xsl:value-of select ="'420-20740'"/>
					</xsl:when>
					<!--HJ Heinz III Descendants Trust-->
					<xsl:when test="AccountNo ='010F260223160'">
						<xsl:value-of select ="'420-20746'"/>
					</xsl:when>
					<!--HJ Heinz II Family Trust-->
					<xsl:when test="AccountNo ='010F260223130'">
						<xsl:value-of select ="'420-20741'"/>
					</xsl:when>
					<!--HJ Heinz III Granchildrens Trust-->
					<xsl:when test="AccountNo ='010F260223150'">
						<xsl:value-of select ="'420-20749'"/>
					</xsl:when>
					<!--Teresa F Heinz Marital Trust-->
					<xsl:when test="AccountNo ='010F260223140'">
						<xsl:value-of select ="'420-20742'"/>
					</xsl:when>
					<!--Heinz Family Foundation Awars Endowment Fund-->
					<xsl:when test="AccountNo ='10260741947'">
						<xsl:value-of select ="'420-26090'"/>
					</xsl:when>
					<!--Heinz Family Foundation T&J Endowment Fund-->
					<xsl:when test="AccountNo ='10260743410'">
						<xsl:value-of select ="'420-26091'"/>
					</xsl:when>
					<!--The John Herma 1987 Trust-->
					<xsl:when test="AccountNo ='2650390'">
						<xsl:value-of select ="'420-20868'"/>
					</xsl:when>
					<!--Jewish Community Foundation LA-->
					<xsl:when test="AccountNo ='2650400'">
						<xsl:value-of select ="'420-20875-91'"/>
					</xsl:when>
					<!--Jay H Baker Living Trust-->
					<xsl:when test="AccountNo ='2614745'">
						<xsl:value-of select ="'420-25419'"/>
					</xsl:when>
					<!--Jewish Home & Hospital Retirement Plan-->
					<xsl:when test="AccountNo ='1045217'">
						<xsl:value-of select ="'420-20987-96'"/>
					</xsl:when>
					<!--The William S. Kellogg Irrevocable Trust-->
					<xsl:when test="AccountNo ='2650385'">
						<xsl:value-of select ="'420-20871'"/>
					</xsl:when>
					<!--Kentucky Center-->
					<xsl:when test="AccountNo ='21-75-080-4794669'">
						<xsl:value-of select ="'420-20881'"/>
					</xsl:when>
					<!--Landair Holding Inc.-->
					<xsl:when test="AccountNo ='10571177700'">
						<xsl:value-of select ="'420-25893-98'"/>
					</xsl:when>
					<!--LYRIX-->
					<xsl:when test="AccountNo ='000000000000940'">
						<xsl:value-of select ="'420-10302-95'"/>
					</xsl:when>
					<!--Memorial Hermann Health System-->
					<xsl:when test="AccountNo ='MHNF20011002'">
						<xsl:value-of select ="'420-20883-91'"/>
					</xsl:when>
					<!--Memorial Hermann Benefit Fund-->
					<xsl:when test="AccountNo ='MHBF10011202'">
						<xsl:value-of select ="'420-25476-93'"/>
					</xsl:when>
					<!--The Molly Trust-->
					<xsl:when test="AccountNo ='BR1287'">
						<xsl:value-of select ="'420-20863-95'"/>
					</xsl:when>
					<!--Okabena U.S. Satellite Equity Fund, LLC-->
					<xsl:when test="AccountNo ='12570901'">
						<xsl:value-of select ="'420-07163-99'"/>
					</xsl:when>
					<!--The Purpleville Foundation-->
					<xsl:when test="AccountNo ='14462208'">
						<xsl:value-of select ="'420-26056'"/>
					</xsl:when>
					<!--PVF-KW, LP-->
					<xsl:when test="AccountNo ='VBIF2014002'">
						<xsl:value-of select ="'420-25522-97'"/>
					</xsl:when>
					<!--Quad/Graphics, Inc.-->
					<xsl:when test="AccountNo ='99-9000-80-5'">
						<xsl:value-of select ="'420-20991-90'"/>
					</xsl:when>
					<!--Salisbury Asset Management LLC-->
					<xsl:when test="AccountNo ='03-00311'">
						<xsl:value-of select ="'420-25757-93'"/>
					</xsl:when>
					<!--Salisbury Family Foundation-->
					<xsl:when test="AccountNo ='03-00312'">
						<xsl:value-of select ="'420-25758-92'"/>
					</xsl:when>
					<!--Jay H Baker Children's Trust FBO Stephen M. Baker-->
					<xsl:when test="AccountNo ='2650389'">
						<xsl:value-of select ="'420-25423-97'"/>
					</xsl:when>
					<!--Socatean Partners-->
					<xsl:when test="AccountNo ='441050501'">
						<xsl:value-of select ="'420-24117-71'"/>
					</xsl:when>
					<!--Michael Stern-->
					<xsl:when test="AccountNo ='10571013500'">
						<xsl:value-of select ="'420-25449-97'"/>
					</xsl:when>
					<!--Stetson University-->
					<xsl:when test="AccountNo ='3040000762'">
						<xsl:value-of select ="'420-25699-94'"/>
					</xsl:when>
					<!--Telesis II-->
					<xsl:when test="AccountNo ='25D090994768'">
						<xsl:value-of select ="'420-20733-93'"/>
					</xsl:when>
					<!--UCITS-->
					<xsl:when test="AccountNo ='018419'">
						<xsl:value-of select ="'420-11795-97'"/>
					</xsl:when>
					<!--UCITS - Ireland--><!--
					<xsl:when test="FundAccountNo =''">
						<xsl:value-of select ="'420-26001-95'"/>
					</xsl:when>
					--><!----><!--
					<xsl:when test="FundAccountNo =''">
						<xsl:value-of select ="'420-26304-96'"/>
					</xsl:when>-->
					<!--William S. Kellogg-->
					<xsl:when test="AccountNo ='2614746'">
						<xsl:value-of select ="'420-25420'"/>
					</xsl:when>
					<!--The William S. Kellogg Children's Trust FBO Jeff Kellogg-->
					<xsl:when test="AccountNo ='2650386'">
						<xsl:value-of select ="'420-20865'"/>
					</xsl:when>
					<!--The William S. Kellogg Children's Trust FBO Kurt Kellogg-->
					<xsl:when test="AccountNo ='2650387'">
						<xsl:value-of select ="'420-20866'"/>
					</xsl:when>



					<xsl:otherwise>
						<xsl:value-of select ="AccountNo"/>
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
				<!-- ...and building a ThirdPartyFlatFileDetail for each -->
				<xsl:variable name="taxLotIDVar" select="EntityID"/>

				<xsl:variable name="var1">
					<xsl:value-of select="position()"/>
				</xsl:variable>
				
				<ThirdPartyFlatFileDetail
					  Group_Id="" TRADE_ID ="" ACTION ="" SECURITY ="" WI ="" OPTION_TYPE ="" UNDERLYER ="" STRIKE = "" EXPIRY_DATE = "" METHOD = "" MARKET = "" EXECUTING_BROKER = ""
					  CLEARING_BROKER = "" TRADE_DATE = "" SETTLEMENT_DATE = "" SIDE = "" QUANTITY = "{AllocatedQty}" PRICE = "" COMMISSION_TYPE = "" 
					  COMMISSION_AMOUNT = "" IMPACT_SETTLEMENT_MONEY = "" SETTLEMENT_CURRENCY = "" PREFIGURED_PRINCIPAL = "" INTEREST = "" 
					  SERVICE_FEES = "" POSTAGE ="" STAMP_TAX = "" LEVY_TAX = "" ACCOUNT ="{$FundAccountNol}" ACCOUNT_TYPE = "" IS_ALLOCATION = "Y" 
					  TRAILER_CODE_1 = "" TRAILER_DESCRIPTION_1 = "" VERSUS_PURCHASE_DATE_1 = "" VERSUS_PURCHASE_QUANTITY_1 = "" RR= "" 
								EntityID="{EntityID}" TaxLotState="{TaxLotState}"  FileHeader="TRUE" FileFooter="TRUE"/>
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>
