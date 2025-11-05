<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
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

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="substring(COL1,65,19)"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position) and contains(normalize-space(substring(COL1,28,36)),'BANK DEPOSIT PROGRAM')!='true'">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Lyrical'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="substring(COL1,28,36)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--<xsl:variable name="AssetType">
              <xsl:choose>
                <xsl:when test="substring(COL1,228,1)='O'">
                  <xsl:value-of select="'Options'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Equity'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>-->

            <xsl:variable name="Symbol" select="substring(COL1,189,21)"/>

            <Symbol>
              <xsl:choose>

                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>

                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="normalize-space($Symbol)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>

            </Symbol>

            <!--<IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$AssetType='Options'">
                  <xsl:value-of select="concat($Symbol,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>-->

            <xsl:variable name="PB_FUND_NAME" select="substring(COL1,7,9)"/>

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

            <xsl:variable name="Side" select="substring(COL1,325,2)"/>


            <SideTagValue>
              <xsl:choose>

                <!--<xsl:when test="$Side='sl'">
									<xsl:value-of select="'2'"/>
								</xsl:when>-->

                <xsl:when test="$Side='by'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:when test="$Side='ss'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </SideTagValue>

            <xsl:variable name="MarketValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="substring(COL1,104,20)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="Costbasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="substring(COL1,210,15)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="Cost">
              <xsl:value-of select="$Costbasis div $Position"/>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>

                <xsl:when test="$Cost &gt; 0">
                  <xsl:value-of select="$Cost"/>
                </xsl:when>

                <xsl:when test="$Cost &lt; 0">
                  <xsl:value-of select="$Cost * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>


            <PBSymbol>
              <xsl:value-of select ="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <xsl:variable name="Year" select="substring(COL1,378,4)"/>
            <xsl:variable name="Month" select="substring(COL1,383,2)"/>
            <xsl:variable name="Day" select="substring(COL1,386,2)"/>

            <xsl:variable name="Date" select="substring(COL1,378,10)"/>

            <PositionStartDate>

              <xsl:choose>
                <xsl:when test="contains(substring(COL1,378,10),'-')">
                  <xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$Date"/>
                </xsl:otherwise>
              </xsl:choose>


            </PositionStartDate>


          </PositionMaster>

        </xsl:if>

      </xsl:for-each>

    </DocumentElement>

  </xsl:template>

</xsl:stylesheet>