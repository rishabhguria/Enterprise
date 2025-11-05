<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

      <xsl:for-each select="//PositionMaster">
        
        <xsl:variable name ="varInstrument">
          <xsl:value-of select="normalize-space(COL11)"/>
        </xsl:variable>
        <xsl:if test ="$varInstrument='CS'">
          <PositionMaster>
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="translate(COL4,'&quot;','')"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='ML']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <FundName>
                  <xsl:value-of select="''"/>
                </FundName>
              </xsl:when>
              <xsl:otherwise>
                <FundName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </FundName>
              </xsl:otherwise>
            </xsl:choose>

            <BaseCurrency>
              <xsl:value-of select="'USD'"/>
            </BaseCurrency>

            <LocalCurrency>
              <xsl:value-of select="'USD'"/>
            </LocalCurrency>

            <xsl:choose>
              <xsl:when test ="boolean(number(COL14))">
                <CashValueBase>
                  <xsl:value-of select="COL14"/>
                </CashValueBase>
              </xsl:when >
              <xsl:otherwise>
                <CashValueBase>
                  <xsl:value-of select="0"/>
                </CashValueBase>
              </xsl:otherwise>
            </xsl:choose >

            <xsl:choose>
              <xsl:when test ="boolean(number(COL14))">
                <CashValueLocal>
                  <xsl:value-of select="COL14"/>
                </CashValueLocal>
              </xsl:when >
              <xsl:otherwise>
                <CashValueLocal>
                  <xsl:value-of select="0"/>
                </CashValueLocal>
              </xsl:otherwise>
            </xsl:choose >

            <xsl:choose>
              <xsl:when test ="COL3 != '' or COL3 != '*'">
                <Date>
                  <xsl:value-of select="COL3"/>
                </Date>
              </xsl:when>
              <xsl:otherwise>
                <Date>
                  <xsl:value-of select="''"/>
                </Date>
              </xsl:otherwise>
            </xsl:choose>

            <PositionType>
              <xsl:value-of select="'Cash'"/>
            </PositionType>

          </PositionMaster>
        </xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>	
</xsl:stylesheet>
