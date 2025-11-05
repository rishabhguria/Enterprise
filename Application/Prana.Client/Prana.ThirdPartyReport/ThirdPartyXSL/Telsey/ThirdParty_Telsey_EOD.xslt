<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>



      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>


          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="varUpperCase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
          <xsl:variable name="varLowerCase">abcdefghijklmnopqrstuvwxyz</xsl:variable>

          <xsl:variable name="PB_NAME" select="''"/>


          <AccountNumber>
            <xsl:value-of select="AccountNo"/>
          </AccountNumber>

          <Ticker>
            <xsl:value-of select="Symbol"/>
          </Ticker>

          <Quantity>
            <xsl:value-of select="AllocatedQty"/>
          </Quantity>

          <EntryPrice>
            <xsl:value-of select="AveragePrice"/>
          </EntryPrice>

          <OrderSide>
            <xsl:value-of select="Side"/>
          </OrderSide>

          <Currency>
            <xsl:value-of select="CurrencySymbol"/>
          </Currency>

          <PositionsType>
            <xsl:choose>
              <xsl:when test="Asset='Equity'">
                <xsl:value-of select="'CS'"/>
              </xsl:when>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="'OPT'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>         
          </PositionsType>

          <Expiration>
            <xsl:value-of select="ExpirationDate"/>
          </Expiration>

          <PUTCALL>
            <xsl:choose>
              <xsl:when test="PutOrCall='Put'">
                <xsl:value-of select="'P'"/>
              </xsl:when>
              <xsl:when test="PutOrCall='Call'">
                <xsl:value-of select="'C'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </PUTCALL>

          <Strike>
            <xsl:value-of select="StrikePrice"/>
          </Strike>

          <SEDOL>
            <xsl:value-of select="SEDOL"/>
          </SEDOL>

          <CUSIP>
            <xsl:value-of select="CUSIP"/>
          </CUSIP>

          <UnderlyingSecNo>
            <xsl:value-of select="UnderlyingSymbol"/>
          </UnderlyingSecNo>
          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>