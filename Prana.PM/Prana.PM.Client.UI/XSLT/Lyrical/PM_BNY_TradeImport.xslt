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
      <xsl:when test="$Suffix = 'T'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'OS'">
        <xsl:value-of select="'-OSE'"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">
        <xsl:if test="number(COL47)">
          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="'BNY'"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <!--   Fund -->
            <!--fundname section-->
            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
              <xsl:value-of select="substring(COL29,1,21)"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL57"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL32"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL6"/>
            </xsl:variable>

            <xsl:variable name="varDescription">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSide">
              <xsl:value-of select="COL20"/>
            </xsl:variable>

            <xsl:variable name="varNetPosition">
              <xsl:value-of select="COL47"/>
            </xsl:variable>

            <xsl:variable name="varCostBasis">
              <xsl:value-of select="COL48"/>
            </xsl:variable>

            <xsl:variable name="varFXConversionMethodOperator">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFXRate">
              <xsl:value-of select="''"/>
            </xsl:variable>


            <xsl:variable name="varCommission">
              <xsl:value-of select="COL67"/>
            </xsl:variable>

            <xsl:variable name="varMiscFee">
              <xsl:value-of select="COL70"/>
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

            <xsl:variable name="PB_Currency_Name">
              <xsl:value-of select="substring-after(COL57, ' ')"/>
            </xsl:variable>
            <xsl:variable name="PB_Suffix">
              <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name = $varPBName]/SymbolData[@TickerSuffixCode = $PB_Currency_Name]/@PBSuffixCode"/>
            </xsl:variable>

            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_Symbol_NAME != ''">
                  <xsl:value-of select="$PRANA_Symbol_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="$PB_Currency_Name = 'USD'">
                      <xsl:value-of select="substring-before($varEquitySymbol, ' ')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="concat(substring-before($varEquitySymbol, ' '), $PB_Suffix)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>
            <CUSIP>
              <xsl:value-of select="$varCUSIP"/>
            </CUSIP>

            <RIC>
              <xsl:value-of select="$varRIC"/>
            </RIC>

            <Bloomberg>
              <xsl:value-of select="$varBloomberg"/>
            </Bloomberg>

            <SEDOL>
              <xsl:value-of select="$varSEDOL"/>
            </SEDOL>

            <Description>
              <xsl:value-of select="$varDescription"/>
            </Description>

            <!--QUANTITY-->

            <xsl:choose>
              <xsl:when test="$varNetPosition &lt; 0">
                <NetPosition>
                  <xsl:value-of select="$varNetPosition * (-1)"/>
                </NetPosition>
              </xsl:when>
              <xsl:when test="$varNetPosition &gt; 0">
                <NetPosition>
                  <xsl:value-of select="$varNetPosition"/>
                </NetPosition>
              </xsl:when>
              <xsl:otherwise>
                <NetPosition>
                  <xsl:value-of select="0"/>
                </NetPosition>
              </xsl:otherwise>
            </xsl:choose>

            <!--Side-->

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide = 'BUY'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'SELL'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'SS'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <CostBasis>
              <xsl:choose>
                <xsl:when test ="boolean(number($varCostBasis))">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>


            <Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

          

            <MiscFees>
              <xsl:choose>
                <xsl:when test="$varMiscFee &gt; 0">
                  <xsl:value-of select="$varMiscFee"/>
                </xsl:when>
                <xsl:when test="$varMiscFee &lt; 0">
                  <xsl:value-of select="$varMiscFee*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MiscFees>

           

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
