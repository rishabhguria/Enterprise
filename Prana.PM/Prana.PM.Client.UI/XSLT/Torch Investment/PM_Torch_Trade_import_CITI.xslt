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
      <xsl:when test="$MonthSett='JAN'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='FEB'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='MAR' ">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='APR' ">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='MAY' ">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='JUN' ">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='JUL'  ">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='AUG'  ">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='SEP' ">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='OCT' ">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='NOV' ">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$MonthSett='DEC' ">
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
            <xsl:with-param name="Number" select="normalize-space(COL8)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($varNetPosition) ">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(substring-before(COL4,'US'))"/>
            </xsl:variable>

          
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:when test="$varSymbol !='*' or $varSymbol !=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
        
            <xsl:variable name="PB_FUND_NAME" select="COL1" />
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
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable>
            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide = 'BUY'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'SELL'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

          
                 <xsl:variable name="varMM">
               <xsl:call-template name="MonthColumn">
                <xsl:with-param name="MonthSett" select="substring-before(substring-after(COL2,'-'),'-')"/>
              </xsl:call-template>
            </xsl:variable>
                        
                        <xsl:variable name="varDD">
               <xsl:value-of select="substring-before(normalize-space(COL2),'-')" />
            </xsl:variable>
                        <xsl:variable name="varYYYY">
               <xsl:value-of select="substring-after(substring-after(normalize-space(COL2),'-'),'-')" />
            </xsl:variable>
			   <xsl:variable name="varTradeDate">
               <xsl:value-of select="concat($varMM,'/',$varDD,'/', $varYYYY)" />
            </xsl:variable>
            
            <PositionStartDate>
              <xsl:value-of select="$varTradeDate"/>
            </PositionStartDate>
            
      
            <PositionSettlementDate>
              <xsl:value-of select="''"/>
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
                <xsl:with-param name="Number" select="COL9"/>
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


            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>
		
			
			
			<Commission>
			  <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission" />
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0" />
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

						
			<SettlCurrencyName>
			  <xsl:value-of select="COL7" />
            </SettlCurrencyName>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>

    </DocumentElement>

  </xsl:template>
</xsl:stylesheet>