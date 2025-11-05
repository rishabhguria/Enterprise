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

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="//ThirdPartyFlatFileDetail[AssetID=2]">

        <ThirdPartyFlatFileDetail>

          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'False'"/>
          </RowHeader>
          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name ="varCount" select ="position()" />

          <xsl:variable name = "recordCount" >
            <xsl:call-template name="noofzeros">
              <xsl:with-param name="count" select="(5) - string-length($varCount)" />
            </xsl:call-template>
          </xsl:variable>

          <Tag>
            <xsl:value-of select ="concat($recordCount,$varCount)"/>
          </Tag>

          <Jefferies_Client_Number>
            <xsl:value-of select ="'032'"/>
          </Jefferies_Client_Number>

          <AccountID>
            <xsl:value-of select ="FundAccountNo"/>
          </AccountID>

          <xsl:choose>
            <xsl:when test ="Side = 'Buy to Open' or Side='Sell to Open'">
              <Open_Close>
                <xsl:value-of select ="'O'"/>
              </Open_Close>
            </xsl:when>
            <xsl:when test ="Side='Sell to Close' or Side = 'Buy to Close'">
              <Open_Close>
                <xsl:value-of select ="'C'"/>
              </Open_Close>
            </xsl:when>
            <xsl:otherwise>
              <Open_Close>
                <xsl:value-of select ="''"/>
              </Open_Close>
            </xsl:otherwise>
          </xsl:choose>


          <!-- Side Starts-->
          <xsl:choose>
            <xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Cover' or Side='Buy to Close'">
              <Buy_Sell>
                <xsl:value-of select="'B'"/>
              </Buy_Sell>
            </xsl:when>
            <xsl:when test="Side='Sell' or Side='Sell to Close'">
              <Buy_Sell>
                <xsl:value-of select="'S'"/>
              </Buy_Sell>
            </xsl:when>
            <!--<xsl:when test="Side='Buy to Cover' or Side='Buy to Close'">
              <Buy_Sell>
                <xsl:value-of select="'BC'"/>
              </Buy_Sell>
            </xsl:when>-->
            <xsl:when test="Side='Sell short' or Side='Sell to Open'">
              <Buy_Sell>
                <xsl:value-of select="'SS'"/>
              </Buy_Sell>
            </xsl:when>
            <xsl:otherwise >
              <Buy_Sell>
                <xsl:value-of select="''"/>
              </Buy_Sell>
            </xsl:otherwise>
          </xsl:choose >

          <SecurityType>
            <xsl:value-of select ="'O'"/>
          </SecurityType>

          <Quantity>
            <xsl:value-of select ="AllocatedQty"/>
          </Quantity>

          <Price>
            <xsl:value-of select ="AveragePrice"/>
          </Price>

			<xsl:choose>
				<xsl:when test ="Asset='EquityOption'">
					<!--<SYMBOL>
						<xsl:value-of select="substring-before(Symbol,' ')"/>
					</SYMBOL>-->
					<SYMBOL>
						<xsl:value-of select="UnderlyingSymbol"/>
					</SYMBOL>
				</xsl:when>
				<xsl:otherwise>
					<SYMBOL>
						<xsl:value-of select="Symbol"/>
					</SYMBOL>
				</xsl:otherwise>
			</xsl:choose>

          <Description>
            <xsl:value-of select="''"/>
          </Description>

          <TradeDate>
            <xsl:value-of select ="concat(substring(TradeDate,7,4),substring(TradeDate,1,2),substring(TradeDate,4,2))"/>
          </TradeDate>

          <SettleDate>
            <xsl:value-of select ="concat(substring(SettlementDate,7,4),substring(SettlementDate,1,2),substring(SettlementDate,4,2))"/>
          </SettleDate>


          <ExecutingFirm>
            <xsl:value-of select ="''"/>
          </ExecutingFirm>

          <WhenIssueFlag>
            <xsl:value-of select="''"/>
          </WhenIssueFlag>

          <Interest>
            <xsl:value-of select ="''"/>
          </Interest>

          <CommissionIndicator>
            <xsl:value-of select ="''"/>
          </CommissionIndicator>

          <CommissionAmount>
            <xsl:value-of select ="''"/>
          </CommissionAmount>

          <Principal>
            <xsl:value-of select ="''"/>
          </Principal>

          <SECFee>
            <xsl:value-of select ="''"/>
          </SECFee>

          <PostageCharge>
            <xsl:value-of select ="''"/>
          </PostageCharge>

          <OtherMoney>
            <xsl:value-of select ="''"/>
          </OtherMoney>

          <TrailerCode>
            <xsl:value-of select ="''"/>
          </TrailerCode>

          <BlotterCode>
            <xsl:value-of select ="''"/>
          </BlotterCode>

          <AE>
            <xsl:value-of select ="''"/>
          </AE>

          <Cusip>
            <xsl:value-of select ="''"/>
          </Cusip>

          <Rule80A>
            <xsl:value-of select ="''"/>
          </Rule80A>

          <OptionExpiration>
            <xsl:value-of select="concat(substring(ExpirationDate,9,2),substring(ExpirationDate,1,2),substring(ExpirationDate,4,2))"/>
          </OptionExpiration>

          <xsl:choose>
            <xsl:when test ="PutOrCall='CALL'">
              <Call_Put>
                <xsl:value-of select ="'C'"/>
              </Call_Put>
            </xsl:when>
            <xsl:when test ="PutOrCall = 'PUT'">
              <Call_Put>
                <xsl:value-of select ="'P'"/>
              </Call_Put>
            </xsl:when>
            <xsl:otherwise>
              <Call_Put>
                <xsl:value-of select ="PutOrCall"/>
              </Call_Put>
            </xsl:otherwise>
          </xsl:choose>         
         

          <StrikePrice>
            <xsl:value-of select ="StrikePrice"/>
          </StrikePrice>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
