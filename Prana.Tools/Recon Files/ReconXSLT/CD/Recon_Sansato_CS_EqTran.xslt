<?xml version="1.0" encoding="UTF-8"?>
<!-- Object -Trade Recon for GS, Date -01-11-2012(dd/MM/yyyy) -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:if test="COL3 != 'Account' and normalize-space(COL12) != 'CASH'">
          <PositionMaster>

            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <!--Need to Add here PB Name From FundMapping.xml-->
            <xsl:variable name="PRANA_FUND_NAME">
              <!--<xsl:value-of select="''"/>-->
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='SANSATO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>


            <xsl:variable name="PB_COMPANY_NAME" select="COL11"/>

            <PBSymbol>
              <xsl:value-of select="COL11"/>
            </PBSymbol>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <!--<xsl:value-of select="''"/>-->
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='SANSATO']/SymbolData[@CompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <CompanyName>
              <xsl:value-of select="$PB_COMPANY_NAME"/>
            </CompanyName>

            <xsl:choose>
              <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
                <SEDOL>
                  <xsl:value-of select="''"/>
                </SEDOL>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>
                <SEDOL>
                  <xsl:value-of select="COL6"/>
                </SEDOL>
              </xsl:otherwise>
            </xsl:choose>

            <Side>
              <xsl:choose>
                <xsl:when test="COL4='SHORT'">
                  <xsl:value-of select="'Sell short'"/>
                </xsl:when>
                <xsl:when test="COL4!='SHORT' and COL1 ='SEL'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'Buy'"/>
                </xsl:otherwise>
              </xsl:choose>
            </Side>




            <!--BEGIN FOR NET POSITION ie QUANTITY -->
            <Quantity>
              <xsl:choose>
                <xsl:when  test="number(COL18) and COL18 &lt; 0">
                  <xsl:value-of select="COL18 * (-1)"/>
                </xsl:when>
                <xsl:when  test="number(COL18) and COL18 &gt; 0">
                  <xsl:value-of select="COL18"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'0'"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>


            <!-- Value in Local Currency-->
            <AvgPX>
              <xsl:choose>
                <xsl:when test="number(COL19) and  number(COL19) &lt; 0">
                  <xsl:value-of select= "COL19 *(-1)"/>
                </xsl:when>
                <xsl:when test="number(COL19) and  number(COL19) &gt; 0">
                  <xsl:value-of select= "COL19"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select= "0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPX>


            <!-- Commission Value in Local Currency-->
            <Commission>
              <xsl:choose>
                <xsl:when test="number(COL22) and COL22 &lt; 0 ">
                  <xsl:value-of select="COL22 *(-1)"/>
                </xsl:when>
                <xsl:when test="number(COL22) and COL22 &gt; 0 ">
                  <xsl:value-of select="COL22"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <!-- FEES Value in Local Currency-->
            <Fees>
              <xsl:choose>
                <xsl:when test="number(COL23) and COL23 &lt; 0 ">
                  <xsl:value-of select="COL23 *(-1)"/>
                </xsl:when>
                <xsl:when test="number(COL23) and COL23 &gt; 0 ">
                  <xsl:value-of select="COL23"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>


            <!--GROSS NOTIONAL in Local Currency-->
            <GrossNotionalValue>
              <xsl:choose>
                <xsl:when test="number(COL21) and COL21 &lt; 0 ">
                  <xsl:value-of select="COL21 *(-1)"/>
                </xsl:when>
                <xsl:when test="number(COL21) and COL21 &gt; 0 ">
                  <xsl:value-of select="COL21"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </GrossNotionalValue>


            <!--NET NOTIONAL in Local Currency -->
            <NetNotionalValueLocal>
              <xsl:choose>
                <xsl:when test="number(COL27) and COL27 &lt; 0 ">
                  <xsl:value-of select="COL27 *(-1)"/>
                </xsl:when>
                <xsl:when test="number(COL27) and COL27 &gt; 0 ">
                  <xsl:value-of select="COL27"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueLocal>


			  <!--NET NOTIONAL in Base Currency -->
			  <!--<NetNotionalValue>
				  <xsl:value-of select="0"/>
			  </NetNotionalValue>-->

			  <!--For SEDOL Search-->
			  <SMRequest>
				  <xsl:value-of select ="'TRUE'"/>
			  </SMRequest>

		  </PositionMaster>
	  </xsl:if >
  </xsl:for-each>
    </DocumentElement>
  </xsl:template>


</xsl:stylesheet>
