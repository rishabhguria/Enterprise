<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

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
    <xsl:if test="$varPutCall='CALL'">
      <xsl:choose>
        <xsl:when test="contains($Month,'JAN')">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'FEB')">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'MAR')">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'APR')">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'MAY')">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'JUN')">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'JUL')">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'AUG')">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'SEP')">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'OCT')">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'NOV')">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'DEC')">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$varPutCall='PUT'">
      <xsl:choose>
        <xsl:when test="contains($Month,'JAN')">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'FEB')">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'MAR')">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'APR')">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'MAY')">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'JUN')">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'JUL')">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'AUG')">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'SEP')">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'OCT')">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'NOV')">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="contains($Month,'DEC')">
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
      <xsl:value-of select="normalize-space(translate(translate($Symbol,$upper_CONST,''),'&amp;',''))"/>
    </xsl:variable>

    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before(substring-after($Symbol,'('),')')"/>
    </xsl:variable>
    
    <xsl:variable name="ExpiryDay">
      <xsl:value-of select="substring-before($var,' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring-before(substring-after($var,' '),' ')"/>
    </xsl:variable>
    <xsl:variable name="PutORCall">
      <xsl:value-of select="substring-before($Symbol,' ')"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="number-format(substring-before(substring-after(substring-after($var,' '),' '),' '),'#.00')"/>
    </xsl:variable>
    <xsl:variable name="MonthCode">
      <xsl:call-template name="MonthCodevar">
        <xsl:with-param name="Month" select="$Symbol"/>
        <xsl:with-param name="varPutCall" select="substring-before($Symbol,' ')"/>
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
    <xsl:param name="Date"/>
    
    <xsl:variable name="Month">
      <xsl:choose>
        <xsl:when test="string-length(substring-before(substring-after($Date,'-'),'-'))=1">
          <xsl:value-of select="concat('0',substring-before(substring-after($Date,'-'),'-'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring-before(substring-after($Date,'-'),'-')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="Year">
      <xsl:choose>
        <xsl:when test="string-length(substring-before($Date,'-'))=1">
          <xsl:value-of select="concat('0',substring-before($Date,'-'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring-before($Date,'-')"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="Day">
      <xsl:value-of select="substring-after(substring-after($Date,'-'),'-')"/>
    </xsl:variable>

    <xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL12)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Quantity)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Fidelity'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL11)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name = "varCusip" >
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                
                <xsl:when test="contains(COL11,'PUT') or contains(COL11,'CALL')">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="$PB_SYMBOL_NAME"/>
                  </xsl:call-template>
                </xsl:when>
                
                <xsl:when test="not(contains(COL11,'PUT')) and not(contains(COL11,'CALL'))">
                  <xsl:value-of select="normalize-space(COL10)"/>
                </xsl:when>
                
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL5)"/>
            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

            <xsl:variable name="varQuantity">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL12)"/>
              </xsl:call-template>
            </xsl:variable>
            <Quantity>
              <xsl:choose>
                <xsl:when test="$varQuantity &gt; 0">
                  <xsl:value-of select="$varQuantity"/>

                </xsl:when>
                <xsl:when test="$varQuantity &lt; 0">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </Quantity>

            <Side>
              <xsl:choose>
                <xsl:when test="contains(COL11,'PUT') or contains(COL11,'CALL')">
                  <xsl:choose>
                    <xsl:when test="$Quantity &gt; 0">
                      <xsl:value-of select="'Buy to Open'"/>
                    </xsl:when>
                    <xsl:when test="$Quantity &lt; 0">
                      <xsl:value-of select="'Sell to Open'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$Quantity &gt; 0">
                      <xsl:value-of select="'Buy'"/>
                    </xsl:when>
                    <xsl:when test="$Quantity &lt; 0">
                      <xsl:value-of select="'Sell short'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </Side>

            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL16)"/>
              </xsl:call-template>
            </xsl:variable>

            <MarketValue>
              <xsl:choose>
                <xsl:when test="number($MarketValue)">
                  <xsl:value-of select="$MarketValue"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>


            <xsl:variable name="varNetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL14)"/>
              </xsl:call-template>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$varQuantity &gt; 0">
                  <xsl:value-of select="$varNetNotionalValue * -1"/>

                </xsl:when>
				
                <xsl:when test="$varQuantity &lt; 0">
                  <xsl:value-of select="$varNetNotionalValue * -1"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </NetNotionalValue>
			
			      <xsl:variable name="varAvgPX">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL13)"/>
              </xsl:call-template>
            </xsl:variable>
            <AvgPX>
              <xsl:choose>
                <xsl:when test="$varAvgPX &gt; 0">
                  <xsl:value-of select="$varAvgPX"/>

                </xsl:when>
                <xsl:when test="$varAvgPX &lt; 0">
                  <xsl:value-of select="$varAvgPX"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </AvgPX>

            <OriginalPurchaseDate>
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="Date" select="substring-before(COL20,' ')"/>
              </xsl:call-template>
            </OriginalPurchaseDate>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ.()$#-'"/>

</xsl:stylesheet>