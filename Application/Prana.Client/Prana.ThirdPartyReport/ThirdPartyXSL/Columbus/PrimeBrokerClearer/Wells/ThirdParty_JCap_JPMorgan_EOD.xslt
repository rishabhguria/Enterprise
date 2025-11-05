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

		<xsl:variable name="PB_EXCHANGE">
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
		</xsl:variable>

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
			METHOD = "{$varMethod}" MARKET = "{$varExchangeName}" EXECUTING_BROKER = "{$varCounterParty}" CLEARING_BROKER = "" TRADE_DATE = "{TradeDate}" SETTLEMENT_DATE = "{SettlementDate}"
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
					  SERVICE_FEES = "" POSTAGE ="" STAMP_TAX = "" LEVY_TAX = "" ACCOUNT ="{FundAccountNo}" ACCOUNT_TYPE = "" IS_ALLOCATION = "Y" 
					  TRAILER_CODE_1 = "" TRAILER_DESCRIPTION_1 = "" VERSUS_PURCHASE_DATE_1 = "" VERSUS_PURCHASE_QUANTITY_1 = "" RR= "" 
								EntityID="{EntityID}" TaxLotState="{TaxLotState}"  FileHeader="TRUE" FileFooter="TRUE"/>
			</xsl:for-each>
		</Group>
	</xsl:template>
</xsl:stylesheet>
