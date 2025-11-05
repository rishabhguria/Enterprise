<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <!--Third Friday check-->
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


  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='JAN'">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='FEB'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='MAR'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='APR'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='MAY'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='JUN'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='JUL'">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='AUG'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='SEP'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='OCT'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='NOV'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='DEC'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='JAN'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='FEB'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='MAR'">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='APR'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='MAY'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='JUN'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='JUL'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='AUG'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='SEP'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='OCT'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='NOV'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='DEC'">
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
    <xsl:param name="Suffix"/>
    <xsl:if test="substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)='C' or substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)='P'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(normalize-space(COL3),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL3),' '),' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(normalize-space(COL3),' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),3,2)"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),'##.00')"/>
      </xsl:variable>
      <xsl:variable name="MonthCodVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="$ExpiryMonth"/>
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
      <xsl:variable name="ThirdFriday">
        <xsl:choose>
          <xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
            <xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
          </xsl:when>
        </xsl:choose>
      </xsl:variable>
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
    </xsl:if>
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
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL7"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'GS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_COMPANY" >
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL1"/>
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
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name = "varAsset" >
              <xsl:choose>
                <xsl:when test="substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)='C' or substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)='P'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

           

           <xsl:variable name="Symbol">
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable> 
			

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="normalize-space(COL30)"/>
            </xsl:variable>
			
			  <xsl:variable name="varCUSIP">
              <xsl:value-of select="normalize-space(COL29)"/>
            </xsl:variable>
			
            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>

                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="normalize-space(COL3)"/>
                    <xsl:with-param name="Suffix" select="''"/>
                  </xsl:call-template>
                </xsl:when>
				

				<xsl:when test="$varCUSIP !='' and contains(COL3,'%')">
                  <xsl:value-of select="''"/>
                </xsl:when>
				<xsl:when test="$Symbol!=''">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>
				
                <xsl:when test="$varSEDOL !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
				
                
                <xsl:otherwise>
                  <xsl:value-of select="COL3"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


         <CUSIP>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select="''"/>
                </xsl:when>
				<xsl:when test="$varCUSIP !='' or contains(COL3,'%')">
                  <xsl:value-of select="$varCUSIP"/>
                </xsl:when>
				<xsl:when test="$varSEDOL !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
				 <xsl:when test="$varAsset='EquityOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>

				 <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
				
				<xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
			</xsl:choose>
                
            </CUSIP>

            <SEDOL>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select="''"/>
                </xsl:when>
			<xsl:when test="$varCUSIP !='' or contains(COL3,'%')">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varSEDOL !=''">
                  <xsl:value-of select="$varSEDOL"/>
                </xsl:when>

                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

          
            <xsl:variable name="PB_CountnerParty" select="COL33"/>
            <xsl:variable name="PRANA_CounterPartyID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
            </xsl:variable>
            <CounterPartyID>
              <xsl:choose>
          	
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
                <xsl:with-param name="Number" select="COL9"/>
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

           

            <xsl:variable name="SecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test="$SecFee &gt; 0">
                  <xsl:value-of select="$SecFee"/>
                </xsl:when>
                <xsl:when test="$SecFee &lt; 0">
                  <xsl:value-of select="$SecFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>
			
			 <xsl:variable name="ORFFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>
            <OrfFee>
              <xsl:choose>
                <xsl:when test="$ORFFee &gt; 0">
                  <xsl:value-of select="$ORFFee"/>
                </xsl:when>
                <xsl:when test="$ORFFee &lt; 0">
                  <xsl:value-of select="$ORFFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrfFee>
			
			<xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL40"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>
                </xsl:when>
                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>
			
			<xsl:variable name="ClearingFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL38"/>
              </xsl:call-template>
            </xsl:variable>
            <ClearingFee>
              <xsl:choose>
                <xsl:when test="$ClearingFee &gt; 0">
                  <xsl:value-of select="$ClearingFee"/>
                </xsl:when>
                <xsl:when test="$ClearingFee &lt; 0">
                  <xsl:value-of select="$ClearingFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </ClearingFee>
			
			<xsl:variable name="Fees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL39"/>
              </xsl:call-template>
            </xsl:variable>
            <Fees>
              <xsl:choose>
                <xsl:when test="$Fees &gt; 0">
                  <xsl:value-of select="$Fees"/>
                </xsl:when>
                <xsl:when test="$Fees &lt; 0">
                  <xsl:value-of select="$Fees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>
			
			
			
			<xsl:variable name="AccruedInterest">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
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



            <xsl:variable name="varDateTime">
              <xsl:value-of select="COL4"/>
            </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="$varDateTime"/>
            </PositionStartDate>
      
            <PositionSettlementDate>
              <xsl:value-of select="''"/>
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
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:value-of select="concat(normalize-space(COL2),' ',normalize-space(COL28))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(COL2)"/>
                </xsl:otherwise>
              </xsl:choose>             
            </xsl:variable>
            <SideTagValue>

              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                    <xsl:when  test="$varSide='Buy TO OPEN'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>

                    <xsl:when  test="$varSide='Sell TO OPEN'">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>

                    <xsl:when  test="$varSide='Sell TO CLOSE'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide='Buy TO CLOSE'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when  test="$varSide ='Buy'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide ='Sell'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide ='Short Sell'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide ='Cover Buy'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
             
            </SideTagValue>


            <OriginalPurchaseDate>
              <xsl:value-of select="''"/>
            </OriginalPurchaseDate>
			
			  <CommissionSource>
              <xsl:value-of select="0"/>
            </CommissionSource>

            <SoftCommissionSource>
              <xsl:value-of select="0"/>
            </SoftCommissionSource>

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


