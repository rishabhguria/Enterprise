<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),'%',''),$SingleQuote,''))"/>
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
        <xsl:when test="$Month='01' ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='06' ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='01' ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='02' ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='03' ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='04'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='05'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='07'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='08'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='09'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='10'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='11'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='12'">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>


  <xsl:template name="MonthCodevar">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month='Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$Month='Feb'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$Month='Mar'">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$Month='Apr'">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test="$Month='Jun'">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$Month='Jul' ">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$Month='Aug'">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$Month='Sep'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$Month='Oct'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$Month='Nov'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$Month='Dec'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before(normalize-space(COL4),' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(substring-after(normalize-space(COL4),' '),5,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(substring-after(normalize-space(COL4),' '),3,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(substring-after(normalize-space(COL4),' '),1,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring(substring-after(normalize-space(COL4),' '),7,1)"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(substring-after(normalize-space(COL4),' '),8) div 1000,'##.00')"/>
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

    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL6)"/>
          </xsl:call-template>
        </xsl:variable>

        <!-- <xsl:if test="number($Position) and number(COL11)"> -->
		<xsl:if test="number($Position) and  COL4 !='NOGXX' and (COL11 !='*' or COL11 !='')"> 


          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'NTSI'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="COL11='8'">
                  <xsl:value-of select="'EquityOption'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
			
			<xsl:variable name="varCusip">
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable>
			<xsl:variable name="varSedol">
              <xsl:value-of select="normalize-space(COL15)"/>
            </xsl:variable>
			<xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>
			
            
			
			<xsl:variable name="varCUSIP">
              <xsl:choose>
                <xsl:when test="normalize-space(COL14)='USD' and ($varCusip ='' or $varCusip = '*')">
                  <xsl:value-of select="COL4"/>
                </xsl:when>
				<xsl:when test="normalize-space(COL14)='USD' and ($varCusip !='' or $varCusip != '*')">
                  <xsl:value-of select="COL3"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>             
            </xsl:variable>
            
            
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL4"/>
                  </xsl:call-template>
                </xsl:when>
				
				<xsl:when test="normalize-space(COL14)='USD' and ($varSymbol ='' or $varSymbol = '*')">
                  <xsl:value-of select="''"/>
                </xsl:when>
				
				<xsl:when test="normalize-space(COL14) !='USD' and ($varSedol !='' or $varSedol != '*')">
                  <xsl:value-of select="''"/>
                </xsl:when>
				
                <xsl:when test="$varSymbol!='*' or $varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
			
			<CUSIP>
              <xsl:choose>
             
				<xsl:when test="normalize-space(COL14)='USD' and ($varSymbol ='' or $varSymbol = '*')">
                  <xsl:value-of select="$varCusip"/>
                </xsl:when>
				
				<xsl:when test="normalize-space(COL14) !='USD' and ($varSedol !='' or $varSedol != '*')">
                  <xsl:value-of select="''"/>
                </xsl:when>
				
                <xsl:when test="$varSymbol!='*' or $varSymbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </CUSIP>
			
			<SEDOL>
              <xsl:choose>
             
				<xsl:when test="normalize-space(COL14) !='USD' and ($varSedol !='' or $varSedol != '*')">
                  <xsl:value-of select="$varSedol"/>
                </xsl:when>
				
				<xsl:when test="normalize-space(COL14)='USD' and ($varSymbol ='' or $varSymbol = '*')">
                  <xsl:value-of select="''"/>
                </xsl:when>
				
                <xsl:when test="$varSymbol!='*' or $varSymbol!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SEDOL>

            <xsl:variable name="PB_FUND_NAME" select="concat(COL2,COL12)"/>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <xsl:variable name="PB_CURRENCY_NAME">
              <xsl:value-of select="COL14"/>
            </xsl:variable>

            <xsl:variable name="PRANA_CURRENCY_ID">
              <xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
            </xsl:variable>

            <CurrencyID>              
                  <xsl:choose>
                    <xsl:when test="number($PRANA_CURRENCY_ID)">
                      <xsl:value-of select="$PRANA_CURRENCY_ID"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>               
            </CurrencyID>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$Position &gt; 0">
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

            <xsl:variable name="varMarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varMarkPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL5"/>
              </xsl:call-template>
            </xsl:variable>


            <xsl:variable name="varCostBasis">
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$Position !='0'">
                      <xsl:value-of select ="($varMarketValue div $Position)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="0"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varCostBasis &gt; 0">
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

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:choose>
                    <xsl:when test="$Position &gt; 0">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>

                    <xsl:when test="$Position &lt; 0">
                      <xsl:value-of select="'C'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="'0'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>

                  <xsl:choose>
                    <xsl:when test="$Position &gt; 0">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>

                    <xsl:when test="$Position &gt; 0">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="'0'"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>


            </SideTagValue>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <PositionStartDate>
              <xsl:value-of select="''"/>
            </PositionStartDate>


            <PositionSettlementDate>
              <xsl:value-of select="''"/>
            </PositionSettlementDate>



          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>
  
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>