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

  <xsl:template name="MonthCodes">
    <xsl:param name="vrMonth"/>
    <xsl:choose>
      <xsl:when test="$vrMonth='Jan'">
        <xsl:value-of select="'01'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Feb'">
        <xsl:value-of select="'2'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Mar'">
        <xsl:value-of select="'3'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Apr'">
        <xsl:value-of select="'4'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='May'">
        <xsl:value-of select="'5'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Jun'">
        <xsl:value-of select="'6'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Jul'">
        <xsl:value-of select="'7'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Aug'">
        <xsl:value-of select="'8'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Sep'">
        <xsl:value-of select="'09'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Oct'">
        <xsl:value-of select="'10'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Nov'">
        <xsl:value-of select="'11'"/>
      </xsl:when>
      <xsl:when test="$vrMonth='Dec'">
        <xsl:value-of select="'12'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//Comparision">

        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL11"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="COL1='Security'">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL2"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:variable name="varSymbol">
                <xsl:value-of select="translate(normalize-space(COL1),$vLowercaseChars_CONST,$vUppercaseChars_CONST)"/>
              </xsl:variable>
            

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>

                  <xsl:when test="$varSymbol!=''">
                    <xsl:value-of select="$varSymbol"/>
                  </xsl:when>


                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>


              <xsl:variable name="varSedol">
                <xsl:value-of select="COL5"/>
              </xsl:variable>
             
              <SEDOL>
                <xsl:value-of select="$varSedol"/>
              </SEDOL>

              <xsl:variable name="varCusip">
                <xsl:value-of select="COL3"/>
              </xsl:variable>
              <CUSIP>
                <xsl:value-of select="$varCusip"/>
              </CUSIP>

              <xsl:variable name="varISIN">
                <xsl:value-of select="COL4"/>
              </xsl:variable>
              <ISIN>
                <xsl:value-of select="$varISIN"/>
              </ISIN>

              <xsl:variable name="PB_FUND_NAME" select="''"/>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>


              <Side>
                <xsl:value-of select="''"/>
              </Side>
              

              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>

   
            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>


              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>



              <FundName>
                <xsl:value-of select="''"/>
              </FundName>


              <Side>
                <xsl:value-of select="''"/>
              </Side>
             

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>

        
            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>

      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


