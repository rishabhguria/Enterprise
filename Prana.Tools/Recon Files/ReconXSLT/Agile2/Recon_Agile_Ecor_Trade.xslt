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
			<xsl:when  test ="$varMonth='01' or $varMonth='1'and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='02'  or $varMonth='2' and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='03' or $varMonth='3' and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='04' or $varMonth='4' and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='05'or $varMonth='5'  and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='06' or $varMonth='6' and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='07' or $varMonth='7' and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='08' or $varMonth='8' and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='09' or $varMonth='9' and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='10'  and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='11' and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='12'  and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='01'  or $varMonth='1' and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='02' or $varMonth='2'  and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='03'or $varMonth='3' and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='04' or $varMonth='4' and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='05' or $varMonth='5' and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='06' or $varMonth='6' and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='07' or $varMonth='7' and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='08' or $varMonth='8' and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='09'  or $varMonth='9' and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='10' and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='11' and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth='12' and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select="//Comparision">

				<xsl:if test ="number(COL18)">

					<Comparision>

						<xsl:variable name="varPB_Name">
							<xsl:value-of select="'BTIG'"/>
						</xsl:variable>

						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:value-of select="normalize-space(COL2)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$varPB_Name]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME!=''">
								<AccountName>
									<xsl:value-of select="$PRANA_FUND_NAME"/>
								</AccountName>
							</xsl:when>
							<xsl:otherwise>
								<AccountName>
									<xsl:value-of select="$PB_FUND_NAME"/>
								</AccountName>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:variable name = "PB_COMPANY" >
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPB_Name]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varUnderlyingSymbol">
							<xsl:choose>
								<xsl:when test ="COL53!=''">
									<xsl:value-of select ="normalize-space(COL7)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<!--substring(substring-after(substring-after(normalize-space(COL54),'/'),'/'),3,2)-->
						<xsl:variable name="varYear">
							<xsl:choose>
								<xsl:when test ="COL53!=''">
									<xsl:value-of select ="substring(COL54,8,2)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varMonth">
							<xsl:choose>
								<xsl:when test ="COL53!=''">
									<xsl:value-of select ="substring(COL54,1,1)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varPutCall">
							<xsl:choose>
								<xsl:when test ="COL53!=''">
									<xsl:value-of select ="normalize-space(COL53)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name ="varMonthCode">
							<xsl:call-template name ="MonthCode">
								<xsl:with-param name ="varMonth" select ="$varMonth"/>
								<xsl:with-param name ="varPutCall" select="$varPutCall"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name ="varStrikePrice">
							<xsl:choose>
								<xsl:when test ="COL53!=''">
									<xsl:value-of select ="format-number(substring-after(substring-after(substring-after(normalize-space(COL9),' '),' '),' '),'#.00')"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varDateCode">
							<xsl:choose>
								<xsl:when test ="COL53!=''">
									<xsl:value-of select ="substring(COL54,3,2)"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varDays">
							<xsl:choose>
								<xsl:when test="substring($varDateCode,1,1)='0'">
									<xsl:value-of select="substring($varDateCode,2,1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varDateCode"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<!--<xsl:variable name="varThirdFriday">
							<xsl:choose>
								<xsl:when test ="COL53!=''">
									<xsl:value-of select='my:Now(number(concat(20,$varYear)),number($varMonth))'/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>-->

						<xsl:variable name="varOptionSymbol">
							<xsl:choose>
								<xsl:when test ="COL53!=''">
									<!--<xsl:choose>
										<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varDateCode - 1)">
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,$varStrikePrice)"/>
										</xsl:when>
										<xsl:otherwise>-->
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,$varStrikePrice,'D',$varDays)"/>
										<!--</xsl:otherwise>
									</xsl:choose>-->
								</xsl:when>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<!--<xsl:value-of select="COL2"/>-->
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL!=''">
									<xsl:value-of select="$PRANA_SYMBOL"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="COL53!=''">
											<xsl:value-of select="$varOptionSymbol"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="COL7!=''">
													<xsl:value-of select="COL7"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="COL9"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<PBSymbol>
							<xsl:value-of select="COL9"/>
						</PBSymbol>

						<xsl:variable name="varQty">
							<xsl:value-of select="COL18"/>
						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varQty)">
									<xsl:value-of select="$varQty"/>
								</xsl:when>
								<!--<xsl:when test="$varQty &gt; 0">
									<xsl:value-of select="$varQty"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL51)"/>
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="$varSide='BUY'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$varSide='SELL'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:when test="$varSide='SELL SHORT'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>
								<xsl:when test="$varSide='COVER SHORT'">
									<xsl:value-of select="'Buy to Close'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="varAvgPrice">
							<xsl:value-of select="COL19"/>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$varAvgPrice &lt; 0">
									<xsl:value-of select="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test="$varAvgPrice &gt; 0">
									<xsl:value-of select="$varAvgPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>

						<TradeDate>
							<xsl:value-of select ="COL10"/>
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select="COL11"/>
						</SettlementDate>



						<xsl:variable name="varNotional">
						<xsl:value-of select="COL34"/>
						</xsl:variable>

						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="number($varNotional)">
									<xsl:value-of select="$varNotional"/>
								</xsl:when>
								<!--<xsl:when test="$varNotional &gt; 0">
									<xsl:value-of select="$varNotional"/>
								</xsl:when>-->
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>
						
						<xsl:variable name="varCommission">
							<xsl:value-of select="COL21"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &lt; 0">
									<xsl:value-of select="$varCommission *-1"/>
								</xsl:when>
								<xsl:when test="$varCommission &gt; 0">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="TotalCommissionandFees" select="number(COL21 + COL22 + COL36)"/>

						<TotalCommissionandFees>
							<xsl:choose>
								<xsl:when test="$TotalCommissionandFees &lt; 0">
									<xsl:value-of select="$TotalCommissionandFees *-1"/>
								</xsl:when>
								<xsl:when test="$TotalCommissionandFees &gt; 0">
									<xsl:value-of select="$TotalCommissionandFees"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalCommissionandFees>

						<xsl:variable name="varStampDuty">
							<xsl:value-of select="COL22"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>
								<xsl:when test="$varStampDuty &lt; 0">
									<xsl:value-of select="$varStampDuty *-1"/>
								</xsl:when>
								<xsl:when test="$varStampDuty &gt; 0">
									<xsl:value-of select="$varStampDuty"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>
						<xsl:variable name="varMiscFees">
							<xsl:value-of select="COL36"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>
								<xsl:when test="$varMiscFees &lt; 0">
									<xsl:value-of select="$varMiscFees *-1"/>
								</xsl:when>
								<xsl:when test="$varMiscFees &gt; 0">
									<xsl:value-of select="$varMiscFees"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Fees>


						<CompanyName>
							<xsl:value-of select="$PB_COMPANY"/>
						</CompanyName>

					</Comparision>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
