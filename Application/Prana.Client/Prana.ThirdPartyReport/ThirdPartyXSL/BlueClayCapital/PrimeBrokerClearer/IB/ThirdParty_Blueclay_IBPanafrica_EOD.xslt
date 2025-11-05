<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">

    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

        <IBAccountNO>
          <xsl:value-of select="'IB Acct #'"/>
        </IBAccountNO>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <SecurityType>
          <xsl:value-of select="'Security Type'"/>
        </SecurityType>

        <Action>
          <xsl:value-of select="'Action'"/>
        </Action>

        <Shares>
          <xsl:value-of select="'# Shares'"/>
        </Shares>

        <TradePrice>
          <xsl:value-of select="'Trade Price'"/>
        </TradePrice>

        <Currency>
          <xsl:value-of select="'Currency'"/>
        </Currency>

        <Commission>
          <xsl:value-of select="'Comm/Sh'"/>
        </Commission>

        <CUSIPISIN>
          <xsl:value-of select="'Cusip/ISIN'"/>
        </CUSIPISIN>

        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>

        <SD>
          <xsl:value-of select="'S/D'"/>
        </SD>

        <BrokerName>
          <xsl:value-of select="'Broker Name'"/>
        </BrokerName>

        <BrokerCode>
          <xsl:value-of select="'Broker Code'"/>
        </BrokerCode>

        <Exchange>
          <xsl:value-of select="'Exchange'"/>
        </Exchange>

        <SettlementAmount>
          <xsl:value-of select="'Settlement Amount'"/>
        </SettlementAmount>

        <ISINForHKD>
          <xsl:value-of select="'ISIN for HK'"/>
        </ISINForHKD>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail[AccountName='PANAFR-IB 3356' and Asset='Equity' and CounterParty='FCSTNE']">


        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountNo"/>
          </xsl:variable>

          <xsl:variable name ="PB_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='IB']/FundData[@NirvanaAccountName=$PRANA_FUND_NAME]/@AccountNumber"/>
          </xsl:variable>
          <IBAccountNO>
            <xsl:choose>
              <xsl:when test ="$PB_FUND_CODE!=''">
                <xsl:value-of select ="$PB_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </IBAccountNO>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <SecurityType>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'EQUITY'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityType>

          <Action>
            <xsl:choose>
              <xsl:when test="Asset='Equity' and Side='Buy to Close'">
                <xsl:value-of select="'Buy to Cover'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
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
            <xsl:value-of select="CurrencySymbol"/>
          </Currency>

         

          <xsl:variable name="COMM2">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>         
          <Commission>
            <xsl:choose>
              <xsl:when test="number($COMM2)">
                <xsl:value-of select="format-number($COMM2 div (AllocatedQty),'0.####')"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Commission>

          <CUSIPISIN>
            <xsl:value-of select="''"/>
          </CUSIPISIN>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SD>
            <xsl:value-of select="''"/>
          </SD>


          <xsl:variable name="PB_NAME" select="'IB'"/>
          
          <xsl:variable name = "PB_CounterParty">
            <xsl:value-of select="CounterParty"/>
          </xsl:variable>

          <xsl:variable name="PRANA_BrokerCode">
            <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBrokerCode=$PB_CounterParty]/@PranaBroker"/>
          </xsl:variable>
         
          <BrokerName>
            <xsl:choose>
              <xsl:when test="CounterParty='FCSTNE'">
                <xsl:value-of select="'FCST'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="$PB_CounterParty"/>
              </xsl:otherwise>
            </xsl:choose>
          </BrokerName>

          <BrokerCode>
            <xsl:choose>
              <xsl:when test="CurrencySymbol='GBP'">
                <xsl:value-of select="'601'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'443'"/>
              </xsl:otherwise>
            </xsl:choose>            
          </BrokerCode>

          <Exchange>
            <xsl:value-of select="''"/>
          </Exchange>

          <SettlementAmount>
            <xsl:value-of select="''"/>
          </SettlementAmount>

          <ISINForHKD>
            <xsl:value-of select="''"/>
          </ISINForHKD>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

</xsl:stylesheet>