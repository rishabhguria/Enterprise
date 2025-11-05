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

  <xsl:template match="/">
    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:if test ="number(COL8)">

          <PositionMaster>

            <xsl:variable name = "varCouponFrequencyID" >
              <xsl:call-template name="tempCouponFrequencyID">
                <xsl:with-param name="paramCouponFrequencyID" select="COL7" />
              </xsl:call-template>
            </xsl:variable>

            <AUECID>
              <xsl:value-of select="158"/>
            </AUECID>

            <CouponFrequencyID>
              <xsl:value-of select="$varCouponFrequencyID"/>
            </CouponFrequencyID>

            <CurrencyID>
              <xsl:value-of select ="1"/>
            </CurrencyID>

            <xsl:variable name="varSymbol" select="COL1"/>
            
            <TickerSymbol>
                <xsl:value-of select="normalize-space($varSymbol)"/>
            </TickerSymbol>
			
			<LongName>
			    <xsl:value-of select="normalize-space(COL3)"/>
			</LongName>

            <UnderLyingSymbol>
              <xsl:value-of select="normalize-space($varSymbol)"/>
            </UnderLyingSymbol>

            <xsl:variable name="varBloomberg" select="COL21"/>
            
           <BloombergSymbol>
							<xsl:value-of select="normalize-space($varBloomberg)"/>
						</BloombergSymbol>

            <FirstCouponDate>

              <xsl:choose>
                <xsl:when test="contains(COL18,'N/A')">
                  <xsl:value-of select="'1/1/1800'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL18"/>
                </xsl:otherwise>
              </xsl:choose>
            </FirstCouponDate>

            <IssueDate>

              <xsl:choose>
                <xsl:when test="contains(COL5,'N/A')">
                  <xsl:value-of select="'1/1/1800'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL5"/>
                </xsl:otherwise>
              </xsl:choose>
            </IssueDate>

            <ExpirationDate>
              <xsl:choose>
                <xsl:when test="contains(COL6,'N/A')">
                  <xsl:value-of select="'1/1/1800'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="COL6"/>
                </xsl:otherwise>
              </xsl:choose>
            </ExpirationDate>

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
        
                <Coupon>
                  <xsl:choose>
                    <xsl:when test="number(COL9)">
                  <xsl:value-of select="COL9"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Coupon>
            
            <IsZero>
			<xsl:choose>
                    <xsl:when test="number(COL9)=0">
                  <xsl:value-of select="'TRUE'"/>
                    </xsl:when>
                    <xsl:otherwise>
                     <xsl:value-of select="'FALSE'"/>
                    </xsl:otherwise>
                  </xsl:choose>
            </IsZero>

            <AccrualBasisID>
							<xsl:choose>
								<xsl:when test="COL10='US MUNI: 30/360'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test="COL10='ACT/360'">
									<xsl:value-of select ="'3'"/>
								</xsl:when>
								<xsl:when test="COL10='ACT/ACT'">
									<xsl:value-of select ="'4'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccrualBasisID>

           <DaysToSettlement>
							<xsl:value-of select ="format-number(COL8,'#')"/>
						</DaysToSettlement>

            <Multiplier>
              <xsl:value-of select="0.01"/>
            </Multiplier>

            <AssetCategory>
              <xsl:value-of select="'FixedIncome'"/>
            </AssetCategory>

            <UnderLyingID>
              <xsl:value-of select="'10'"/>
            </UnderLyingID>

            <ExchangeID>
              <xsl:value-of select="'77'"/>
            </ExchangeID>

            <xsl:variable name="BondType" select ="COL22"/>
            
            
            <SecurityTypeID>
              <xsl:choose>
                <xsl:when test="$BondType ='Treasury'">
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:when test="$BondType ='Municipal'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:when test="$BondType ='Agency'">
                  <xsl:value-of select="2"/>
                </xsl:when>
                <xsl:when test="$BondType ='Corporate'">
                  <xsl:value-of select="3"/>
                </xsl:when>
                <xsl:when test="$BondType ='Sovereign'">
                  <xsl:value-of select="4"/>
                </xsl:when>
                <xsl:when test="$BondType ='CommercialPaper'">
                  <xsl:value-of select="5"/>
                </xsl:when>
                <xsl:when test="$BondType ='Credit'">
                  <xsl:value-of select="6"/>
                </xsl:when>
                <xsl:when test="$BondType ='BankDebt'">
                  <xsl:value-of select="7"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecurityTypeID>
			
			<!-- <BondTypeID> -->
              <!-- <xsl:choose> -->
                <!-- <xsl:when test="$BondType ='Treasury'"> -->
                  <!-- <xsl:value-of select="0"/> -->
                <!-- </xsl:when> -->
                <!-- <xsl:when test="$BondType ='Municipal'"> -->
                  <!-- <xsl:value-of select="1"/> -->
                <!-- </xsl:when> -->
                <!-- <xsl:when test="$BondType ='Agency'"> -->
                  <!-- <xsl:value-of select="2"/> -->
                <!-- </xsl:when> -->
                <!-- <xsl:when test="$BondType ='Corporate'"> -->
                  <!-- <xsl:value-of select="3"/> -->
                <!-- </xsl:when> -->
                <!-- <xsl:when test="$BondType ='Sovereign'"> -->
                  <!-- <xsl:value-of select="4"/> -->
                <!-- </xsl:when> -->
                <!-- <xsl:when test="$BondType ='CommercialPaper'"> -->
                  <!-- <xsl:value-of select="5"/> -->
                <!-- </xsl:when> -->
                <!-- <xsl:when test="$BondType ='Credit'"> -->
                  <!-- <xsl:value-of select="6"/> -->
                <!-- </xsl:when> -->
                <!-- <xsl:when test="$BondType ='BankDebt'"> -->
                  <!-- <xsl:value-of select="7"/> -->
                <!-- </xsl:when> -->
                <!-- <xsl:otherwise> -->
                  <!-- <xsl:value-of select="0"/> -->
                <!-- </xsl:otherwise> -->
              <!-- </xsl:choose> -->
            <!-- </BondTypeID> -->
          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>

