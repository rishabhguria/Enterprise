<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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


  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">
        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL26"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:if test ="number($Position)">

          <PositionMaster>

            <xsl:variable name = "varCouponFrequencyID" >
              <xsl:call-template name="tempCouponFrequencyID">
                <xsl:with-param name="paramCouponFrequencyID" select="COL9" />
              </xsl:call-template>
            </xsl:variable>

            <!--<AUECID>
              <xsl:value-of select="158"/>
            </AUECID>-->

            <CouponFrequencyID>
              <xsl:value-of select="$varCouponFrequencyID"/>
            </CouponFrequencyID>

            <CurrencyID>
              <xsl:choose>

                <xsl:when test="COL25='USD'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:when test="COL25='HKD'">
                  <xsl:value-of select="2"/>
                </xsl:when>

                <xsl:when test="COL25='GBP'">
                  <xsl:value-of select="4"/>
                </xsl:when>

                <xsl:when test="COL25='EUR'">
                  <xsl:value-of select="8"/>
                </xsl:when>

                <xsl:when test="COL25='JPY'">
                  <xsl:value-of select="3"/>
                </xsl:when>

                <xsl:when test="COL25='AED'">
                  <xsl:value-of select="5"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </CurrencyID>



            <xsl:variable name="varSymbol">
              <xsl:choose>
                <xsl:when test="COL25='USD'">
                  <xsl:value-of select="COL24"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="COL15"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <TickerSymbol>
              <xsl:value-of select="normalize-space($varSymbol)"/>
            </TickerSymbol>

            <LongName>
              <xsl:value-of select="normalize-space(COL2)"/>
            </LongName>

            <UnderLyingSymbol>
              <xsl:value-of select="normalize-space($varSymbol)"/>
            </UnderLyingSymbol>

            <xsl:variable name="varBloomberg" select="COL19"/>

            <BloombergSymbol>
              <xsl:value-of select="normalize-space($varBloomberg)"/>
            </BloombergSymbol>
            
           <SedolSymbol>
		   
		  <xsl:choose>
                <xsl:when test="COL15='#N/A Field Not Applicable' or COL15='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL15"/>
                </xsl:otherwise>
              </xsl:choose>
            </SedolSymbol>

            <xsl:variable name="varFCIMonth">
              <xsl:value-of select="substring(COL40,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varFCYear">
              <xsl:value-of select="substring(COL40,1,4)"/>
            </xsl:variable>
            <xsl:variable name="varFCDay">
              <xsl:value-of select="substring(COL40,7,2)"/>
            </xsl:variable>
            <FirstCouponDate>
              <xsl:value-of select ="COL4"/>
            </FirstCouponDate>


            <xsl:variable name="varIMonth">
              <xsl:value-of select="substring(COL18,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varIYear">
              <xsl:value-of select="substring(COL18,1,4)"/>
            </xsl:variable>
            <xsl:variable name="varIDay">
              <xsl:value-of select="substring(COL18,7,2)"/>
            </xsl:variable>
            <IssueDate>
              <xsl:value-of select ="COL3"/>
            </IssueDate>


            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL19,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL19,1,4)"/>
            </xsl:variable>
            <xsl:variable name="varDay">
              <xsl:value-of select="substring(COL19,7,2)"/>
            </xsl:variable>
            <ExpirationDate>
              <xsl:choose>
                <xsl:when test="COL19='' or COL19='*'">
                  <xsl:value-of select="'01-01-2050'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL6"/>
                </xsl:otherwise>
              </xsl:choose>
            </ExpirationDate>

            <MaturityDate>
              <xsl:choose>
                <xsl:when test="COL19='' or COL19='*'">
                  <xsl:value-of select="'01-01-2050'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL6"/>
                </xsl:otherwise>
              </xsl:choose>
            </MaturityDate>



            <xsl:variable name="varCouponLen" select="string-length(substring-before(COL9,'%'))-5"/>
            <xsl:variable name="varCoupon" select="COL8"/>
            <Coupon>
              <xsl:choose>
                <xsl:when test="$varCoupon=''">
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$varCoupon"/>
                </xsl:otherwise>
              </xsl:choose>
            </Coupon>

            <IsZero>
              <xsl:choose>
                <xsl:when test="number($varCoupon)=0">
                  <xsl:value-of select="'TRUE'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'FALSE'"/>
                </xsl:otherwise>
              </xsl:choose>
            </IsZero>


            <AccrualBasisID>
              <xsl:choose>
                <xsl:when test="COL10='ACT/ACT'">
                  <xsl:value-of select ="'4'"/>
                </xsl:when>
                <xsl:when test="contains(COL10,'ACT/360')">
                  <xsl:value-of select ="'1'"/>
                </xsl:when>
                <xsl:when test="contains(COL10,'ACT/365')">
                  <xsl:value-of select ="'0'"/>
                </xsl:when>
                <xsl:when test="contains(COL10,'30/360')">
                  <xsl:value-of select ="'2'"/>
                </xsl:when>               
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccrualBasisID>

            <xsl:variable name="varDate1">
              <xsl:value-of select="substring(COL42,7,2)"/>
            </xsl:variable>
            <xsl:variable name="varDate2">
              <xsl:value-of select="substring(COL43,7,2)"/>
            </xsl:variable>
            <xsl:variable name="DaysToSettlement">
              <xsl:value-of select="($varDate1)-($varDate2)"/>
            </xsl:variable>


            <DaysToSettlement>
              <xsl:value-of select ="COL22"/>
            </DaysToSettlement>

            <Multiplier>
              <xsl:value-of select="0.01"/>
            </Multiplier>

          

            <AssetID>
              <xsl:value-of select="'8'"/>
            </AssetID>
            
            <UnderLyingID>
              <xsl:value-of select="'10'"/>
            </UnderLyingID>

            <ExchangeID>
              <xsl:value-of select="'77'"/>
            </ExchangeID>

            <xsl:variable name="BondType" select ="COL18"/>


            <!--<SecurityTypeID>
              <xsl:choose>
                <xsl:when test="$BondType ='Govt'">
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:when test="$BondType ='Muni'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:when test="$BondType ='Corp'">
                  <xsl:value-of select="3"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecurityTypeID>-->

         <BondTypeID>
              <xsl:choose>
                <xsl:when test="$BondType ='Govt'">
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:when test="$BondType ='Muni'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:when test="$BondType ='Corp'">
                  <xsl:value-of select="3"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </BondTypeID>

          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>

