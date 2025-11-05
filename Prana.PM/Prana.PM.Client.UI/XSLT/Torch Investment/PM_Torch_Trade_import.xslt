<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
  <xsl:output method="xml" indent="yes" />
  <xsl:template name="Translate">
    <xsl:param name="Number" />
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="MonthColumn">
    <xsl:param name="MonthSett"/>
    <xsl:choose>
      <xsl:when test="$MonthSett='Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='Feb'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='Mar' ">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='Apr' ">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='May' ">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='Jun' ">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='Jul'  ">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='Aug'  ">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='Sep' ">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='Oct' ">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='Nov' ">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='Dec' ">
        <xsl:value-of select="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
 
  <xsl:template match="/">


    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:variable name="varNetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL13)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($varNetPosition) and (COL10 = 'Asset Sold' or COL10 = 'Asset Purchased') and COL37 !=''">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'CT'"/>
            </xsl:variable>
            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL26"/>
            </xsl:variable>
            <xsl:variable name="varSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varISIN">
              <xsl:value-of select="COL24"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varISIN !='*' or $varISIN !=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
            <ISIN>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varISIN!='*' or $varISIN!=''">
                  <xsl:value-of select="$varISIN"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ISIN>

            <xsl:variable name="PB_FUND_NAME" select="COL3" />
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME" />
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>
            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL10)"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide = 'Asset Purchased'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'Asset Sold'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

          

            <xsl:variable name="varYYYY">
              <xsl:value-of select="substring(COL29,1,4)"/>
            </xsl:variable>
            <xsl:variable name="varMM">
              <xsl:value-of select="substring(COL29,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varDD">
              <xsl:value-of select="substring(COL29,7,2)"/>
            </xsl:variable>
            
            <PositionStartDate>
              <xsl:value-of select="concat($varMM,'/', $varDD ,'/', $varYYYY)"/>
            </PositionStartDate>
            
            <xsl:variable name="varSYYYY">
              <xsl:value-of select="substring(COL27,1,4)"/>
            </xsl:variable>
            <xsl:variable name="varSMM">
              <xsl:value-of select="substring(COL27,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varSDD">
              <xsl:value-of select="substring(COL27,7,2)"/>
            </xsl:variable>
            <PositionSettlementDate>
              <xsl:value-of select="concat($varSMM,'/', $varSDD ,'/', $varSYYYY)"/>
            </PositionSettlementDate>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varNetPosition &gt; 0">
                  <xsl:value-of select="$varNetPosition"/>
                </xsl:when>
                <xsl:when test="$varNetPosition &lt; 0">
                  <xsl:value-of select="$varNetPosition * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL37"/>
              </xsl:call-template>
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

            <xsl:variable name="varSCurrency">
              <xsl:value-of select="COL21"/>
            </xsl:variable>
            <SettlCurrencyName>
              <xsl:value-of select="$varSCurrency"/>
            </SettlCurrencyName>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL33"/>
              </xsl:call-template>
            </xsl:variable>
			
			  <xsl:variable name="varMultiply">
               <xsl:choose>
                <xsl:when test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varNetPosition * $varCostBasis"/>
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (-1) * ($varNetPosition * $varCostBasis)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
			
            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission -($varMultiply)"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="(($varCommission * (-1)) -($varMultiply))"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>


            <xsl:variable name="varFXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL19"/>
              </xsl:call-template>
            </xsl:variable>

            <FXRate>
              <xsl:choose>
                <xsl:when test="$varFXRate &gt; 0">
                  <xsl:value-of select="$varFXRate" />
                </xsl:when>
                <xsl:when test="$varFXRate &lt; 0">
                  <xsl:value-of select="$varFXRate * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>

    </DocumentElement>

  </xsl:template>
</xsl:stylesheet>