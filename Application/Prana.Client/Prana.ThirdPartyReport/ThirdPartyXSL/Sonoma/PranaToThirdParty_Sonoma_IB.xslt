<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
        <!-- if do not print the column headers, then uncomment the RowHeader tag-->
        <!--<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>-->

        <!--for system use only-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!-- for system use only-->
        <TaxlotState>
          <xsl:value-of select ="'TaxlotState'"/>
        </TaxlotState>

        <AccountNumber>
          <xsl:value-of select="'IB Acct #'"/>
        </AccountNumber>

        <Symbol>
          <xsl:value-of select ="'Symbol'"/>
        </Symbol>

        <SecurityType>
          <xsl:value-of select ="'Security Type'"/>
        </SecurityType>

        <Action>
          <xsl:value-of select ="'Action'"/>
        </Action>

        <Quantity>
          <xsl:value-of select ="'#Shares'"/>
        </Quantity>

        <TradePrice>
          <xsl:value-of select="'Trade Price'"/>
        </TradePrice>

        <Currency>
          <xsl:value-of select="'Currency'"/>
        </Currency>

        <Commission>
          <xsl:value-of select ="'Total Comm'"/>
        </Commission>

        <CUSIP>
          <xsl:value-of select ="'CUSIP'"/>
        </CUSIP>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'S/D'"/>
        </SettleDate>

        <Broker>
          <xsl:value-of select ="'Broker'"/>
        </Broker>
        <OCC_DTC>
          <xsl:value-of select ="'OCC/DTC'"/>
        </OCC_DTC>

        <!-- system internal use -->
        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          <!-- if do not print the column headers, then uncomment the RowHeader tag-->
          <!--<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>-->

          <!--for system use only-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>
					
					<!-- for internal use -->
					<TaxlotState>
						<xsl:value-of select ="TaxLotState"/>
					</TaxlotState>

					<AccountNumber>
						<xsl:value-of select="FundAccountNo"/>
					</AccountNumber>
			<xsl:variable name ="varCheckWarrant">
				<xsl:value-of select ="substring-before(Symbol,'/W')"/>
			</xsl:variable>

			<xsl:choose>
				<xsl:when test="$varCheckWarrant!=''">
					<Symbol>
						<xsl:value-of select="concat(translate(Symbol,'/W',' W'),'S')"/>
					</Symbol>
				</xsl:when>
						<xsl:when test="Asset ='Equity' ">
							<Symbol>
								<xsl:value-of select ="Symbol"/>
							</Symbol>
						</xsl:when>
						<xsl:when test="Asset ='EquityOption' ">
							<Symbol>
								<xsl:value-of select ="OSIOptionSymbol"/>
							</Symbol>
							
							<!--<Symbol>
								<xsl:value-of select ="translate(Symbol, ' ', '+')"/>
							</Symbol>-->
						</xsl:when>
						<xsl:when test="Asset ='Future' ">
							<Symbol>
								<xsl:value-of select ="translate(Symbol, ' ', '')"/>
							</Symbol>
						</xsl:when>
						<xsl:otherwise>
							<Symbol>
								<xsl:value-of select="Symbol"/>
							</Symbol>
					</xsl:otherwise>
						<!--<xsl:value-of select ="Symbol"/>-->
					</xsl:choose>

					<!--define security type-->
					<xsl:choose>
						<xsl:when test="Asset ='Equity' ">
							<SecurityType>
								<xsl:value-of select ="'EQUITY'"/>
							</SecurityType>
						</xsl:when>
						<xsl:when test="Asset ='EquityOption' ">
							<SecurityType>
								<xsl:value-of select ="'OPTIONS'"/>
							</SecurityType>
						</xsl:when>
						<xsl:when test="Asset ='Future' ">
							<SecurityType>
								<xsl:value-of select ="'FUTURE'"/>
							</SecurityType>
						</xsl:when>						
						<xsl:when test="Asset ='FutureOption' ">
							<SecurityType>
								<xsl:value-of select ="'FUTUREOPTION'"/>
							</SecurityType>
						</xsl:when>
						<xsl:otherwise>
							<SecurityType>
								<xsl:value-of select="Asset"/>
							</SecurityType>
						</xsl:otherwise>
					</xsl:choose>

					<xsl:choose>
						<xsl:when test="Side='SellShort'">
							<Action>
								<xsl:value-of select ="'Sell short'"/>
							</Action>
						</xsl:when>
						<xsl:otherwise>
							<Action>
							<xsl:value-of select ="Side"/>
							</Action>
						</xsl:otherwise>
					</xsl:choose>

					<Quantity>
						<xsl:value-of select ="AllocatedQty"/>
					</Quantity>

					<TradePrice>
						<xsl:value-of select="AveragePrice"/>
					</TradePrice>
					
					<Currency>
						<xsl:value-of select="CurrencySymbol"/>
					</Currency>

					<Commission>
						<xsl:value-of select ="CommissionCharged"/>						
					</Commission>

				     <CUSIP>
						<xsl:value-of select ="CUSIP"/>
					</CUSIP>

					<TradeDate>
						<xsl:value-of select="TradeDate"/>
					</TradeDate>

					<SettleDate>
						<xsl:value-of select="SettlementDate"/>
					</SettleDate>

					<xsl:variable name="VarCONTERPARTY">
						<xsl:value-of select ="CounterParty"/>
					</xsl:variable>

					<xsl:variable name="PB_CODE">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='IB']/BrokerData[@PranaBroker=$VarCONTERPARTY]/@PBCode"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="$PB_CODE != ''">
							<Broker>
								<xsl:value-of select ="$PB_CODE"/>
							</Broker>
						</xsl:when>
						<xsl:otherwise>
							<Broker>
								<xsl:value-of select ="CounterParty"/>
							</Broker>
						</xsl:otherwise>
					</xsl:choose>


					<xsl:variable name="PB_CLEARINGCODE">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='IB']/BrokerData[@PranaBroker=$VarCONTERPARTY]/@ClearingCode"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="$PB_CODE != ''">
							<OCC_DTC>
								<xsl:value-of select ="$PB_CLEARINGCODE"/>
							</OCC_DTC>
						</xsl:when>
						<xsl:otherwise>
							<OCC_DTC>
								<xsl:value-of select ="''"/>
							</OCC_DTC>
						</xsl:otherwise>
					</xsl:choose>


					<!-- system internal use -->
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>
			</xsl:for-each>
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>
