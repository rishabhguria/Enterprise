<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>


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
  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:if test="COL39='OPTIONS'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(normalize-space(COL5),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-before(substring-after(normalize-space(COL5),' '),' '),3,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-before(substring-after(normalize-space(COL5),' '),' '),1,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-before(substring-after(normalize-space(COL5),' '),' '),7,2)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring-after(substring-after(substring-after(normalize-space(COL5),' '),' '),' '),'#.00')"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL5),' '),' '),' ')"/>
      </xsl:variable>

      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:variable name="Day">
        <xsl:choose>
          <xsl:when test="substring($ExpiryDay,1,1)='0'">
            <xsl:value-of select="substring($ExpiryDay,2,1)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ExpiryDay"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL7"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Position) and COL28!='N/A'">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Cowen'"/>
            </xsl:variable>
            <xsl:variable name = "PB_COMPANY" >
              <xsl:value-of select="normalize-space(COL20)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="varUnderlying">
              <xsl:value-of select="normalize-space(COL5)"/>
            </xsl:variable>

            <xsl:variable name="CUSIP">
              <xsl:value-of select="COL21"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>

                <xsl:when test="COL39='OPTIONS'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL5"/>
                  </xsl:call-template>
                </xsl:when>

                <xsl:when test ="$CUSIP != ''">
                  <xsl:value-of select ="''"/>
                </xsl:when>

                <xsl:when test ="$varUnderlying != ''">
                  <xsl:value-of select ="$varUnderlying"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>

            <CUSIP>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="''"/>
                </xsl:when>

                <xsl:when test ="$CUSIP != ''">
                  <xsl:value-of select ="$CUSIP"/>
                </xsl:when>

                <xsl:when test="COL12='Equity Options'">
                  <xsl:value-of select ="''"/>
                </xsl:when>
                
                <xsl:when test ="$varUnderlying != ''">
                  <xsl:value-of select ="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="''"/>
                </xsl:otherwise>
              </xsl:choose>

            </CUSIP>

            <xsl:variable name="PB_CountnerParty" select="COL26"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>
            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="contains(COL26,'BROKER ACTED AS AGENT')">
                  <xsl:value-of select="'7'"/>
                </xsl:when>
                <xsl:when test="contains(COL26,'PIPER')">
                  <xsl:value-of select="'14'"/>
                </xsl:when>
				<xsl:when test="contains(COL26,'GOLDMAN')">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL26,'PIPJ')">
                  <xsl:value-of select="'14'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL26,'WEEDEN PRIME')">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL26,'NEIL &amp; CO')">
                  <xsl:value-of select="'69'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL26,'ONEL')">
                  <xsl:value-of select="'69'"/>
                </xsl:when>
				
				<xsl:when test="contains(COL26,'CANTOR')">
                  <xsl:value-of select="'16'"/>
                </xsl:when>
				
				
                <xsl:when test="$PRANA_CounterPartyID !=''">
                  <xsl:value-of select ="$PRANA_CounterPartyID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>


            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY"/>
            </PBSymbol>


            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL8"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when  test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <PositionStartDate>
              <xsl:value-of select="COL28"/>
            </PositionStartDate>

            <PositionSettlementDate>
              <xsl:value-of select="COL27"/>
            </PositionSettlementDate>




            <NetPosition>
              <xsl:choose>
                <xsl:when  test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL6"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="COL39='OPTIONS'">
                  <xsl:choose>
                    <xsl:when  test="$varSide ='BUY TO OPEN'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>

                    <xsl:when  test="$varSide='SELL TO CLOSE'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
					
					<xsl:when  test="$varSide ='BUY TO CLOSE'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
					
					<xsl:when  test="$varSide ='SELL TO OPEN'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>
					
                    <xsl:otherwise>

                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when  test="$varSide ='BUY'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>

                    <xsl:when  test="$varSide='SELL'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
					
					<xsl:when  test="$varSide='SELL SHORT'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>            
              
            
            </SideTagValue>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="(COL10)"/>
              </xsl:call-template>
            </xsl:variable>

           


            <xsl:variable name="varCOMM">
              <xsl:choose>
                <xsl:when test="COL6='BUY'">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="COL6='SELL'">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when  test="$varCOMM &gt; 0">
                  <xsl:value-of select="$varCOMM"/>
                </xsl:when>
                <xsl:when test="$varCOMM &lt; 0">
                  <xsl:value-of select="$varCOMM * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>
            <xsl:variable name="AccruedInterest">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL16"/>
              </xsl:call-template>
            </xsl:variable>

						<AccruedInterest>
							<xsl:choose>
								<xsl:when test="$AccruedInterest &gt; 0">
									<xsl:value-of select="$AccruedInterest"/>
								</xsl:when>
								<xsl:when test="$AccruedInterest &lt; 0">
									<xsl:value-of select="$AccruedInterest*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccruedInterest>
            <xsl:variable name="varFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
              </xsl:call-template>
            </xsl:variable>
            <Fees>
              <xsl:choose>
                <xsl:when test ="$varFees &gt; 0">
                  <xsl:value-of select ="$varFees"/>
                </xsl:when>
                <xsl:when test ="$varFees &lt; 0">
                  <xsl:value-of select ="$varFees * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>

            <xsl:variable name="varFXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL25 div COL12"/>
              </xsl:call-template>
            </xsl:variable>
            <FXRate>
              <xsl:choose>
                <xsl:when test ="number($varFXRate)">
                  <xsl:value-of select ="$varFXRate"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>


          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


