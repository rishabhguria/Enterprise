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

	<xsl:template name ="MonthCode">
		<xsl:param name ="varMonth"/>
		<xsl:param name ="varPutCall"/>
		<xsl:choose>
			<xsl:when test ="$varMonth=1 and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=2 and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=3 and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=4 and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=5 and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=6 and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=7 and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=8 and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=9 and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test =" $varMonth=10 and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=11 and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=12 and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=1 and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=2 and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=3 and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=4 and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=5 and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=6 and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=7 and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=8 and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=9 and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=10 and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=11 and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth=12 and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="varSymbol"/>

		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:value-of select ="substring-before($varSymbol,' ')"/>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:value-of select ="substring-before(substring-after(substring-after(COL6,' '),' '),'/')"/>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:value-of select ="substring-before(substring-after($varSymbol,' '),' ')"/>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:value-of select ="substring-before(substring-after(substring-after(COL6,'/'),'/'),' ')"/>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:value-of select ="substring(substring-after(substring-after($varSymbol,' '),' '),1,1)"/>
		</xsl:variable>

		<xsl:variable name ="varStrikePrice">
			<xsl:value-of select ="substring-after(substring-after($varSymbol,' '),$varPutCall)"/>
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

		<xsl:variable name="varThirdFriday">

			<xsl:choose>
				<xsl:when test="number($varYear) and number($varMonth)">
					<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
				</xsl:when>
			</xsl:choose>

		</xsl:variable>

		<!--<xsl:value-of select="concat($varUnderlyingSymbol,number(number($varExDay)-1),substring(substring-after($varThirdFriday,'/'),1,2))"/>-->

		<xsl:choose>
			<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
				<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position" select="COL9"/>

				<xsl:variable name="Asset" select="normalize-space(COL4)"/>

				<xsl:if test="number($Position) and $Asset!='Cash'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'UBS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME">
							<xsl:value-of select ="normalize-space(COL6)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" select ="normalize-space(COL5)"/>

						<xsl:variable name="OptionSymbol">
							<xsl:choose>
								<xsl:when test="$Asset='Option'">
									<xsl:call-template name="Option">
										<xsl:with-param name="varSymbol" select="$Symbol"/>
									</xsl:call-template>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Ticker">
							<xsl:choose>
								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="substring-before($Symbol,' ')"/>
								</xsl:when>
								<xsl:when test="$Asset='Option'">
									<xsl:value-of select="$OptionSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL5"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Ticker!='' or $Ticker!='*'">
									<xsl:value-of select="$Ticker"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<xsl:variable name="Side" select="normalize-space(COL7)"/>

						<Side>
							<xsl:choose>

								<xsl:when test="$Asset='Option'">
									<xsl:choose>

										<xsl:when test="$Side='Buy'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>

										<xsl:when test="$Side='Sale'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>

										<xsl:when test="$Side='Short Sale'">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>

										<xsl:when test="$Side='Cover Short'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test="$Side='Buy'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>

										<xsl:when test="$Side='Sale'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>

										<xsl:when test="$Side='Short Sale'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>

										<xsl:when test="$Side='Cover Short'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>

							</xsl:choose>
						</Side>

						<Quantity>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPX" select="number(COL10)"/>

						<AvgPX>
							<xsl:choose>

								<xsl:when test="$AvgPX &gt; 0">
									<xsl:value-of select="$AvgPX"/>
								</xsl:when>

								<xsl:when test="$AvgPX &lt; 0">
									<xsl:value-of select="$AvgPX * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>

						<TradeDate>
							<xsl:value-of select="COL2"/>
						</TradeDate>

						<xsl:variable name="NetNotionalValue" select="number(COL13)"/>

						<NetNotionalValue>
							<xsl:choose>

								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>

								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name="NetNotionalValueBase" select="number(COL19)"/>

						<NetNotionalValueBase>
							<xsl:choose>

								<xsl:when test="$NetNotionalValueBase &gt; 0">
									<xsl:value-of select="$NetNotionalValueBase"/>
								</xsl:when>

								<xsl:when test="$NetNotionalValueBase &lt; 0">
									<xsl:value-of select="$NetNotionalValueBase * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetNotionalValueBase>

						<xsl:variable name="Commission" select="number(COL11)"/>

						<Commission>
							<xsl:choose>

								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>

								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>

						<xsl:variable name="StampDuty" select="number(COL12)"/>

						<StampDuty>
							<xsl:choose>

								<xsl:when test="$StampDuty &gt; 0">
									<xsl:value-of select="$StampDuty"/>
								</xsl:when>

								<xsl:when test="$StampDuty &lt; 0">
									<xsl:value-of select="$StampDuty * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</StampDuty>

						<xsl:variable name="TotalCommissionandFees" select="number(COL11+COL12)"/>

						<TotalCommissionandFees>
							<xsl:choose>

								<xsl:when test="$TotalCommissionandFees &gt; 0">
									<xsl:value-of select="$TotalCommissionandFees"/>
								</xsl:when>

								<xsl:when test="$TotalCommissionandFees &lt; 0">
									<xsl:value-of select="$TotalCommissionandFees * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</TotalCommissionandFees>

						<xsl:variable name="FXRate" select="number(COL21)"/>

						<FXRate>
							<xsl:choose>

								<xsl:when test="$FXRate &gt; 0">
									<xsl:value-of select="$FXRate"/>
								</xsl:when>

								<xsl:when test="$FXRate &lt; 0">
									<xsl:value-of select="$FXRate * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</FXRate>

						<xsl:variable name="PB_COUNTERPARTY_NAME" select="normalize-space(COL14)"/>

						<xsl:variable name="PRANA_COUNTERPARTY_NAME">
							<xsl:value-of select="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PBBroker=$PB_COUNTERPARTY_NAME]/@PranaBroker"/>
						</xsl:variable>

						<CounterParty>
							<xsl:choose>
								<xsl:when test="$PRANA_COUNTERPARTY_NAME!=''">
									<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_COUNTERPARTY_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterParty>

						<CurrencySymbol>
							<xsl:value-of select="COL22"/>
						</CurrencySymbol>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>