<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>

    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber * (-1)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="MonthsCode">
    <xsl:param name="varMonth"/>
    <xsl:choose>
      <xsl:when test="$varMonth='Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Feb'">
        <xsl:value-of select="'02'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Mar'">
        <xsl:value-of select="'03'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Apr'">
        <xsl:value-of select="'04'"/>
      </xsl:when>
      <xsl:when test="$varMonth='May'">
        <xsl:value-of select="'05'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Jun'">
        <xsl:value-of select="'06'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Jul'">
        <xsl:value-of select="'07'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Aug'">
        <xsl:value-of select="'08'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Sep'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Oct'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Nov'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$varMonth='Dec'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  
  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="(COL8)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="normalize-space(COL4)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="Asset">
              <xsl:value-of select="COMMON"/>
            </xsl:variable>

            <xsl:variable name = "varSymbol" >
              <xsl:value-of select ="normalize-space(COL26)"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>
            
            <xsl:variable name="PB_FUND_NAME" select="COL3"/>

            <xsl:variable name ="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <FundName>
              <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </FundName>

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

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </SideTagValue>


            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
              </xsl:call-template>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>

                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>
                </xsl:when>

                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>
            
            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

           
            <PBAssetType>
              <xsl:value-of select="COL7"/>
            </PBAssetType>
            
            <xsl:variable name ="varMonth">
              <xsl:value-of select="substring-before(substring-after(COL1,'-'),'-')"/>
            </xsl:variable>
            
            <xsl:variable name="MonthCodeVar">
              <xsl:call-template name="MonthsCode">
                <xsl:with-param name="varMonth" select="$varMonth"/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name ="varDay">
              <xsl:value-of select="substring-before(COL1,'-')"/>
            </xsl:variable>

            <xsl:variable name ="varYear">
              <xsl:value-of select="substring-after(substring-after(COL1,'-'),'-')"/>
            </xsl:variable>

            <PositionStartDate>
              <xsl:value-of select="concat($MonthCodeVar,'/',$varDay,'/',$varYear)"/>
            </PositionStartDate>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>

    </DocumentElement>

  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>