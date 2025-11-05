<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
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
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:if test="contains(COL5,'Hand')">
          
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Geneva'"/>
            </xsl:variable>

            <xsl:variable name="varLocalCash">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL6)"/>
              </xsl:call-template>
            </xsl:variable>

            <Symbol>
              <xsl:value-of select="normalize-space(COL4)"/>
            </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
            
            <CashValueLocal>
              <xsl:choose>
                <xsl:when test="number($varLocalCash)">
                  <xsl:value-of select="$varLocalCash"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CashValueLocal>

            <xsl:variable name="varBaseCash">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL12)"/>
              </xsl:call-template>
            </xsl:variable>
            <CashValueBase>
              <xsl:choose>
                <xsl:when test="number($varBaseCash)">
                  <xsl:value-of select="$varBaseCash"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CashValueBase>
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>