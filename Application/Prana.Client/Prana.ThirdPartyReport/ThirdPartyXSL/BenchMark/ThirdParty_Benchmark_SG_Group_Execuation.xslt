<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="GetMonth">
    <xsl:param name="Month"/>

    <xsl:choose>
      <xsl:when test="$Month='Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$Month='Feb'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$Month='March'">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$Month='April'">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test=" $Month='Jun'">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$Month='Jul'">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$Month='Aug'">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$Month='Sep'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$Month='Oct'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$Month='Nov'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$Month='Dec'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="/NewDataSet">

    <ThirdPartyFlatFileDetailCollection>

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'False'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

        <TranType>
          <xsl:value-of select="'TranType'"/>
        </TranType>

        <Quantity>
          <xsl:value-of select="'Last Shares'"/>
        </Quantity>

        <BBGTicker>
          <xsl:value-of select="'BBG Ticker'"/>
        </BBGTicker>

        <ProductCode>
          <xsl:value-of select="'Product Code'"/>
        </ProductCode>

        <SecurityType>
          <xsl:value-of select="'Security Type'"/>
        </SecurityType>

        <MaturityMonth>
          <xsl:value-of select="'Maturity Month'"/>
        </MaturityMonth>

        <MaturityYear>
          <xsl:value-of select="'Maturity Year'"/>
        </MaturityYear>

        <Exchange>
          <xsl:value-of select="'Exchange'"/>
        </Exchange>

        <SecurityDescription>
          <xsl:value-of select="'SecurityDescription'"/>
        </SecurityDescription>

        <PricePremium>
          <xsl:value-of select="'PricePremium'"/>
        </PricePremium>

        <PutorCall>
          <xsl:value-of select="'PutorCall'"/>
        </PutorCall>

        <StrikePrice>
          <xsl:value-of select="'StrikePrice'"/>
        </StrikePrice>

        <ExecutingBroker>
          <xsl:value-of select="'ExecutingBroker'"/>
        </ExecutingBroker>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <HoldingAccount>
          <xsl:value-of select="'HoldingAccount'"/>
        </HoldingAccount>

        <FCMAccount>
          <xsl:value-of select="'FCMAccount'"/>
        </FCMAccount>

        <FCM>
          <xsl:value-of select="'FCM'"/>
        </FCM>

        <AllocType>
          <xsl:value-of select="'AllocType'"/>
        </AllocType>

        <TradeID>
          <xsl:value-of select="'TradeID'"/>
        </TradeID>

        <APSID>
          <xsl:value-of select="'APSID'"/>
        </APSID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">


        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'False'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

          <TranType>
            <xsl:value-of select="Side"/>
          </TranType>

          <Quantity>
            <xsl:choose>
              <xsl:when test="OrderStatus!='*'">
                <xsl:value-of select="LastShares"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CumQty"/>
              </xsl:otherwise>
            </xsl:choose>
          </Quantity>

          <BBGTicker>
            <xsl:value-of select ="Symbol"/>
          </BBGTicker>

          <ProductCode>
            <xsl:value-of select ="substring(Symbol,1,2)"/>
          </ProductCode>

          <SecurityType>
            <xsl:value-of select="'F'"/>
          </SecurityType>

          <MaturityMonth>
            <xsl:call-template name="GetMonth">
              <xsl:with-param name="Month" select="substring-before(ExpirationDate,' ')"/>
              <xsl:with-param name="Suffix" select="''"/>
            </xsl:call-template>
          </MaturityMonth>

          <MaturityYear>
            <xsl:value-of select ="substring-before(substring-after(substring-after(ExpirationDate,' '),' '),' ')"/>
          </MaturityYear>

          <Exchange>
            <xsl:value-of select="Exchange"/>
          </Exchange>

          <SecurityDescription>
            <xsl:value-of select="CompanyName"/>
          </SecurityDescription>

          <PricePremium>
            <xsl:value-of select ="format-number(AvgPrice,'#.#####')"/>
          </PricePremium>

          <PutorCall>
            <xsl:value-of select="''"/>
          </PutorCall>

          <StrikePrice>
            <xsl:value-of select="StrikePrice"/>
          </StrikePrice>

          <ExecutingBroker>
            <xsl:value-of select="FullName"/>
          </ExecutingBroker>

          <xsl:variable name="Date">
            <xsl:value-of select="substring-before(TransactionTime,'-')"/>
          </xsl:variable>

          <xsl:variable name="Month">
            <xsl:value-of select="substring($Date,5,2)"/>
          </xsl:variable>

          <xsl:variable name="Day">
            <xsl:value-of select="substring($Date,7,2)"/>
          </xsl:variable>

          <xsl:variable name="Year">
            <xsl:value-of select="substring($Date,1,4)"/>
          </xsl:variable>

          <TradeDate>
            <xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
          </TradeDate>

          <HoldingAccount>
            <xsl:choose>
              <xsl:when test="OrderStatus!='*'">
                <xsl:value-of select ="'43119401'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="FundName"/>
              </xsl:otherwise>
            </xsl:choose>
          </HoldingAccount>

          <xsl:variable name="varAccount">
            <xsl:value-of select ="concat(ABC,PBUniqueID)"/>
          </xsl:variable>
          <FCMAccount>
            <xsl:value-of select ="''"/>
          </FCMAccount>

          <FCM>
            <xsl:value-of select="'GU FCM'"/>
          </FCM>

          <AllocType>
            <xsl:choose>
              <xsl:when test="OrderStatus!='*'">
                <xsl:value-of select="'FILL'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'AVG'"/>
              </xsl:otherwise>
            </xsl:choose>
          </AllocType>

          <TradeID>
            <xsl:value-of select="''"/>
          </TradeID>

          <APSID>
            <xsl:value-of select="APSID"/>
          </APSID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

</xsl:stylesheet>