<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>

  


  <xsl:template name="noofBlanks">
    <xsl:param name="count1"/>
    <xsl:if test="$count1 > 0">
      <xsl:value-of select ="' '"/>
      <xsl:call-template name="noofBlanks">
        <xsl:with-param name="count1" select="$count1 - 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test ="number(COL8) and COL23 = 'Active'">

          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Knight']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name="PB_Symbol" select="COL34"/>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='Knight']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>

           

            <Symbol>
              <xsl:value-of select="COL7"/>
            </Symbol>
            <Dividend>
              <xsl:value-of select="translate(COL12,'N/A','0')"/>
            </Dividend>
            <PayoutDate>
              <xsl:value-of select="substring-after(substring-before(COL10,'for CA ID'),'Payable Date:')"/>
            </PayoutDate>
            <ExDate>
              <xsl:value-of select="substring-after(substring-before(COL10,'Payout Rate'),'Record Date:')"/>
            </ExDate>

           
            
            


            <PBSymbol>
              <xsl:value-of select="COL34"/>
            </PBSymbol>

            <!--<PBAssetType>
              <xsl:value-of select ="COL5"/>
            </PBAssetType>-->

            <!--QUANTITY-->


            <!--Side-->


            







          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
