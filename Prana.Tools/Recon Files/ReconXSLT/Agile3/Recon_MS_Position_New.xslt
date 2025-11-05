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

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>

		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
				</xsl:when>
				<xsl:when test="$Month=6">
					<xsl:value-of select="'JUN'"/>
				</xsl:when>
				<xsl:when test="$Month=7">
					<xsl:value-of select="'JUL'"/>
				</xsl:when>
				<xsl:when test="$Month=8">
					<xsl:value-of select="'AUG'"/>
				</xsl:when>
				<xsl:when test="$Month=9">
					<xsl:value-of select="'SEP'"/>
				</xsl:when>
				<xsl:when test="$Month=10">
					<xsl:value-of select="'OCT'"/>
				</xsl:when>
				<xsl:when test="$Month=11">
					<xsl:value-of select="'NOV'"/>
				</xsl:when>
				<xsl:when test="$Month=12">
					<xsl:value-of select="'DEC'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:template>

	<xsl:template name ="MonthCode">
		<xsl:param name ="varMonth"/>
		<xsl:param name ="varPutCall"/>
		<xsl:choose>

			<xsl:when test ="$varMonth='01' or $varMonth='1' and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='02' or $varMonth='2' and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='03'or $varMonth='3' and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='04' or $varMonth='4' and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='05' or $varMonth='5' and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='06' or $varMonth='6' and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='07' or $varMonth='7' and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='08' or $varMonth='8' and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='09' or $varMonth='9' and $varPutCall='C'">
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

			<xsl:when test ="$varMonth='01' or $varMonth='1'and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='02' or $varMonth='2' and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='03' or $varMonth='3' and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='04' or $varMonth='4' and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='05'or $varMonth='5' and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='06' or $varMonth='6' and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='07' or $varMonth='7' and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='08' or $varMonth='8' and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='09' or $varMonth='9' and $varPutCall='P'">
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

	<xsl:template name="Option">
		<xsl:param name="VarSymbol"/>
		<xsl:param name="varExDate"/>


		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:choose>
				<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD'">
					<xsl:value-of select ="normalize-space($VarSymbol)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:choose>
				<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD'">
					<xsl:value-of select ="substring(substring-after(substring-after($varExDate,'/'),'/'),3,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:choose>
				<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD'">
					<xsl:value-of select ="substring-before($varExDate,'/')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:choose>
				<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD'">
					<xsl:value-of select ="substring-before(substring-after($varExDate,'/'),'/')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:choose>
				<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD'">
					<xsl:value-of select ="substring(COL50,1,1)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varStrikePrice">
			<xsl:choose>

				<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD'">
					<xsl:value-of select ="format-number(number(COL56),'#.00')"/>
				</xsl:when>

			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varMonthCode">
			<xsl:call-template name ="MonthCode">
				<xsl:with-param name ="varMonth" select ="number($varMonth)"/>
				<xsl:with-param name ="varPutCall" select="$varPutCall"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="varDays">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)='0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<!--<xsl:variable name="varThirdFriday">
			<xsl:choose>
				<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD' and number($varMonth) and number($varYear)">
					<xsl:value-of select='my:Now(number(concat(20,$varYear)),number($varMonth))'/>
				</xsl:when>
			</xsl:choose>

		</xsl:variable>-->


		<xsl:choose>
			<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD'">
				<!--<xsl:choose>
					<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
					</xsl:when>
					<xsl:otherwise>-->
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
					<!--</xsl:otherwise>
				</xsl:choose>-->
			</xsl:when>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//Comparision">
				<xsl:variable name="varPB_Name">
					<xsl:value-of select="'MS'"/>
				</xsl:variable>

				<xsl:if test ="number(COL27) and contains(normalize-space(COL50),'CASH')!='true' or contains(normalize-space(COL51),'MONEY')='true'">

					<PositionMaster>
						<!--   Fund -->
						<!--fundname section-->

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name= $varPB_Name]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>




						<!--<xsl:variable name="PB_CountnerParty" select="COL5"/>
						<xsl:variable name="PRANA_CounterPartyID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= $varPB_Name]/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
						</xsl:variable>-->


						<xsl:variable name="varPBSymbol">
							<xsl:value-of select="COL14"/>
						</xsl:variable>

						<xsl:variable name="varDescription">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varNetPosition">
							<xsl:value-of select="COL27"/>
						</xsl:variable>

						<xsl:variable name="varCostBasis">
							<xsl:value-of select="COL30"/>
						</xsl:variable>

						<xsl:variable name="varFXConversionMethodOperator">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varFXRate">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varCommission">
							<xsl:value-of select="COL12"/>
						</xsl:variable>

						<xsl:variable name="varFees">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varMiscFees">
							<xsl:value-of select="COL16"/>
						</xsl:variable>

						<xsl:variable name="varClearingFee">
							<xsl:value-of select="COL22"/>
						</xsl:variable>

						<xsl:variable name="varStampDuty">
							<xsl:value-of select="COL23"/>
						</xsl:variable>
						

						<xsl:variable name="varMarketValue">
							<xsl:choose>
								<xsl:when test="COL51 ='EQUITY SWAP'">
							
									<xsl:value-of select="COL34"/>
					
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL34"/>
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


						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:choose>
								<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL44!='USD'">
									<xsl:value-of select ="normalize-space(COL6)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL14"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PBSuffixCode">
							<xsl:value-of select = "substring-after(COL14, ' ')"/>
						</xsl:variable>

						<!--<xsl:variable name="PB_ExchangeCODE">
							<xsl:value-of select="substring-after(substring-before(COL5, ' Equity'), ' ')"/>
						</xsl:variable>-->

						<xsl:variable name="PRANA_Exchange">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name='MS']/SymbolData[@PBSuffixCode=$PBSuffixCode]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varOption">
							<xsl:call-template name ="Option">
								<xsl:with-param name ="VarSymbol" select ="COL19"/>
								<xsl:with-param name ="varExDate" select="COL55"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Month">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(COL86,'/')"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="ExpDate">
							<xsl:value-of select="concat('0',substring-before(substring-after(COL86,'/'),'/'),$Month,substring-after(substring-after(COL86,'/'),'/'))"/>
						</xsl:variable>


						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="(contains(COL50, 'PUT') or contains(COL50, 'CALL'))  and COL45='USD'">
									<xsl:value-of select="$varOption"/>
								</xsl:when>

								<xsl:when test="contains(COL52, 'FX')">
									<xsl:value-of select="concat(substring-before(COL6,'/'),'-',substring-before(substring-after(COL6,'/'),' '),' ',$ExpDate)"/>
								</xsl:when>


								<xsl:when test="$PBSuffixCode='UN' or $PBSuffixCode='US'">
									<xsl:value-of select="substring-before(COL14,' ')"/>
								</xsl:when>

								<xsl:when test="$PRANA_Exchange!=''">
									<xsl:value-of select="concat(substring-before(COL14,' '),$PRANA_Exchange)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<Side>
							<xsl:choose>
								<xsl:when test="COL29 = 'L'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="COL29 = 'S'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<!--QUANTITY-->

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varNetPosition)">
									<xsl:value-of select="$varNetPosition"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<ISINSymbol>
							<xsl:value-of select ="COL10"/>
						</ISINSymbol>


						<MarkPrice>
							<xsl:choose>
								<xsl:when test ="number($varCostBasis) &gt; 0">
									<xsl:value-of select="$varCostBasis"/>
								</xsl:when>
								<xsl:when test ="number($varCostBasis) &lt; 0">
									<xsl:value-of select="$varCostBasis*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<CurrencySymbol>
							<xsl:value-of select ="COL44"/>
						</CurrencySymbol>

						<MarketValue>
							<xsl:choose>
								<xsl:when test ="boolean(number($varMarketValue))">
									<xsl:value-of select="$varMarketValue"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test ="boolean(number(COL35))">
									<xsl:value-of select="COL35"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>

</xsl:stylesheet>
