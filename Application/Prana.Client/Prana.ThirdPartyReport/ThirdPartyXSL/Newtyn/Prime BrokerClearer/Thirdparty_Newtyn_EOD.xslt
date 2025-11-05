<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="GetMonth">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 1" >
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month = 2" >
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month = 3" >
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month = 4" >
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month = 5" >
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month = 6" >
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month = 7" >
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month = 8" >
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month = 9" >
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month = 10" >
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month = 11" >
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month = 12" >
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>



	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<xsl:for-each select="ThirdPartyFlatFileDetail[Symbol !='BYMA AR']">
				

				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<FileHeader>
						<xsl:value-of select="'true'"/>
					</FileHeader>

					<FileFooter>
						<xsl:value-of select="'true'"/>
					</FileFooter>
					
					<TaxlotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxlotState>

					<ClientReferencenumber>
						<xsl:value-of select="PBUniqueID"/>
					</ClientReferencenumber>

					<Side>
						<xsl:choose>


							<xsl:when test="Side='Buy to Open'">
								<xsl:value-of select="'BTO'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Close'">
								<xsl:value-of select="'STC'"/>
							</xsl:when>
							<xsl:when test="Side='Sell to Open'">
								<xsl:value-of select="'STO'"/>
							</xsl:when>
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
								<xsl:value-of select="'BTC'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="Side"/>
							</xsl:otherwise>
						</xsl:choose>
					</Side>

					<Symbol>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="OSIOptionSymbol"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>

					</Symbol>

					<CusipSedol>
						<xsl:choose>
							<xsl:when test="Symbol='BITC/U CN'">
								<xsl:value-of select="'09173K404'"/>
							</xsl:when>
							<xsl:when test="SEDOL !=''">
								<xsl:value-of select="SEDOL"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="CUSIP"/>

							</xsl:otherwise>
						</xsl:choose>

					</CusipSedol>
					
					<xsl:variable name="varAssetMultiplier">
						<!-- <xsl:value-of select="(AllocatedQty * AssetMultiplier)"/> -->
						<xsl:value-of select="(AllocatedQty)"/>
					</xsl:variable>
					<Qty>
						<xsl:value-of select="$varAssetMultiplier"/>
					</Qty>

					<!--<InternalNetNotional>
						<xsl:value-of select="$varAssetMultiplier"/>
					</InternalNetNotional>-->
					
					<xsl:variable name="PB_NAME" select="'Fidelity'"/>

					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='$PB_NAME']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
              <xsl:when test="CurrencySymbol='CHF' or CurrencySymbol='DKK' or CurrencySymbol='EUR' or CurrencySymbol='GBP' or CurrencySymbol='GBX' or CurrencySymbol='NOK'">
                <xsl:choose>
                  <xsl:when test="CounterParty='TOUR'">
                    <xsl:value-of select="'TOPA'"/>
                  </xsl:when>
                </xsl:choose>
				      </xsl:when>
							<xsl:when test="CounterParty ='JONO'">
								<xsl:value-of select="'JONE'"/>
							</xsl:when>
							<xsl:when test="CounterParty ='CNGF'">
								<xsl:value-of select="'CCAM'"/>
							</xsl:when>
							<xsl:when test="CounterParty ='GMPS'">
								<xsl:value-of select="'FCST'"/>
							</xsl:when>
							<xsl:when test="CounterParty ='ODEO'">
								<xsl:value-of select="'ODCO'"/>
							</xsl:when>
              <xsl:when test="CounterParty='CGOA'">
                <xsl:value-of select="'CITI'"/>
              </xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<ExBroker>
						<xsl:value-of select="$Broker"/>
					</ExBroker>

					<BlankColumn>
						<xsl:value-of select="''"/>
					</BlankColumn>

					<SettleCcy>
						<xsl:value-of select="SettlCurrency"/>
					</SettleCcy>

					<Price>
						<xsl:value-of select="AveragePrice"/>
					</Price>



					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>


					<xsl:variable name="varAccountName">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<Account>
						<xsl:value-of select="$varAccountName"/>
					</Account>

					<xsl:variable name="varDate">
						<xsl:value-of select="concat(substring-after(substring-after(TradeDate,'/'),'/'),substring-before(TradeDate,'/'),substring-before(substring-after(TradeDate,'/'),'/'))"/>
					</xsl:variable>

					<TradeDate>
						<xsl:value-of select="$varDate"/>
					</TradeDate>

					<xsl:variable name="varSettleDate">
						<xsl:value-of select="concat(substring-after(substring-after(SettlementDate,'/'),'/'),substring-before(SettlementDate,'/'),substring-before(substring-after(SettlementDate,'/'),'/'))"/>
					</xsl:variable>
					<SettleDate>
						<xsl:value-of select="$varSettleDate"/>
					</SettleDate>

					<Asset>
						<xsl:choose>
							<xsl:when test="Asset='Equity'">
								<xsl:value-of select="'STOCK'"/>
							</xsl:when>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="'OPTION'"/>
							</xsl:when>
							<xsl:when test="Asset='Future'">
								<xsl:value-of select="'FUTURE'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Asset"/>
							</xsl:otherwise>
						</xsl:choose>

					</Asset>

					<Interest>
						<xsl:value-of select="''"/>
					</Interest>

					<CancelCorrect>
						<xsl:choose>

							<xsl:when test ="TaxLotState = 'Amended'">
								<xsl:value-of  select="'X'"/>
							</xsl:when>
							<xsl:when test ="TaxLotState = 'Deleted'">
								<xsl:value-of  select="'C'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of  select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</CancelCorrect>

					<BlankColumn1>
						<xsl:value-of select="''"/>
					</BlankColumn1>

					<CommType>
						<xsl:value-of select="'G'"/>
					</CommType>

					<xsl:variable name="varTotalCommission">
						<xsl:value-of select="SoftCommissionCharged + CommissionCharged"/>
					</xsl:variable>
					<Commission>
						<xsl:value-of select="format-number($varTotalCommission,'#.##')"/>
					</Commission>

					<NetMoney>
						<xsl:value-of select="format-number(NetAmount,'#.##')"/>
					</NetMoney>
					
					
					
					<UnderlyingSymbol>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="UnderlyingSymbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

					</UnderlyingSymbol>
					
					<xsl:variable name="varExpirationDateDate">
						<xsl:value-of select="concat(substring-after(substring-after(ExpirationDate,'/'),'/'),substring-before(ExpirationDate,'/'),substring-before(substring-after(ExpirationDate,'/'),'/'))"/>
					</xsl:variable>

					<ExpirationDate>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="$varExpirationDateDate"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</ExpirationDate>	
					
					<OptionType>
						<xsl:value-of select="substring(PutOrCall,1,1)"/>
					</OptionType>

					<StrikePrice>
						<xsl:choose>
							<xsl:when test="Asset='EquityOption'">
								<xsl:value-of select="StrikePrice"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>

						
					</StrikePrice>


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
