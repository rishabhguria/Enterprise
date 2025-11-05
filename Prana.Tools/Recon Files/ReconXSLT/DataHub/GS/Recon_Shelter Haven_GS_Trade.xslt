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

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:param name="varPutCall"/>

		<!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
		<xsl:choose>
			<xsl:when test ="$varMonth = 1 and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=2 and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 3 and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 4 and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 5 and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 6 and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 7 and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=8 and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 9 and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 10 and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 11 and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 12 and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 1 and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 2 and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 3 and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 4 and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 5 and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth =6 and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 7 and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 8 and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 9 and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 10 and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 11 and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth = 12 and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GetMonth">
		<xsl:param name="varMonth"/>

		<!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
		<xsl:choose>
			<xsl:when test ="$varMonth='Jan'">
				<xsl:value-of select ="1"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Feb'">
				<xsl:value-of select ="2"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Mar' ">
				<xsl:value-of select ="3"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Apr'">
				<xsl:value-of select ="4"/>
			</xsl:when>
			<xsl:when test ="$varMonth='May'">
				<xsl:value-of select ="5"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Jun'">
				<xsl:value-of select ="6"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Jul'">
				<xsl:value-of select ="7"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Aug'">
				<xsl:value-of select ="8"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Sep' ">
				<xsl:value-of select ="9"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Oct'">
				<xsl:value-of select ="10"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Nov' ">
				<xsl:value-of select ="11"/>
			</xsl:when>
			<xsl:when test ="$varMonth='Dec'">
				<xsl:value-of select ="12"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

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
			<xsl:when test="$Suffix = 'FP'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'NA'">
				<xsl:value-of select="'-EEB'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'BZ'">
				<xsl:value-of select="'-BSP'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'LN'">
				<xsl:value-of select="'-LON'"/>
			</xsl:when>
			<xsl:when test="$Suffix = 'JP'">
				<xsl:value-of select="'-TSE'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="ConvertBBCodetoTicker">
		<xsl:param name="varBBCode"/>

		<xsl:variable name="varRoot">
			<xsl:value-of select="substring-before($varBBCode,' ')"/>
		</xsl:variable>

		<xsl:variable name="varExYear">
			<xsl:value-of select="substring(substring-after($varBBCode,' '),7,2)"/>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:value-of select="substring(substring-after(substring-after($varBBCode,' '),' '),1,1)"/>
		</xsl:variable>

		<xsl:variable name="varStrike">
			<xsl:value-of select="format-number(substring(substring-after(substring-after($varBBCode,' '),' '),2),'#.00')"/>
		</xsl:variable>

		<xsl:variable name="varExDay">
			<xsl:value-of select="substring(substring-after($varBBCode,' '),4,2)"/>
		</xsl:variable>

		<xsl:variable name="varExpiryDay">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)= '0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varExMonth">
			<xsl:value-of select="substring(substring-after($varBBCode,' '),1,2)"/>
		</xsl:variable>

		<xsl:variable name="varMonthCode">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="varMonth" select="$varExMonth"/>
				<xsl:with-param name="varPutCall" select="$varPutCall"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name='varThirdFriday'>
			<xsl:value-of select='my:Now(number(concat("20",$varExYear)),number($varExMonth))'/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
				<xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike))"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike,'D',$varExpiryDay))"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>


	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL13"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="varCostBasis">
					<xsl:value-of select="COL14"/>
				</xsl:variable>


				<xsl:choose>
					<xsl:when test="number($Quantity)">
						<PositionMaster>

							<xsl:variable name="varPBName">
								<xsl:value-of select="'Goldman Sachs'"/>
							</xsl:variable>

							<xsl:variable name = "PB_FUND_NAME">
								<xsl:value-of select="COL2"/>
							</xsl:variable>
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>

							<xsl:variable name="PB_Symbol">
								<xsl:value-of select = "COL8"/>
							</xsl:variable>
							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
							</xsl:variable>
							<CompanyName>
								<xsl:value-of select="$PB_Symbol"/>
							</CompanyName>

							<xsl:variable name="varNetPosition">
								<xsl:value-of select="COL13"/>
							</xsl:variable>



							<xsl:variable name="varCommission">
								<xsl:value-of select="COL16"/>
							</xsl:variable>

							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME=''">
									<FundName>
										<xsl:value-of select='$PB_FUND_NAME'/>
									</FundName>
								</xsl:when>
								<xsl:otherwise>
									<FundName>
										<xsl:value-of select='$PRANA_FUND_NAME'/>
									</FundName>
								</xsl:otherwise>
							</xsl:choose>

							<Symbol>
								<xsl:choose>
									<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
										<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
									</xsl:when>
									<xsl:when test="string-length(COL8) = 21">
										<xsl:value-of select="''"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="COL8"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>

							<IDCOOptionSymbol>
								<xsl:choose>
									<xsl:when test="string-length(COL8) = 21">
										<xsl:value-of select="concat(COL8, 'U')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="''"/>
									</xsl:otherwise>
								</xsl:choose>
							</IDCOOptionSymbol>


							<AvgPx>
								<xsl:choose>
									<xsl:when test ="boolean(number($varCostBasis))">
										<xsl:value-of select="$varCostBasis"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</AvgPx>


							<!--QUANTITY-->

							<xsl:choose>
								<xsl:when test="$varNetPosition &lt; 0">
									<Quantity>
										<xsl:value-of select="$varNetPosition * (-1)"/>
									</Quantity>
								</xsl:when>
								<xsl:when test="$varNetPosition &gt; 0">
									<Quantity>
										<xsl:value-of select="$varNetPosition"/>
									</Quantity>
								</xsl:when>
								<xsl:otherwise>
									<Quantity>
										<xsl:value-of select="0"/>
									</Quantity>
								</xsl:otherwise>
							</xsl:choose>

							<Side>
								<xsl:choose>
									<xsl:when test="COL11 = 'BUY TO COVER'">
										<xsl:value-of select="'Buy to Close'"/>
									</xsl:when>
									<xsl:when test="COL11 = 'SELL'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>
									<xsl:when test="COL11 = 'SELL TO OPEN'">
										<xsl:value-of select="'Sell to Open'"/>
									</xsl:when>
									<xsl:when test="COL11 = 'SHORT SELL'">
										<xsl:value-of select="'Sell Short'"/>
									</xsl:when>
									<xsl:when test="COL11 = 'SELL TO CLOSE'">
										<xsl:value-of select="'Sell to Close'"/>
									</xsl:when>
									<xsl:when test="COL11 = 'BUY TO OPEN'">
										<xsl:value-of select="'Buy to Open'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL11"/>
									</xsl:otherwise>
								</xsl:choose>
							</Side>

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
							<TotalCommissionandFees>
								<xsl:value-of select="0"/>
							</TotalCommissionandFees>

							<NetNotionalValue>
								<xsl:choose>
									<xsl:when test="COL17 &gt; 0">
										<xsl:value-of select="COL17"/>
									</xsl:when>
									<xsl:when test="COL17 &lt; 0">
										<xsl:value-of select="COL17*(-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</NetNotionalValue>

							<TradeDate>
								<xsl:value-of select="normalize-space(COL9)"/>
							</TradeDate>
							<GrossNotionalValue>
								<xsl:value-of select="0"/>
							</GrossNotionalValue>

							<NetNotionalValueBase>
								<xsl:value-of select="0"/>
							</NetNotionalValueBase>

							<SMRequest>
								<xsl:value-of select="'TRUE'"/>
							</SMRequest>

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

							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>

							

							<GrossNotionalValue>
								<xsl:value-of select="0"/>
							</GrossNotionalValue>

							<TotalCommissionandFees>
								<xsl:value-of select="0"/>
							</TotalCommissionandFees>
							
							<NetNotionalValueBase>
								<xsl:value-of select="0"/>
							</NetNotionalValueBase>


							<Commission>
								<xsl:value-of select="0"/>
							</Commission>



							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>



							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>

							<OriginalPurchaseDate>
								<xsl:value-of select="''"/>
							</OriginalPurchaseDate>



							<CurrencySymbol>
								<xsl:value-of select="''"/>
							</CurrencySymbol>

							<Side>
								<xsl:value-of select="''"/>
							</Side>

							<PBSymbol>
								<xsl:value-of select="''"/>
							</PBSymbol>


							<SMRequest>
								<xsl:value-of select="''"/>
							</SMRequest>

						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>

			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


