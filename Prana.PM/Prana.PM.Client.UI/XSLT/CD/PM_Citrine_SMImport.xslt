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

  <xsl:template name="MonthCode">
    <xsl:param name="varMonth"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='01' ">
        <xsl:value-of select ="'F'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='02' ">
        <xsl:value-of select ="'G'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='03'">
        <xsl:value-of select ="'H'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='04' ">
        <xsl:value-of select ="'J'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='05' ">
        <xsl:value-of select ="'K'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='06'">
        <xsl:value-of select ="'M'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='07' ">
        <xsl:value-of select ="'N'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='08' ">
        <xsl:value-of select ="'Q'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='09' ">
        <xsl:value-of select ="'U'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='10' ">
        <xsl:value-of select ="'V'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='11'">
        <xsl:value-of select ="'X'"/>
      </xsl:when>
      <xsl:when test ="$varMonth='12'">
        <xsl:value-of select ="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="MonthNo">
    <xsl:param name="varMonth"/>

    <!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
    <xsl:choose>
      <xsl:when test ="$varMonth='F' or $varMonth='f'">
        <xsl:value-of select ="01"/>
      </xsl:when>
      <xsl:when test ="$varMonth='G' or $varMonth ='g'">
        <xsl:value-of select ="02"/>
      </xsl:when>
      <xsl:when test ="$varMonth='H' or $varMonth ='h'">
        <xsl:value-of select ="03"/>
      </xsl:when>
      <xsl:when test ="$varMonth='J' or $varMonth ='j'">
        <xsl:value-of select ="04"/>
      </xsl:when>
      <xsl:when test ="$varMonth='K' or $varMonth ='k'">
        <xsl:value-of select ="05"/>
      </xsl:when>
      <xsl:when test ="$varMonth='M' or $varMonth ='m'">
        <xsl:value-of select ="06"/>
      </xsl:when>
      <xsl:when test ="$varMonth='N' or $varMonth ='n'">
        <xsl:value-of select ="07"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Q' or $varMonth ='q'">
        <xsl:value-of select ="08"/>
      </xsl:when>
      <xsl:when test ="$varMonth='U' or $varMonth ='u'">
        <xsl:value-of select ="09"/>
      </xsl:when>
      <xsl:when test ="$varMonth='V' or $varMonth ='v'">
        <xsl:value-of select ="10"/>
      </xsl:when>
      <xsl:when test ="$varMonth='X' or $varMonth ='x'">
        <xsl:value-of select ="11"/>
      </xsl:when>
      <xsl:when test ="$varMonth='Z' or $varMonth ='z'">
        <xsl:value-of select ="12"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select ="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
      <xsl:for-each select="//PositionMaster">


        <!--IF NOT CONTAIN HEADER ROW -->
        <xsl:if test="COL1 != 'Confirmed'">

          <PositionMaster>

            <xsl:variable name="varPBName">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varMultiplier">
              <xsl:value-of select="COL40"/>
            </xsl:variable>

            <xsl:variable name="varAssetCategory">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varCurrency">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCusipSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varRICSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloombergSymbol">
              <xsl:value-of select="COL25"/>
            </xsl:variable>

            <xsl:variable name = "PB_Broker" >
              <xsl:value-of select="''"/>
            </xsl:variable>
            <xsl:variable name="PRANA_Broker">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$varPBName]/BrokerData[@PBBroker = $PB_Broker]/@PranaBrokerCode"/>
            </xsl:variable>

            <xsl:variable name="varISINSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSedolSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOPRAOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>


            <xsl:variable name="varLongName">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquityOptionAUECID">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFutureAUECID">
              <xsl:value-of select="24"/>
            </xsl:variable>

            <xsl:variable name="varFutureOptionAUECID">
              <xsl:value-of select="12"/>
            </xsl:variable>

            <xsl:variable name="varFutureExpirationDate">
              <xsl:value-of select="substring-after(COL24,' ')"/>
            </xsl:variable>

            <xsl:variable name="varFutureUnderlyingSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varFutureCallPutCode">
              <xsl:value-of select="COL18"/>
            </xsl:variable>

            <xsl:variable name="varFutureStrikePrice">
              <xsl:choose>
                <xsl:when test="number(COL19)">
                  <xsl:value-of select="COL19"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varMonthCode">
              <xsl:call-template name="MonthCode">
                <xsl:with-param name="varMonth" select="substring(COL24,12,2)"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:choose>
                <xsl:when test="(normalize-space(COL8) = 'Future' or normalize-space(COL8) = 'future') and COL13 = 'LME'">
                  <xsl:value-of select="concat(substring(COL24,3,3),' ', substring(COL24,11,1),$varMonthCode,substring(COL24,14,2),'-LME')"/>
                </xsl:when>
                <xsl:when test="(normalize-space(COL8) = 'Future' or normalize-space(COL8) = 'future') and COL13 != 'LME'">
                  <xsl:value-of select="concat(substring(COL24,1,2),' ', substring(COL24,3,2))"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>



            <xsl:variable name="varFutOptionExpiry">
              <xsl:if test="$varAssetCategory = 'Option'">
                <xsl:choose>
                  <xsl:when test="COL32 = '#N/A Field Not Applicable' or COL32 = '#N/A N Ap'">
                    <xsl:value-of select="concat(substring(COL31,3,2),'/',substring(COL31,1,2),'/',substring(COL31,5,4))"/>
                  </xsl:when>
                  <xsl:when test="COL32 = '#N/A Requesting Data...' or COL32 = '#N/A Sec' or COL32 = '#NAME?' or COL32 = 'True' or COL32 = '*'">
                    <xsl:variable name="varMonthNo">
                      <xsl:call-template name="MonthNo">
                        <xsl:with-param name="varMonth" select="substring(COL24,3,1)"/>
                      </xsl:call-template>
                    </xsl:variable>
                    <xsl:value-of select =" substring-before(my:Now(number(concat('201',substring(COL24,4,1))),number($varMonthNo)),' ')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="string-length(COL32) = 8">
                        <xsl:value-of select="concat(substring(COL32,3,2),'/',substring(COL32,1,2),'/',substring(COL32,5,4))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat(substring(COL32,2,2),'/',substring(COL32,1,1),'/',substring(COL32,4,4))"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>
            </xsl:variable>

            <xsl:variable name="varOptionDay">
              <xsl:value-of select="substring-before(substring-after($varFutOptionExpiry,'/'),'/')"/>
            </xsl:variable>

            <xsl:variable name="varFutureOption">
              <xsl:choose>
                <xsl:when test="normalize-space(COL8) = 'Option' and COL13 = 'LME'">
                  <xsl:value-of select="concat(COL11,' ',substring(COL24,4,1),substring(COL24,3,1),$varOptionDay,substring(COL24,5,1),substring-after(COL24,' '),'-LME')"/>
                </xsl:when>
                <xsl:when test="normalize-space(COL8) = 'Option' and COL13 != 'LME'">
                  <xsl:value-of select="concat(substring(COL24,1,2),' ',substring(COL24,3,2),substring-after(COL24,' '))"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            

            <xsl:choose>
              <!--Handling for Equity Options-->
              <xsl:when test="$varAssetCategory = 'Option'">
                <TickerSymbol>
                  <xsl:value-of select="$varFutureOption"/>
                </TickerSymbol>


                <ExpirationDate>
                  <xsl:value-of select="$varFutOptionExpiry"/>
                </ExpirationDate>

                <UnderLyingSymbol>
                  <xsl:value-of select="''"/>
                </UnderLyingSymbol>

                <StrikePrice>
                  <xsl:value-of select="$varFutureStrikePrice"/>
                </StrikePrice>

                <PutOrCall>
                  <xsl:choose>
                    <xsl:when test="$varFutureCallPutCode='P' or $varFutureCallPutCode='p'">
                      <xsl:value-of select="0"/>
                    </xsl:when>

                    <xsl:when test="$varFutureCallPutCode='C' or $varFutureCallPutCode='c'">
                      <xsl:value-of select="1"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="2"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </PutOrCall>

                <AUECID>
                  <xsl:value-of select="$varFutureOptionAUECID"/>
                </AUECID>

              </xsl:when>

              <!--Handling for Futures-->
              <xsl:otherwise>
                <TickerSymbol>
                  <xsl:value-of select="$varFutureSymbol"/>
                </TickerSymbol>

                <ExpirationDate>
                  <xsl:choose>
                    <xsl:when test="COL32 = '#N/A Field Not Applicable'">
                      <xsl:value-of select="concat(substring(COL31,3,2),'/',substring(COL31,1,2),'/',substring(COL31,5,4))"/>
                    </xsl:when>
                    <xsl:when test="COL32 = '#N/A N Ap' and number(COL31) and string-length(COL31) = 8">
                      <xsl:value-of select="concat(substring(COL31,3,2),'/',substring(COL31,1,2),'/',substring(COL31,5,4))"/>
                    </xsl:when>
                    <xsl:when test="COL32 = '#N/A N Ap' and number(COL31) and string-length(COL31) = 7">
                      <xsl:value-of select="concat(substring(COL31,2,2),'/',substring(COL31,1,1),'/',substring(COL31,4,4))"/>
                    </xsl:when>
                    <xsl:when test="(COL32 = '#N/A N Ap' and COL31 = '#N/A N/A') or COL32 = '#N/A Requesting Data...' or COL32 = '#N/A Sec'">
                      <xsl:value-of select="concat(substring($varFutureExpirationDate,5,2),'/',substring($varFutureExpirationDate,7,2),'/',substring($varFutureExpirationDate,1,4))"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:choose>
                        <xsl:when test="string-length(COL32) = 8">
                          <xsl:value-of select="concat(substring(COL32,3,2),'/',substring(COL32,1,2),'/',substring(COL32,5,4))"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat(substring(COL32,2,2),'/',substring(COL32,1,1),'/',substring(COL32,4,4))"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:otherwise>
                  </xsl:choose>
                </ExpirationDate>

                <UnderLyingSymbol>
                  <xsl:value-of select="$varFutureUnderlyingSymbol"/>
                </UnderLyingSymbol>

                <StrikePrice>
                  <xsl:value-of select="0"/>
                </StrikePrice>

                <PutOrCall>
                  <xsl:value-of select="0"/>
                </PutOrCall>

                <AUECID>
                  <xsl:value-of select="$varFutureAUECID"/>
                </AUECID>

              </xsl:otherwise>
            </xsl:choose>

            <IDCOOptionSymbol>
              <xsl:value-of select="''"/>
            </IDCOOptionSymbol>

            <DividendYield>
              <xsl:value-of select ="0"/>
            </DividendYield>

            <Dividend>
              <xsl:value-of select ="0"/>
            </Dividend>

            <DividendAmtRate>
              <xsl:value-of select ="0"/>
            </DividendAmtRate>

            <DivDistributionDate>
              <xsl:value-of select ="'1/1/0001'"/>
            </DivDistributionDate>

            <RequestedSymbology>
              <xsl:value-of select="0"/>
            </RequestedSymbology>

            <Multiplier>
              <xsl:value-of select="$varMultiplier"/>
            </Multiplier>

            <LongName>
              <xsl:value-of select="$varLongName"/>
            </LongName>

            <!--<UnderLyingSymbol>
							<xsl:value-of select="$varSymbol"/>
						</UnderLyingSymbol>-->

            <AssetCategory>
              <xsl:value-of select="$varAssetCategory"/>
            </AssetCategory>

            <Symbol_PK>
              <xsl:value-of select ="0"/>
            </Symbol_PK>

            <ReutersSymbol>
              <xsl:value-of select="$varRICSymbol"/>
            </ReutersSymbol>

            <BloombergSymbol>
              <xsl:value-of select="$varBloombergSymbol"/>
            </BloombergSymbol>

            <ISINSymbol>
              <xsl:value-of select="$varISINSymbol"/>
            </ISINSymbol>

            <SedolSymbol>
              <xsl:value-of select="$varSedolSymbol"/>
            </SedolSymbol>

            <OSIOptionSymbol>
              <xsl:value-of select="$varOSISymbol"/>
            </OSIOptionSymbol>

            <OPRAOptionSymbol>
              <xsl:value-of select="''"/>
            </OPRAOptionSymbol>

            <CusipSymbol>
              <xsl:value-of select="$varCusipSymbol"/>
            </CusipSymbol>

            <SymbolType>
              <xsl:value-of select="'0'"/>
            </SymbolType>

            <Delta>
              <xsl:value-of select="'1'"/>
            </Delta>

            <!--<PutOrCall>
							<xsl:choose>
								<xsl:when test="$varCallPutCode='P'">
									<xsl:value-of select="0"/>
								</xsl:when>

								<xsl:when test="$varCallPutCode='C'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="2"/>
								</xsl:otherwise>
							</xsl:choose>
						</PutOrCall>-->

            <AccrualBasisID>
              <xsl:value-of select="'0'"/>
            </AccrualBasisID>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>