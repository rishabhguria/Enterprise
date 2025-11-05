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

		  <xsl:variable name="Cash" >
			  <xsl:choose>
				  <xsl:when test="contains(normalize-space(substring(COL1,28,4)),'$$$$')">
					  <xsl:value-of select="number(substring(COL1,85,19))"/>
				  </xsl:when>

				  <!--<xsl:otherwise>
					  <xsl:value-of select="substring(COL1,347,8)"/>
				  </xsl:otherwise>-->
				  <xsl:otherwise>
					  <xsl:value-of select="number(substring(COL1,338,17))"/>
				  </xsl:otherwise>
			  </xsl:choose>
		  </xsl:variable>

		  <xsl:if test="number($Cash) and contains(normalize-space(substring(COL1,28,36)),'BANK DEPOSIT PROGRAM')='true' or contains(normalize-space(substring(COL1,28,4)),'$$$$')='true' or contains(normalize-space(substring(COL1,28,28)),'MORGAN STANLEY SICAV US$ LIQ')='true'">

          <PositionMaster>

            <!--<xsl:variable name="PB_NAME">
              <xsl:value-of select="'BNY_WB'"/>
            </xsl:variable>-->


            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Morgan Stanley'"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="substring(COL1,7,9)"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <Symbol>

              <xsl:value-of select="substring(COL1,189,21)"/>

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
            <xsl:variable name="Year" select="substring(COL1,378,4)"/>
            <xsl:variable name="Month" select="substring(COL1,383,2)"/>
            <xsl:variable name="Day" select="substring(COL1,386,2)"/>

            <xsl:variable name="Date" select="substring(COL1,378,10)"/>

            <TradeDate>

              <xsl:choose>
                <xsl:when test="contains(substring(COL1,378,10),'-')">
                  <xsl:value-of select="concat($Month,'/',$Day,'/',$Year)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$Date"/>
                </xsl:otherwise>
              </xsl:choose>


            </TradeDate>

			  <xsl:variable name="Sign" select="substring(COL1,337,1)"/>
            <EndingQuantity>
				<xsl:choose>
					<xsl:when test="contains(normalize-space(substring(COL1,28,4)),'$$$$')">
						<xsl:choose>
							<xsl:when test="$Sign ='+'">
								<xsl:value-of select="$Cash*(-1)"/>
							</xsl:when>
							<xsl:when test="$Sign ='-'">
								<xsl:value-of select="$Cash"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$Sign ='+'">
								<xsl:value-of select="$Cash"/>
							</xsl:when>
							<xsl:when test="$Sign ='-'">
								<xsl:value-of select="$Cash * (-1)"/>
							</xsl:when>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
            </EndingQuantity>
       

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>


</xsl:stylesheet>