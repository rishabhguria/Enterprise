<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>
        <!--for system internal use-->
        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <!--for system internal use-->
        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <!--for system internal use-->
        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>

        <TRADETYPE>
          <xsl:value-of select="'TRADE TYPE'"/>
        </TRADETYPE>

        <TRADINGACCT>
          <xsl:value-of select="'TRADING ACCT'"/>
        </TRADINGACCT>

        <TRADEDATE>
          <xsl:value-of select="'TRADE DATE'"/>
        </TRADEDATE>

        <BUY_SELL>
          <xsl:value-of select="'BUY/SELL IND'"/>
        </BUY_SELL>

        <SYMBOL>
          <xsl:value-of select="'SYMBOL'"/>
        </SYMBOL>

        <QUANTITY>
          <xsl:value-of select="'QUANTITY'"/>
        </QUANTITY>

        <PRICE>
          <xsl:value-of select="'PRICE'"/>
        </PRICE>

        <exec_brkr>
          <xsl:value-of select="'EXEC BRKR'"/>
        </exec_brkr>

        <COMMCODE>
          <xsl:value-of select="'COMM CODE'"/>
        </COMMCODE>

        <COMMValue>
          <xsl:value-of select="'COMM $'"/>
        </COMMValue>
        
        <RR>
          <xsl:value-of select="'RR'"/>
        </RR>

        <BLOTTER>
          <xsl:value-of select="'BLOTTER'"/>
        </BLOTTER>

        <CONTRA_BRKER>
          <xsl:value-of select ="'CONTRA BRKER'"/>
        </CONTRA_BRKER>

        <STRIKE_PX>
          <xsl:value-of select ="'STRIKE PX'"/>
        </STRIKE_PX>

        <PUT_CALL>
          <xsl:value-of select="'PUT/CALL IND'"/>
        </PUT_CALL>

        <EXPIRATION_DATE>
          <xsl:value-of select="'EXPIRATION DATE'"/>
        </EXPIRATION_DATE>

        <!-- system use only-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>
      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>

          <!--for system internal use-->
          <IsCaptionChangeRequired>
            <xsl:value-of select ="'true'"/>
          </IsCaptionChangeRequired>

          <FileHeader>
            <xsl:value-of select ="'true'"/>
          </FileHeader>
          <FileFooter>
            <xsl:value-of select ="'true'"/>
          </FileFooter>

          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <TRADETYPE>
            <xsl:choose>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="'BTS'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'CMTA'"/>
              </xsl:otherwise>
            </xsl:choose>
          </TRADETYPE>

          <TRADINGACCT>
            <xsl:choose>
              <xsl:when test="AccountName='Maerisland S-1 BNY'">
                <xsl:value-of select="'141-65147-90 (BONY -  DTC# 0901)'"/>
              </xsl:when>
              <xsl:when test="AccountName='Maerisland S-1 GSCO'">
                <xsl:value-of select="'141-65148-99  (GSCO – DTC# 0005)'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'141-65149-90  (JEFF – DTC# 0019)'"/>
              </xsl:otherwise>
            </xsl:choose>
          </TRADINGACCT>

          <TRADEDATE>
            <xsl:value-of select="TradeDate"/>
          </TRADEDATE>

          <BUY_SELL>
            <xsl:choose>
              <xsl:when test="Side = 'Buy'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
              <xsl:when test="Side = 'Buy to Close'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:when test="Side = 'Sell short'">
                <xsl:value-of select="'SS'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </BUY_SELL>

          <SYMBOL>
            <xsl:choose>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="Symbol"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="UnderlyingSymbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </SYMBOL>

          <QUANTITY>
            <xsl:value-of select="AllocatedQty"/>
          </QUANTITY>

          <PRICE>
            <xsl:value-of select="AveragePrice"/>
          </PRICE>

          <exec_brkr>
            <xsl:value-of select="CounterParty"/>
          </exec_brkr>

          <COMMCODE>
            <xsl:value-of select="''"/>
          </COMMCODE>

          <COMMValue>
            <xsl:value-of select="CommissionCharged"/>
          </COMMValue>

          
          <RR>
            <xsl:value-of select="''"/>
          </RR>

          <BLOTTER>
            <!--<xsl:choose>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="'18'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>-->
            <xsl:value-of select="''"/>
          </BLOTTER>

          <CONTRA_BRKER>
            <!--<xsl:choose>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="'100'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'577'"/>
              </xsl:otherwise>
            </xsl:choose>-->
            <xsl:value-of select="''"/>
          </CONTRA_BRKER>

          <STRIKE_PX>
            <xsl:choose>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="StrikePrice"/>
              </xsl:otherwise>
            </xsl:choose>
          </STRIKE_PX>

          <PUT_CALL>
            <xsl:choose>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="substring(PutOrCall,1,1)"/>
              </xsl:otherwise>
            </xsl:choose>
          </PUT_CALL>

          <EXPIRATION_DATE>
            <xsl:choose>
              <xsl:when test="Asset = 'Equity'">
                <xsl:value-of select="''"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="ExpirationDate"/>
              </xsl:otherwise>
            </xsl:choose>
          </EXPIRATION_DATE>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
