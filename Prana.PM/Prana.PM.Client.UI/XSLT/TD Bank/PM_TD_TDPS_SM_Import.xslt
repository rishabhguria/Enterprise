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
            <xsl:with-param name="Number" select="COL43"/>
          </xsl:call-template>
        </xsl:variable>


        <xsl:if test ="number($Position) and (COL39 !='' or COL39 !='*')">

          <PositionMaster>

            <xsl:variable name = "varCouponFrequencyID" >
              <xsl:call-template name="tempCouponFrequencyID">
                <xsl:with-param name="paramCouponFrequencyID" select="COL30" />
              </xsl:call-template>
            </xsl:variable>

            <AUECID>
              <xsl:value-of select="158"/>
            </AUECID>

            <CouponFrequencyID>
              <xsl:value-of select="$varCouponFrequencyID"/>
            </CouponFrequencyID>

            <CurrencyID>
              <xsl:choose>

                <xsl:when test="COL8='USD'">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <xsl:when test="COL8='HKD'">
                  <xsl:value-of select="2"/>
                </xsl:when>

                <xsl:when test="COL8='GBP'">
                  <xsl:value-of select="4"/>
                </xsl:when>

                <xsl:when test="COL8='EUR'">
                  <xsl:value-of select="8"/>
                </xsl:when>

                <xsl:when test="COL8='JPY'">
                  <xsl:value-of select="3"/>
                </xsl:when>

                <xsl:when test="COL8='AED'">
                  <xsl:value-of select="5"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            
            </CurrencyID>

          

            <xsl:variable name="varSymbol">
              <xsl:choose>
                <xsl:when test="COL8='USD'">
                  <xsl:value-of select="COL5"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="COL7"/>
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

            <xsl:variable name="varBloomberg" select="COL44"/>
            
           <BloombergSymbol>
							<xsl:value-of select="normalize-space($varBloomberg)"/>
						</BloombergSymbol>


            <xsl:variable name="varFCIMonth">
              <xsl:value-of select="substring(COL19,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varFCYear">
              <xsl:value-of select="substring(COL19,1,4)"/>
            </xsl:variable>
            <xsl:variable name="varFCDay">
              <xsl:value-of select="substring(COL19,7,2)"/>
            </xsl:variable>
            <FirstCouponDate>
              <xsl:value-of select ="concat($varFCIMonth,'/',$varFCDay,'/',$varFCYear)"/>
            </FirstCouponDate>


            <xsl:variable name="varIMonth">
              <xsl:value-of select="substring(COL19,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varIYear">
              <xsl:value-of select="substring(COL19,1,4)"/>
            </xsl:variable>
            <xsl:variable name="varIDay">
              <xsl:value-of select="substring(COL19,7,2)"/>
            </xsl:variable>
            <IssueDate>
              <xsl:value-of select ="concat($varIMonth,'/',$varIDay,'/',$varIYear)"/>
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
                  <xsl:value-of select ="concat($varMonth,'/',$varDay,'/',$varYear)"/>
                </xsl:otherwise>
              </xsl:choose>
            </ExpirationDate>

            <MaturityDate>
              <xsl:choose>
                <xsl:when test="COL19='' or COL19='*'">
                  <xsl:value-of select="'01-01-2050'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="concat($varMonth,'/',$varDay,'/',$varYear)"/>
                </xsl:otherwise>
              </xsl:choose>
            </MaturityDate>
        
                <Coupon>
                  <xsl:choose>
                    <xsl:when test="number(COL7)">
                  <xsl:value-of select="COL7"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </Coupon>
            
            <IsZero>
              <xsl:value-of select="false()"/>
            </IsZero>

            <AccrualBasisID>
							<xsl:choose>
								<xsl:when test="contains(COL39,'Euro')">
									<xsl:value-of select ="'3'"/>
								</xsl:when>
								<xsl:when test="not(contains(COL39,'Euro'))">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test="substring-before(COL39,' ')='Actual/Actual'">
									<xsl:value-of select ="'4'"/>
								</xsl:when>
                <xsl:when test="contains(COL39,'Actual/360')">
                  <xsl:value-of select ="'1'"/>
                </xsl:when>
                <xsl:when test="contains(COL39,'Actual/365')">
                  <xsl:value-of select ="'0'"/>
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
							<xsl:value-of select ="$DaysToSettlement"/>
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

            <xsl:variable name="BondType" select ="COL41"/>
            
            <!--<BondTypeID>
              <xsl:choose>
                <xsl:when test="$BondType ='SOVR' and contains(COL2,'NITED STATES TREASURY')">
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:when test="$BondType ='SOVR' and not(contains(COL2,'NITED STATES TREASURY'))">
                  <xsl:value-of select="1"/>
                </xsl:when>
                --><!--<xsl:when test="$BondType ='Agency'">
                  <xsl:value-of select="2"/>
                </xsl:when>
                <xsl:when test="$BondType ='CORP'">
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
                </xsl:when>--><!--
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </BondTypeID>-->
            <SecurityTypeID>
              <xsl:choose>
                <xsl:when test="$BondType ='SOVR' and contains(COL2,'NITED STATES TREASURY')">
                  <xsl:value-of select="0"/>
                </xsl:when>
                <xsl:when test="$BondType ='SOVR' and not(contains(COL2,'NITED STATES TREASURY'))">
                  <xsl:value-of select="1"/>
                </xsl:when>
                <!--<xsl:when test="$BondType ='Agency'">
                  <xsl:value-of select="2"/>
                </xsl:when>
                <xsl:when test="$BondType ='CORP'">
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
                </xsl:when>-->
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecurityTypeID>
            <!--<BondType>
              <xsl:value-of select="'Municipal'"/>
            </BondType>-->
          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>

