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

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">


        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL4"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash) and contains(COL2,'Cash')='true' and not(contains(COL6,'Dividends'))">

          <PositionMaster>

            <!--<xsl:variable name="PB_NAME">
              <xsl:value-of select="'BNY_WB'"/>
            </xsl:variable>-->


            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'BTIG'"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL1"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

			  <xsl:variable name ="PB_COMPANY">

				  <xsl:value-of select ="COL5"/>
			  </xsl:variable>
			  <xsl:variable name="PRANA_SYMBOL">

				  <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
			  </xsl:variable>

			  <Symbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL!=''">
						  <xsl:value-of select="$PRANA_SYMBOL"/>
					  </xsl:when>
					  <xsl:when test="contains(COL5,'38142B609')">
						  <xsl:value-of select="'USD'"/>
					  </xsl:when>
					  <xsl:when test="contains(COL5,'_')">
						<xsl:value-of select="substring-before(COL5,'_')"/>
					</xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>


			  </Symbol>

            <AccountName>
              <xsl:choose>

                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
              <!--<xsl:value-of select ="'Alephpoint Capital LP'"/>-->

            </AccountName>

            <xsl:variable name ="Date" select="COL18"/>


            <xsl:variable name="Year1" select="substring-after(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Month" select="substring-before(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Day" select="substring-before($Date,'/')"/>



            <TradeDate>

              <xsl:value-of select="$Date"/>

            </TradeDate>


            <CashValueLocal>
              <xsl:choose>
                <xsl:when test="number($Cash)">
                  <xsl:value-of select="$Cash"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CashValueLocal>

            <xsl:variable name="CashBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>

            <CashValueBase>
              <xsl:choose>
                <xsl:when test="number($CashBase)">
                  <xsl:value-of select="$CashBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CashValueBase>

            <!--<Currency>
              <xsl:value-of select="COL5"/>
            </Currency>-->

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>
</xsl:stylesheet>