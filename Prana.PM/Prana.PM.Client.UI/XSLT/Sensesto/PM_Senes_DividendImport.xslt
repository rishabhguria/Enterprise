<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL27)">
          <!--TABLE-->
          <PositionMaster>

            <!--FundNameSection -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <FundName>
              <xsl:value-of select='$PRANA_FUND_NAME'/>
            </FundName>

            <xsl:variable name="PRANA_FUND_ID">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFundID"/>
            </xsl:variable>

            <FundID>
              <xsl:value-of select="''"/>
            </FundID>

            <Symbol>
              <xsl:value-of select="''"/>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="COL10"/>
            </PBSymbol>          

            <xsl:choose>
              <xsl:when test="COL13 != '*' and COL13 != ''">
                <CUSIP>
                  <xsl:value-of select="COL13"/>
                </CUSIP>
                <Bloomberg>
                  <xsl:value-of select="''"/>
                </Bloomberg>
                <SEDOL>
                  <xsl:value-of select="''"/>
                </SEDOL>
              </xsl:when>
              <xsl:when test="COL12 != '*' and COL12 != ''">
                <CUSIP>
                  <xsl:value-of select="''"/>
                </CUSIP>
                <Bloomberg>
                  <xsl:value-of select="COL12"/>
                </Bloomberg>
                <SEDOL>
                  <xsl:value-of select="''"/>
                </SEDOL>
              </xsl:when>
              <xsl:otherwise>
                <CUSIP>
                  <xsl:value-of select="''"/>
                </CUSIP>
                <Bloomberg>
                  <xsl:value-of select="''"/>
                </Bloomberg>
                <SEDOL>
                  <xsl:value-of select="COL10"/>
                </SEDOL>
              </xsl:otherwise>
            </xsl:choose>


            <Dividend>
              <xsl:value-of select="COL27"/>
            </Dividend>
            
            <PayoutDate>
              <xsl:value-of select="COL23"/>
            </PayoutDate>
            
            <ExDate>
              <xsl:value-of select="COL21"/>
            </ExDate>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
