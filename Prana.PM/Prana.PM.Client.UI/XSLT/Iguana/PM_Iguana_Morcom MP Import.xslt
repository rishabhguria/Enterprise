<?xml version="1.0" encoding="utf-8"?>

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

  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>
    <xsl:param name="varPutCall"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="($varMonth='01' or $varMonth='1') and $varPutCall='C'">
        <xsl:value-of select ="'A'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='02' or $varMonth='2') and $varPutCall='C'">
        <xsl:value-of select ="'B'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='03' or $varMonth='3') and $varPutCall='C'">
        <xsl:value-of select ="'C'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='04' or $varMonth='4') and $varPutCall='C'">
        <xsl:value-of select ="'D'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='05' or $varMonth='5') and $varPutCall='C'">
        <xsl:value-of select ="'E'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='06' or $varMonth='6') and $varPutCall='C'">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='07' or $varMonth='7') and $varPutCall='C'">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='08' or $varMonth='8') and $varPutCall='C'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='09' or $varMonth='9') and $varPutCall='C'">
        <xsl:value-of select ="'I'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='10' and $varPutCall='C'">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='11' and $varPutCall='C'">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='12' and $varPutCall='C'">
        <xsl:value-of select ="'L'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='01' or $varMonth='1') and $varPutCall='P'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='02' or $varMonth='2') and $varPutCall='P'">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='03' or $varMonth='3') and $varPutCall='P'">
        <xsl:value-of select ="'O'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='04' or $varMonth='4') and $varPutCall='P'">
        <xsl:value-of select ="'P'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='05' or $varMonth='5') and $varPutCall='P'">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='06' or $varMonth='6') and $varPutCall='P'">
        <xsl:value-of select ="'R'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='07' or $varMonth='7') and $varPutCall='P'">
        <xsl:value-of select ="'S'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='08' or $varMonth='8') and $varPutCall='P'">
        <xsl:value-of select ="'T'"/>
      </xsl:when>
      <xsl:when test ="($varMonth='09' or $varMonth='9') and $varPutCall='P'">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='10' and $varPutCall='P'">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='11' and $varPutCall='P'">
        <xsl:value-of select ="'W'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='12' and $varPutCall='P'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL21)">
          <PositionMaster>
            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name="varPBName">
              <xsl:value-of select="'Morcom'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="COL10"/>
		  </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSIBlanks">
              <xsl:call-template name="noofBlanks">
                <xsl:with-param name="count1" select="(6-string-length(substring-before(COL4,' ')))"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="concat(substring-before(COL4,' '),$varOSIBlanks,substring-after(COL4,' '))"/>
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
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="COL26"/>
            </xsl:variable>

            <xsl:variable name="varExYear">
              <xsl:value-of select="substring-after(substring-after($varOptionExpiry,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varStrike">
              <xsl:value-of select="COL27"/>
            </xsl:variable>

            <xsl:variable name="varPutCall">
              <xsl:value-of select="COL25"/>
            </xsl:variable>


            <xsl:variable name="varUnderlying">
              <xsl:value-of select="substring-before(COL12,' ')"/>
            </xsl:variable>
            
            <xsl:variable name="varCurrency">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:value-of select="COL31"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varMarkPrice">
              <xsl:value-of select="COL21"/>
            </xsl:variable>
            <!--<xsl:variable name="varSMRequest">
              <xsl:value-of select="'TRUE'"/>
            </xsl:variable>-->


            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_Symbol_NAME != ''">
                  <xsl:value-of select="$PRANA_Symbol_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$varAssetType = 'EQUITIES'">
                      <xsl:value-of select="$varEquitySymbol"/>
                    </xsl:when>
                    <xsl:when test="$varAssetType = 'OPTIONS'">
                      <xsl:variable name="MonthCode">
                        <xsl:call-template name="MonthCode">
                          <xsl:with-param name="varMonth" select="substring-before($varOptionExpiry,'/')"/>
                          <xsl:with-param name="varPutCall" select="$varPutCall"/>
                        </xsl:call-template>
                      </xsl:variable>
                      <!--<xsl:variable name="varThirdFriday">
                        <xsl:value-of select =" my:Now(number($varExYear),number(substring-before($varOptionExpiry,'/')))"/>
                      </xsl:variable>

                      <xsl:variable name="varIsFlex">
                        <xsl:choose>
                          <xsl:when test="(substring-before(substring-after($varThirdFriday,'/'),'/') + 1) = substring-before(substring-after($varOptionExpiry,'/'),'/')">
                            <xsl:value-of select="0"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="1"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>-->

                      <xsl:variable name="varDate">
                        <!--<xsl:choose>
                          <xsl:when test="string-length(substring-before(substring-after($varOptionExpiry,'/'),'/'))=0">
                            <xsl:value-of select="concat('0',substring-before(substring-after($varOptionExpiry,'/'),'/'))"/>
                          </xsl:when>
                          <xsl:otherwise>-->
                            <xsl:value-of select="substring-before(substring-after($varOptionExpiry,'/'),'/')"/>
                          <!--</xsl:otherwise>
                        </xsl:choose>-->
                      </xsl:variable>

						<xsl:variable name="varExDate">
							<xsl:choose>
								<xsl:when test="string-length($varDate) = 1">
									<xsl:value-of select="concat('0',$varDate)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varDate"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
                      <!--<xsl:choose>
                        <xsl:when test="$varIsFlex = 0">
                          <xsl:value-of select="concat('O:',$varUnderlying,' ',substring($varExYear,3,2),$MonthCode,format-number($varStrike,'#.00'))"/>
                        </xsl:when>
                        <xsl:otherwise>-->
                          <xsl:value-of select="concat('O:',$varUnderlying,' ',substring($varExYear,3,2),$MonthCode,format-number($varStrike,'#.00'),'D',$varDate)"/>
                        <!--</xsl:otherwise>
                      </xsl:choose>-->
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>


            <Date>
              <xsl:value-of select="$varPositionStartDate"/>
            </Date>

            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>


            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarkPrice))">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
