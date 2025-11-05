<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL4)">
          <PositionMaster>

            <!--Start Of mandatory columns-->

            <!--<xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL12"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Morcom']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name = "amount" >
              <xsl:choose>
                <xsl:when test="COL9 &gt; 0">
                  <xsl:value-of select="COL9"/>
                </xsl:when>
                <xsl:when test="COL9 &lt; 0">
                  <xsl:value-of select="COL9*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_ACRONYM_NAME">
              <xsl:choose>
                <xsl:when test="COL1 = 'dp'">
                  <xsl:value-of select="'CASH-DEP'"/>
                </xsl:when>
               <xsl:when test="COL1 = 'wd'">
                  <xsl:value-of select="'CASH-WDL'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>-->


            <AccountName>
              <!--<xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME = ''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>-->
              <xsl:value-of select="''"/>
            </AccountName>

            <!--<FundID>
              <xsl:value-of select="COL2"/>
            </FundID>-->

            <Date>
              <xsl:value-of select="COL7"/>
            </Date>

            <Description>
              <xsl:value-of select="COL6"/>
            </Description>

            <CurrencyName>
              <xsl:value-of select ="COL5"/>
            </CurrencyName>

            <JournalEntries>
              <!--<xsl:choose>
                --><!-- Note
							  
							  * Sub account acronyms being used, must exists in db. New sub account may be added through cash management's account setup UI. 
							  * Multiple account will be seperated by separetor ; i.e- 
								concat('Cash:' , $amount , ';Transaction_Levy:' , $amount , '|Interest_Income:' , $amount, ';Transaction_Levy:' , $amount).
							  * Separator | is used to separate out the Dr entries from cr entries, Initially Dr entries and then Cr enties.
							  
							  --><!--
                --><!-- Amount negative--><!--
                <xsl:when test="COL1 = 'wd'">
                  <xsl:value-of select="concat($PRANA_ACRONYM_NAME, ':' , $amount, '|Cash:', $amount )"/>
                </xsl:when>
                --><!-- Amount positive--><!--
                <xsl:when  test="COL1 = 'dp'">
                  <xsl:value-of select="concat( 'Cash:' , $amount, '|', $PRANA_ACRONYM_NAME,':' , $amount)"/>
                </xsl:when>

              </xsl:choose>-->

              <xsl:value-of select="concat(COL1, ':', COL4, '|', COL2, ':', COL4)"/>
            </JournalEntries>

            <!--End Of mandatory columns-->

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
