<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/DocumentElement">
    <DocumentElement>
      <xsl:for-each select="PositionMaster">
        <xsl:if test="COL16 = 'DV' and COL15 ='NEW'">

          <!--TABLE-->
          <PositionMaster>

            <!--FundNameSection -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='DB']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>


            <AccountName>
              <xsl:value-of select='$PRANA_FUND_NAME'/>
            </AccountName>

            <xsl:variable name="PRANA_FUND_ID">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='DB']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFundID"/>
            </xsl:variable>

            <FundID>
              <xsl:value-of select="$PRANA_FUND_ID"/>
            </FundID>

            <Symbol>
              <xsl:value-of select="COL22"/>
            </Symbol>
            
            <Dividend>
              <xsl:choose>
                <xsl:when test="number(COL42)">
                  <xsl:value-of select="COL42"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
              <!--<xsl:value-of select="translate(COL12,'N/A','0')"/>-->
            </Dividend>
            
            <PayoutDate>
              <xsl:value-of select="concat(substring(COL11,5,2),'/',substring(COL11,7,2),'/',substring(COL11,1,4))"/>
            </PayoutDate>
            
            <ExDate>
              <xsl:value-of select="concat(substring(COL10,5,2),'/',substring(COL10,7,2),'/',substring(COL10,1,4))"/>
            </ExDate>

          </PositionMaster>

        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
