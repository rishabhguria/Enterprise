<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <ThirdPartyFlatFileDetail>
        
        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select ="TaxLotState"/>
        </TaxLotState>

        <ISIN>
          <xsl:value-of select ="'ISIN'"/>
        </ISIN>

        <CODE>
          <xsl:value-of select ="'CODE'"/>
        </CODE>

        <UNITS>
          <xsl:value-of select ="'UNITS'"/>
        </UNITS>
        
        <PRSHR>
          <xsl:value-of select ="'PRSHR'"/>
        </PRSHR>

        <BROKER>
          <xsl:value-of select ="'BROKER'"/>
        </BROKER>

        <TRADDT>
          <xsl:value-of select ="'TRADDT'"/>
        </TRADDT>

        <CONTDT>
          <xsl:value-of select ="'CONTDT'"/>
        </CONTDT>

        <COMMS>
          <xsl:value-of select ="'COMMS'"/>
        </COMMS>

        <SECFEES>
          <xsl:value-of select ="'SEC FEES'"/>
        </SECFEES>

        <NET>
          <xsl:value-of select ="'NET'"/>
        </NET>

        <Currency>
          <xsl:value-of select ="'Currency'"/>
        </Currency>

        <TICKER>
          <xsl:value-of select ="'TICKER'"/>
        </TICKER>

        <SECURITYDESCRIPTION>
          <xsl:value-of select ="'SECURITY DESCRIPTION'"/>
        </SECURITYDESCRIPTION>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>
          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select ="TaxLotState"/>
          </TaxLotState>

          <ISIN>
            <xsl:value-of select ="ISIN"/>
          </ISIN>

          <xsl:variable name="Side1">
            <xsl:choose>
              <xsl:when test="Side='Buy to Open' or Side='Buy' ">
                <xsl:value-of select ="'BUY'"/>
              </xsl:when>
              <xsl:when test="Side='Sell' or Side='Sell to Close' ">
                <xsl:value-of select ="'SELL'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
                <xsl:value-of select ="'SELL SHORT'"/>
              </xsl:when>
             
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <CODE>
            <xsl:value-of select ="$Side1"/>
          </CODE>

          <UNITS>
            <xsl:value-of select ="AllocatedQty"/>
          </UNITS>
          
		  <xsl:variable name="varPrice">
		     <xsl:value-of select ="NetAmount div AllocatedQty"/>
		  </xsl:variable>
          <PRSHR>
            <xsl:value-of select="format-number($varPrice, '0.############')"/>
          </PRSHR>

          <BROKER>
            <xsl:value-of select ="'JPM'"/>
          </BROKER>
          
          <TRADDT>
            <xsl:value-of select ="TradeDate"/>
          </TRADDT>

          <CONTDT>
            <xsl:value-of select ="SettlementDate"/>
          </CONTDT>
          
          <COMMS>
            <xsl:value-of select ="'0'"/>
          </COMMS>

          <SECFEES>
            <xsl:value-of select ="'0'"/>
          </SECFEES>
          
          <NET>
            <xsl:value-of select ="format-number(NetAmount,'0.##')"/>
          </NET>

          <Currency>
            <xsl:value-of select ="CurrencySymbol"/>
          </Currency>

          <TICKER>
            <xsl:value-of select ="Symbol"/>
          </TICKER>

          <SECURITYDESCRIPTION>
            <xsl:value-of select ="FullSecurityName"/>
          </SECURITYDESCRIPTION>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>


    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>