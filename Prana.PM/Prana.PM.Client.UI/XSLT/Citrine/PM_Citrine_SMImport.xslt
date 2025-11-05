<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
	  public string Now(int year, int month)
	  {
	  DateTime firstWednesday= new DateTime(year, month, 1);
	  while (firstWednesday.DayOfWeek != DayOfWeek.Wednesday)
	  {
	  firstWednesday = firstWednesday.AddDays(1);
	  }
	  return firstWednesday.ToString();
	  }
  </msxsl:script>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now1(int year, int month)
		{
		DateTime thirdWednesday= new DateTime(year, month, 15);
		while (thirdWednesday.DayOfWeek != DayOfWeek.Wednesday)
		{
		thirdWednesday = thirdWednesday.AddDays(1);
		}
		return thirdWednesday.ToString();
		}
	</msxsl:script>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now2(int year, int month)
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

			  <xsl:variable name ="varExchange">
				  <xsl:value-of select ="translate(COL13,$varSmall,$varCapital)"/>
			  </xsl:variable>

            <xsl:variable name="varFutureAUECID">
				<xsl:choose>
					<xsl:when test ="COL13 = 'LME'">
						<xsl:value-of select ="95"/>
					</xsl:when>
					<xsl:when test ="COL13 = 'CME'">
						<xsl:value-of select ="105"/>
					</xsl:when>

					<xsl:when test ="COL13 = 'COMEX' or COL13 = 'Comex'">
						<xsl:value-of select ="85"/>
					</xsl:when>
					<xsl:when test ="COL13 = 'Eurex'">
						<xsl:value-of select ="113"/>
					</xsl:when>
					<xsl:when test ="COL13 = 'NYMEX' or COL13 = 'Nymex'">
						<xsl:value-of select ="84"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="0"/>
					</xsl:otherwise>
				</xsl:choose>
            </xsl:variable>

            <xsl:variable name="varFutureOptionAUECID">
				<xsl:choose>
					<xsl:when test ="COL13 = 'Nymex' or COL13 = 'COMEX' or COL13 = 'Comex'">
						<xsl:value-of select ="19"/>
					</xsl:when>
					<xsl:when test ="COL13 = 'LME' ">
						<xsl:value-of select ="129"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select ="0"/>
				</xsl:otherwise>
				</xsl:choose>
            </xsl:variable>

            <xsl:variable name="varFutureExpirationDate">
              <xsl:value-of select="substring-after(COL24,' ')"/>
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

			  <xsl:variable name ="varFutureExpiry">
				  <xsl:if test ="$varAssetCategory = 'Future' or $varAssetCategory = 'future'">
					  <xsl:variable name="varMonthNo">
						  <xsl:choose>
							  <xsl:when test ="COL13 = 'LME'">
								  <xsl:value-of select ="substring(substring-after(COL24,' '),5,2)"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:call-template name="MonthNo">
									  <xsl:with-param name="varMonth" select="substring(COL24,3,1)"/>
								  </xsl:call-template>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:variable>
					  <xsl:variable name="varYearNo">
						  <xsl:choose>
							  <xsl:when test ="COL13 = 'LME'">
								  <xsl:value-of select ="substring(substring-after(COL24,' '),1,4)"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select ="concat('201',substring(COL24,4,1))"/>
							  </xsl:otherwise>
						  </xsl:choose>
					  </xsl:variable>
					  <xsl:choose>
						  <xsl:when test ="$varExchange = 'LME'">
							  <xsl:value-of select ="substring-before(my:Now1(number($varYearNo),number($varMonthNo)),' ')"/>
						  </xsl:when>
						  <xsl:when test ="$varExchange = 'CME'">
							  <xsl:value-of select ="substring-before(my:Now2(number($varYearNo),number($varMonthNo)),' ')"/>
						  </xsl:when>
						  <xsl:otherwise>
							  <xsl:value-of select ="concat(substring(COL31,3,2),'/',substring(COL31,1,2),'/',substring(COL31,5,4))"/>
						  </xsl:otherwise>
					  </xsl:choose>
				  </xsl:if>
			  </xsl:variable>

			  <xsl:variable name ="varFutDay">
				  <xsl:value-of select ="substring(substring-after($varFutureExpiry,'/'),1,2)"/>
			  </xsl:variable>

			  
            <xsl:variable name="varFutureSymbol">
              <xsl:choose>
                <xsl:when test="(normalize-space(COL8) = 'Future' or normalize-space(COL8) = 'future') and COL13 = 'LME'">
                  <xsl:value-of select="concat(substring(COL24,3,3),' ', substring(COL24,11,1),$varMonthCode,substring(substring-after(COL24, ' '),7,2),'-LME')"/>
                </xsl:when>
                <xsl:when test="(normalize-space(COL8) = 'Future' or normalize-space(COL8) = 'future') and COL13 != 'LME'">
                  <xsl:value-of select="concat(substring(COL24,1,2),' ', substring(COL24,3,2))"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>

			  <xsl:variable name="varFutureUnderlyingSymbol">
				  <xsl:value-of select="$varFutureSymbol"/>
			  </xsl:variable>



            <xsl:variable name="varFutOptionExpiry">
              <xsl:if test="$varAssetCategory = 'Option'">
				  <xsl:variable name="varMonthNo">
					  <xsl:call-template name="MonthNo">
						  <xsl:with-param name="varMonth" select="substring(COL17,1,1)"/>
					  </xsl:call-template>
				  </xsl:variable>
				  <xsl:choose>
					  <xsl:when test ="$varExchange = 'LME'">
						  <xsl:value-of select ="substring-before(my:Now(number(concat('20',substring(COL17,2,2))),number($varMonthNo)),' ')"/>
					  </xsl:when>
					  <xsl:when test ="$varExchange = 'NYMEX'">
						  <xsl:value-of select ="substring-before(my:Now1(number(concat('20',substring(COL17,2,2))),number($varMonthNo)),' ')"/>
					  </xsl:when>
					  <xsl:when test ="$varExchange = 'COMEX'">
						  <xsl:value-of select ="concat(substring(COL31,3,2),'/',substring(COL31,1,2),'/',substring(COL31,5,4))"/>
					  </xsl:when>
				  
				  </xsl:choose>
              </xsl:if>
            </xsl:variable>

			  <xsl:variable name="varFutureOptionUnderlyingSymbol">
				  <xsl:value-of select="concat(substring(COL24,1,2),' ',substring(COL24,3,2))"/>
			  </xsl:variable>

				  <xsl:variable name="varOptionDay">
					  <xsl:value-of select="concat('0',substring-before(substring-after($varFutOptionExpiry,'/'),'/'))"/>
				  </xsl:variable>

				  <xsl:variable name="varOptionMonth">
					  <xsl:value-of select="substring-before($varFutOptionExpiry,'/')"/>
				  </xsl:variable>

				  <xsl:variable name="varOptionYear">
					  <xsl:value-of select="substring-after(substring-after($varFutOptionExpiry,'/'),'/')"/>
				  </xsl:variable>




			  <xsl:variable name="varFutureOption">
				  <xsl:choose>
					  <xsl:when test="normalize-space(COL8) = 'Option' and COL13 = 'LME'">
						  <xsl:value-of select="translate(concat(COL11,' ',substring(COL24,4,1),substring(COL24,3,1),$varOptionDay,substring(COL24,5,1),substring-after(COL24,' '),'-LME'),$varSmall,$varCapital)"/>
					  </xsl:when>
					  <xsl:when test="normalize-space(COL8) = 'Option' and COL13 != 'LME'">
						  <xsl:value-of select="translate(concat(substring(COL24,1,2),' ',substring(COL24,3,2),substring(COL24,5,1),substring-after(COL24,' ')),$varSmall,$varCapital)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


			  <xsl:choose>
				  <!--Handling for Equity Options-->
				  <xsl:when test="$varAssetCategory = 'Option'">

					  <xsl:variable name ="thirdWednesday">
						  <xsl:value-of select ="my:Now1(number($varOptionYear), number($varOptionMonth))"/>
					  </xsl:variable>

					  <xsl:variable name ="thirdWednesdayDate">
						  <xsl:value-of select ="substring-before(substring-after($thirdWednesday,'/'),'/')"/>
					  </xsl:variable>
					  <TickerSymbol>
						  <xsl:value-of select="$varFutureOption"/>
					  </TickerSymbol>


					  <ExpirationDate>
						  <xsl:value-of select="$varFutOptionExpiry"/>
					  </ExpirationDate>

					  <UnderLyingSymbol>
						  <xsl:choose>
							  <xsl:when test ="COL13 != 'LME'">
								  <xsl:value-of select="translate($varFutureOptionUnderlyingSymbol,$varSmall,$varCapital)"/>
							  </xsl:when>
							  <xsl:otherwise>
								  <xsl:value-of select="translate(concat(substring($varFutureOption,1,6),$thirdWednesdayDate,'-LME'),$varSmall,$varCapital)"/>

								  <!--<xsl:choose>
								<xsl:when test ="string-length(substring-before(substring-after($varFutOptionExpiry,'/'),'/')) = 1">
									<xsl:value-of select="translate(concat(substring($varFutureOption,1,6),'0',substring-before(substring-after($varFutOptionExpiry,'/'),'/'),'-LME'),$varSmall,$varCapital)"/>

								</xsl:when>
								-->
								  <!--<xsl:value-of select ="$varFutureOption"/>-->
								  <!--
								<xsl:otherwise>
									<xsl:value-of select="translate(concat(substring($varFutureOption,1,6),substring-before(substring-after($varFutOptionExpiry,'/'),'/'),'-LME'),$varSmall,$varCapital)"/>
								</xsl:otherwise>
							</xsl:choose>-->
							  </xsl:otherwise>
						  </xsl:choose>
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
						  <xsl:value-of select ="$varFutureExpiry"/>
					  </ExpirationDate>

                <UnderLyingSymbol>
                  <xsl:value-of select="translate($varFutureUnderlyingSymbol,$varSmall,$varCapital)"/>
                </UnderLyingSymbol>

                <StrikePrice>
                  <xsl:value-of select="0"/>
                </StrikePrice>

                <PutOrCall>
                  <xsl:value-of select="2"/>
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
              <xsl:value-of select="translate($varBloombergSymbol,$varSmall,$varCapital)"/>
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