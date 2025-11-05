<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>     

      <xsl:for-each select="ThirdPartyFlatFileDetail[Asset='EquityOption']">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <xsl:variable name="PB_NAME" select="'Wedbush'"/>
          <Account >
            <xsl:value-of select="AccountNo"/>
          </Account >

          <BS>
            <xsl:choose>
              <xsl:when test="Side='Buy to Open'">
                <xsl:value-of select="'B'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Open'">
                <xsl:value-of select="'S'"/>
              </xsl:when>
             
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'BC'"/>
              </xsl:when>
              <xsl:when test="Side='Sell to Close'">
                <xsl:value-of select="'SC'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </BS>

          <Quantity>
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="AllocatedQty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Quantity>

          <Symbol>
            <xsl:value-of select="UnderlyingSymbol"/>
          </Symbol>



          <Price>
            <xsl:choose>
              <xsl:when test="number(AveragePrice)">
                <xsl:value-of select="format-number(AveragePrice,'0.####')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Price>


          <Type>
            <xsl:choose>
              <xsl:when test="Side='Buy to Open' or Side='Sell to Close'">
                <xsl:value-of select="'2'"/>
              </xsl:when>

              <xsl:when test="Side='Buy to Close' or Side='Sell to Open'">
                <xsl:value-of select="'6'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </Type>


          <SN>
            <xsl:value-of select="'N'"/>
          </SN>

          <OpenClose>
            <xsl:choose>
              <xsl:when test="Side='Buy to Open' or Side='Sell to Open'">
                <xsl:value-of select="'O'"/>
              </xsl:when>

              <xsl:when test="Side='Buy to Close' or Side='Sell short' or  Side='Sell to Close'">
                <xsl:value-of select="'C'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </OpenClose>

          <EXCH>
            <xsl:value-of select="'OTHO'"/>
          </EXCH>

          <xsl:variable name="varCommissionFlat">
            <xsl:value-of select="CommissionCharged + SoftCommissionCharged"/>
          </xsl:variable>

          <COMMAMT>
            <xsl:value-of select="format-number($varCommissionFlat,'0.####')"/>
          </COMMAMT>

          <COMMTPYE>
            <xsl:choose>
              <xsl:when test="$varCommissionFlat='0'">
                <xsl:value-of select="'P'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'F'"/>
              </xsl:otherwise>
            </xsl:choose>

          </COMMTPYE>


          <Maturity>
            <xsl:value-of select="ExpirationDate"/>
          </Maturity>

          <CallPut>
          <xsl:value-of select="substring(PutOrCall,1,1)"/>
          </CallPut>

          <Strike>
            <xsl:value-of select="StrikePrice"/>
          </Strike>
     
          <End>
            <xsl:value-of select="'END'"/>
          </End>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>