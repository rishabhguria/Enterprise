<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template name="tempCouponFrequencyID">
    <xsl:param name="paramCouponFrequencyID"/>
    <xsl:choose>
      <xsl:when test ="$paramCouponFrequencyID='12'">
        <xsl:value-of select ="'0'"/>
      </xsl:when>
      <xsl:when test ="$paramCouponFrequencyID='4'">
        <xsl:value-of select ="'1'"/>
      </xsl:when>
      <xsl:when test ="$paramCouponFrequencyID='2'">
        <xsl:value-of select ="'2'"/>
      </xsl:when>
      <xsl:when test ="$paramCouponFrequencyID='1'">
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

          <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="translate(COL74,'+','')"/>
          </xsl:call-template>
        </xsl:variable>	  

        <xsl:if test ="number($Position) and normalize-space(COL15) ='BOND'">

          <PositionMaster>

            <xsl:variable name = "varCouponFrequencyID" >
              <xsl:call-template name="tempCouponFrequencyID">
                <xsl:with-param name="paramCouponFrequencyID" select="''" />
              </xsl:call-template>
            </xsl:variable>

            <CouponFrequencyID>
              <xsl:value-of select="'2'"/>
            </CouponFrequencyID>

            <CurrencyID>
			        <xsl:choose>
                <xsl:when test="COL76='EUR'">
                  <xsl:value-of select="'8'"/>
                </xsl:when>
				      <xsl:when test="COL76='ARS'">
                  <xsl:value-of select="'44'"/>
                </xsl:when>
				      <xsl:when test="COL76='GBP'">
                  <xsl:value-of select="'4'"/>
                </xsl:when>
				      <xsl:when test="COL76='USD'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CurrencyID>

            <xsl:variable name="varSymbol" select="COL19"/>

            <TickerSymbol>
              <xsl:value-of select="normalize-space($varSymbol)"/>
            </TickerSymbol>
			
			      <CusipSymbol>
              <xsl:value-of select="normalize-space($varSymbol)"/>
            </CusipSymbol>


            <LongName>
              <xsl:value-of select="normalize-space(COL66)"/>
            </LongName>

            <UnderLyingSymbol>
              <xsl:value-of select="normalize-space(COL19)"/>
            </UnderLyingSymbol>

            

            <BloombergSymbol>
              <xsl:value-of select="''"/>
            </BloombergSymbol>

            <FirstCouponDate>
              <xsl:value-of select ="'04/07/2020'"/>
            </FirstCouponDate>

            <IssueDate>
              <xsl:value-of select ="'04/07/2020'"/>
            </IssueDate>


            <xsl:variable name="varYear">
              <xsl:value-of select="substring(normalize-space(COL69),1,4)"/>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(normalize-space(COL69),5,2)"/>
            </xsl:variable>
            <xsl:variable name="varDay">
              <xsl:value-of select="substring(normalize-space(COL69),7,2)"/>
            </xsl:variable>
            <xsl:variable name="varExdate">
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </xsl:variable>
            <ExpirationDate>
              <xsl:value-of select ="$varExdate"/>
            </ExpirationDate>

            <MaturityDate>
              <xsl:value-of select ="$varExdate"/>
            </MaturityDate>

            <Coupon>
              <xsl:choose>
			        <xsl:when test="translate(COL68,'+','')=''">
                  <xsl:value-of select="'0.00'"/>
                </xsl:when>  
			        <xsl:when test="translate(COL68,'+','')='*'">
                  <xsl:value-of select="'0.00'"/>
                </xsl:when>
                <xsl:when test="number(COL68)">
                  <xsl:value-of select="COL68"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate(COL68,'+','')"/>
                </xsl:otherwise>
              </xsl:choose>
            </Coupon>

            <IsZero>
              <xsl:choose>
                <xsl:when test="number(translate(COL68,'+',''))">
                  <xsl:value-of select="'FALSE'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'TRUE'"/>
                </xsl:otherwise>
              </xsl:choose>
            </IsZero>

            <AccrualBasisID>
              <xsl:value-of select="0"/>
            </AccrualBasisID>

            <DaysToSettlement>
              <xsl:value-of select ="'2'"/>
            </DaysToSettlement>

            <Multiplier>
              <xsl:value-of select="number(translate(COL70,'+',''))"/>
            </Multiplier>

            <AssetCategory>
              <xsl:value-of select="'FixedIncome'"/>
            </AssetCategory>

            <UnderLyingID>
              <xsl:value-of select="'10'"/>
            </UnderLyingID>
			
			      <AssetID>
              <xsl:value-of select="'8'"/>
            </AssetID>

            <ExchangeID>
              <xsl:value-of select="'77'"/>
            </ExchangeID>
			
			      <SecApprovalStatus>
              <xsl:value-of select="'Approved'"/>
            </SecApprovalStatus>

            <xsl:variable name="BondType" select ="COL22"/>

            <SecurityTypeID>
              <xsl:value-of select="0"/>
            </SecurityTypeID>

            <UDASector>
              <xsl:value-of select="'Undefined'"/>
            </UDASector>

            <UDASubSector>
              <xsl:value-of select="'Undefined'"/>
            </UDASubSector>

            <UDASecurityType>
              <xsl:value-of select="'Undefined'"/>
            </UDASecurityType>

            <UDAAssetClass>
              <xsl:value-of select="'Undefined'"/>
            </UDAAssetClass>

            <UDACountry>
              <xsl:value-of select="'Undefined'"/>
            </UDACountry>
          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>

