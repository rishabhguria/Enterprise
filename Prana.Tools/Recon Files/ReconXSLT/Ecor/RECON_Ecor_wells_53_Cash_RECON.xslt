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
            <xsl:with-param name="Number" select="COL22"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Cash)  and (contains(COL12,'USD')='true' or contains(COL12,'CHF')='true' or contains(COL12,'CAD')='true' or contains(COL12,'EUR')='true')">

          <PositionMaster>

            <!--<xsl:variable name="PB_NAME">
              <xsl:value-of select="'BNY_WB'"/>
            </xsl:variable>-->


            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'WELLS FARGO'"/>
            </xsl:variable>

            <xsl:variable name="PB_FUND_NAME" select="COL9"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <Symbol>
				<xsl:choose>
					<!--<xsl:when test="COL12='PISXX'">
						<xsl:value-of select="'USD'"/>
					</xsl:when>-->

					<xsl:when test="contains(COL27,'USD') ">
						<xsl:value-of select="'USD'"/>
					</xsl:when>
					<xsl:when test="contains(COL27,'CHF_CUR') ">
						<xsl:value-of select="'CHF'"/>
					</xsl:when>
					<xsl:when test="contains(COL27,'CAD_CUR') ">
						<xsl:value-of select="'CAD'"/>
					</xsl:when>

					<xsl:when test="contains(COL27,'EUR_CUR') ">
						<xsl:value-of select="'EUR'"/>
					</xsl:when>


					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>

            </Symbol>

			  <!--<Asset>
				  <xsl:value-of select="normalize-space(COL11)"/>
			  </Asset>-->

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

            <xsl:variable name="TradeDate" select="COL1"/>

            <TradeDate>
              <xsl:value-of select="$TradeDate"/>
            </TradeDate>


            <!--<TradeDate>
							<xsl:value-of select="COL1"/>
						</TradeDate>-->

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

            <!--<xsl:variable name="CashBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
              </xsl:call-template>
            </xsl:variable>

            <BeginningQuantity>
              <xsl:choose>
                <xsl:when test="number($CashBase)">
                  <xsl:value-of select="$CashBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </BeginningQuantity>-->


          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>
</xsl:stylesheet>