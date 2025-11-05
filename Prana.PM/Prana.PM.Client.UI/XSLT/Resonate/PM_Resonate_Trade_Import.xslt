<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">

    public static string NowSwap(int year, int month, int date)
    {
    DateTime weekEnd= new DateTime(year, month, date);
    weekEnd = weekEnd.AddDays(1);
    while (weekEnd.DayOfWeek == DayOfWeek.Saturday || weekEnd.DayOfWeek == DayOfWeek.Sunday)
    {
    weekEnd = weekEnd.AddDays(1);
    }
    return weekEnd.ToString();
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

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL5"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="number($Position)">

          <PositionMaster>
            <xsl:variable name="PB_NAME">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="normalize-space('')"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

            <PositionStartDate>
              <xsl:value-of select="''"/>
            </PositionStartDate>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>
                <xsl:when test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL3)"/>
            </xsl:variable>

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide = 'B'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'S'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'BC'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'SS'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
              </xsl:choose>
            </SideTagValue>
            
             <xsl:variable name="varAvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL6"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when test="$varAvgPrice &gt; 0">
                  <xsl:value-of select="$varAvgPrice"/>
                </xsl:when>
                <xsl:when test="$varAvgPrice &lt; 0">
                  <xsl:value-of select="$varAvgPrice*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL8)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
            </xsl:variable>

            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_BROKER_ID)">
                  <xsl:value-of select="$PRANA_BROKER_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>

            <xsl:variable name="varAsset">
              <xsl:choose>
                <xsl:when test="COL2 = 'Cash' or COL2 ='CASH'">
                  <xsl:value-of select="'Equity'"/>
                </xsl:when>
                <xsl:when test="COL2 = 'Swap' or COL2 ='SWAP'">
                  <xsl:value-of select="'EquitySwap'"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varOrigTransDate">
              <xsl:value-of select ="COL1"/>
            </xsl:variable>
            
              <xsl:if test="COL2 = 'Swap' or COL2 ='SWAP'">
              <IsSwapped>
                <xsl:value-of select ="1"/>
              </IsSwapped>

              <SwapDescription>
                <xsl:value-of select ="'SWAP'"/>
              </SwapDescription>

              <DayCount>
                <xsl:value-of select ="365"/>
              </DayCount>

              <ResetFrequency>
                <xsl:value-of select ="'Monthly'"/>
              </ResetFrequency>

                <OrigTransDate>
                  <xsl:value-of select ="$varOrigTransDate"/>
                </OrigTransDate>
                
              <xsl:variable name="varYear">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:value-of select="substring-after(substring-after($varOrigTransDate,'/'),'/')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring-after(substring-after($varOrigTransDate,'-'),'-')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varDay">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before(substring-after($varOrigTransDate,'/'),'/'))) = 1">
                        <xsl:value-of select="concat(0,substring-before(substring-after($varOrigTransDate,'/'),'/'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before(substring-after($varOrigTransDate,'/'),'/')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before(substring-after($varOrigTransDate,'-'),'-'))) = 1">
                        <xsl:value-of select="concat(0,substring-before(substring-after($varOrigTransDate,'-'),'-'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before(substring-after($varOrigTransDate,'-'),'-')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varMonth">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before($varOrigTransDate,'/'))) = 1">
                        <xsl:value-of select="concat(0,substring-before($varOrigTransDate,'/'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before($varOrigTransDate,'/')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before($varOrigTransDate,'-'))) = 1">
                        <xsl:value-of select="concat(0,substring-before($varOrigTransDate,'-'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before($varOrigTransDate,'-')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <xsl:variable name="FirstResetDate">
                <xsl:value-of select='my:NowSwap(number($varYear),number($varMonth),number($varDay))'/>
              </xsl:variable>

              <FirstResetDate>
                <xsl:value-of select ="$FirstResetDate"/>
              </FirstResetDate>

            </xsl:if>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
