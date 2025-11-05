<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/NewDataSet">

    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <xsl:for-each select="ThirdPartyFlatFileDetail[Account='Resonate Core']">
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <Account>
            <xsl:value-of select="Account"/>
          </Account>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <BloombergSymbol>
            <xsl:value-of select="BloombergSymbol"/>
          </BloombergSymbol>

          <SecurityName>
            <xsl:value-of select="Description"/>
          </SecurityName>
          
          <TradeCurrency>
            <xsl:value-of select="TradeCurrency"/>
          </TradeCurrency>

          <OpenTradeDate>
            <xsl:value-of select="OpenTradeDate"/>
          </OpenTradeDate>

          <OpenOriginalPurchaseDate>
            <xsl:value-of select="OriginalPurchaseDate"/>
          </OpenOriginalPurchaseDate>

          <CloseTradeDate>
            <xsl:value-of select="ClosingTradeDate"/>
          </CloseTradeDate>
          
          <PositionType>
            <xsl:choose>
              <xsl:when test="PositionType=1">
                <xsl:value-of select="'Long'"/>
              </xsl:when>
              <xsl:when test="PositionType=-1">
                <xsl:value-of select="'Short'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </PositionType>
          
          <CloseQuantity>
            <xsl:value-of select="ClosedQty"/>
          </CloseQuantity>

          <OpenPriceLocal>
            <xsl:value-of select="format-number(OpenPriceLocal,'##.####')"/>
          </OpenPriceLocal>

          <ClosePriceLocal>
            <xsl:value-of select="format-number(ClosePriceLocal,'##.####')"/>
          </ClosePriceLocal>
		      <xsl:variable name="varProcedsL">
            <xsl:value-of select="format-number(Proceeds_Local,'##.####')"/>
          </xsl:variable>
		  
          <xsl:variable name="varTotalCost_Local">
            <xsl:value-of select="format-number(TotalCost_Local,'##.####')"/>
          </xsl:variable>
		  
          <TotalCostLocal>
            <xsl:value-of select="$varTotalCost_Local"/>
          </TotalCostLocal>

		      <xsl:variable name="varTotalCost_Base">
            <xsl:value-of select="format-number(TotalCost_Base,'##.####')"/>
          </xsl:variable>
		  
          <TotalCostBase>
            <xsl:value-of select="$varTotalCost_Base"/>
          </TotalCostBase>
		  
			  
          <ProceedsLocal>
            <xsl:value-of select="$varProcedsL"/>
          </ProceedsLocal>
		  
		      <xsl:variable name="varProcedsBase">
            <xsl:value-of select="format-number(Proceeds_Base,'##.####')"/>
          </xsl:variable>

          <ProceedsBase>
            <xsl:value-of select="$varProcedsBase"/>
          </ProceedsBase>
		  
		  
          <TotalRealizedPNLLocal>
            <xsl:value-of select="format-number(($varProcedsL - $varTotalCost_Local),'##.####')"/>
          </TotalRealizedPNLLocal>

          <TotalRealizedPNLBase>
            <xsl:value-of select="format-number(($varProcedsBase - $varTotalCost_Base),'##.####')"/>
          </TotalRealizedPNLBase>

          <OpenStrategy>
            <xsl:value-of select="OpenStrategy"/>
          </OpenStrategy>

          <CloseStrategy>
            <xsl:value-of select="CloseStrategy"/>
          </CloseStrategy>
          
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
        <!--</if>-->
      </xsl:for-each>

    </ThirdPartyFlatFileDetailCollection>

  </xsl:template>

</xsl:stylesheet>