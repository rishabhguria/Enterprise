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
				<xsl:when test="$Month=6 ">
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

	<xsl:template name="MonthsCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth=01">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth=02">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth=03">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth=04">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth=05">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth=06">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth=07">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth=08">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth=09">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth=10">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth=11">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth=12">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL18,'/')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL9,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),2,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(normalize-space(COL9),' '),4,2)"/>
			</xsl:variable>

			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after(normalize-space(COL9),' '),8),'##.##')"/>
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


			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$ExpiryMonth,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>



	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL4"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity) ">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL22"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="COL24 ='EQUITY' and COL17 ='SWAP'">
									<xsl:value-of select="'EquitySwap'"/>
								</xsl:when>
								<xsl:when test="COL24 ='EQUITY' and COL17 ='CASH'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL24 ='FUTURE'">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="COL24 ='FX FWD'">
									<xsl:value-of select="'FXForward'"/>
								</xsl:when>
								<xsl:when test="COL24 ='FX SPOT'">
									<xsl:value-of select="'FXSpot'"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<Asset>
							<xsl:value-of select="$Asset"/>
						</Asset>
						<xsl:variable name ="varFXMonths">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(COL19,'/')) ='1'">
									<xsl:value-of select="concat('0',substring-before(COL19,'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(COL19,'/')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFXDay">
							<xsl:choose>
								<xsl:when test="string-length(substring-before(substring-after(COL19,'/'),'/')) ='1'">
									<xsl:value-of select="concat('0',substring-before(substring-after(COL19,'/'),'/'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(substring-after(COL19,'/'),'/')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFXYear">
							<xsl:value-of select="substring(substring-after(substring-after(COL19,'/'),'/'),3,2)"/>
						</xsl:variable>



						<xsl:variable name="varFXForward">
							<xsl:choose>
								<xsl:when test="COL13='EUR' or COL13='GBP' or COL13='NZD' or COL13='AUD'">
									<xsl:value-of select="concat(COL13,'/','USD',' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',COL13,' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFXSpot">
							<xsl:choose>
								<xsl:when test="COL13='EUR' or COL13='GBP' or COL13='NZD' or COL13='AUD'">
									<xsl:value-of select="concat(COL13,'/','USD')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',COL13)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varFutureSymbol">
							<xsl:value-of select="substring-before(normalize-space(COL5),'=')"/>
						</xsl:variable>

						<xsl:variable name="varFutureCode">
							<xsl:value-of select="substring-before(substring-after(normalize-space(COL5),'='),' ')"/>
						</xsl:variable>
						

						<xsl:variable name="varFuture">
							<xsl:choose>
								<xsl:when test="contains(COL24,'FUTURE') and contains(COL5,'IS')">
									<xsl:value-of select="concat($varFutureSymbol,' ',$varFutureCode,'-NSF')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($varFutureSymbol,' ',$varFutureCode)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name = "PB_SUFFIX_NAME" >
							<xsl:value-of select ="substring-after(COL5,' ')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="contains(COL5,' ')">
									<xsl:value-of select="substring-before(COL5,' ')"/>
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


								<!--<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="COL3"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>-->

								<xsl:when test="$Asset='EquitySwap'">
									<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
								</xsl:when>
								
								<xsl:when test="$Asset='FXSpot'">
									<xsl:value-of select="$varFXSpot"/>
								</xsl:when>

								<xsl:when test="$Asset='FXForward'">
									<xsl:value-of select="$varFXForward"/>
								</xsl:when>

								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>

								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="$varFuture"/>
								</xsl:when>

							

								<xsl:otherwise>
									<xsl:value-of select="$varSymbol"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<xsl:variable name="PB_FUND_NAME" select="COL25"/>
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
								<xsl:with-param name="Number" select="COL9"/>
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
								<xsl:with-param name="Number" select="COL8"/>
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
								<xsl:with-param name="Number" select="COL7"/>
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



						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL37 div COL36"/>
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



						<TradeDate>
							<xsl:value-of select ="COL18"/>
						</TradeDate>



						<SettlementDate>
							<xsl:value-of select ="COL19"/>
						</SettlementDate>



						<xsl:variable name="Side">
							<xsl:value-of select="COL3"/>
						</xsl:variable>
						<Side>
							<xsl:choose>

								<xsl:when test ="$Side='SELL'">
									<xsl:value-of select ="'Sell'"/>
								</xsl:when>
								<xsl:when test ="$Side='BUY'">
									<xsl:value-of select ="'Buy'"/>
								</xsl:when>
								<xsl:when test ="$Side='SHRT'">
									<xsl:value-of select ="'Sell short'"/>
								</xsl:when>
								<xsl:when test ="$Side='BC'">
									<xsl:value-of select ="'Buy to Close'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

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


