<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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

  <xsl:template name="MonthCodevar">
    <xsl:param name="Month"/>
    <xsl:param name="varPutCall"/>
    <xsl:if test="$varPutCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
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
        <xsl:when test="$Month='06'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='07' ">
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
    <xsl:if test="$varPutCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='01'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='02'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='03'">
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

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:variable name="var">
      <xsl:value-of select="substring-after($Symbol,' ')"/>
    </xsl:variable>

    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before($Symbol,' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),5,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),3,2)"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),1,2)"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="translate($var, '0123456789.', '')"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(translate($var,translate($var, '0123456789.', ''), ''),7,8) div 1000,'##.00')"/>
    </xsl:variable>
    <xsl:variable name="MonthCode">
      <xsl:call-template name="MonthCodevar">
        <xsl:with-param name="Month" select="$ExpiryMonth"/>
        <xsl:with-param name="varPutCall" select="$PutORCall"/>
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

    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCode,$StrikePrice,'D',$Day)"/>
  </xsl:template>

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- converts date time double number to 18/12/2009 -->

    <xsl:variable name="varDay">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:choose>
            <xsl:when test="string-length(substring-before(substring-after(normalize-space($DateTime),'/'),'/'))=1">
              <xsl:value-of select="concat(0,substring-before(substring-after($DateTime,'/'),'/'))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring-before(substring-after($DateTime,'/'),'/')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(substring-after(normalize-space($DateTime),'-'),'-'))=1">
                  <xsl:value-of select="concat(0,substring-before(substring-after($DateTime,'-'),'-'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before(substring-after($DateTime,'-'),'-')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varMonth">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:choose>
            <xsl:when test="string-length(substring-before(normalize-space($DateTime),'/'))=1">
              <xsl:value-of select="concat(0,substring-before($DateTime,'/'))"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring-before($DateTime,'/')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains($DateTime,'-')">
              <xsl:choose>
                <xsl:when test="string-length(substring-before(normalize-space($DateTime),'-'))=1">
                  <xsl:value-of select="concat(0,substring-before($DateTime,'-'))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring-before($DateTime,'-')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="varYear">
      <xsl:choose>
        <xsl:when test="contains($DateTime,'/')">
          <xsl:value-of select="substring-after(substring-after($DateTime,'/'),'/')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="contains(COL3,'-')">
              <xsl:value-of select="substring-after(substring-after($DateTime,'-'),'-')"/>
            </xsl:when>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:value-of select="$varMonth"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$varDay"/>
    <xsl:value-of select="'/'"/>
    <xsl:value-of select="$varYear"/>

  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="varQty">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL5)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varQty)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
             <xsl:value-of select="'M&amp;T Bank'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="varSymbol">
              <xsl:value-of select="COL10"/>
            </xsl:variable>



            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varSymbol!='' or $varSymbol!='*'">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

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

            <Quantity>
              <xsl:choose>
                <xsl:when test="$varQty &gt; 0">
                  <xsl:value-of select="$varQty"/>
                </xsl:when>

                <xsl:when test="$varQty &lt; 0">
                  <xsl:value-of select="$varQty * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL22)"/>
              </xsl:call-template>
            </xsl:variable>
           <AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPrice &gt; 0">
									<xsl:value-of select="$AvgPrice"/>

								</xsl:when>
								<xsl:when test="$AvgPrice &lt; 0">
									<xsl:value-of select="$AvgPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL2)"/>
            </xsl:variable>
            <Side>
              <xsl:choose>
                
                    <xsl:when test="$varSide='PURCHASED'">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>

                    <xsl:when test="$varSide='SOLD'">
                      <xsl:value-of select="'Sell'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                 
              </xsl:choose>
            </Side>

            <xsl:variable name="varTradeDate">                
                <xsl:value-of select="COL3"/>          
            </xsl:variable>
			
			 <xsl:variable name="varMM">                
                <xsl:value-of select="substring($varTradeDate,5,2)"/>          
            </xsl:variable>
			 <xsl:variable name="varDD">                
                   <xsl:value-of select="substring($varTradeDate,7,2)"/>      
            </xsl:variable>
			 <xsl:variable name="varYYYY">                
                <xsl:value-of select="substring($varTradeDate,1,4)"/>          
            </xsl:variable>
			 <xsl:variable name="varDate">                
                <xsl:value-of select="concat($varMM,'/', $varDD,'/', $varYYYY)"/>          
            </xsl:variable>
            <TradeDate>
              <xsl:value-of select="$varDate"/>
            </TradeDate>
		
            <xsl:variable name="varSettlementDate">
              <xsl:value-of select="COL4"/>
            </xsl:variable>
			
			
			 <xsl:variable name="varSMM">                
                <xsl:value-of select="substring($varSettlementDate,5,2)"/>          
            </xsl:variable>
			 <xsl:variable name="varSDD">                
                   <xsl:value-of select="substring($varSettlementDate,7,2)"/>      
            </xsl:variable>
			 <xsl:variable name="varSYYYY">                
                <xsl:value-of select="substring($varSettlementDate,1,4)"/>          
            </xsl:variable>
			 <xsl:variable name="varSDate">                
                <xsl:value-of select="concat($varSMM,'/', $varSDD,'/', $varSYYYY)"/>          
            </xsl:variable>
            <SettlementDate>
              <xsl:value-of select="$varSDate"/>
            </SettlementDate>
			
				<xsl:variable name="varNetAmount">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$varNetAmount &gt; 0">
                  <xsl:value-of select="$varNetAmount"/>
                </xsl:when>
                <xsl:when test="$varNetAmount &lt; 0">
                  <xsl:value-of select="$varNetAmount * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>