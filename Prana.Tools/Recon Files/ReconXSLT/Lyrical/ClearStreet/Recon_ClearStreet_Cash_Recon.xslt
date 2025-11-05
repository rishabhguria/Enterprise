<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),'�',''),$SingleQuote,''))"/>
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
      <xsl:for-each select="//Comparision">

        <xsl:variable name="varLCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL10"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($varLCash)">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varLocalCurrency">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <LocalCurrency>
              <xsl:value-of select="$varLocalCurrency"/>
            </LocalCurrency>


            <Symbol>
              <xsl:value-of select="$varLocalCurrency"/>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="COL4"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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


            <CashValueLocal>
              <xsl:choose>
                <xsl:when test="$varLCash &gt; 0">
                  <xsl:value-of select="$varLCash"/>
                </xsl:when>
                <xsl:when test="$varLCash &lt; 0">
                  <xsl:value-of select="$varLCash * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CashValueLocal>

            <xsl:variable name="varBCash">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="''"/>
              </xsl:call-template>
            </xsl:variable>

            <CashValueBase>
              <xsl:choose>
                <xsl:when test="$varBCash &gt; 0">
                  <xsl:value-of select="$varBCash"/>
                </xsl:when>
                <xsl:when test="$varBCash &lt; 0">
                  <xsl:value-of select="$varBCash * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CashValueBase>

            <xsl:variable name="varDay">
              <xsl:value-of select="substring(COL1,7,2)"/>
            </xsl:variable>
            <xsl:variable name="varMonth">
              <xsl:value-of select="substring(COL1,5,2)"/>
            </xsl:variable>
            <xsl:variable name="varYear">
              <xsl:value-of select="substring(COL1,1,4)"/>
            </xsl:variable>
            <Date>
              <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
            </Date>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>