<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

  <xsl:output method="xml" indent="yes"/>

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
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'$',''))"/>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">


        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="substring-after(COL7,' ')"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition)"/>

        <PositionMaster>


          <xsl:variable name="PB_NAME">
            <xsl:value-of select="''"/>
          </xsl:variable>

          <xsl:variable name = "PB_SYMBOL_NAME" >
            <xsl:value-of select ="COL6"/>
          </xsl:variable>

          <xsl:variable name="PRANA_SYMBOL_NAME">
            <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
          </xsl:variable>

          <xsl:variable name ="varISIN" >
            <xsl:value-of select ="substring-after(substring-after(substring-after(normalize-space(COL14),' '),'-'),' ')"/>
          </xsl:variable>


          <Symbol>
            <xsl:choose>
              <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                <xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
              </xsl:when>

              <xsl:when test ="$varISIN != ''">
                <xsl:value-of select ="''"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select ="$PB_SYMBOL_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </Symbol>

          <ISIN>
            <xsl:choose>
              <xsl:when test ="$PRANA_SYMBOL_NAME != ''">
                <xsl:value-of select ="''"/>
              </xsl:when>

              <xsl:when test ="$varISIN != ''">
                <xsl:value-of select ="$varISIN"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select ="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </ISIN>

          <xsl:variable name="PB_FUND_NAME" select="COL21"/>

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
              <xsl:when test="$NetPosition&gt; 0">
                <xsl:value-of select="$NetPosition"/>
              </xsl:when>
              <xsl:when test="$NetPosition&lt; 0">
                <xsl:value-of select="$NetPosition* (-1)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </NetPosition>


          <xsl:variable name="CostBasis" select="''"/>

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


          <SideTagValue>
            <xsl:choose>
              <xsl:when test="$NetPosition &gt; 0">
                <xsl:value-of select="'1'"/>
              </xsl:when>

              <xsl:when test="$NetPosition &lt; 0">
                <xsl:value-of select="'2'"/>
              </xsl:when>

              <xsl:otherwise>
                <xsl:value-of select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </SideTagValue>

          <PBSymbol>
            <xsl:value-of select ="$PB_SYMBOL_NAME"/>
          </PBSymbol>


          <PositionStartDate>
            <xsl:value-of select="COL3"/>
          </PositionStartDate>




        </PositionMaster>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->


</xsl:stylesheet>
