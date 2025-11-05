<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template name="MonthName">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month=1">
        <xsl:value-of select="'JAN'"/>
      </xsl:when>
      <xsl:when test="$Month=2">
        <xsl:value-of select="'FEB'"/>
      </xsl:when>
      <xsl:when test="$Month=3">
        <xsl:value-of select="'MAR'"/>
      </xsl:when>
      <xsl:when test="$Month=4">
        <xsl:value-of select="'APR'"/>
      </xsl:when>
      <xsl:when test="$Month=5">
        <xsl:value-of select="'MAY'"/>
      </xsl:when>
      <xsl:when test="$Month=6">
        <xsl:value-of select="'JUN'"/>
      </xsl:when>
      <xsl:when test="$Month=7">
        <xsl:value-of select="'JUL'"/>
      </xsl:when>
      <xsl:when test="$Month=8">
        <xsl:value-of select="'AUG'"/>
      </xsl:when>
      <xsl:when test="$Month=9">
        <xsl:value-of select="'SEP'"/>
      </xsl:when>
      <xsl:when test="$Month=10">
        <xsl:value-of select="'OCT'"/>
      </xsl:when>
      <xsl:when test="$Month=11">
        <xsl:value-of select="'NOV'"/>
      </xsl:when>
      <xsl:when test="$Month=12">
        <xsl:value-of select="'DEC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxlotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxlotState>

          <xsl:variable name="PB_NAME" select="'IB'"/>


          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>


          <IBAcct>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </IBAcct>

          <xsl:variable name="Symbol">
            <xsl:choose>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="OSIOptionSymbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <Symbol>

            <xsl:value-of select="$Symbol"/>
          </Symbol>

          <SecurityType>
			  <xsl:choose>
				  <xsl:when test="Asset='EquityOption'">
					  <xsl:value-of select="'OPTIONS'"/>
				  </xsl:when>
				  <xsl:when test="Asset='FixedIncome'">
					  <xsl:value-of select="'BOND'"/>
				  </xsl:when>
				  <xsl:when test="Asset='Future'">
					  <xsl:value-of select="'FUT'"/>
				  </xsl:when>
				  <xsl:when test="Asset='FutureOption'">
					  <xsl:value-of select="'FUT OPT'"/>
				  </xsl:when>
				  <xsl:when test="Asset='Equity'">
					  <xsl:value-of select="'EQUITY'"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="Asset"/>
				  </xsl:otherwise>
				  
			  </xsl:choose>

            
          </SecurityType>

          <Action>

            <xsl:choose>
              <xsl:when test="Side='Buy' or Side='Buy to Open'">
                <xsl:value-of select="'Buy'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'Buy to Cover'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select="'Sell Short'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close' or Side='Sell'">
                <xsl:value-of select="'Sell'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Action>

          <Shares>

            <xsl:value-of select="AllocatedQty"/>
          </Shares>

          <TradePrice>
            <xsl:value-of select="AveragePrice"/>

          </TradePrice>

          <Currency>
            <xsl:value-of select="CurrencySymbol" />

          </Currency>

          <CommPershare>
            <xsl:value-of select="format-number(CommissionCharged div AllocatedQty,'#.####')"/>

          </CommPershare>

          <Cusip>

            <xsl:value-of select="''"/>
          </Cusip>

          <TradeDate>

            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SD>
            <xsl:value-of select="SettlementDate"/>

          </SD>

          <BrokerName>
            <xsl:value-of select="CounterParty"/>
          </BrokerName>





          <xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>
          <xsl:variable name="Exchange_Name" select="Exchange"/>
          <xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
            <xsl:value-of select="document('../ReconMappingXml/ThirdParty_Exchange_Broker_Mapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@Broker=$PRANA_COUNTERPARTY_NAME and @Exchange=$Exchange_Name]/@BrokerCode"/>
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

          <BrokerCode>
            <xsl:value-of select="$Broker"/>
          </BrokerCode>

			<Exchange>
				<xsl:value-of select="''"/>
			</Exchange>

			<SettleAmount>
				<xsl:value-of select="''"/>
			</SettleAmount>

			<ISIN>
				<xsl:value-of select="''"/>
			</ISIN>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>

      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>