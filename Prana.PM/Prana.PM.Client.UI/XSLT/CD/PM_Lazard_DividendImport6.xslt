<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL5) ">

          <!--TABLE-->
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varDividend">
              <xsl:value-of select="COL5"/>
            </xsl:variable>

            <xsl:variable name="varPayoutDate">
              <xsl:value-of select="COL13"/>
            </xsl:variable>

            <xsl:variable name="varRecordDate">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="varExDate">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <!--FundNameSection -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name = $varPBName]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>


            <AccountName>
              <xsl:value-of select='""'/>
            </AccountName>

            <PBSymbol>
              <xsl:value-of select="COL1"/>
            </PBSymbol>

            <Symbol>
              <xsl:value-of select="COL1"/>
            </Symbol>

            <Dividend>
              <xsl:value-of select="$varDividend"/>
            </Dividend>

            <PayoutDate>
              <xsl:value-of select="$varPayoutDate"/>
            </PayoutDate>

            <RecordDate>
              <xsl:value-of select="$varRecordDate"/>
            </RecordDate>

            <ExDate>
              <xsl:value-of select="$varExDate"/>
            </ExDate>

            <Description>
              <xsl:value-of select="COL14"/>
            </Description>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>