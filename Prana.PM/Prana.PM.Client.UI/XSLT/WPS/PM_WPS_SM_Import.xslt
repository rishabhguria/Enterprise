<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template name="tempCouponFrequencyID">
    <xsl:param name="paramCouponFrequencyID"/>
    <xsl:choose>
      <xsl:when test ="$paramCouponFrequencyID='Annually'">
        <xsl:value-of select ="'0'"/>
      </xsl:when>
      <xsl:when test ="$paramCouponFrequencyID='Quarterly'">
        <xsl:value-of select ="'1'"/>
      </xsl:when>
      <xsl:when test ="$paramCouponFrequencyID='SemiAnnually'">
        <xsl:value-of select ="'2'"/>
      </xsl:when>
      <xsl:when test ="$paramCouponFrequencyID='Monthly'">
        <xsl:value-of select ="'3'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="'4'"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:if test ="number(COL10)">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="normalize-space(COL1)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "varSymbol" >
              <xsl:value-of select ="normalize-space(COL4)"/>
            </xsl:variable>

            <xsl:variable name = "varBloomberg" >
              <xsl:value-of select ="normalize-space(COL2)"/>
            </xsl:variable>

            <xsl:variable name = "varSEDOL" >
              <xsl:value-of select ="normalize-space(COL5)"/>
            </xsl:variable>
			
			<xsl:variable name = "varCUSIP" >
              <xsl:value-of select ="normalize-space(COL4)"/>
            </xsl:variable>
            
            <TickerSymbol>
              <xsl:value-of select="$varSymbol"/>
            </TickerSymbol>

            <Multiplier>
              <xsl:value-of select="'0.01'"/>
            </Multiplier>

            <UnderLyingSymbol>
              <xsl:value-of select="$varSymbol"/>
            </UnderLyingSymbol>

            <SedolSymbol>
              <xsl:value-of select="$varSEDOL"/>
            </SedolSymbol>
			
			<CusipSymbol>
              <xsl:value-of select="$varCUSIP"/>
            </CusipSymbol>

            <BloombergSymbol>
              <xsl:value-of select="$varBloomberg"/>
            </BloombergSymbol>

            <MaturityDate>
            <xsl:choose>
              <xsl:when test="contains(COL6,'N/A')">
                <xsl:value-of select="'1/1/1800'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select ="COL6"/>
              </xsl:otherwise>
            </xsl:choose>
            </MaturityDate>

            <AssetCategory>
              <xsl:value-of select="'Fixed Income'"/>
            </AssetCategory>

            <xsl:variable name="PB_CURRENCY_NAME" select="COL3"/>

            <xsl:variable name="PRANA_CURRENCY_ID">
              <xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@PranaCurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
            </xsl:variable>
            <CurrencyID>
              <xsl:choose>
                <xsl:when test="$PRANA_CURRENCY_ID !=''">
                  <xsl:value-of select="$PRANA_CURRENCY_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CurrencyID>
            <xsl:variable name="varExpirationDate">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <ExpirationDate>
              <xsl:choose>
                <xsl:when test="contains($varExpirationDate,'N/A')">
                  <xsl:value-of select="'1/1/1800'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$varExpirationDate"/>
                </xsl:otherwise>
              </xsl:choose>
            </ExpirationDate>

            <xsl:variable name="varIssueDate">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <IssueDate>
              <xsl:choose>
                <xsl:when test="contains($varIssueDate,'N/A')">
                  <xsl:value-of select="'1/1/1800'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$varIssueDate"/>
                </xsl:otherwise>
              </xsl:choose>
            </IssueDate>

            <xsl:variable name="varCoupon">
              <xsl:value-of select="COL10"/>
            </xsl:variable>
            
            <Coupon>
              <xsl:choose>
                <xsl:when test="number($varCoupon)">
                  <xsl:value-of select="$varCoupon"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Coupon>

            <IsZero>
              <xsl:choose>
                <xsl:when test="contains(COL12,'N/A')">
                  <xsl:value-of select="'TRUE'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="'FALSE'"/>
                </xsl:otherwise>
              </xsl:choose>
            </IsZero>

            <AccrualBasisID>
              <xsl:choose>
                <xsl:when test="contains(COL8,'30/360')">
                  <xsl:value-of select ="'2'"/>
                </xsl:when>
                <xsl:when test="contains(COL8,'ACT/360')">
                  <xsl:value-of select ="'3'"/>
                </xsl:when>
                <xsl:when test="contains(COL8,'ACT/ACT')">
                  <xsl:value-of select ="'4'"/>
                </xsl:when>
                <xsl:when test="contains(COL8,'ACT/365')">
                  <xsl:value-of select ="'1'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccrualBasisID>

            <DaysToSettlement>
              <xsl:choose>
                <xsl:when test="number(COL14)">
                  <xsl:value-of select="COL14"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </DaysToSettlement>

            <xsl:variable name="BondType" select ="COL20"/>

          <SecurityTypeID> 
            <xsl:value-of select="'1'"/>
            </SecurityTypeID> 
			
			 <BondTypeID> 
			<xsl:value-of select="'1'"/> 
           </BondTypeID> 
            
            <xsl:variable name="varAUECID">
              <xsl:value-of select="'80'"/>
            </xsl:variable>

            <AUECID>
              <xsl:value-of select="$varAUECID"/>
            </AUECID>

            <FirstCouponDate>
              <xsl:choose>
                <xsl:when test="contains(COL12,'N/A')">
                  <xsl:value-of select="'1/1/1800'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL12"/>
                </xsl:otherwise>
              </xsl:choose>
            </FirstCouponDate>
			
            <xsl:variable name = "varCouponFrequencyID" >
              <xsl:call-template name="tempCouponFrequencyID">
                <xsl:with-param name="paramCouponFrequencyID" select="COL9" />
              </xsl:call-template>
            </xsl:variable>
			
			<CouponFrequencyID>
              <xsl:value-of select="$varCouponFrequencyID"/>
            </CouponFrequencyID>
			
            <LongName>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </LongName>


          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>

