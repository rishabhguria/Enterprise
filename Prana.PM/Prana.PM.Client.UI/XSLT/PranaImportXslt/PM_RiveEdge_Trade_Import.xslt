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

  <xsl:template name="FormatDate">
    <xsl:param name="varFullDate" />
    <xsl:variable name="varYear">
      <xsl:value-of select="substring($varFullDate, 1, string-length($varFullDate) - 4)"/>
    </xsl:variable>
    <xsl:variable name="varWithoutYear">
      
      <xsl:value-of select="substring($varFullDate, string-length($varFullDate) - 3 , 4)"/>
    </xsl:variable>
    <xsl:variable name="varDay">
      <xsl:choose>
        <xsl:when test="$varWithoutYear &lt; 100">
          <xsl:value-of select="concat('0',substring($varWithoutYear, string-length($varWithoutYear) - 0, 1))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($varWithoutYear, string-length($varWithoutYear) - 1, string-length($varWithoutYear))"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="varMonth">

       
       
          <xsl:value-of select="substring($varWithoutYear, 1, 2)"/>
      
    </xsl:variable>
    <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
  </xsl:template>
  <xsl:template name="MonthCode">
    <xsl:param name="Month" />
    <xsl:param name="PutOrCall" />
    <xsl:if test="$PutOrCall='Call'">
      <xsl:choose>
        <xsl:when test="$Month='Jan'">
          <xsl:value-of select="'A'" />
        </xsl:when>
        <xsl:when test="$Month='Feb'">
          <xsl:value-of select="'B'" />
        </xsl:when>
        <xsl:when test="$Month='Mar'">
          <xsl:value-of select="'C'" />
        </xsl:when>
        <xsl:when test="$Month='Apr'">
          <xsl:value-of select="'D'" />
        </xsl:when>
        <xsl:when test="$Month='May'">
          <xsl:value-of select="'E'" />
        </xsl:when>
        <xsl:when test="$Month='Jun'">
          <xsl:value-of select="'F'" />
        </xsl:when>
        <xsl:when test="$Month='Jul'">
          <xsl:value-of select="'G'" />
        </xsl:when>
        <xsl:when test="$Month='Aug'">
          <xsl:value-of select="'H'" />
        </xsl:when>
        <xsl:when test="$Month='Sep'">
          <xsl:value-of select="'I'" />
        </xsl:when>
        <xsl:when test="$Month='Oct'">
          <xsl:value-of select="'J'" />
        </xsl:when>
        <xsl:when test="$Month='Nov'">
          <xsl:value-of select="'K'" />
        </xsl:when>
        <xsl:when test="$Month='Dec'">
          <xsl:value-of select="'L'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='Put'">
      <xsl:choose>
        <xsl:when test="$Month='Jan'">
          <xsl:value-of select="'M'" />
        </xsl:when>
        <xsl:when test="$Month='Feb'">
          <xsl:value-of select="'N'" />
        </xsl:when>
        <xsl:when test="$Month='Mar'">
          <xsl:value-of select="'O'" />
        </xsl:when>
        <xsl:when test="$Month='Apr'">
          <xsl:value-of select="'P'" />
        </xsl:when>
        <xsl:when test="$Month='May'">
          <xsl:value-of select="'Q'" />
        </xsl:when>
        <xsl:when test="$Month='Jun'">
          <xsl:value-of select="'R'" />
        </xsl:when>
        <xsl:when test="$Month='Jul'">
          <xsl:value-of select="'S'" />
        </xsl:when>
        <xsl:when test="$Month='Aug'">
          <xsl:value-of select="'T'" />
        </xsl:when>
        <xsl:when test="$Month='Sep'">
          <xsl:value-of select="'U'" />
        </xsl:when>
        <xsl:when test="$Month='Oct'">
          <xsl:value-of select="'V'" />
        </xsl:when>
        <xsl:when test="$Month='Nov'">
          <xsl:value-of select="'W'" />
        </xsl:when>
        <xsl:when test="$Month='Dec'">
          <xsl:value-of select="'X'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>


  <xsl:template name="Option">
    <xsl:param name="varSymbol" />
    <xsl:variable name="var">
      <xsl:value-of select="substring-after($varSymbol,' ')" />
    </xsl:variable>
    <xsl:variable name="UnderlyingSymbol">
      <xsl:value-of select="substring-before(normalize-space($varSymbol),' ')" />
    </xsl:variable>
    <xsl:variable name="ExpiryDay">

      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),1,2)" />
    </xsl:variable>
    <!--<xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),3,2)" />
    </xsl:variable>-->
    <xsl:variable name="ExpiryMonth">
      <xsl:value-of select="substring-before(translate($var, '0123456789.', ''),' ')"/>
    </xsl:variable>
    <xsl:variable name="ExpiryYear">
      <xsl:value-of select="substring(normalize-space(translate($var,translate($var, '0123456789.', ''), '')),5,2)" />
    </xsl:variable>
    <xsl:variable name="PutORCall">

      <xsl:value-of select="substring-after(translate($var, '0123456789.', ''),'    ')"/>
    </xsl:variable>
    <xsl:variable name="StrikePrice">
      <xsl:value-of select="format-number(substring(translate($var,translate($var, '0123456789.', ''), ''),7,7),'##.00')" />
    </xsl:variable>
    <xsl:variable name="MonthCodVar">
      <xsl:call-template name="MonthCode">
        <xsl:with-param name="Month" select="$ExpiryMonth" />
        <xsl:with-param name="PutOrCall" select="$PutORCall" />
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Day">
      <xsl:choose>
        <xsl:when test="substring($ExpiryDay,1,1)='0'">
          <xsl:value-of select="substring($ExpiryDay,2,1)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$ExpiryDay" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ', $ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)" />
  </xsl:template>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="varPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL21)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varPosition)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
            <xsl:variable name="varSymbol">
              <xsl:value-of select="(normalize-space(COL5))" />
            </xsl:variable>

            <xsl:variable name="varSymbols">
              <xsl:value-of select="(normalize-space(COL9))" />
            </xsl:variable>
            <xsl:variable name="PB_FUND_NAME">
              <xsl:value-of select="COL4"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME" />
                </xsl:when>
               
                <xsl:when test="$varSymbols!='*'">
                  <xsl:value-of select="$varSymbols" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME" />
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

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
            <xsl:variable name="varSCurrency">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <SettlCurrencyName>
              <xsl:value-of select="$varSCurrency"/>
            </SettlCurrencyName>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$varPosition &gt; 0">
                  <xsl:value-of select="$varPosition"/>
                </xsl:when>
                <xsl:when test="$varPosition &lt; 0">
                  <xsl:value-of select="$varPosition * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL22"/>
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
            <xsl:variable name="varTaxFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL27"/>
              </xsl:call-template>
            </xsl:variable>
            <TaxOnCommissions>
              <xsl:choose>
                <xsl:when test="$varTaxFees &gt; 0">
                  <xsl:value-of select="$varTaxFees"/>

                </xsl:when>
                <xsl:when test="$varTaxFees &lt; 0">
                  <xsl:value-of select="$varTaxFees * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </TaxOnCommissions>
            <xsl:variable name="varSecFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL29"/>
              </xsl:call-template>
            </xsl:variable>
            <SecFee>
              <xsl:choose>
                <xsl:when test="$varSecFee &gt; 0">
                  <xsl:value-of select="$varSecFee"/>

                </xsl:when>
                <xsl:when test="$varSecFee &lt; 0">
                  <xsl:value-of select="$varSecFee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>
            <xsl:variable name="varLevy">
            <xsl:call-template name="Translate">
              <xsl:with-param name="Number" select="COL28"/>
            </xsl:call-template>
            </xsl:variable>
            <TransactionLevy>
              <xsl:choose>
                <xsl:when test="$varLevy &gt; 0">
                  <xsl:value-of select="$varLevy"/>

                </xsl:when>
                <xsl:when test="$varLevy &lt; 0">
                  <xsl:value-of select="$varLevy * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </TransactionLevy>

            <!--<xsl:variable name="varSide">
              <xsl:value-of select="COL16"/>
            </xsl:variable>-->
            <SideTagValue>
              <xsl:choose>
              

                    <xsl:when test="contains(COL16,'buy') and contains(COL17,'*')">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>

                <xsl:when test="contains(COL16,'sell') and contains(COL17,'*')">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="contains(COL16,'sell') and contains(COL17,'short')">
                  <xsl:value-of select="'5'"/>
                </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                 
              </xsl:choose>
            </SideTagValue>
           
            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="varSettlementDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="varFullDate" select="normalize-space(COL20)"/>
              </xsl:call-template>
            </xsl:variable>
            <PositionSettlementDate>
              <xsl:value-of select="$varSettlementDate"/>
            </PositionSettlementDate>

            <xsl:variable name="varPositionStartDate">
              <xsl:call-template name="FormatDate">
                <xsl:with-param name="varFullDate" select="normalize-space(COL19)"/>
              </xsl:call-template>
            </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>