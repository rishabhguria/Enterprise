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


  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CHF'">
        <xsl:value-of select="'-SWX'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'EUR'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="COL1 != 'Account' and number(COL13) and normalize-space(COL6) != 'Cash and Equivalents'">
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'Jefferies'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="(COL4)"/>
            </xsl:variable>

			  <xsl:variable name = "PB_Currency_NAME" >
				  <xsl:value-of select="COL5"/>
			  </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME and @PBCurrencyName=$PB_Currency_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varImportDate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varRIC">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="COL19"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="varMarkPrice">
              <xsl:value-of select="COL13"/>
            </xsl:variable>

            <xsl:variable name="varCounterPartyID">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$varAssetType='Options'">

                <Symbol>
                  <xsl:value-of select="''"/>
                </Symbol>

                <IDCOOptionSymbol>
                  <xsl:value-of select="concat($varOSISymbol,'U')"/>
                </IDCOOptionSymbol>


              </xsl:when>

              <xsl:when test="$varAssetType = 'Equity'">
                <xsl:variable name="varSuffix">
                  <xsl:call-template name="GetSuffix">
                    <xsl:with-param name="Suffix" select="COL6"/>
                  </xsl:call-template>
                </xsl:variable>

                <Symbol>
                  <xsl:choose>
                    <xsl:when test="$PRANA_Symbol_NAME != ''">
                      <xsl:value-of select="$PRANA_Symbol_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:choose>
                        <xsl:when test="COL6 != 'USD'">
                          <xsl:value-of select="concat($varEquitySymbol, $varSuffix)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="$varEquitySymbol"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:otherwise>
                  </xsl:choose>
                </Symbol>

                <IDCOOptionSymbol>
                  <xsl:value-of select="''"/>
                </IDCOOptionSymbol>

              </xsl:when>
            </xsl:choose>

            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>

            <MarkPrice>
              <xsl:choose>
                <xsl:when  test="number($varMarkPrice)">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </MarkPrice>

            <Date>
              <xsl:value-of select="$varImportDate"/>
            </Date>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>