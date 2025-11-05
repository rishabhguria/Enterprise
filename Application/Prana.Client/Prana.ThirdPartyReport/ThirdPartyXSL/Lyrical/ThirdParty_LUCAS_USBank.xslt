<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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
					<xsl:value-of select="'TaxLotState'"/>
				</TaxLotState>

        <Buy_Sell>
          <xsl:value-of select="'Buy/Sell'"/>
        </Buy_Sell>

        <Trade_Date>
          <xsl:value-of select="'Trade Date'"/>
        </Trade_Date>

        <Settle_Date>
          <xsl:value-of select="'Settle Date'"/>
        </Settle_Date>


        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <DTCBrokerID>
          <xsl:value-of select="'DTC Broker ID'"/>
        </DTCBrokerID>

        <CurrentFace>
          <xsl:value-of select="'Current Face or Shares'"/>
        </CurrentFace>

        <OriginalFace>
          <xsl:value-of select="'OriginalFace'"/>
        </OriginalFace>


        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <PrincipalAmt>
					<xsl:value-of select="'Principal Amt'"/>
				</PrincipalAmt>

        <AccruedInt>
          <xsl:value-of select="'Accrued Int'"/>
        </AccruedInt>

        <Comm>
          <xsl:value-of select="'Comm'"/>
        </Comm>

        <SECFee>
          <xsl:value-of select="'SEC Fee'"/>
        </SECFee>

        <NetMoney>
          <xsl:value-of select="'Net Money'"/>
        </NetMoney>

        <TrustAccountNumber>
          <xsl:value-of select="'Trust Account Number'"/>
        </TrustAccountNumber>

        <Comments>
          <xsl:value-of  select="'Comments'"/>
        </Comments>

				<!-- system use only-->
				<EntityID>
					<xsl:value-of select="'EntityID'"/>
				</EntityID>
			</ThirdPartyFlatFileDetail>

			<xsl:for-each select="ThirdPartyFlatFileDetail">

				<ThirdPartyFlatFileDetail>
					<!--for system internal use-->
					<RowHeader>
						<xsl:value-of select ="'true'"/>
					</RowHeader>

					<!--for system internal use-->
					<IsCaptionChangeRequired>
						<xsl:value-of select ="true"/>
					</IsCaptionChangeRequired>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

          <Buy_Sell>
            <xsl:value-of select="Side"/>
          </Buy_Sell>

          <Trade_Date>
            <xsl:value-of select="TradeDate"/>
          </Trade_Date>

          <Settle_Date>
            <xsl:value-of select="SettlementDate"/>
          </Settle_Date>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>
          
          <xsl:variable name = "PRANA_CounterParty">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>
          <xsl:variable name="PB_BrokerCode">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_BrokerMapping.xml')/BrokerMapping/PB[@Name='USB']/BrokerData[@PranaBroker=$PRANA_CounterParty]/@PBBrokerCode"/>
          </xsl:variable>


          <DTCBrokerID>
            <xsl:value-of select="$PB_BrokerCode"/>
          </DTCBrokerID>

          <CurrentFace>
            <xsl:value-of select="''"/>
          </CurrentFace>

          <OriginalFace>
            <xsl:value-of select="''"/>
          </OriginalFace>

          <Price>
            <xsl:value-of select="format-number(AveragePrice,'#.0000')"/>
          </Price>

          <PrincipalAmt>
            <xsl:value-of select="format-number(GrossAmount,'#.00')"/>
          </PrincipalAmt>

          <AccruedInt>
            <xsl:choose>
              <xsl:when test="number(AccruedInterest)">
                <xsl:value-of select="format-number(AccruedInterest,'0.00')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </AccruedInt>

          <Comm>
            <xsl:value-of select="format-number(CommissionCharged,'0.00')"/>
          </Comm>

          <SECFee>
            <xsl:value-of select="format-number(StampDuty,'0.00')"/>
          </SECFee>

          <NetMoney>
            <xsl:value-of select="format-number(NetAmount,'0.00')"/>
          </NetMoney>

          <TrustAccountNumber>
            <xsl:value-of select="'19-0674'"/>
          </TrustAccountNumber>

          <Comments>
            <xsl:value-of  select="''"/>
          </Comments>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="varLower" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="varUpper" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>