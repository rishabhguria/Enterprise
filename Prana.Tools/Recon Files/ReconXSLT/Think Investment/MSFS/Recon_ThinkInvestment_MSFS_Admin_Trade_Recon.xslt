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
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test=" $Month=6">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL45,Options)">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(substring-after(substring-after(COL3,'-'),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL62,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(COL62,'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-after(substring-after(COL62,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(COL3,'-'),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(COL65,'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>

	<xsl:template name="Date">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month='JAN'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month='FEB'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month='MAR'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month='APR'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month='MAY'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month='JUN'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month='JUL'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month='AUG'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month='SEP'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month='OCT'">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$Month='NOV'">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$Month='DEC'">
				<xsl:value-of select="12"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="FutureCode">
		<xsl:param name="FMonth"/>
		<xsl:choose>
			<xsl:when test="$FMonth='JAN'">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$FMonth='FEB'">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$FMonth='MAR'">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$FMonth='APR'">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$FMonth='MAY'">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$FMonth='JUN'">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$FMonth='JUL'">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$FMonth='AUG'">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$FMonth='SEP'">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$FMonth='OCT'">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$FMonth='NOV'">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$FMonth='DEC'">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>



	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL14"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity) and COL45 !='Cash'">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL45,'Options')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>

								<xsl:when test="COL45='Equities' and COL58='Equity Swap'">
									<xsl:value-of select="'EquitySwap'"/>
								</xsl:when>

								<xsl:when test="COL45='Equities'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="contains(COL45,'Futures')">
									<xsl:value-of select="'FUTURE'"/>
								</xsl:when>

								<xsl:when test="COL45='FX Forward' and COL58='FX Spot'">
									<xsl:value-of select="'FxSpot'"/>
								</xsl:when>

								<xsl:when test="contains(COL45,'FX Forward')">
									<xsl:value-of select="'FxForward'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Asset>
							<xsl:value-of select="$Asset"/>
						</Asset>

						<xsl:variable name ="varFXMonths">
							<xsl:call-template name="Date">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL3,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varFXDay">
							<xsl:value-of select="substring-before(substring-after(substring-after(COL3,':'),' '),'-')"/>
						</xsl:variable>

						<xsl:variable name="varFXYear">
							<xsl:value-of select="substring(substring-after(substring-after(COL3,'-'),'-'),3,2)"/>
						</xsl:variable>


						<xsl:variable name="varFXForward">
							<xsl:choose>
								<xsl:when test="COL36='EUR' or COL36='GBP' or COL36='NZD'">
									<xsl:value-of select="concat(COL36,'/','USD',' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',COL36,' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFXSpot">
							<xsl:choose>
								<xsl:when test="COL36='EUR' or COL36='GBP' or COL36='NZD' or COL36='AUD'">
									<xsl:value-of select="concat(COL36,'/','USD')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',COL36)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name = "PB_SUFFIX_NAME" >
							<xsl:value-of select ="substring-after(COL6,'.')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varFutureSymbol">
							<xsl:value-of select="substring-before(substring-after(normalize-space(COL3),' '),' ')"/>
						</xsl:variable>

						<xsl:variable name="varFutureYear">
							<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),5,1)"/>
						</xsl:variable>

						<xsl:variable name ="varFutureMonthCode">
							<xsl:call-template name="FutureCode">
								<xsl:with-param name="FMonth" select="substring(substring-before(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),1,3)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varFuture">
							<xsl:choose>
								<xsl:when test="contains(COL45,'Futures') and COL36='INR'">
									<xsl:value-of select="concat($varFutureSymbol,' ',$varFutureMonthCode,$varFutureYear,'-NSF')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($varFutureSymbol,' ',$varFutureMonthCode,$varFutureYear)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="contains(COL6,'.')">
									<xsl:value-of select="substring-before(COL6,'.')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL6"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Suffix" select="''"/>
										<xsl:with-param name="Symbol" select="COL3"/>
									</xsl:call-template>
								</xsl:when>

								<xsl:when test="$Asset='FxSpot'">
									<xsl:value-of select="$varFXSpot"/>
								</xsl:when>

								<xsl:when test="$Asset='FxForward'">
									<xsl:value-of select="$varFXForward"/>
								</xsl:when>

								<xsl:when test="$Asset='EquitySwap'">
									<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
								</xsl:when>

								<xsl:when test="$Asset='FUTURE'">
									<xsl:value-of select="$varFuture"/>
								</xsl:when>

								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<SEDOL>
							<xsl:value-of select="$varSEDOL"/>
						</SEDOL>

						<xsl:variable name="PB_FUND_NAME" select="''"/>

						<xsl:variable name="PRANA_FUND_NAME">
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

						<Quantity>
							<xsl:choose>
								<xsl:when test="$Quantity &gt; 0">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>

								<xsl:when test="$Quantity &lt; 0">
									<xsl:value-of select="$Quantity * -1"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>
						<AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPrice &gt; 0">
									<xsl:value-of select="$AvgPrice"/>

								</xsl:when>
								<xsl:when test="$AvgPrice &lt; 0">
									<xsl:value-of select="$AvgPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>


						<xsl:variable name="varSecFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFees>
							<xsl:choose>
								<xsl:when test="$Asset='EquitySwap'">
									<xsl:value-of select="'0'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varSecFees &gt; 0">
											<xsl:value-of select="$varSecFees"/>
										</xsl:when>
										<xsl:when test="$varSecFees &lt; 0">
											<xsl:value-of select="$varSecFees * (-1)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SecFees>

						<xsl:variable name="varCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL28"/>
							</xsl:call-template>
						</xsl:variable>
						<Commission>
							<xsl:choose>
								<xsl:when test="$Asset='EquitySwap'">
									<xsl:value-of select="'0'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varCommission &gt; 0">
											<xsl:value-of select="$varCommission"/>
										</xsl:when>
										<xsl:when test="$varCommission &lt; 0">
											<xsl:value-of select="$varCommission * (-1)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>


						<xsl:variable name="varGrossNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL20"/>
							</xsl:call-template>
						</xsl:variable>
						<GrossNotionalValue>
							<xsl:choose>
								<xsl:when test="$varGrossNotionalValue &gt; 0">
									<xsl:value-of select="$varGrossNotionalValue"/>
								</xsl:when>
								<xsl:when test="$varGrossNotionalValue &lt; 0">
									<xsl:value-of select="$varGrossNotionalValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</GrossNotionalValue>

						<xsl:variable name="varGrossNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL21"/>
							</xsl:call-template>
						</xsl:variable>
						<GrossNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$varGrossNotionalValueBase &gt; 0">
									<xsl:value-of select="$varGrossNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$varGrossNotionalValueBase &lt; 0">
									<xsl:value-of select="$varGrossNotionalValueBase * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</GrossNotionalValueBase>



						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL33"/>
							</xsl:call-template>
						</xsl:variable>
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

						<xsl:variable name="NetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL34"/>
							</xsl:call-template>
						</xsl:variable>
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

						<TradeDate>
							<xsl:value-of select ="COL11"/>
						</TradeDate>

						<SettlementDate>
							<xsl:value-of select ="COL12"/>
						</SettlementDate>

						<xsl:variable name="Side">
							<xsl:value-of select="COL9"/>
						</xsl:variable>
						<Side>

							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when test="$Side='BLX'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>
										<xsl:when test="$Side='BL'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when test="$Side='SL'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>
										<xsl:when test="$Side='SS'">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Side='BLX'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>
										<xsl:when test="$Side='BL'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when test="$Side='SL'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:when test="$Side='SS'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</Side>

						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="COL23"/>
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


						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>


	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>


