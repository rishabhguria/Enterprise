<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/ThirdPartyFlatFileDetailCollection">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'true'"/>
				</RowHeader>

			
				<IsCaptionChangeRequired>
					<xsl:value-of select ="'true'"/>
				</IsCaptionChangeRequired>

			
				<TaxLotState>
					<xsl:value-of select ="'TaxLotState'"/>
				</TaxLotState>

        <Side>
          <xsl:value-of select="'Side'"/>
        </Side>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>
        
        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>
        
        <Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

        <AccountNumber>
					<xsl:value-of select="'AccountNumber'"/>
				</AccountNumber>

				<ExecBroker>
					<xsl:value-of select="'ExecBroker'"/>
				</ExecBroker>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <OpenCloseIndicator>
          <xsl:value-of select="'OpenClose Indicator'"/>
        </OpenCloseIndicator>
        <Year>
          <xsl:value-of select="'Year'"/>
        </Year>

        <Month>
          <xsl:value-of select="'Month'"/>
        </Month>

        <Day>
          <xsl:value-of select="'Day'"/>
        </Day>

        <PutCall>
          <xsl:value-of select="'PutCall'"/>
        </PutCall>
        
        <StrikePrice>
          <xsl:value-of select="'StrikePrice'"/>
        </StrikePrice>
				
				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>
			
			
				<xsl:for-each select="ThirdPartyFlatFileDetail[(Asset='EquityOption')]">
				<ThirdPartyFlatFileDetail>
					
					<RowHeader>
						<xsl:value-of select ="'True'"/>
					</RowHeader>
					
					<IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

          <Side>
            <xsl:choose>
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
          </Side>

          <Symbol>
            <xsl:choose>
              <xsl:when test ="Asset='EquityOption'">
                <xsl:value-of select="concat(substring-before(normalize-space(OSIOptionSymbol),' '),substring-after(normalize-space(OSIOptionSymbol),' '))"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </Symbol>

          <Price>
            <xsl:choose>
              <xsl:when test ="Asset='EquityOption'">
                <xsl:choose>
                  <xsl:when test="number(AveragePrice)">
                    <xsl:value-of select ="format-number(AveragePrice,'.0000')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </Price>

          <Quantity>
            <xsl:choose>
              <xsl:when test ="Asset='EquityOption'">
                <xsl:choose>
                  <xsl:when test="number(AllocatedQty)">
                    <xsl:value-of select ="AllocatedQty"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </Quantity>
          <xsl:variable name="PB_NAME" select="'BAML'"/>
          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountNo"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>


          <AccountNumber>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccountNumber>
          
          <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

          <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
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
          <ExecBroker>
            <xsl:value-of select="$Broker"/>
          </ExecBroker>

          <xsl:variable name="COMM">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

          <Commission>
            <xsl:value-of select="format-number($COMM,'.00')"/>
          </Commission>


          <OpenCloseIndicator>
            <xsl:choose>
              <xsl:when test="Side='Buy to Open' or Side='Sell to Open'">
                <xsl:value-of select="'O'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close' or Side='Buy to Close'">
                <xsl:value-of select="'C'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </OpenCloseIndicator>
          <xsl:variable name="ExpiryDate">
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="ExpirationDate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <Year>
            <xsl:value-of select="substring-after(substring-after(ExpirationDate,'/'),'/')"/>
          </Year>
          
          <Month>
            <xsl:value-of select="substring-before(ExpirationDate,'/')"/>
          </Month>
          
          <Day>
            <xsl:value-of select="substring-before(substring-after(ExpirationDate,'/'),'/')"/>
          </Day>

          <PutCall>
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="substring(PutOrCall,1,1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </PutCall>

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
