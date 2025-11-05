<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>


				<!--<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>-->


				<!--<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>-->

				<Account>
					<xsl:value-of select="'Account'"/>
					
				</Account>


				
				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>

				<Symbol>	
					<xsl:value-of select="'Symbol'"/>	
				</Symbol>

				<BuyOrSell>
					
					<xsl:value-of select="'BuyOrSell'"/>
						
				</BuyOrSell>

				<TotalVolume>
					
						<xsl:value-of select="'Total Volume'"/>
					
				</TotalVolume>

				<AvgPrice>
						<xsl:value-of select="'Avg Price'"/>
				</AvgPrice>

			

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<TradeDate>
					<xsl:value-of select="'TradeDate'"/>
				</TradeDate>

				<SettleDate>
					<xsl:value-of select="'SettleDate'"/>
				</SettleDate>
			


				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>
				
			</ThirdPartyFlatFileDetail>


		

				<xsl:for-each select="ThirdPartyFlatFileDetail[(Asset='Equity') and (CounterParty!='Transfer' and CounterParty!='GSTMTDIS-SWAP')]">
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<!--<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>-->

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<xsl:variable name="PB_NAME" select="'BAML'"/>
					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>

	    <Account>
            <xsl:choose>
              <xsl:when test="AccountName ='SURS XPONANCE FITHIAN LLC'">
                <xsl:value-of select="'3E901ETF'"/>
              </xsl:when>
              <xsl:when test="AccountName ='Fithian BNY 109595'">
                <xsl:value-of select="'3E901EU1'"/>
              </xsl:when>

              <xsl:when test="AccountName ='Fithian'">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol ='USD'">
                    <xsl:value-of select="'3E901EQR'"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='CHF'">
                    <xsl:value-of select="'9233E90P'"/>
                  </xsl:when>
                  <xsl:when test="CurrencySymbol ='EUR'">
                    <xsl:value-of select="'9173E9HE'"/>
                  </xsl:when>                 
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="AccountNo"/>
              </xsl:otherwise>
            </xsl:choose>
          </Account>


					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@MLPBroker=$PRANA_COUNTERPARTY_NAME]/@PranaBroker"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<Broker>
						<xsl:value-of select="$Broker"/>
					</Broker>

					<Symbol>
						<xsl:choose>
							<xsl:when test ="Symbol!=''">
								<xsl:value-of select="Symbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<BuyOrSell>
						<xsl:choose>
							<xsl:when test="contains(Side,'Sell short')">
								<xsl:value-of select="'SS'"/>
							</xsl:when>
							<xsl:when test="contains(Side,'Buy to Close')">
								<xsl:value-of select="'BC'"/>
							</xsl:when>
							<xsl:when test="contains(Side,'Buy')">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="contains(Side,'Sell')">
								<xsl:value-of select="'S'"/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</BuyOrSell>

					<TotalVolume>
						<xsl:choose>
							<xsl:when test="number(OrderQty)">
								<xsl:value-of select ="OrderQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</TotalVolume>

					<AvgPrice>
						<xsl:choose>
							<xsl:when test="number(AvgPrice)">
								<xsl:value-of select ="format-number(AvgPrice,'.0000')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</AvgPrice>

					<xsl:variable name="COMM">
						<xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
					</xsl:variable>

					<Commission>
						<xsl:value-of select="format-number($COMM,'.00')"/>
					</Commission>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>
				


					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
