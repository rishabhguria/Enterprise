<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  
  <xsl:template name="DateFormate">
    <xsl:param name="Date"/>

    <xsl:variable name="varMonth">
      <xsl:value-of select="substring-before($Date,'/')"/>
    </xsl:variable>

    <xsl:variable name="varDay">
      <xsl:value-of select="substring-before(substring-after($Date,'/'),'/')"/>
    </xsl:variable>

    <xsl:variable name="varYear">
      <xsl:value-of select="substring-after(substring-after($Date,'/'),'/')"/>
    </xsl:variable>
    
        <xsl:value-of select="concat($varDay,'-',$varMonth,'-',$varYear)"/>

  </xsl:template>
  
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <!--for system use only-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select ="'TaxLotState'"/>
        </TaxLotState>

        <Action>
          <xsl:value-of select ="'Action'"/>
        </Action>
        
        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <SettleDate>
          <xsl:value-of select="'SettleDate'"/>
        </SettleDate>

        <ISIN>
          <xsl:value-of select="'ISIN'"/>
        </ISIN>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <Ticker>
          <xsl:value-of select="'Ticker'"/>
        </Ticker>

        <Name>
          <xsl:value-of select="'Name'"/>
        </Name>

        <Shares>
          <xsl:value-of select="'Shares'"/>
        </Shares>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>

        <GrossPrincipal>
          <xsl:value-of select="'GrossPrincipal'"/>
        </GrossPrincipal>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <SECFees>
          <xsl:value-of select="'SEC Fees'"/>
        </SECFees>

        <Net>
          <xsl:value-of select="'Net'"/>
        </Net>

        <!-- system inetrnal use-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      
        <xsl:for-each select="ThirdPartyFlatFileDetail">
          
      <ThirdPartyFlatFileDetail>
        <xsl:if test="CounterParty='SMBC'">
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <!--for system use only-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <Action>
          <xsl:value-of select ="translate(Side,$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
        </Action>
        
        <xsl:variable name="PB_NAME" select="''"/>
        <xsl:variable name = "PRANA_FUND_NAME">
          <xsl:value-of select="AccountName"/>
        </xsl:variable>

        <xsl:variable name ="THIRDPARTY_FUND_CODE">
          <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
        </xsl:variable>

        <Account>
          <xsl:choose>
            <xsl:when test="$PRANA_FUND_NAME = 'LYRIX-000000000000940'">
              <xsl:value-of select="'P6B755486'"/>
            </xsl:when>
            <xsl:when test="$PRANA_FUND_NAME = 'UCITS: 018419'">
              <xsl:value-of select="'P6B755924'"/>
            </xsl:when>
            <xsl:when test="$PRANA_FUND_NAME = 'SSCSIL SIG LYRICAL FUND'">
              <xsl:value-of select="'P6B769255'"/>
            </xsl:when>
            <xsl:when test="$PRANA_FUND_NAME = 'TWGEFF LYRICAL ASSET MANGMNT: TWY08'">
              <xsl:value-of select="'P6B769271'"/>
            </xsl:when>
            <xsl:when test="$PRANA_FUND_NAME = 'Alliance Trust PLC LAM Natwest Dep: 372503'">
              <xsl:value-of select="'P6B769289'"/>
            </xsl:when>
            <xsl:when test="$PRANA_FUND_NAME = 'WTW GT Lyrical: 225240'">
              <xsl:value-of select="'P6B769297'"/>
            </xsl:when>
            <xsl:when test="$PRANA_FUND_NAME = 'The Citigroup Plan: 576064'">
              <xsl:value-of select="'P6B771848'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$PRANA_FUND_NAME"/>
            </xsl:otherwise>
          </xsl:choose>
        </Account>


        <xsl:variable name="varTradeDate" select="TradeDate"/>
        
        <xsl:variable name="TradeDate">
          <xsl:call-template name="DateFormate">
            <xsl:with-param name="Date" select="$varTradeDate"/>
          </xsl:call-template>
        </xsl:variable>
        <TradeDate>
          <xsl:value-of select="$TradeDate"/>
        </TradeDate>

        <xsl:variable name="varSettlementDate" select="SettlementDate"/>
        
        <xsl:variable name="SettlementDate">
          <xsl:call-template name="DateFormate">
            <xsl:with-param name="Date" select="$varSettlementDate"/>
          </xsl:call-template>
        </xsl:variable>
        <SettleDate>
          <xsl:value-of select="$SettlementDate"/>
        </SettleDate>

        <ISIN>
          <xsl:value-of select="ISIN"/>
        </ISIN>

        <CUSIP>
          <xsl:value-of select="CUSIP"/>
        </CUSIP>

        <Ticker>
          <xsl:value-of select="Symbol"/>
        </Ticker>

        <Name>
          <xsl:value-of select="FullSecurityName"/>
        </Name>

        <Shares>
          <xsl:value-of select="ExecutedQty"/>
        </Shares>

          <xsl:variable name="Price">
            <xsl:value-of select="AveragePrice"/>
          </xsl:variable>

          <Price>
            <xsl:value-of select="format-number($Price,'0.####')"/>
          </Price>

        <xsl:variable name="Gross">
          <xsl:value-of select="GrossAmount"/>
        </xsl:variable>
        
          <GrossPrincipal>
            <xsl:value-of select="format-number($Gross,'0.##')"/>
          </GrossPrincipal>

        <xsl:variable name="Commission">
          <xsl:value-of select="CommissionCharged"/>
        </xsl:variable>
        
          <Commission>
            <xsl:value-of select="format-number($Commission,'0.##')"/>
          </Commission>

        <xsl:variable name="SecFee">
          <xsl:value-of select="SecFee"/>
        </xsl:variable>
        
          <SECFees>
            <xsl:value-of select="format-number($Commission,'0.##')"/>
          </SECFees>

        <xsl:variable name="NetAmount">
          <xsl:value-of select="NetAmount"/>
        </xsl:variable>
        
          <Net>
            <xsl:value-of select="format-number($NetAmount,'0.##')"/>
          </Net>

          <!-- system inetrnal use-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>
        </xsl:if>
        </ThirdPartyFlatFileDetail>
       
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>

