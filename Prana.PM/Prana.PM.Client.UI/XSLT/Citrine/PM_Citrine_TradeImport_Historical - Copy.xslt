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
              <xsl:value-of select="'JPM'"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL7"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

			  <xsl:variable name ="varExchange">
				  <xsl:value-of select ="translate(COL13,$varSmall,$varCapital)"/>
			  </xsl:variable>


			  <xsl:variable name="varEquityOptionAUECID">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="COL4"/>
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

			  <xsl:variable name="varNetPosition">
              <xsl:value-of select="translate(translate(COL16,'(',''),')','')"/>
            </xsl:variable>

            <xsl:variable name="varSide">
              <xsl:value-of select="translate(normalize-space(COL14),$varSmall,$varCapital)"/>
            </xsl:variable>

            

            <xsl:variable name="varDescription">
              <xsl:value-of select="COL23"/>
            </xsl:variable>

			  <xsl:variable name ="varCommission">
				  <xsl:value-of select="COL41"/>
			  </xsl:variable>

			  <xsl:variable name ="varFees">
				  <xsl:value-of select="COL42"/>
			  </xsl:variable>

			  <xsl:variable name ="varExBroker">
				  <xsl:value-of select="COL21"/>
			  </xsl:variable>

			  <xsl:variable name ="varRoot">
				  <xsl:value-of select ="COL11"/>	
			  </xsl:variable>
			  
			  <xsl:variable name="PRANA_Multiplier">
				  <xsl:value-of select="document('../ReconMappingXml/PriceMulMapping.xml')/PriceMulMapping/PB[@Name='JPM']/MultiplierData[@PranaRoot=$varRoot]/@Multiplier"/>
			  </xsl:variable>

			  <xsl:variable name="varCostBasis">
				  <xsl:choose>
					  <xsl:when test ="number($PRANA_Multiplier)">
						  <xsl:value-of select ="$PRANA_Multiplier*COL20"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="COL20"/>
					  </xsl:otherwise>
				  </xsl:choose>
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

            <xsl:choose>
              <!--Handling for Equity Options-->
              <xsl:when test="$varAssetCategory = 'Option'">

                <Symbol>
                  <xsl:value-of select="translate($varFutureOption,$varSmall,$varCapital)"/>
                </Symbol>
              </xsl:when>

              <!--Handling for Futures-->
              <xsl:otherwise>
                
                <Symbol>
                  <xsl:value-of select="translate($varFutureSymbol,$varSmall,$varCapital)"/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose>

			  <xsl:choose>
				  <xsl:when test ="$varAssetCategory = 'Option' and $varExchange = 'LME'">
					  <xsl:variable name ="thirdWednesday">
						  <xsl:value-of select ="my:Now1(number($varOptionYear), number($varOptionMonth))"/>
					  </xsl:variable>

					  <xsl:variable name ="thirdWednesdayDate">
						  <xsl:value-of select ="substring-before(substring-after($thirdWednesday,'/'),'/')"/>
					  </xsl:variable>
					  <PBSymbol>
						  <xsl:value-of select="translate(concat(substring($varFutureOption,1,6),$thirdWednesdayDate,'-LME'),$varSmall,$varCapital)"/>
					  </PBSymbol>
				  </xsl:when>
				  <xsl:when test ="$varAssetCategory = 'Option' and $varExchange != 'LME'">
					  <PBSymbol>
						  <xsl:value-of select ="$varFutureOptionUnderlyingSymbol"/>
					  </PBSymbol>
				  </xsl:when>
				  <xsl:otherwise>
					  <PBSymbol>
						  <xsl:value-of select ="$varFutureUnderlyingSymbol"/>
					  </PBSymbol>
				  </xsl:otherwise>
			  </xsl:choose>

			  <Bloomberg>
				  <xsl:value-of select ="$varBloombergSymbol"/>
			  </Bloomberg>

			  <xsl:choose>
				  <xsl:when test ="$varAssetCategory = 'Option' and $varExchange = 'LME'">
					  <xsl:variable name ="thirdWednesday">
						  <xsl:value-of select ="my:Now1(number($varOptionYear), number($varOptionMonth))"/>
					  </xsl:variable>

					  <xsl:variable name ="thirdWednesdayDate">
						  <xsl:value-of select ="substring-before(substring-after($thirdWednesday,'/'),'/')"/>
					  </xsl:variable>
					  <UnderlyingSymbol>
						  <xsl:value-of select="translate(concat(substring($varFutureOption,1,6),$thirdWednesdayDate,'-LME'),$varSmall,$varCapital)"/>
					  </UnderlyingSymbol>
				  </xsl:when>
				  <xsl:when test ="$varAssetCategory = 'Option' and $varExchange != 'LME'">
					  <UnderlyingSymbol>
						  <xsl:value-of select ="$varFutureOptionUnderlyingSymbol"/>
					  </UnderlyingSymbol>
				  </xsl:when>
				  <xsl:otherwise>
					  <UnderlyingSymbol>
						  <xsl:value-of select ="$varFutureUnderlyingSymbol"/>
					  </UnderlyingSymbol>
				  </xsl:otherwise>
			  </xsl:choose>


			  <ExpirationDate>
				  <xsl:choose>
					  <xsl:when test ="$varExchange = 'LME' and $varAssetCategory = 'Option'">
						  <xsl:value-of select ="$varFutOptionExpiry"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </ExpirationDate>

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

            <SideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide = 'B'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'S'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <!--<xsl:when test="$varSide = 'SS'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'BC'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'BO'">
                  <xsl:value-of select="'A'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'SO'">
                  <xsl:value-of select="'C'"/>
                </xsl:when>
                <xsl:when test="$varSide = 'SC'">
                  <xsl:value-of select="'D'"/>
                </xsl:when>-->
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

			  <CounterPartyID>
				  <xsl:choose>
					  <xsl:when test ="$varExBroker = 'Jefferies Bache'">
						  <xsl:value-of select ="17"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Deutsche'">
						  <xsl:value-of select ="47"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Citigroup'">
						  <xsl:value-of select ="22"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Macquarie'">
						  <xsl:value-of select ="48"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'JPM / Sempra'">
						  <xsl:value-of select ="23"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'ICAP'">
						  <xsl:value-of select ="11"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'BNP Paribas'">
						  <xsl:value-of select ="16"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'BofA Merrill'">
						  <xsl:value-of select ="19"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Goldman Sachs '">
						  <xsl:value-of select ="56"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Tower (ABN)'">
						  <xsl:value-of select ="24"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Barclays'">
						  <xsl:value-of select ="13"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Standard Bank'">
						  <xsl:value-of select ="15"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Newedge'">
						  <xsl:value-of select ="57"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'XCONNECT'">
						  <xsl:value-of select ="96"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'INTL FCStone'">
						  <xsl:value-of select ="14"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Delphi (Marex) '">
						  <xsl:value-of select ="26"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'EXEROPT'">
						  <xsl:value-of select ="53"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'ED&amp;F Mann'">
						  <xsl:value-of select ="49"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'MBFCC'">
						  <xsl:value-of select ="97"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Koch'">
						  <xsl:value-of select ="105"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Tower'">
						  <xsl:value-of select ="106"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Sucden'">
						  <xsl:value-of select ="18"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Tullett Prebon'">
						  <xsl:value-of select ="46"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'RBC'">
						  <xsl:value-of select ="7"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Mitsui'">
						  <xsl:value-of select ="50"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'ED&amp;F'">
						  <xsl:value-of select ="107"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Marex'">
						  <xsl:value-of select ="98"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Exercised'">
						  <xsl:value-of select ="51"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Goldman'">
						  <xsl:value-of select ="109"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'UBS'">
						  <xsl:value-of select ="108"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'xconnect'">
						  <xsl:value-of select ="96"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'RJ Obrian'">
						  <xsl:value-of select ="110"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'RJObrien'">
						  <xsl:value-of select ="111"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Capfeather'">
						  <xsl:value-of select ="112"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'BNP'">
						  <xsl:value-of select ="114"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'JPM / F&amp;O'">
						  <xsl:value-of select ="20"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Morgan Stanley'">
						  <xsl:value-of select ="52"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Soc Gen'">
						  <xsl:value-of select ="12"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Tradition'">
						  <xsl:value-of select ="113"/>
					  </xsl:when>
					  <xsl:when test ="$varExBroker = 'Sunrise/ABN'">
						  <xsl:value-of select ="118"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </CounterPartyID>

			  <TradeAttribute1>
				  <xsl:value-of select ="COL15"/>
			  </TradeAttribute1>

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

            <Description>
              <xsl:value-of select="COL23"/>
            </Description>

            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

			  <Commission>
				  <xsl:choose>
					  <xsl:when test="$varCommission &gt; 0">
						  <xsl:value-of select ="$varCommission"/>
					  </xsl:when>
					  <xsl:when test="$varCommission &lt; 0">
						  <xsl:value-of select ="$varCommission*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Commission>

			  <Fees>
				  <xsl:choose>
					  <xsl:when test="$varFees &gt; 0">
						  <xsl:value-of select ="$varFees"/>
					  </xsl:when>
					  <xsl:when test="$varFees &lt; 0">
						  <xsl:value-of select ="$varFees*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select ="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Fees>

			  <SMMappingReq>
				  <xsl:value-of select="'SecMasterMapping.xml'"/>
			  </SMMappingReq>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
  </xsl:stylesheet>