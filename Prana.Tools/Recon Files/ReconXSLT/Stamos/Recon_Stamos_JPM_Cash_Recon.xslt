<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

 

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

        <xsl:variable name="Cash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL10 div 100"/>
          </xsl:call-template>
        </xsl:variable>
		  <!-- Adjusted by sign (COL11) -->
		  <xsl:variable name="adjustedAmount">
			  <xsl:choose>
				  <xsl:when test="COL11 = '-'">
					  <xsl:value-of select="$Cash div 100"/>
				  </xsl:when>
				  <xsl:otherwise>
					  <xsl:value-of select="($Cash div 100) * -1"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </xsl:variable>
          <xsl:if test="number($Cash) and COL7 ='A'">
          <PositionMaster>
            <xsl:variable name="varPBName">
              <xsl:value-of select="'JPM'"/>
            </xsl:variable>
            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
			  <xsl:variable name="currencyCode">
				  <xsl:choose>
					  <xsl:when test="COL19 = 'U$'">
					   <xsl:value-of select ="'USD'"/>
					  </xsl:when>
					  <xsl:when test="COL19 = 'BP'">
						  <xsl:value-of select ="'GBP'"/>
					  </xsl:when>
					  <xsl:when test="COL19 = 'H$'">
						  <xsl:value-of select ="'HKD'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="COL19"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <Symbol>
              <xsl:value-of select ="$currencyCode"/>
            </Symbol>

			  <CashValueLocal>
				  <xsl:value-of select ="$adjustedAmount"/>
			  </CashValueLocal>

			<CashValueBase>
				  <xsl:value-of select ="$adjustedAmount"/>
			  </CashValueBase>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
