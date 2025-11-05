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
        <xsl:variable name="varRoundCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(substring(COL1,353,4))"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:variable name="varDecimalCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(substring(COL1,338,14))"/>
          </xsl:call-template>
        </xsl:variable>

        
        <xsl:variable name="Cash">
          <xsl:value-of select ="concat($varRoundCash,'.',$varDecimalCash)"/>
        </xsl:variable>

        <xsl:variable name ="varCashCol">
          <xsl:value-of select ="normalize-space(substring(COL1,28,20))"/>
        </xsl:variable>
        <xsl:if test="number($Cash) and ($varCashCol='$$$$' or $varCashCol='BANK DEPOSIT PROGRAM')">
          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="varCurrency">
              <xsl:value-of select="'USD'"/>
            </xsl:variable>

            <Symbol>
              <xsl:value-of select="$varCurrency"/>
            </Symbol>


            <xsl:variable name="PB_FUND_NAME" select="''"/>

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

            <xsl:variable name="varPosOrNeg" select="normalize-space(substring(COL1,325,2))"/>

            <CashValueLocal>
              <xsl:choose>
                <xsl:when test="$varPosOrNeg='dp'">
                  <xsl:value-of select="$Cash"/>
                </xsl:when>
                <xsl:when test="$varPosOrNeg='wd'">
                  <xsl:value-of select="($Cash * (-1))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CashValueLocal>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>